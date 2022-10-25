using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float START_DELAY = 1f; // delay before start combat routine starts
    public static float ENEMY_SPAWN_DELAY = 0.25f; // delay before combat starts
    public static float BEFORE_DRAW_CARD_DELAY = 0.5f; // delay between spawning enemies and drawing cards
    public static float END_PLAYER_TURN_DELAY = 0.5f; // delay once player turn ends
    public static float DRAW_CARD_DELAY = 0.1f; // delay between drawing cards
    public static float PRE_ENEMY_TURN_DELAY = 0.5f; // delay after enemy intents
    public static float END_ENEMY_TURN_DELAY = 1f; // delay after enemy intents
    public static float END_COMBAT_DELAY = 0.5f; // delay once combat is over before discarding player hand
    public static CombatManager instance;
    void Awake()
    {
        // set this combat manager to be the only instance
        if (!instance)
        {
            instance = this;
        }
    }

    public int enemy_count = 3;

    // private vars
    private int combat_turn = 1;
    private bool can_end_player_turn = false;
    private bool combat_over = false; public bool GetCombatOver() { return combat_over; }

    //// ******** START COMBAT ******** ////

    public void StartCombat()
    {
        // stop player input
        GameManager.instance.allow_player_input = false;
        // reset turn number
        combat_turn = 1;
        // start combat after a delay
        StartCoroutine(StartCombatRoutine());
    }
    private IEnumerator StartCombatRoutine()
    {
        List<EnemyData> enemy_data = new List<EnemyData>();
        enemy_data.AddRange(GameManager.instance.GetEnemyData());
        // wait for start delay
        yield return new WaitForSeconds(START_DELAY);
        // spawn enemy(ies)
        for (int i = 0; i < enemy_data.Count; i++)
        {
            EnemyManager.instance.SpawnEnemy(enemy_data[i]);
            yield return new WaitForSeconds(ENEMY_SPAWN_DELAY);
        }
        // reset draw and discard piles
        CardManager.instance.ResetAndClearPiles();
        // set deck
        CardManager.instance.SetPlayerDeck(Player.instance.GetPlayerDeck());
        // wait for start delay again
        yield return new WaitForSeconds(BEFORE_DRAW_CARD_DELAY);
        // start player turn
        StartPlayerTurn();
    }

    //// ******** END COMBAT ******** ////

    public void CheckEndCombat()
    {
        if (EnemyManager.instance.AllEnemiesDead() || PlayerEntity.instance.PlayerDead())
        {
            EndCombat();
        }
    }

    public void EndCombat()
    {
        if (!combat_over)
        {
            combat_over = true;
            StartCoroutine(EndCombatRoutine());
        }
    }
    private IEnumerator EndCombatRoutine()
    {
        GameManager.instance.allow_player_input = false; // stop player input
        can_end_player_turn = false; // player can no longer end their turn
        // short delay before discarding player hand
        yield return new WaitForSeconds(END_COMBAT_DELAY);
        // discard player hand
        StartCoroutine(DiscardEntireHandRoutine());
        // TODO: after combat stuff (add new cards, award gold, etc.)
        yield return new WaitForSeconds(END_COMBAT_DELAY);
        GameManager.instance.ReturnToHexMap();
    }   

    //// ******** START PLAYER TURN ******** ////

    public void StartPlayerTurn()
    {
        // check if combat is over
        if (!combat_over)
        {
            can_end_player_turn = true; // player is able to end their turn now
            StartCoroutine(StartPlayerTurnRoutine());
        }
    }
    private IEnumerator StartPlayerTurnRoutine()
    {
        // determine enemy intents
        EnemyManager.instance.DetermineEnemyIntents();
        // draw cards
        for (int i = 0; i < Player.instance.GetDrawCardAmount(); i++)
        {
            CardManager.instance.DrawCard();
            yield return new WaitForSeconds(DRAW_CARD_DELAY);
        }
        // reset energy
        CardManager.instance.ResetEnergy();
        // allow player input
        GameManager.instance.allow_player_input = true;
    }

    //// ******** END PLAYER TURN ******** ////

    public void EndPlayerTurn()
    {
        // check if combat is over
        if (!combat_over)
        {
            // only end turn if allowed
            if (can_end_player_turn)
            {
                can_end_player_turn = false; // player can no longer end their turn
                GameManager.instance.allow_player_input = false; // stop player input
                CardManager.instance.card_UI.AnimateEndTurnButton(); // animate end turn button
                StartCoroutine(EndPlayerTurnRoutine());
            }
        }
    }
    private IEnumerator EndPlayerTurnRoutine()
    {
        // discard hand
        StartCoroutine(DiscardEntireHandRoutine());
        yield return new WaitForSeconds(END_PLAYER_TURN_DELAY);
        // start enemy turn
        StartEnemyTurn();
    }

    private IEnumerator DiscardEntireHandRoutine()
    {
        // discard all cards in hand
        List<CardObject> player_hand = new List<CardObject>();
        player_hand.AddRange(CardManager.instance.card_UI.GetCardObjects());
        foreach (CardObject card in player_hand)
        {
            CardManager.instance.DiscardCard(card.GetCardData(), card);
            yield return new WaitForSeconds(DRAW_CARD_DELAY);
        }
    }

    //// ******** START ENEMY TURN ******** ////

    public void StartEnemyTurn()
    {
        // check if combat is over
        if (!combat_over)
        {
            StartCoroutine(StartEnemyTurnRoutine());
        }
    }
    private IEnumerator StartEnemyTurnRoutine()
    {
        yield return new WaitForSeconds(PRE_ENEMY_TURN_DELAY);
        EnemyManager.instance.ExecuteEnemyIntents();
        // wait for enemies to finish executing intents
        while (EnemyManager.instance.GetEnemiesExecutingIntents())
            yield return null;
        EndEnemyTurn();
    }

    //// ******** END ENEMY TURN ******** ////

    public void EndEnemyTurn()
    {
        // check if combat is over
        if (!combat_over)
        {
            combat_turn += 1;
            StartCoroutine(EndEnemyTurnRoutine());
        }
    }
    private IEnumerator EndEnemyTurnRoutine()
    {
        yield return new WaitForSeconds(END_ENEMY_TURN_DELAY);
        StartPlayerTurn();
    }
}

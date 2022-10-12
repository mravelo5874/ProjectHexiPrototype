using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float START_DELAY = 1f; // delay before start combat routine starts
    public static float ENEMY_SPAWN_DELAY = 0.25f; // delay before combat starts
    public static float BEFORE_DRAW_CARD_DELAY = 0.5f; // delay between spawning enemies and drawing cards
    public static float DRAW_CARD_DELAY = 0.1f; // delay between drawing cards
    public static CombatManager instance;
    void Awake()
    {
        // set this combat manager to be the only instance
        if (!instance)
        {
            instance = this;
        }
    }


    // TODO: get enemy data from hex map stuff
    public EnemyData skeleton_enemy;
    public int enemy_count = 2;

    // private vars
    private int combat_turn = 1;

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
        // wait for start delay
        yield return new WaitForSeconds(START_DELAY);
        // spawn enemy(ies)
        for (int i = 0; i < enemy_count; i++)
        {
            EnemyManager.instance.SpawnEnemy(skeleton_enemy);
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

    //// ******** START PLAYER TURN ******** ////

    public void StartPlayerTurn()
    {
        StartCoroutine(StartPlayerTurnRoutine());
    }
    private IEnumerator StartPlayerTurnRoutine()
    {
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
        // stop player input
        GameManager.instance.allow_player_input = false;
        StartCoroutine(EndPlayerTurnRoutine());
    }
    private IEnumerator EndPlayerTurnRoutine()
    {
        yield break;
    }

    //// ******** START ENEMY TURN ******** ////

    public void StartEnemyTurn()
    {

    }

    //// ******** END ENEMY TURN ******** ////

    public void EndEnemyTurn()
    {

    }
}

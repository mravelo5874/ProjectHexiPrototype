using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public override void Awake()
    {
        base.Awake(); // call base awake
    }

    void Start()
    {
        // init set player's deck
        Player.instance.AddCardsToPlayerDeck(GameManager.instance.default_knight_deck);
        ResetScene();
    }

    public void ResetScene()
    {
        EnemyManager.instance.ResetEnemies();
        // start combat
        CombatManager.instance.StartCombat();
    }
}

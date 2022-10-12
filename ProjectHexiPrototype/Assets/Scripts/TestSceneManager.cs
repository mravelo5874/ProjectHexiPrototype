using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneManager : MonoBehaviour
{
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

    public void DrawCard()
    {
        CardManager.instance.DrawCard();
    }

    public void AddEnergy()
    {
        CardManager.instance.AddEnergy();
    }

    public void SpawnEnemy()
    {
        EnemyManager.instance.SpawnEnemy(CombatManager.instance.skeleton_enemy);
    }
}
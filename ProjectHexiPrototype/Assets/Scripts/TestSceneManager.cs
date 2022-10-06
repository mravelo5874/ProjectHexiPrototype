using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    void Start()
    {
        ResetScene();
        // allow player input
        GameManager.instance.allow_player_input = true;
    }

    public void ResetScene()
    {
        CardManager.instance.ResetAndClearPiles();
        CardManager.instance.SetMasterList(GameManager.instance.default_knight_deck);
    }

    public void DrawCard()
    {
        CardManager.instance.DrawCard();
    }

    public void AddEnergy()
    {

    }

    public void SpawnEnemy()
    {

    }
}

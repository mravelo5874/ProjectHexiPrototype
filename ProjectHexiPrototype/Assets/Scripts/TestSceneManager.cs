using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    void Start()
    {
        ResetScene();
    }

    public void ResetScene()
    {
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

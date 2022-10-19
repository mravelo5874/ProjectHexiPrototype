using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        // set this game manager to be the only instance
        if (!instance)
        {
            instance = this;
        }
    }

    // TODO: move this somewhere else - not here
    public List<CardData> default_knight_deck;

    public bool allow_player_input = false;

    public void GoToScene(string scene_name)
    {
        
    }
}

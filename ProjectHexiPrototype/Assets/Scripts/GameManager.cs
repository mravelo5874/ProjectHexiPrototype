using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Card> default_knight_deck;

    public bool allow_player_input = false;
}

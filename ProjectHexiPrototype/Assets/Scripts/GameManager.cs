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

    public List<Card> default_knight_deck;
}

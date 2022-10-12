using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;
    void Awake()
    {
        // set this player to be the only instance
        if (!instance)
        {
            instance = this;
        }
        // init player deck
        player_deck = new List<CardData>();
    }

    private List<CardData> player_deck; public List<CardData> GetPlayerDeck() { return player_deck; } // public getter for player deck
    private int energy;
    private int total_energy;

    private int draw_card_amount = 5;
    public int GetDrawCardAmount()
    {
        return draw_card_amount;
    }

    // override Entity Start() and call it
    public override void Start()
    {
        base.Start();
    }

    public void AddCardToPlayerDeck(CardData new_card)
    {
        player_deck.Add(new_card);
    }

    public void AddCardsToPlayerDeck(List<CardData> new_cards)
    {
        player_deck.AddRange(new_cards);
    }

    
}

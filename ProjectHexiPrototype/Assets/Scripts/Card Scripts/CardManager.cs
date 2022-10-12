using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static int TRUE_MAX_ENERGY = 8; // the max amount of energy a player can have at once
    // TODO: change to maybe be infinite or much larger

    public static CardManager instance;
    // set this card manager to be the only instance
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        // create lists
        player_deck = new List<CardData>();
        draw_pile = new List<CardData>();
        hand = new List<CardData>();
        discard_pile = new List<CardData>();
    }

    public CardUI card_UI;
    public int max_hand_cards = 8;

    // card lists
    private List<CardData> player_deck; // contains every card in player deck
    private List<CardData> draw_pile;
    private List<CardData> hand;
    private List<CardData> discard_pile;

    // getters for card lists
    public List<CardData> GetMasterList() { return player_deck; }
    public List<CardData> GetDrawPile() { return draw_pile; }
    public List<CardData> GetHand() { return hand; }
    public List<CardData> GetDiscardPile() { return discard_pile; }

    // energy
    private int max_energy = 3;
    private int current_energy;

    // getters for energy values
    public int GetMaxEnergy() { return max_energy; }
    public int GetCurrentEnergy() { return current_energy; }

    public void ResetAndClearPiles()
    {
        // create lists
        player_deck = new List<CardData>();
        draw_pile = new List<CardData>();
        hand = new List<CardData>();
        discard_pile = new List<CardData>();
        // update card UI
        card_UI.ClearCardsInHand();
        card_UI.UpdateVisuals();
    }

    public void ResetEnergy()
    {
        // reset energy
        current_energy = max_energy;
        card_UI.ResetEnergy();
        // update card UI
        card_UI.UpdateVisuals();
    }

    // completely overrides master-list
    // should only be done on init when player starts game
    public void SetPlayerDeck(List<CardData> cards)
    {
        // add range to master list
        player_deck.Clear();
        player_deck.AddRange(cards);
        // place all cards into draw pile
        draw_pile.Clear();
        draw_pile.AddRange(player_deck);
        // update card UI
        card_UI.UpdateVisuals();
    }

    // should not be called faster than CardUI.CARD_DRAW_SPEED
    public void DrawCard()
    {
        // refresh deck if no cards left in draw pile
        if (draw_pile.Count <= 0)
        {
            RefreshDeck();
        }   
        // if still no cards in draw pile, return (all cards are in hand)
        if (draw_pile.Count <= 0)
        {
            // TODO: message to player that no cards are available to draw
            print ("no cards available in draw pile");
            return;
        }

        // get random card from draw pile and add to hand
        int index = Random.Range(0, draw_pile.Count);
        CardData card = draw_pile[index];

        // if hand is at max capacity, send card straight to discard pile
        if (hand.Count >= max_hand_cards)
        {
            discard_pile.Add(card);
            draw_pile.RemoveAt(index);
            card_UI.DiscardCardFromDrawPile(card);
        }
        // else add card to hand
        else
        {
            hand.Add(card);
            draw_pile.RemoveAt(index);
            card_UI.AddCardToHand(card);
        }
    }

    // attempt to play a card
    public void AttemptPlayCard(CardData card_data, CardObject card_object)
    {   
        bool play_card = false;
        // determine card target
        string tag_target = "";
        switch (card_data.card_target)
        {
            case CardData.CardTarget.Player:
                tag_target = "Player";
                break;

            case CardData.CardTarget.Enemy:
                tag_target = "Enemy";
                break;

            case CardData.CardTarget.Environment:
                tag_target = "Environment";
                break;
        }
        // send raycast at mouse position to check for enemies / player / environment
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag == tag_target)
            {
                play_card = true;
            }
        }

        if (play_card)
        {
            // determine if player has enough energy to play card
            if (card_data.energy_cost <= current_energy)
            {
                current_energy -= card_data.energy_cost;
                card_UI.PlayCard(card_object, hit.transform.GetComponentInParent<Entity>());
            }
            else
            {
                // TODO: inform player that they do not have enough energy to play this card
                card_UI.UnfollowCard();
            }
        }
        else
        {
            card_UI.UnfollowCard();
        }   
    }

    public void DiscardCard(CardData card_data, CardObject card_object)
    {
        // return if hand does not contain card - this should never happen so send ERROR
        if (!hand.Contains(card_data))
        {
            Debug.LogError("attempting to discard card that is not in player hand");
            return;
        }
        
        // remove card from hand and add to discard pile
        hand.Remove(card_data);
        discard_pile.Add(card_data);
        // animate card being discarded
        card_UI.DiscardCardIntoPile(card_object);
    }
    
    // place all cards in discard pile into draw pile
    public void RefreshDeck()
    {
        int total_discarded_cards = discard_pile.Count;
        for (int i = 0; i < total_discarded_cards; i++)
        {
            int index = Random.Range(0, discard_pile.Count);
            CardData card = discard_pile[index];
            draw_pile.Add(card);
            discard_pile.RemoveAt(index);
        }

        // update card UI
        card_UI.UpdateVisuals();
    }

    // adds one energy
    public void AddEnergy()
    {
        current_energy++;
        // make sure energy does not exceed max energy
        if (current_energy > TRUE_MAX_ENERGY)
        {
            current_energy = TRUE_MAX_ENERGY;
        }
        card_UI.UpdateVisuals();
    }
}

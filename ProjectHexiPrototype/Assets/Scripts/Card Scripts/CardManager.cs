using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    // set this card manager to be the only instance
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        // create lists
        master_list = new List<Card>();
        draw_pile = new List<Card>();
        hand = new List<Card>();
        discard_pile = new List<Card>();
    }

    public CardUI card_UI;
    public int max_hand_cards = 8;

    // card lists
    private List<Card> master_list; // contains every card in player deck
    private List<Card> draw_pile;
    private List<Card> hand;
    private List<Card> discard_pile;

    // getters for card lists
    public List<Card> GetMasterList() { return master_list; }
    public List<Card> GetDrawPile() { return draw_pile; }
    public List<Card> GetHand() { return hand; }
    public List<Card> GetDiscardPile() { return discard_pile; }

    // energy
    private int max_energy = 3;
    private int current_energy;

    // getters for energy values
    public int GetMaxEnergy() { return max_energy; }
    public int GetCurrentEnergy() { return current_energy; }

    public void ResetAndClearPiles()
    {
        // reset energy
        current_energy = max_energy;
        // create lists
        master_list = new List<Card>();
        draw_pile = new List<Card>();
        hand = new List<Card>();
        discard_pile = new List<Card>();
        // update card UI
        card_UI.SetMaxEnergy(max_energy);
        card_UI.ClearCardsInHand();
        card_UI.UpdateVisuals();
    }

    // completely overrides master-list
    // should only be done on init when player starts game
    public void SetMasterList(List<Card> cards)
    {
        // add range to master list
        master_list.Clear();
        master_list.AddRange(cards);
        // place all cards into draw pile
        draw_pile.Clear();
        draw_pile.AddRange(master_list);
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
        Card card = draw_pile[index];

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
    public void AttemptPlayCard(CardObject card)
    {   
        // TODO: play card depending on card type
        bool play_card = false;
        // send raycast at mouse position to check for enemies / player
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag == "Enemy" || hit.transform.tag == "Player")
            {
                play_card = true;
            }
        }

        if (play_card)
        {
            // determine if player has enough energy to play card
            if (card.GetCardData().energy_cost <= current_energy)
            {
                current_energy -= card.GetCardData().energy_cost;
                card_UI.PlayCard(card);
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

    public void DiscardCard(Card discarded_card)
    {
        // return if hand does not contain card - this should never happen so send ERROR
        if (!hand.Contains(discarded_card))
        {
            Debug.LogError("attempting to discard card that is not in player hand");
            return;
        }
        
        // remove card from hand and add to discard pile
        hand.Remove(discarded_card);
        discard_pile.Add(discarded_card);
    }
    
    // place all cards in discard pile into draw pile
    public void RefreshDeck()
    {
        int total_discarded_cards = discard_pile.Count;
        for (int i = 0; i < total_discarded_cards; i++)
        {
            int index = Random.Range(0, discard_pile.Count);
            Card card = discard_pile[index];
            draw_pile.Add(card);
            discard_pile.RemoveAt(index);
        }

        // update card UI
        card_UI.UpdateVisuals();
    }

    public void EndPlayerTurn()
    {
        Debug.Log("ending player turn");
        // TODO: end player turn
    }

    // add energy
    public void AddEnergy()
    {
        
    }
}

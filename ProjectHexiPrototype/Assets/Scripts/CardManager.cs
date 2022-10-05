using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // card lists
    private List<Card> master_list; // contains every card in player deck
    private List<Card> draw_pile;
    private List<Card> hand;
    private List<Card> discard_pile;

    // UI components
    public MyObject discardIcon;
    public TextMeshProUGUI discardText;
    public MyObject drawIcon;
    public TextMeshProUGUI drawText;


    // completely overrides master-list
    // should only be done on init when player starts game
    public void SetMasterList(List<Card> cards)
    {
        master_list.Clear();
        master_list.AddRange(cards);
    }

    public void DrawCard()
    {
        // refresh deck if no cards left in draw pile
        if (draw_pile.Count <= 0)
        {
            RefreshDeck();
        }

        // get random card from draw pile and add to hand
        int index = Random.Range(0, draw_pile.Count);
        Card card = draw_pile[index];
        hand.Add(card);
        draw_pile.RemoveAt(index);

        // update cards
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {

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

        // update cards
        UpdateVisuals();
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

        // update cards
        UpdateVisuals();
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class CardUI : MonoBehaviour
{   
    //// STATIC VARIABLES ////
    public static float CARD_DRAW_SPEED = 0.1f;
    public static float CARD_MOVE_SPEED = 0.2f;
    public static float MAX_CARD_ANGLE = 30f;
    public static float CARD_Y_MULTIPLIER = -100f;
    public static float SELECTED_CARD_OFFSET = 100f;
    public static float DEFAULT_HAND_POS_WIDTH = 50f;
    public static float SELECTED_WIDTH_MULTIPLIER = 4f;

    // UI components
    [Header("Hand")]
    public Transform card_parent;
    public GameObject card_prefab;
    public List<GameObject> hand_positions;
    
    private List<CardObject> card_objects;
    private CardObject selected_card = null;
    private int prev_selected_card_index = 0;
    private bool follow_player = false;

    [Header("Energy")]
    public List<MyObject> energy_icons;
    private int prev_energy_amount = 0;
    
    [Header("Draw Pile")]
    public MyObject draw_icon;
    public TextMeshProUGUI draw_text;

    [Header("Discard Pile")]
    public MyObject discard_icon;
    public TextMeshProUGUI discard_text;

    void Awake()
    {
        // set default values
        draw_text.text = "0";
        discard_text.text = "0";

        card_objects = new List<CardObject>();

        // disable hand positions
        foreach (GameObject pos in hand_positions)
        {
            pos.SetActive(false);
        }
        // disable energy icons
        foreach (MyObject icon in energy_icons)
        {
            icon.gameObject.SetActive(false);
        }
        prev_energy_amount = 0;
    }

    void FixedUpdate()
    {
        // if card is selected and following player, move card to mouse pos
        if ((selected_card && follow_player))
        {
            selected_card.transform.position = Vector2.Lerp(selected_card.transform.position, Input.mousePosition, CARD_MOVE_SPEED);
        }
    }

    public void SetMaxEnergy(int max_energy)
    {
        // disable energy icons
        foreach (MyObject icon in energy_icons)
        {
            icon.gameObject.SetActive(false);
        }
        // enable max energy icons
        for (int i = 0; i < max_energy; i++)
        {
            energy_icons[i].gameObject.SetActive(true);
            energy_icons[i].transform.localScale = Vector3.zero;
        }
        prev_energy_amount = 0;
    }

    public void UpdateVisuals()
    {
        StartCoroutine(UpdateVisualsRoutine());
    }
    
    private IEnumerator UpdateVisualsRoutine()
    {
        // need to wait here for hand positions to update correctly
        yield return null;
        // set correct texts
        draw_text.text = CardManager.instance.GetDrawPile().Count.ToString();
        discard_text.text = CardManager.instance.GetDiscardPile().Count.ToString();
        // return (yield break) if card is following player
        if (follow_player)
        {
            yield break;
        }
        // set card positions and rotations
        int i = 0;
        foreach (CardObject card in card_objects)
        {
            card.myObject.MoveToPosition(CalculateCardPos(i), CARD_DRAW_SPEED, true, false);
            card.myObject.ChangeRotation(CalculateCardAngle(i), CARD_DRAW_SPEED, true);
            i++;
        }
        // set selected card to be last child (so that it is in front of every other card)
        if (selected_card)
        {
            selected_card.transform.SetAsLastSibling();
        }

        // set energy indicator icons
        int new_energy_amount = CardManager.instance.GetCurrentEnergy();
        // remove energy icons
        if (new_energy_amount < prev_energy_amount)
        {
            for (int x = prev_energy_amount - 1; x > new_energy_amount - 1; x--)
            {
                energy_icons[x].SquishyChangeScale(1.1f, 0f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
        }
        // add energy icons
        if (new_energy_amount > prev_energy_amount)
        {
            for (int x = prev_energy_amount; x < new_energy_amount; x++)
            {
                energy_icons[x].SquishyChangeScale(1.1f, 1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0.05f);
            }
        }
        // set new energy amount
        prev_energy_amount = new_energy_amount;
    }

    private Vector3 CalculateCardPos(int index)
    {
        Vector3 hand_pos = hand_positions[index].transform.position;

        // determine if card is selected
        if (selected_card == card_objects[index])
        {
            // else return selected card offset
            return new Vector3(hand_pos.x, hand_pos.y + SELECTED_CARD_OFFSET, hand_pos.z);
        }
        // return hand_pos iff 1 card in hand
        if (card_objects.Count == 1)
            return new Vector3(hand_pos.x, hand_pos.y + (CARD_Y_MULTIPLIER * 0.5f), hand_pos.z);
        
        // get position in list between 0 and 1
        float floatPos = (float)(index) / (float)(card_objects.Count - 1);
        // determine distance from mid
        float midDis = Mathf.Abs(floatPos - 0.5f);
        // lower card the further it is from mid
        float new_y_pos = hand_pos.y + (CARD_Y_MULTIPLIER * midDis);
        return new Vector3(hand_pos.x, new_y_pos, hand_pos.z);
    }

    private float CalculateCardAngle(int index)
    {   
        // return 0 if only 1 card in hand
        if (card_objects.Count == 1)
            return 0f;
        // return 0 if card is selected
        if (selected_card == card_objects[index])
        {
            return 0f;
        }
        
        float angle = 0f;
        // get position in list between 0 and 1
        float floatPos = (float)(index) / (float)(card_objects.Count - 1);
        // determine distance from mid
        float midDis = floatPos - 0.5f;
        //print ("index: " + index + " float pos: " + floatPos + " mid distance: " + midDis);
        // determine angle
        angle = Mathf.Lerp(0f, MAX_CARD_ANGLE, Mathf.Abs(midDis));
        // dampen angle more when less cards
        float dampAmount = (float)card_objects.Count / (float)CardManager.instance.max_hand_cards;
        angle *= dampAmount;
        // negate if on left side of mid
        if (floatPos > 0.5f)
        {
            angle *= -1;
        }
        return angle;
    }

    public void ClearCardsInHand()
    {
        // destroy each card object
        foreach (CardObject card in card_objects) 
        {
            GameObject.Destroy(card.gameObject);
        }
        card_objects.Clear();
        // disable hand positions
        foreach (GameObject pos in hand_positions)
        {
            pos.SetActive(false);
        }
    }

    public void AddCardToHand(Card card)
    {
        draw_icon.SquishyChangeScale(0.8f, 1f, 0.1f, 0.1f);
        // instantiate new card with 0 scale
        GameObject newCard = Instantiate(card_prefab, draw_icon.transform.position, Quaternion.identity, card_parent);
        newCard.transform.localScale = Vector3.zero;
        // set card data
        CardObject cardObject = newCard.GetComponent<CardObject>();
        cardObject.SetCard(card);
        card_objects.Add(cardObject);
        // add new hand pos
        hand_positions[card_objects.Count - 1].SetActive(true);
        // scale new card to full size
        cardObject.myObject.ChangeScale(1f, CARD_DRAW_SPEED);
        // update card positions and rotations
        UpdateVisuals();
    }

    public void SetSelectedCard(CardObject card)
    {
        prev_selected_card_index = card_objects.IndexOf(card);
        hand_positions[prev_selected_card_index].GetComponent<MyObject>().SetRectTransformWidth(DEFAULT_HAND_POS_WIDTH * SELECTED_WIDTH_MULTIPLIER);
        selected_card = card;
        UpdateVisuals();
    }

    public void UnselectCard()
    {
        if (selected_card)
        {
            selected_card.transform.SetSiblingIndex(prev_selected_card_index);
            hand_positions[prev_selected_card_index].GetComponent<MyObject>().SetRectTransformWidth(DEFAULT_HAND_POS_WIDTH);
            selected_card = null;
            UpdateVisuals();
        }   
    }

    public void DetermineSelectCard(CardObject card)
    {
        // if have a card selected and hovered over a new card without letting go of screen, select new card
        if (selected_card)
        {
            // stop following player + unselect card
            UnfollowCard();
            SetSelectedCard(card);
        }
    }

    public void CardFollowPlayer()
    {
        // if card not following player, start following
        if (!follow_player && selected_card)
        {
            selected_card.myObject.ChangeScale(0.5f, 0.1f);
            follow_player = true;

            // TODO: highlight where player needs to play card (Enemies, Player, Environment)
        }
    }

    public void UnfollowCard()
    {
        // if card following player, stop following
        if (follow_player)
        {
            selected_card.myObject.ChangeScale(1f, 0.1f);
            follow_player = false;
        }
        // unselect card
        UnselectCard();
    }

    public void PlayCard(CardObject card)
    {
        StartCoroutine(PlayCardRoutine(card));
    }

    private IEnumerator PlayCardRoutine(CardObject card)
    {
        // unselect + unfollow card and play
        follow_player = false;
        selected_card = null;
        hand_positions[prev_selected_card_index].GetComponent<MyObject>().SetRectTransformWidth(DEFAULT_HAND_POS_WIDTH);
        card.PlayCard();
        card.myObject.SquishyChangeScale(0.3f, 0.5f, 0.1f, 0.1f);
        yield return new WaitForSeconds(0.2f);
        CardManager.instance.DiscardCard(card.GetCardData());
        // move card to discard pile
        card.myObject.MoveToTransform(discard_icon.transform, CARD_DRAW_SPEED, true, false);
        card.myObject.ChangeScale(0f, CARD_DRAW_SPEED);
        yield return new WaitForSeconds(CARD_DRAW_SPEED);
        // remove card object from list
        hand_positions[card_objects.Count - 1].SetActive(false);
        card_objects.Remove(card);
        Destroy(card.gameObject);
        discard_icon.SquishyChangeScale(0.8f, 1f, 0.1f, 0.1f);
        // update visuals
        UpdateVisuals();
    }

    public void DiscardCardFromDrawPile(Card card)
    {
        // TODO: do this
    }
}

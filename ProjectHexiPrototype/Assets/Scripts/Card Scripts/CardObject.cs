using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
    , IPointerDownHandler
    , IPointerUpHandler
    , IPointerExitHandler
    , IDragHandler
    , IPointerEnterHandler
{   
    public MyObject myObject;
    private Card myCard;

    public TextMeshProUGUI cost_text;
    public TextMeshProUGUI card_name_text;
    public TextMeshProUGUI card_description_text;

    public void SetCard(Card card)
    {
        myCard = card;
        // set card UI
        cost_text.text = myCard.energy_cost.ToString();
        card_name_text.text = myCard.card_name.ToString();
        card_description_text.text = myCard.card_description.ToString();
    }

    public Card GetCardData()
    {
        return myCard;
    }

    public void PlayCard()
    {
        // TODO: play card stuff idk
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CardManager.instance.card_UI.SetSelectedCard(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        CardManager.instance.card_UI.CardFollowPlayer();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CardManager.instance.AttemptPlayCard(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // CardManager.instance.card_UI.UnselectCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // CardManager.instance.card_UI.DetermineSelectCard(this);
    }
}
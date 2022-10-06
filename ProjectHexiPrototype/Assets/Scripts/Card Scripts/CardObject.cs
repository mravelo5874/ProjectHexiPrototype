using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
    , IPointerClickHandler
    , IDragHandler
    , IPointerEnterHandler
    , IPointerExitHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        CardManager.instance.card_UI.SetSelectedCard(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("I'm being dragged!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardManager.instance.card_UI.UnselectCard();
    }
}
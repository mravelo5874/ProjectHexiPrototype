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
    private CardData myCard;

    public TextMeshProUGUI cost_text;
    public TextMeshProUGUI card_name_text;
    public TextMeshProUGUI card_description_text;

    public void SetCard(CardData card)
    {
        myCard = card;
        // set card UI
        cost_text.text = myCard.energy_cost.ToString();
        card_name_text.text = myCard.card_name.ToString();
        card_description_text.text = myCard.card_description.ToString();
    }

    public CardData GetCardData()
    {
        return myCard;
    }

    public void PlayCard(Entity target)
    {
        foreach (CardModifier mod in myCard.modifiers)
        {
            switch (mod.modifier)
            {
                case CardModifier.Modifier.DealDamage:
                    target.ApplyDamage(mod.amount);
                    break;

                case CardModifier.Modifier.GainBlock:
                    target.GainBlock(mod.amount);
                    break;

                case CardModifier.Modifier.Heal:
                    target.Heal(mod.amount);
                    break;
            }
        }
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
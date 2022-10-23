using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexOptionPrefab : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float OPTION_REVEAL_DURATION = 0.2f;
    public static float DELETE_DURATION = 0.2f;

    public MyObject my_object;
    public TextMeshProUGUI option_text;

    // private vars
    private HexOption my_option;
    private int my_index;

    void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public void SetHexOption(HexOption option, int index)
    {
        my_option = option;
        my_index = index;
        UpdateString();
    }

    private void UpdateString()
    {
        // create option string
        string option_string = "[" + my_index.ToString() + "]\t";
        option_string += my_option.GetText();
        // only show cost if greater than 0
        if (my_option.cost > 0)
        {
            option_string += " (-" + my_option.cost + " world energy)";
        }

        // set text
        option_text.text = option_string;
        
        // change option based on availability of option
        if (!my_option.used)
        {
            my_object.ChangeImageAlpha(0.5f, OPTION_REVEAL_DURATION);
        }
        else
        {
            my_object.ChangeImageAlpha(0.25f, OPTION_REVEAL_DURATION);
        }
    }

    public void Delete()
    {
        StartCoroutine(DeleteRoutine());
    }
    private IEnumerator DeleteRoutine()
    {
        my_object.ChangeScale(0f, DELETE_DURATION);
        yield return new WaitForSeconds(DELETE_DURATION);
        Destroy(this.gameObject);
    }

    public void OnHexOptionPressed()
    {
        // return if no charges are left
        if (my_option.used)
        {
            return;
        }
        // execute option    
        my_object.SquishyChangeScale(0.9f, 1.0f, 0.1f, 0.1f);
        my_option.Execute();
        UpdateString();
    }
}

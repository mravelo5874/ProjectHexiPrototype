using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    // TODO: using text to display intent for now - switch to better UI later :)
    public MyObject intent_text;

    // private vars
    private EnemyData enemy_data;
    private List<EnemyData.Intent> intent_list;
    private EnemyData.Intent current_intent;
    private int current_intent_index = 0;
    private int enemy_index; 
    public void SetEnemyIndex(int index) { enemy_index = index; } // public setter for enemy_index
    public int GetEnemyIndex() { return enemy_index; } // public getter for enemy_index


    // override Entity Start() and call it
    public override void Start()
    {
        base.Start();
        // no intent
        intent_text.SetTextMeshText("");
        intent_text.transform.localScale = Vector3.zero;
    }

    public void SetEnemyData(EnemyData data)
    {
        enemy_data = data;
        // set intent list
        intent_list = new List<EnemyData.Intent>();
        intent_list.AddRange(data.intents);
        current_intent_index = 0;
        // set health
        SetMaxHealth(data.max_health);
    }

    

    public void DetermineEnemyIntent()
    {
        // if execute intents in order
        // set next intent in list
        if (enemy_data.execute_intents_in_order)
        {
            current_intent = intent_list[current_intent_index];
            current_intent_index++;
            if (current_intent_index >= intent_list.Count)
            {
                current_intent_index = 0;
            }
        }
        // else select new random intent from list
        else
        {
            current_intent_index = Random.Range(0, intent_list.Count);
            current_intent = intent_list[current_intent_index];
        }

        // show intent string
        string intent_string = "";
        foreach (CardModifier mod in current_intent.actions)
        {
            intent_string += mod.modifier.ToString() + "(" + mod.amount.ToString() + ")\n";
        }
        intent_text.SetTextMeshText(intent_string);
        intent_text.SquishyChangeScale(1.1f, 1f, 0.1f, 0.1f);
    }

    public void ExecuteEnemyIntents()
    {
        foreach (CardModifier mod in current_intent.actions)
        {
            switch (mod.modifier)
            {
                case CardModifier.Modifier.DealDamage:
                    Player.instance.ApplyDamage(mod.amount);
                    break;

                case CardModifier.Modifier.GainBlock:
                    GainBlock(mod.amount);
                    break;

                case CardModifier.Modifier.Heal:
                    Heal(mod.amount);
                    break;
            }
        }
        // animate entity
        intent_text.SquishyChangeScale(1.1f, 0f, 0.1f, 0.1f);
        entity_object.SquishyChangeScale(11f, 10, 0.2f, 0.2f);
    }
}

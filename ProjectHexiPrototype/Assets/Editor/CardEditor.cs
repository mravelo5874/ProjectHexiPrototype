using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardEditor : Editor
{
    Card myCard;

    // non-editor properties (belong to Card.cs)
    SerializedProperty obj_deal_damage_bool;
    SerializedProperty obj_damage_amount;
    SerializedProperty obj_gain_block_bool;
    SerializedProperty obj_block_amount;

    // local editor variables
    bool edit_deal_damage_bool;
    int edit_damage_amount;
    bool edit_gain_block_bool;
    int edit_block_amount;

#if UNITY_EDITOR
    void OnEnable()
    {
        myCard = (Card)target;

        // wiggle variables
        obj_deal_damage_bool = serializedObject.FindProperty("deal_damage");
        obj_damage_amount = serializedObject.FindProperty("damage_amount");
        obj_gain_block_bool = serializedObject.FindProperty("gain_block");
        obj_block_amount = serializedObject.FindProperty("block_amount");

        edit_deal_damage_bool = obj_deal_damage_bool.boolValue;
        edit_damage_amount = obj_damage_amount.intValue; 
        edit_gain_block_bool = obj_gain_block_bool.boolValue;
        edit_block_amount = obj_block_amount.intValue;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        edit_deal_damage_bool = EditorGUILayout.Toggle("Deal Damage", edit_deal_damage_bool);
        EditorGUILayout.Space();

        if (edit_deal_damage_bool)
        {
            edit_damage_amount = EditorGUILayout.IntField("Damage Amount: ", edit_damage_amount);
        }
        EditorGUILayout.Space();

        edit_gain_block_bool = EditorGUILayout.Toggle("Gain Block", edit_gain_block_bool);
        EditorGUILayout.Space();

        if (edit_gain_block_bool)
        {
            edit_block_amount = EditorGUILayout.IntField("Block Amount: ", edit_block_amount);
        }

        obj_deal_damage_bool.boolValue = edit_deal_damage_bool;
        obj_damage_amount.intValue = edit_damage_amount;

        obj_gain_block_bool.boolValue = edit_gain_block_bool;
        obj_block_amount.intValue = edit_block_amount;

        serializedObject.ApplyModifiedProperties();
    }
#endif
}

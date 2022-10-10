using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemy_name;
    public int max_health;
    // list of intents that the enemy will do
    public bool execute_intents_in_order; // in order or random intents
    public List<Intent> intents;
    [System.Serializable]
    public class Intent
    {
        public List<CardModifier> actions;
    }

}

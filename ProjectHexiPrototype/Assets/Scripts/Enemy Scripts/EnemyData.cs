using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemy_name;
    public int max_health;
    public List<CardModifier.DamageType> weaknesses;
    public List<CardModifier.DamageType> immunities;
    public bool execute_intents_in_order; // in order or random intents
    public List<Intent> intents; // list of intents that the enemy will do
    [System.Serializable]
    public class Intent
    {
        public List<CardModifier> actions;
    }

}

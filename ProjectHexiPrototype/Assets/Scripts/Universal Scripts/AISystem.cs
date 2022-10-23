using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : MonoBehaviour
{
    public static AISystem instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public EnemyData skeleton_data;

    public List<EnemyData> DetermineEnemies()
    {
        List<EnemyData> enemy_data = new List<EnemyData>();
        enemy_data.Add(skeleton_data);
        return enemy_data;
    }
}

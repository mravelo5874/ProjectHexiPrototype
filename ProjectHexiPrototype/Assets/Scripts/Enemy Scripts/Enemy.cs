using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private EnemyData enemy_data;
    private int enemy_index;

    public void SetEnemyData(EnemyData data)
    {
        enemy_data = data;
        SetMaxHealth(data.max_health);
    }

    public void SetEnemyIndex(int index)
    {
        enemy_index = index;
    }

    public int GetEnemyIndex()
    {
        return enemy_index;
    }
}

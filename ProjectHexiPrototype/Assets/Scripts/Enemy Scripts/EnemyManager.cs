using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static int MAX_ENEMIES = 3;

    public static EnemyManager instance;
    // set this enemy manager to be the only instance
    void Awake() 
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public List<Transform> enemy_positions; // enemy spawn positions
    public GameObject enemy_prefab;
    // private variables
    private List<bool> enemy_in_place; // is an enemy currently in this location?

    public void ResetEnemies()
    {
        // remove all enemies
        foreach (Transform enemyPos in enemy_positions)
        {
            if (enemyPos.childCount == 1)
            {
                Destroy(enemyPos.GetChild(0).gameObject);
            }
        }
        // reset in-place list
        enemy_in_place = new List<bool>();
        for (int i = 0; i < MAX_ENEMIES; i++)
        {
            enemy_in_place.Add(false);
        }
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < MAX_ENEMIES; i++)
        {
            if (!enemy_in_place[i])
            {
                enemy_in_place[i] = true;
                GameObject new_enemy = Instantiate(enemy_prefab, enemy_positions[i]);
                new_enemy.transform.localScale = Vector3.zero;
                new_enemy.GetComponent<MyObject>().SquishyChangeScale(11f, 10f, 0.2f, 0.2f);
                return;
            }
        }
        // TODO: do something when enemy cannot be spawned
        Debug.Log("enemy could not be spawned!");
    }

    public void DeleteEnemy()
    {
        // TODO make this
    }
}

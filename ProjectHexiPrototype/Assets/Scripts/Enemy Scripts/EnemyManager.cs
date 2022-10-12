using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static int MAX_ENEMIES = 3;
    public static float DELAY_BETWEEN_SHOW_INTENTS = 0.1f;
    public static float DELAY_BETWEEN_EXECUTE_INTENTS = 1.5f;

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
    private List<Enemy> current_enemies;

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
        // reset lists
        enemy_in_place = new List<bool>();
        current_enemies = new List<Enemy>();
        for (int i = 0; i < MAX_ENEMIES; i++)
        {
            enemy_in_place.Add(false);
        }
    }

    public void SpawnEnemy(EnemyData enemy_data)
    {
        for (int i = 0; i < MAX_ENEMIES; i++)
        {
            if (!enemy_in_place[i])
            {
                enemy_in_place[i] = true;
                GameObject new_enemy = Instantiate(enemy_prefab, enemy_positions[i]);
                new_enemy.transform.localScale = Vector3.zero;
                new_enemy.GetComponent<Entity>().entity_object.SquishyChangeScale(11f, 10f, 0.2f, 0.2f);
                // set enemy's index
                Enemy new_enemy_entity = new_enemy.GetComponent<Enemy>();
                new_enemy_entity.SetEnemyData(enemy_data);
                new_enemy_entity.SetEnemyIndex(i);
                current_enemies.Add(new_enemy_entity);
                return;
            }
        }
        // TODO: do something when enemy cannot be spawned
        Debug.Log("enemy could not be spawned!");
    }

    public void DetermineEnemyIntents()
    {
        StartCoroutine(DetermineEnemyIntentsRoutine());
    }
    private IEnumerator DetermineEnemyIntentsRoutine()
    {
        foreach (Enemy enemy in current_enemies)
        {
            enemy.DetermineEnemyIntent();
            yield return new WaitForSeconds(DELAY_BETWEEN_SHOW_INTENTS);
        }
    }

    public void ExecuteEnemyIntents()
    {
        StartCoroutine(ExecuteEnemyIntentsRoutine());
    }
    private IEnumerator ExecuteEnemyIntentsRoutine()
    {
        foreach (Enemy enemy in current_enemies)
        {
            enemy.ExecuteEnemyIntents();
            yield return new WaitForSeconds(DELAY_BETWEEN_EXECUTE_INTENTS);
        }
    }

    public void DeleteEnemy(Entity enemy)
    {
        // get enemy's index
        int index = enemy.GetComponent<Enemy>().GetEnemyIndex();
        // check to see if there is an enemy at this index
        if (enemy_in_place[index])
        {
            MyObject enemy_object = enemy.GetComponent<MyObject>();
            StartCoroutine(DeleteEnemyRoutine(enemy_object));
            enemy_in_place[index] = false;
            current_enemies.Remove(enemy.GetComponent<Enemy>());
        }   
    }
    // delete enemy routine animation
    private IEnumerator DeleteEnemyRoutine(MyObject enemy_object)
    {
        yield return new WaitForSeconds(HealthBar.TIME_BETWEEN_RED_ORANGE_BARS);
        enemy_object.SquishyChangeScale(11f, 0f, 0.2f, 0.2f);
        yield return new WaitForSeconds(0.4f);
        Destroy(enemy_object.gameObject);

    }
}

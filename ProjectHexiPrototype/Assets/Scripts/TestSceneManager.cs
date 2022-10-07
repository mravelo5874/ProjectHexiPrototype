using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour
{
    public static int MAX_ENEMIES = 3;

    public List<Transform> enemy_positions;
    public GameObject enemy_prefab;

    private int current_enemies = 0;

    void Start()
    {
        ResetScene();
        // allow player input
        GameManager.instance.allow_player_input = true;
    }

    public void ResetScene()
    {
        // remove all enemies
        foreach (Transform enemyPos in enemy_positions)
        {
            if (enemyPos.childCount == 1)
            {
                Destroy(enemyPos.GetChild(0).gameObject);
            }
        }
        current_enemies = 0;
        CardManager.instance.ResetAndClearPiles();
        CardManager.instance.SetMasterList(GameManager.instance.default_knight_deck);
    }

    public void DrawCard()
    {
        CardManager.instance.DrawCard();
    }

    public void AddEnergy()
    {
        
    }

    public void SpawnEnemy()
    {
        if (current_enemies < MAX_ENEMIES)
        {
            GameObject new_enemy = Instantiate(enemy_prefab, enemy_positions[current_enemies]);
            new_enemy.transform.localScale = Vector3.zero;
            new_enemy.GetComponent<MyObject>().SquishyChangeScale(11f, 10f, 0.2f, 0.2f);
            current_enemies++;
        }
    }
}

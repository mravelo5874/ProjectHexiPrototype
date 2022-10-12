using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float ENTITY_START_DELAY = 0.25f;

    private int current_health = 0;
    private int max_health = 18;
    private int current_block = 0;

    public HealthBar health_bar;
    public TextMeshProUGUI block_text;

    void Start()
    {
        // set health
        current_health = max_health;

        StartCoroutine(StartDelay());
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(ENTITY_START_DELAY);
        health_bar.ShowBar();
        UpdateEntity();
    }

    public virtual void SetMaxHealth(int new_max_health, bool update_current_health = true)
    {
        max_health = new_max_health;
        if (update_current_health)
        {
            // set current health
            current_health = max_health;
        }
        UpdateEntity();
    }

    public void Heal(int amount)
    {
        current_health += amount;
        // do not exceed max health
        if (current_health > max_health)
        {
            current_health = max_health;
        }
        UpdateEntity();
    }

    public void ApplyDamage(int amount)
    {
        // deal damage to block first
        if (current_block > 0)
        {
            current_block -= amount;
            if (current_block < 0)
            {
                amount = Mathf.Abs(current_block);
                current_block = 0;
            }
        }

        // deal damage to heath
        if (amount > 0)
        {   
            current_health -= amount;
            // death if health is 0
            if (current_health <= 0)
            {
                current_health = 0;
                Death();
            }
        }
        UpdateEntity();
    }

    public void GainBlock(int amount)
    {
        current_block += amount;
        UpdateEntity();
    }

    private void UpdateEntity()
    {
        // update UI
        health_bar.UpdateBar(current_health, max_health);
        block_text.text = "block: " + current_block;
    }

    private void Death()
    {
        EnemyManager.instance.DeleteEnemy(this);
    }
    
}

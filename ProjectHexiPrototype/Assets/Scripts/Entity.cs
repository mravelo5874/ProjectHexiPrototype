using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int current_health = 0;
    private int max_health = 18;
    private int block = 0;

    public HealthBar health_bar;

    void Start()
    {
        // set health
        current_health = max_health;
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
        
        // update entity
        UpdateEntity();
    }

    public void ApplyDamage(int amount)
    {
        // deal damage to block first
        if (block > 0)
        {
            block -= amount;
            if (block < 0)
            {
                amount = Mathf.Abs(block);
                block = 0;
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
        
        // update
        UpdateEntity();
    }

    public void GainBlock(int amount)
    {
        block += amount;
    }

    private void UpdateEntity()
    {
        health_bar.UpdateBar(current_health, max_health);
    }

    private void Death()
    {
        EnemyManager.instance.DeleteEnemy(this);
    }
    
}

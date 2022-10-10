using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int health = 0;
    private int max_health = 50;
    private int block = 0;

    public HealthBar health_bar;

    void Start()
    {
        health_bar.ShowBar();
    }

    public void Heal(int amount)
    {
        health += amount;
        // do not exceed max health
        if (health > max_health)
        {
            health = max_health;
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
            health -= amount;
            // death if health is 0
            if (health < 0)
            {
                health = 0;
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
        health_bar.UpdateBar((float)health / (float)max_health);
    }

    private void Death()
    {
        // TODO
    }
    
}

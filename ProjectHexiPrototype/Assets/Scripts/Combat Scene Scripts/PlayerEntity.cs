using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public static PlayerEntity instance;
    void Awake()
    {
        // set this player to be the only instance
        if (!instance)
        {
            instance = this;
        }
    }

    // override Entity Start() and call it
    public override void Start()
    {
        base.Start();
    }

    public bool PlayerDead()
    {
        return base.GetIsDead();
    }
}

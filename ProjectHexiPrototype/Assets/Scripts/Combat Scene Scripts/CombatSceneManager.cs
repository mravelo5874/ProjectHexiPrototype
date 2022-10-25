using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public override void Awake()
    {
        base.Awake(); // call base awake
    }

    void Start()
    {
        ResetScene();
    }

    public void ResetScene()
    {
        EnemyManager.instance.ResetEnemies();
        // start combat
        CombatManager.instance.StartCombat();
    }
}

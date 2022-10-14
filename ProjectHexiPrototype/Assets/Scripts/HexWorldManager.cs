using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexWorldManager : MonoBehaviour
{
    public static HexWorldManager instance;
    // set this hex world manager to be the only instance
    void Awake() 
    {
        if (!instance)
        {
            instance = this;
        }
    }

    public HexGrid hex_grid;

    void Start()
    {
        // create grid
        hex_grid.CreateGrid();
    }
}

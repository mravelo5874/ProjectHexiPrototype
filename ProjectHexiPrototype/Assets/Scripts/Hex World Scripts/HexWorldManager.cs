using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexWorldManager : MonoBehaviour
{
    public static int DEFAULT_WORLD_ENERGY = 30;

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
    public HexEntity player;

    [Header("Hex World UI")]
    public TextMeshProUGUI world_energy_text;
    
    // private vars
    private int max_world_energy;
    private int current_world_energy;

    void Start()
    {
        // create grid
        hex_grid.CreateGrid();
        // set player position
        player.SetStartHexCell(hex_grid.GetHexCellFromCoordinates(Vector3Int.zero));
        // set world energy
        max_world_energy = DEFAULT_WORLD_ENERGY;
        current_world_energy = DEFAULT_WORLD_ENERGY;

        UpdateUI();
    }

    public void UpdateUI()
    {
        world_energy_text.text = "world energy: " + current_world_energy + "/" + max_world_energy;

        // set hex options for current cell
        // TODO: do this!!!
    }

    // use one world energy and update UI
    public void ConsumeWorldEnergy()
    {
        current_world_energy--;
        UpdateUI();
    }
}

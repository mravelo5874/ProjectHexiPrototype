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
    public Transform hex_option_holder;
    public HexOptionPrefab hex_option_prefab;
    
    // private vars
    private int max_world_energy;
    private int current_world_energy;
    private List<HexOptionPrefab> current_hex_option_prefabs;

    void Start()
    {
        // init options list
        current_hex_option_prefabs = new List<HexOptionPrefab>();
        // create grid
        hex_grid.CreateGrid();
        // set player position
        player.SetStartHexCell(hex_grid.GetHexCellFromCoordinates(Vector3Int.zero));
        // set current hex options
        SetHexOptions(hex_grid.GetHexCellFromCoordinates(Vector3Int.zero));
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
    public void ConsumeWorldEnergy(int amount = 1)
    {
        current_world_energy -= amount;
        UpdateUI();
    }

    public void SetHexOptions(HexCell cell)
    {
        List<HexOption> hex_options = cell.GetHexOptions();
        for (int i = 0; i < hex_options.Count; i++)
        {
            HexOptionPrefab new_option_prefab = Instantiate(hex_option_prefab, hex_option_holder);
            new_option_prefab.SetHexOption(hex_options[i], i + 1);
            current_hex_option_prefabs.Add(new_option_prefab);
        }
    }

    public void ClearHexOptions()
    {
        foreach (HexOptionPrefab option in current_hex_option_prefabs)
        {
            option.Delete();
        }
        current_hex_option_prefabs.Clear();
    }
}

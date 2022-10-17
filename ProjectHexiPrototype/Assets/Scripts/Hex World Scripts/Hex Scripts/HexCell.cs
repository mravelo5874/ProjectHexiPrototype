using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexCell : MonoBehaviour
{
    void Awake()
    {
        // reset hex text
        hex_text.text = "";
        // create neighbor array
        neighbors = new HexCell[6];
    }
    
    public Color color;
    public TextMeshProUGUI hex_text;

    // private vars
    private HexCell[] neighbors;
    public HexCell GetNeighbor(HexDirection dir) { return neighbors[(int)dir]; } // public getter
    public void SetNeighbor(HexDirection dir, HexCell cell) // pubic setter
    {
        neighbors[(int)dir] = cell;
        cell.neighbors[(int)dir.Opposite()] = this;
    }

    private Vector3Int hex_coordinates;
    public Vector3Int GetHexCoordinates() { return hex_coordinates; } // public getter
    public void SetHexCoordinates(Vector3Int coords, bool set_text = false) // public setter
    {
        hex_coordinates = coords;
        if (set_text)
        {
            hex_text.text = hex_coordinates.ToString();
        }
    }

    private HexType hex_type;
    public void SetHexType(HexType type, bool set_text = false) // public setter
    {
        hex_type = type;
        if (set_text)
        {
            hex_text.text = hex_type.ToString();
        }
        SetHexOptions(type.GetHexOptions());
        // TODO: update cell visuals
    }

    private List<HexOption> hex_options;
    public List<HexOption> GetHexOptions() { return hex_options; } // public getter
    private void SetHexOptions(List<HexOption> options) // public setter
    {   
        hex_options = new List<HexOption>();
        hex_options.AddRange(options);
        // update UI
        HexWorldManager.instance.UpdateUI();
    }
}

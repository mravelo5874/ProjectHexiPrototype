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
    }

    public Color color;

    public TextMeshProUGUI hex_text;

    // private vars
    private Vector3Int hex_coordinates;
    public Vector3Int GetHexCoordinates() { return hex_coordinates; } // public getter

    public void SetHexCoordinates(Vector3Int coords, bool set_text = false) // public setter
    {
        hex_coordinates = coords;
        hex_text.text = hex_coordinates.ToString();
    }
}

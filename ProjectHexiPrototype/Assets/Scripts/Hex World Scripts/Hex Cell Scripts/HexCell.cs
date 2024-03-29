using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexCell : MonoBehaviour
{
    //// STATIC VARIABLES ////
    public static float HEX_CELL_OUTLINE_ANIMATION_DURATION = 1f;
    public static float HEX_CELL_OUTLINE_ALPHA = 0.25f;

    void Awake()
    {
        // reset hex text
        hex_text.SetTextMeshText("");
        // create neighbor array
        neighbors = new HexCell[6];

        // spawn all 6 inner points
        // Instantiate(HexWorldManager.instance.hex_point, HexMetrics.GetInnerPoint(transform.position, HexDirection.N), Quaternion.identity, transform);
        // Instantiate(HexWorldManager.instance.hex_point, HexMetrics.GetInnerPoint(transform.position, HexDirection.NE), Quaternion.identity, transform);
        // Instantiate(HexWorldManager.instance.hex_point, HexMetrics.GetInnerPoint(transform.position, HexDirection.SE), Quaternion.identity, transform);
        // Instantiate(HexWorldManager.instance.hex_point, HexMetrics.GetInnerPoint(transform.position, HexDirection.S), Quaternion.identity, transform);
        // Instantiate(HexWorldManager.instance.hex_point, HexMetrics.GetInnerPoint(transform.position, HexDirection.SW), Quaternion.identity, transform);
        // Instantiate(HexWorldManager.instance.hex_point, HexMetrics.GetInnerPoint(transform.position, HexDirection.NW), Quaternion.identity, transform);
    }
    
    public MyObject hex_text;
    public MyObject hex_outline;

    // private vars
    private bool hex_outline_active = false;

    // copy cell into this hex cell
    public void Copy(HexCell cell)
    {
        SetHexType(cell.GetHexType(), true);
        SetHexOptions(cell.GetHexOptions());
        Elevation = cell.Elevation;
        Discarded = cell.Discarded;
    }

    // hex position
    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    // hex elevation
    public int Elevation 
    {
		get 
        {
			return elevation;
		}
		set
        {
			elevation = value;
            Vector3 pos = transform.localPosition;
            pos.y = value * HexMetrics.ELEVATION_STEP;
            pos.y += (HexMetrics.SampleNoise(pos).y * 2f - 1f) * HexMetrics.PERTURB_STRENGTH_ELEVATION;
            transform.localPosition = pos;
		}
	}
	private int elevation;

    public bool Discarded
    {
        get
        {
            return discarded;
        }
        set
        {
            discarded = value;
            if (discarded)
            {
                // clear hex options
                hex_options.Clear();
                hex_text.SetTextMeshText("");
            }
        }
    }
    private bool discarded;

    private bool passable = true; // can player travel to this hex cell?
    public bool GetHexPassable() { return passable; } // public getter

    // hex layer
    private int hex_layer; // layer is distance away from center (0, 0, 0)
    public void SetHexLayer(int layer) { hex_layer = layer; } // public setter
    public int GetHexLayer() { return hex_layer; } // public layer

    // hex neighbors
    private HexCell[] neighbors;
    public List<HexCell> GetNeighbors() { return new List<HexCell>(neighbors); }
    public HexCell GetNeighbor(HexDirection dir) { return neighbors[(int)dir]; } // public getter
    public void SetNeighbor(HexDirection dir, HexCell cell) // pubic setter
    {
        neighbors[(int)dir] = cell;
        cell.neighbors[(int)dir.Opposite()] = this;
    }

    // hex coordinates
    private Vector3Int hex_coordinates;
    public Vector3Int GetHexCoordinates() { return hex_coordinates; } // public getter
    public void SetHexCoordinates(Vector3Int coords, bool set_text = false) // public setter
    {
        hex_coordinates = coords;
        if (set_text)
        {
            hex_text.SetTextMeshText(hex_coordinates.ToString());
        }
    }

    // hex type
    private HexType hex_type;
    public HexType GetHexType() { return hex_type; } // public getter
    public void SetHexType(HexType type, bool set_text = false) // public setter
    {
        hex_type = type;
        if (set_text)
        {
            hex_text.SetTextMeshText(hex_type.ToString());
        }
        SetHexOptions(type.GetHexOptions());
        // TODO: update cell visuals

        // set hex passable
        passable = true;
        if (hex_type == HexType.Mountain)
        {
            passable = false;
        }
        // set hex elevation
        Elevation = type.GetElevation();
    }

    // hex options
    private List<HexOption> hex_options;
    public List<HexOption> GetHexOptions() { return hex_options; } // public getter
    public void SetHexOptions(List<HexOption> options) // public setter
    {   
        hex_options = new List<HexOption>();
        hex_options.AddRange(options);
        // update UI
        HexWorldManager.instance.UpdateUI();
    }

    public void ShowHexOutline(Color outline_color)
    {
        if (!hex_outline_active)
        {
            hex_outline_active = true;
            hex_outline.SetImageColor(outline_color);
            hex_outline.SetImageAlpha(HEX_CELL_OUTLINE_ALPHA);
            hex_outline.SquishyChangeScale(1.1f, 1f, 0.2f, 0.2f);
        }
    }

    public void HideHexOutline()
    {
        if (hex_outline_active)
        {
            hex_outline_active = false;
            hex_outline.SquishyChangeScale(1.1f, 0f, 0.2f, 0.2f);
        }
    }
}

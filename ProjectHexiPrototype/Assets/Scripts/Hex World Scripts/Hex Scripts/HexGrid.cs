using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int grid_radius = 4;
    public HexCell hex_cell_prefab;

    [Header("Color")]
    public Color default_color = Color.white;

    // private vars
    private HexMesh hex_mesh;
    private List<HexCell> cells;
    private List<HexCell> current_Layer_cells;

    void Awake()
    {
        hex_mesh = GetComponentInChildren<HexMesh>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) 
        {
            if (hit.transform.tag == "HexCell")
            {
                TouchCell(hit.transform.GetComponentInParent<HexCell>());
            }
		}
    }

    private void TouchCell(HexCell cell)
    {
        // float r = Random.Range(0f, 1f);
        // float g = Random.Range(0f, 1f);
        // float b = Random.Range(0f, 1f);
        // cell.color = new Color(r, g, b, 1f);
		// hex_mesh.Triangulate(cells);

        HexWorldManager.instance.player.GoToHexCell(cell);
    }

    public void CreateGrid()
    {
        CreateGridRoutine();
    }
    private void CreateGridRoutine()
    {
        // init lists
        cells = new List<HexCell>();
        current_Layer_cells = new List<HexCell>();
        // create grid layer by layer
        for (int layer = 0; layer < grid_radius; layer++)
        {
            CreateLayerRoutine(current_Layer_cells);
        }
        // create mesh
        hex_mesh.Triangulate(cells);
    }

    public void CreateLayerRoutine(List<HexCell> prev_layer)
    {
        current_Layer_cells = new List<HexCell>();
        // layer should be empty if layer = 0
        if (prev_layer.Count == 0)
        {
            // create first cell at origin
            HexCell new_cell = CreateCell(0f, 0f, new Vector3Int(0, 0, 0));
            current_Layer_cells.Add(new_cell);
            cells.Add(new_cell);
        }
        // else: create cells at each neighbor of each prev cell if no cell exists
        else
        {
            foreach (HexCell cell in prev_layer)
            {
                List<HexCell> created_cells = new List<HexCell>();
                // get neighboring cell coords & positions
                List<Vector3Int> neighboring_cell_coords = HexMetrics.GetNeighborCoordinates(cell.GetHexCoordinates());
                List<Vector3> neighboring_cell_pos = HexMetrics.GetNeighborPositions(cell.transform.localPosition);

                // for each neighboring cell, make sure it does not already exist
                // if it doesn't, create cell
                // if it does, continue to next cell
                for (int i = 0; i < 6; i++)
                {
                    Vector3Int neighbor_coord = neighboring_cell_coords[i];
                    if (!CheckIfCellExists(neighbor_coord))
                    {
                        HexCell new_cell = CreateCell(neighboring_cell_pos[i].x, neighboring_cell_pos[i].z, neighbor_coord);
                        // add cell to lists
                        created_cells.Add(new_cell);
                        cells.Add(new_cell);
                    }
                }

                // add new cells to current layer list
                current_Layer_cells.AddRange(created_cells);
            }
        }
    }

    public HexCell CreateCell(float x, float z, Vector3Int coords)
    {
        // calculate pos
        Vector3 position = new Vector3(x, 0f, z);
        // instantiate and set position
        HexCell cell = Instantiate<HexCell>(hex_cell_prefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        // set cell data
        cell.SetHexCoordinates(coords);
        // determine cell hex type
        HexType type = (HexType)Random.Range((int)HexType.Plain, (int)HexType.Mountain);
        cell.SetHexType(type, true);
        cell.color = default_color;
        // set neighbors
        List<Vector3Int> neighbor_coords = HexMetrics.GetNeighborCoordinates(coords);
        for (int i = 0; i < 6; i ++)
        {
            if (CheckIfCellExists(neighbor_coords[i]))
            {
                cell.SetNeighbor((HexDirection)i, GetHexCellFromCoordinates(neighbor_coords[i]));
            }
        }
        // return cell
        return cell;
    }

    public bool CheckIfCellExists(Vector3Int coords)
    {
        foreach (HexCell cell in cells)
        {
            if (cell.GetHexCoordinates() == coords)
                return true;
        }
        return false;
    }

    public HexCell GetHexCellFromCoordinates(Vector3 coords)
    {
        foreach (HexCell cell in cells)
        {
            if (cell.GetHexCoordinates() == coords)
                return cell;
        }
        return null;
    }
}

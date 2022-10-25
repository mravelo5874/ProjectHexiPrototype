using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int grid_radius = 4;
    public HexCell hex_cell_prefab;

    public Texture2D noise_source;

    // private vars
    private HexMesh hex_mesh;
    private List<HexCell> cells;
    public List<HexCell> GetHexCells() { return cells; }
    private List<HexCell> current_layer_cells;

    void Awake()
    {
        // set hex metrics noise source
        HexMetrics.NOISE_SOURCE = noise_source;
        hex_mesh = GetComponentInChildren<HexMesh>();
    }

    void OnEnable()
    {
		// set hex metrics noise source
        HexMetrics.NOISE_SOURCE = noise_source;
	}

    void Update()
    {
        // return if player input restricted
        if (!GameManager.instance.allow_player_input)
        {
            return;
        }

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
        // only travel to other hex cell if not currently inside a hex cell AND if cell is passable
        if (!HexWorldManager.instance.GetInsideHexCell() && cell.GetHexPassable())
        {
            HexWorldManager.instance.player.GoToHexCell(cell);
        }
    }

    public void Refresh()
    {
        hex_mesh.Triangulate(cells);
    }

    public void LoadGrid(HexCell[] hex_cells, int world_radius)
    {
        // create grid at appropriate radius
        grid_radius = world_radius;
        CreateGrid();
        // load each cell
        foreach (HexCell hex_cell in hex_cells)
        {
            LoadCell(hex_cell);
        }
        // refresh mesh
        Refresh();
    }

    private void LoadCell(HexCell hex_cell_to_load)
    {
        foreach (HexCell hex_cell in cells)
        {
            if (hex_cell.GetHexCoordinates() == hex_cell_to_load.GetHexCoordinates())
            {
                hex_cell.Copy(hex_cell_to_load);
            }
        }
    }

    public void CreateGrid()
    {
        // init lists
        cells = new List<HexCell>();
        current_layer_cells = new List<HexCell>();
        // create grid layer by layer
        for (int layer = 0; layer < grid_radius; layer++)
        {
            // create layer
            CreateLayerRoutine(current_layer_cells, layer);
            // add special hex types
            AddSpecialHexTypes(current_layer_cells, layer);
        }
        // create outer layer of in-passable cells
        CreateLayerRoutine(current_layer_cells, grid_radius, true);
        // refresh mesh
       Refresh();
    }

    private void AddSpecialHexTypes(List<HexCell> layer_cells, int layer_index)
    {
        List<HexCell> current_layer_hex_cells = new List<HexCell>();
        current_layer_hex_cells.AddRange(layer_cells);
        List<HexType> special_hex_types = AISystem.instance.GetSpecialHexTypes(layer_index);
        // return if list is null
        if (special_hex_types == null)
        {
            return;
        }
        // set hex types
        foreach (HexType special_type in special_hex_types)
        {
            // select random cell
            HexCell hex_cell = current_layer_hex_cells[Random.Range(0, current_layer_hex_cells.Count)];
            // get cell's neighbors
            List<HexCell> neighbor_cells = new List<HexCell>();
            neighbor_cells.AddRange(hex_cell.GetNeighbors());
            // remove hex cell and neighbors from layer list
            current_layer_hex_cells.Remove(hex_cell);
            foreach (HexCell cell in neighbor_cells)
            {
                current_layer_hex_cells.Remove(cell);
            }
            // set special hex cell
            hex_cell.SetHexType(special_type, true);
        }
    }

    public void CreateLayerRoutine(List<HexCell> prev_layer, int layer_index, bool outer_layer = false)
    {
        current_layer_cells = new List<HexCell>();
        // layer should be empty if layer = 0
        if (prev_layer.Count == 0)
        {
            // create first cell at origin
            HexCell new_cell = CreateCell(0f, 0f, new Vector3Int(0, 0, 0), 0);
            new_cell.SetHexType(HexType.Init, true);
            current_layer_cells.Add(new_cell);
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
                        HexCell new_cell = CreateCell(neighboring_cell_pos[i].x, neighboring_cell_pos[i].z, neighbor_coord, layer_index);
                        // add cell to lists
                        created_cells.Add(new_cell);
                        cells.Add(new_cell);
                    }
                }

                // add new cells to current layer list
                current_layer_cells.AddRange(created_cells);
            }
        }
    }

    public HexCell CreateCell(float x, float z, Vector3Int coords, int layer)
    {
        // calculate pos
        Vector3 position = new Vector3(x, 0f, z);
        // instantiate and set position
        HexCell cell = Instantiate<HexCell>(hex_cell_prefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        // set cell data
        cell.SetHexCoordinates(coords);
        cell.SetHexLayer(layer);
        // determine cell hex type
        HexType type = HexType.None;
        if (layer == grid_radius)
        {
            type = HexType.Mountain;
        }
        else
        {
            type = AISystem.instance.DetermineHexType(layer);
        }
        cell.SetHexType(type, true);
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

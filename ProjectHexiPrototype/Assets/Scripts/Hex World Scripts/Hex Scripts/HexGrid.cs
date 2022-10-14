using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int grid_radius = 4;
    public HexCell hex_cell_prefab;

    [Header("Debug")]
    public bool time_between_cell_spawns;
    public float hex_spawn_delay = 0.5f;

    // private vars
    private List<HexCell> cells;
    private List<HexCell> current_Layer_cells;

    public void CreateGrid()
    {
        if (!time_between_cell_spawns)
        {
            hex_spawn_delay = 0f;
        }
        StartCoroutine(CreateGridRoutine());
    }
    private IEnumerator CreateGridRoutine()
    {
        // init lists
        cells = new List<HexCell>();
        current_Layer_cells = new List<HexCell>();
        // create grid layer by layer
        for (int layer = 0; layer < grid_radius; layer++)
        {
            yield return StartCoroutine(CreateLayerRoutine(current_Layer_cells));
        }
    }

    public IEnumerator CreateLayerRoutine(List<HexCell> prev_layer)
    {
        current_Layer_cells = new List<HexCell>();
        // layer should be empty if layer = 0
        if (prev_layer.Count == 0)
        {
            // create first cell at origin
            HexCell new_cell = CreateCell(0f, 0f, new Vector3Int(0, 0, 0));
            yield return new WaitForSeconds(hex_spawn_delay);
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
                        yield return new WaitForSeconds(hex_spawn_delay);
                        created_cells.Add(new_cell);
                    }
                }

                // add new cells to list
                cells.AddRange(created_cells);
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
        // set cell coordinates
        cell.SetHexCoordinates(coords, true);
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
}

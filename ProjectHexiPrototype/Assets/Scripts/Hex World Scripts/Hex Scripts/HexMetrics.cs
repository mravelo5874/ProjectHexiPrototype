using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float OUTER_RADIUS = 10f;
    public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f; // ~ e * (sqrt(3) / 2)
    public const float OUTER_RADIUS_MULT = 1.5f;
    public const float INNER_RADIUS_MULT = 2f;

    public static Vector3[] hex_corners = 
    {
        new Vector3(0f, 0f, OUTER_RADIUS),
		new Vector3(OUTER_RADIUS, 0f, 0.5f * OUTER_RADIUS),
		new Vector3(OUTER_RADIUS, 0f, -0.5f * OUTER_RADIUS),
		new Vector3(0f, 0f, -OUTER_RADIUS),
		new Vector3(-OUTER_RADIUS, 0f, -0.5f * OUTER_RADIUS),
		new Vector3(-OUTER_RADIUS, 0f, 0.5f * OUTER_RADIUS)
    };

    // ordered in clock-wise order starting from top-most cell
    public static Vector3[] hex_neighbors =
    {
        new Vector3(0f, INNER_RADIUS * INNER_RADIUS_MULT, 0f),
        new Vector3(INNER_RADIUS, OUTER_RADIUS * OUTER_RADIUS_MULT, 0f),
        new Vector3(-INNER_RADIUS, OUTER_RADIUS * OUTER_RADIUS_MULT, 0f),
        new Vector3(0f, -INNER_RADIUS * INNER_RADIUS_MULT, 0f),
        new Vector3(-INNER_RADIUS, -OUTER_RADIUS * OUTER_RADIUS_MULT, 0f),
        new Vector3(INNER_RADIUS, -OUTER_RADIUS * OUTER_RADIUS_MULT, 0f)
    };

    // ordered in clock-wise order starting from top-most cell
    public static List<Vector3> GetNeighborPositions(Vector3 cell_pos)
    {
        List<Vector3> neighbor_pos = new List<Vector3>();
        neighbor_pos.Add(new Vector3(cell_pos.x, cell_pos.y, cell_pos.z + (INNER_RADIUS * INNER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x + INNER_RADIUS, cell_pos.y, cell_pos.z + (OUTER_RADIUS * OUTER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x - INNER_RADIUS, cell_pos.y, cell_pos.z + (OUTER_RADIUS * OUTER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x, cell_pos.y, cell_pos.z - (INNER_RADIUS * INNER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x - INNER_RADIUS, cell_pos.y, cell_pos.z - (OUTER_RADIUS * OUTER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x + INNER_RADIUS, cell_pos.y, cell_pos.z - (OUTER_RADIUS * OUTER_RADIUS_MULT)));
        return  neighbor_pos;
    }

    // ordered in clock-wise order starting from top-most cell
    public static List<Vector3Int> GetNeighborCoordinates(Vector3Int cell_coords)
    {
        List<Vector3Int> neighbor_coords = new List<Vector3Int>();
        neighbor_coords.Add(new Vector3Int(cell_coords.x, cell_coords.y + 1, cell_coords.z));
        neighbor_coords.Add(new Vector3Int(cell_coords.x, cell_coords.y, cell_coords.z + 1));
        neighbor_coords.Add(new Vector3Int(cell_coords.x + 1, cell_coords.y, cell_coords.z));
        neighbor_coords.Add(new Vector3Int(cell_coords.x, cell_coords.y - 1, cell_coords.z));
        neighbor_coords.Add(new Vector3Int(cell_coords.x, cell_coords.y, cell_coords.z - 1));
        neighbor_coords.Add(new Vector3Int(cell_coords.x - 1, cell_coords.y, cell_coords.z));
        return neighbor_coords;
    }
}

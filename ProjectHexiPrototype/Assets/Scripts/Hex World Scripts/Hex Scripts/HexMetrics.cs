using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float OUTER_RADIUS = 10f;
    public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f; // ~ e * (sqrt(3) / 2)
    public const float INNER_RADIUS_MULT = 2f;
    public const float OUTER_RADIUS_MULT = 1.5f;

    public const float SOLID_FACTOR = 0.75f;
    public const float BLEND_FACTOR = 1f - SOLID_FACTOR;

    public static Vector3[] hex_corners = 
    {
        new Vector3(-OUTER_RADIUS * 0.5f, 0f, INNER_RADIUS),
		new Vector3(OUTER_RADIUS * 0.5f, 0f, INNER_RADIUS),
		new Vector3(OUTER_RADIUS, 0f, 0f),
        new Vector3(OUTER_RADIUS * 0.5f, 0f, -INNER_RADIUS),
        new Vector3(-OUTER_RADIUS * 0.5f, 0f, -INNER_RADIUS),
        new Vector3(-OUTER_RADIUS, 0f, 0f),
        new Vector3(-OUTER_RADIUS * 0.5f, 0f, INNER_RADIUS)
    };

    public static Vector3 GetFirstCorner(HexDirection dir)
    {
        return hex_corners[(int)dir];
    }

    public static Vector3 GetSecondCorner(HexDirection dir)
    {
        return hex_corners[(int)dir + 1];
    }

    // ordered in clock-wise order starting from HexDirection.N cell
    public static List<Vector3> GetNeighborPositions(Vector3 cell_pos)
    {
        List<Vector3> neighbor_pos = new List<Vector3>();
        neighbor_pos.Add(new Vector3(cell_pos.x, cell_pos.y, cell_pos.z + (INNER_RADIUS * INNER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x + (OUTER_RADIUS * OUTER_RADIUS_MULT), cell_pos.y, cell_pos.z + INNER_RADIUS));
        neighbor_pos.Add(new Vector3(cell_pos.x + (OUTER_RADIUS * OUTER_RADIUS_MULT), cell_pos.y, cell_pos.z - INNER_RADIUS));
        neighbor_pos.Add(new Vector3(cell_pos.x, cell_pos.y, cell_pos.z - (INNER_RADIUS * INNER_RADIUS_MULT)));
        neighbor_pos.Add(new Vector3(cell_pos.x - (OUTER_RADIUS * OUTER_RADIUS_MULT), cell_pos.y, cell_pos.z - INNER_RADIUS));
        neighbor_pos.Add(new Vector3(cell_pos.x - (OUTER_RADIUS * OUTER_RADIUS_MULT), cell_pos.y, cell_pos.z + INNER_RADIUS));
        return  neighbor_pos;
    }

    // ordered in clock-wise order starting from top-most cell
    public static List<Vector3Int> GetNeighborCoordinates(Vector3Int cell_coords)
    {
        List<Vector3Int> neighbor_coords = new List<Vector3Int>();
        neighbor_coords.Add(new Vector3Int(cell_coords.x, cell_coords.y - 1, cell_coords.z - 1));
        neighbor_coords.Add(new Vector3Int(cell_coords.x + 1, cell_coords.y, cell_coords.z - 1));
        neighbor_coords.Add(new Vector3Int(cell_coords.x + 1, cell_coords.y + 1, cell_coords.z));
        neighbor_coords.Add(new Vector3Int(cell_coords.x, cell_coords.y + 1, cell_coords.z + 1));
        neighbor_coords.Add(new Vector3Int(cell_coords.x - 1, cell_coords.y, cell_coords.z + 1));
        neighbor_coords.Add(new Vector3Int(cell_coords.x - 1, cell_coords.y - 1, cell_coords.z));
        return neighbor_coords;
    }

    public static Vector3 GetFirstSolidCorner(HexDirection dir)
    {
        return hex_corners[(int)dir] * SOLID_FACTOR;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection dir)
    {
        return hex_corners[(int)dir + 1] * SOLID_FACTOR;
    }

    public static Vector3 GetBridge(HexDirection dir)
    {
        return (hex_corners[(int)dir] + hex_corners[(int)dir + 1]) * BLEND_FACTOR;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float SQRT_3 = 1.73205080757f;

    public const float OUTER_RADIUS = 10f;
    public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f; // ~ e * (sqrt(3) / 2)
    public const float INNER_RADIUS_MULT = 2f;
    public const float OUTER_RADIUS_MULT = 1.5f;

    public const float SOLID_FACTOR = 0.8f;
    public const float BLEND_FACTOR = 1f - SOLID_FACTOR;

    public const float ELEVATION_STEP = 2f;
    public const float MOUNTAIN_HEIGHT = 5f;

    public static Texture2D NOISE_SOURCE;
    public const float NOISE_SCALE = 0.003f;
    public const float PERTURB_STRENGTH = 4f;
    public const float PERTURB_STRENGTH_ELEVATION = 1.5f;

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

    public static Vector3[] hex_inner_points =
    {
        new Vector3(0f, 0f, OUTER_RADIUS / SQRT_3),
        new Vector3(OUTER_RADIUS * 0.5f, 0f, OUTER_RADIUS * 0.5f / SQRT_3),
        new Vector3(OUTER_RADIUS * 0.5f, 0f, -OUTER_RADIUS * 0.5f / SQRT_3),
        new Vector3(0f, 0f, -OUTER_RADIUS / SQRT_3),
        new Vector3(-OUTER_RADIUS * 0.5f, 0f, -OUTER_RADIUS * 0.5f / SQRT_3),
        new Vector3(-OUTER_RADIUS * 0.5f, 0f, OUTER_RADIUS * 0.5f / SQRT_3),
    };

    public static Vector3 GetInnerPoint(Vector3 center, HexDirection dir)
    {
        return center + hex_inner_points[(int)dir];
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

    public static Vector3 GetFirstCorner(HexDirection dir)
    {
        return hex_corners[(int)dir];
    }

    public static Vector3 GetSecondCorner(HexDirection dir)
    {
        return hex_corners[(int)dir + 1];
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

    public static HexDirection GetDirectionBetweenHexCells(HexCell from, HexCell to)
    {
        List<Vector3Int> neighbor_coords = GetNeighborCoordinates(from.GetHexCoordinates());
        // check if "to" cell is a neighbor coord starting from HexDirection.N
        for (int i = 0; i < 6; i++)
        {
            if (neighbor_coords[i] == to.GetHexCoordinates())
            {
                return (HexDirection)i;
            }
        }
        // default return value incase hex cells are not neighbors
        return HexDirection.N;
    }

    public static Vector4 SampleNoise(Vector3 position)
    {
        return NOISE_SOURCE.GetPixelBilinear(
            position.x * NOISE_SCALE, 
            position.z * NOISE_SCALE
        );
    }
}

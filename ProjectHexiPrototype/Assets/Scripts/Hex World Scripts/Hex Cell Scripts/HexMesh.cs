using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    static Color color1 = new Color(1f, 0f, 0f);
    static Color color2 = new Color(0f, 1f, 0f);
    static Color color3 = new Color(0f, 0f, 1f);

    public bool use_terrain_types;

    private Mesh hex_mesh;
    private List<Vector3> vertices, terrain_types;
    private List<int> triangles;
    private List<Color> colors;

    void Awake()
    {
        GetComponent<MeshFilter>().mesh = hex_mesh = new Mesh();
        hex_mesh.name = "Hex Mesh";
        // init lists
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();

        if (use_terrain_types)
        {
            terrain_types = new List<Vector3>();
        }
    }

    private Vector3 Perturb(Vector3 position)
    {
        Vector4 sample = HexMetrics.SampleNoise(position);
        position.x += (sample.x * 2f - 1f) * HexMetrics.PERTURB_STRENGTH;
        //position.y += (sample.y * 2f - 1f) * HexMetrics.PERTURB_STRENGTH;
        position.z += (sample.z * 2f - 1f) * HexMetrics.PERTURB_STRENGTH;
        return position;
    }

    public void Triangulate(List<HexCell> cells)
    {
        hex_mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        terrain_types.Clear();

        // triangulate each cell
        for (int i = 0; i < cells.Count; i++)
        {
            TriangulateCell(cells[i]);
        }

        // create mesh
        hex_mesh.vertices = vertices.ToArray();
        if (use_terrain_types)
        {
            hex_mesh.SetUVs(2, terrain_types);
        }
        hex_mesh.triangles = triangles.ToArray();
        hex_mesh.colors = colors.ToArray();
        hex_mesh.RecalculateNormals();
    }

    private void TriangulateCell(HexCell cell)
    {
        for (HexDirection dir = HexDirection.N; dir <= HexDirection.NW; dir++)
        {
            TriangulateCell(dir, cell);
        }
    }

    private void TriangulateCell(HexDirection dir, HexCell cell)
    {
        Vector3 center = cell.Position;
        EdgeVertices e = new EdgeVertices (
            center + HexMetrics.GetFirstSolidCorner(dir),
            center + HexMetrics.GetSecondSolidCorner(dir)
        );

        TriangulateEdgeFan(center, e, cell.GetHexType());
        if (dir <= HexDirection.SE)
        {
            TriangulateConnection(dir, cell, e);
        }
    }

    private void TriangulateConnection(HexDirection dir, HexCell cell, EdgeVertices e1)
    {
        HexCell neighbor = cell.GetNeighbor(dir);
        if (neighbor == null)
        {
            return;
        }

        // create bridge between hex cells
        Vector3 bridge = HexMetrics.GetBridge(dir);
        bridge.y = neighbor.Position.y - cell.Position.y;
		EdgeVertices e2 = new EdgeVertices
        (
			e1.v1 + bridge,
			e1.v4 + bridge
		);

        TriangulateEdgeStrip(
            e1, color1, cell.GetHexType().GetTerrainIndex(), 
            e2, color2, neighbor.GetHexType().GetTerrainIndex()
        );

        // create triangles 
        HexCell nextNeighbor = cell.GetNeighbor(dir.Next());
		if (dir <= HexDirection.NE && nextNeighbor != null) 
        {
            Vector3 v5 = e1.v4 + HexMetrics.GetBridge(dir.Next());
            v5.y = nextNeighbor.Position.y;
			AddTriangle(e1.v4, e2.v4, v5);
			AddTriangleColor(color1, color2, color3);
            AddTriangleTerrainTypes(new Vector3(
                cell.GetHexType().GetTerrainIndex(), 
                neighbor.GetHexType().GetTerrainIndex(), 
                nextNeighbor.GetHexType().GetTerrainIndex()
            ));
		}
    }

    private void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, HexType hex_type)
    {
        if (hex_type == HexType.Mountain)
        {
            center.y += HexMetrics.MOUNTAIN_HEIGHT;
        }
        AddTriangle(center, edge.v1, edge.v2);
        AddTriangle(center, edge.v2, edge.v3);
        AddTriangle(center, edge.v3, edge.v4);

        AddTriangleColor(color1);
        AddTriangleColor(color1);
        AddTriangleColor(color1);

        Vector3 types = new Vector3(hex_type.GetTerrainIndex(), hex_type.GetTerrainIndex(), hex_type.GetTerrainIndex());
        AddTriangleTerrainTypes(types);
        AddTriangleTerrainTypes(types);
        AddTriangleTerrainTypes(types);
    }

    private void TriangulateEdgeStrip(EdgeVertices e1, Color c1, float type1, EdgeVertices e2, Color c2, float type2)
    {
        AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
        AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
		AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
		
        AddQuadColor(c1, c2);
        AddQuadColor(c1, c2);
        AddQuadColor(c1, c2);

        Vector3 types = new Vector3(type1, type2, type1);
        AddQuadTerrainTypes(types);
        AddQuadTerrainTypes(types);
        AddQuadTerrainTypes(types);
    }

    //// TRIANGLE ////

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertext_index = vertices.Count;
        vertices.Add(Perturb(v1));
        vertices.Add(Perturb(v2));
        vertices.Add(Perturb(v3));
        triangles.Add(vertext_index);
        triangles.Add(vertext_index + 1);
        triangles.Add(vertext_index + 2);
    }

    void AddTriangleColor(Color color)
    {
        colors.Add(color);
		colors.Add(color);
		colors.Add(color);
    }

    void AddTriangleColor(Color c1, Color c2, Color c3) 
    {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
	}

    void AddTriangleTerrainTypes(Vector3 types)
    {
        terrain_types.Add(types);
        terrain_types.Add(types);
        terrain_types.Add(types);
    }

    //// QUAD ////

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) 
    {
		int vertexIndex = vertices.Count;
		vertices.Add(Perturb(v1));
		vertices.Add(Perturb(v2));
		vertices.Add(Perturb(v3));
		vertices.Add(Perturb(v4));
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	void AddQuadColor(Color c1, Color c2) 
    {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}

    void AddQuadTerrainTypes(Vector3 types)
    {
        terrain_types.Add(types);
        terrain_types.Add(types);
        terrain_types.Add(types);
        terrain_types.Add(types);
    }
}

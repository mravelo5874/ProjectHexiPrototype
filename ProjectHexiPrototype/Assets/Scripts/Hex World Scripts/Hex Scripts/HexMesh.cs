using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh hex_mesh;
    private List<Vector3> vertices;
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
    }

    public void Triangulate(List<HexCell> cells)
    {
        hex_mesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        // triangulate each cell
        for (int i = 0; i < cells.Count; i++)
        {
            TriangulateCell(cells[i]);
        }
        // create mesh
        hex_mesh.vertices = vertices.ToArray();
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
        Vector3 center = cell.transform.localPosition;
        Vector3 v1 = center + HexMetrics.GetFirstSolidCorner(dir);
        Vector3 v2 = center + HexMetrics.GetSecondSolidCorner(dir);
        AddTriangle(center, v1, v2);
        AddTriangleColor(cell.color, cell.color, cell.color);

        if (dir <= HexDirection.SE)
        {
            TriangulateConnection(dir, cell, v1, v2);
        }
    }

    private void TriangulateConnection(HexDirection dir, HexCell cell, Vector3 v1, Vector3 v2)
    {
        HexCell neighbor = cell.GetNeighbor(dir);
        if (neighbor == null)
        {
            return;
        }

        // create bridge between hex cells
        Vector3 bridge = HexMetrics.GetBridge(dir);
        Vector3 v3 = v1 + bridge;
        Vector3 v4 = v2 + bridge;
        AddQuad(v1, v2, v3, v4);
        AddQuadColor(cell.color, neighbor.color);

        // create triangles 
        HexCell nextNeighbor = cell.GetNeighbor(dir.Next());
		if (dir <= HexDirection.NE && nextNeighbor != null) 
        {
			AddTriangle(v2, v4, v2 + HexMetrics.GetBridge(dir.Next()));
			AddTriangleColor(cell.color, neighbor.color, nextNeighbor.color);
		}
    }

    //// TRIANGLE ////

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertext_index = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertext_index);
        triangles.Add(vertext_index + 1);
        triangles.Add(vertext_index + 2);
    }

    void AddTriangleColor(Color c1, Color c2, Color c3) 
    {
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c3);
	}

    //// QUAD ////

    void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) 
    {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		vertices.Add(v4);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	void AddQuadColor (Color c1, Color c2) 
    {
		colors.Add(c1);
		colors.Add(c1);
		colors.Add(c2);
		colors.Add(c2);
	}
}

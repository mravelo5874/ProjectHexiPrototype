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
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center,
                        center + HexMetrics.hex_corners[i], 
                        center + HexMetrics.hex_corners[i + 1]);
            // add color for triangles
            AddTriangleColor(cell.color);
        }
        
    }

    void AddTriangleColor(Color color) 
    {
		colors.Add(color);
		colors.Add(color);
		colors.Add(color);
	}

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
}

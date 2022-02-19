/**
 * File Name: ProceduralMesh.cs
 * Description: Script that handles the functionality of building / triangulating a procedural mesh
 * 
 * Authors: Will Lacey
 * Date Created: January 15, 2022
 * 
 * Additional Comments: 
 *      File Line Length: 120
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    /************************************************************/
    #region Class Enums

    private enum MeshShape { Procedural, Triangle, Quad, Cube, Tetrahedron, Sphere }

    #endregion
    /************************************************************/
    #region Fields

    [Header("Settings")]
    [SerializeField] MeshShape meshShape;

    #endregion
    /************************************************************/
    #region Properties

    public Mesh Mesh { get; set; }
    public MeshFilter MeshFilter => GetComponent<MeshFilter>();
    public MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();
    public MeshCollider MeshCollider => GetComponent<MeshCollider>();
    
    public List<Vector3> Vertices { get; set; } = new List<Vector3>();
    public List<int> Triangles { get; set; } = new List<int>();
    public List<Vector3> UVs { get; set; } = new List<Vector3>();
    // public List<Vector3> Normals { get; set; } = new List<Vector3>();
    // public List<Vector3> Tangents { get; set; } = new List<Vector3>();

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Awake()
    {
        Mesh = new Mesh();
        MeshFilter.mesh = Mesh;

        if (meshShape == MeshShape.Procedural) return;
        
        TriangulateMeshShape();
    }

    private void OnDestroy()
    {
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshCollider>());
    }

    #endregion

    #region Mesh Functions

    public void Clear()
    {
        Mesh.Clear();
        Vertices.Clear();
        Triangles.Clear();
    }

    public void Apply(bool hasMeshCollider = false)
    {
        Mesh.SetVertices(Vertices);
        Mesh.SetTriangles(Triangles, 0);

        if (UVs.Count != 0) Mesh.SetUVs(0, UVs);

        Mesh.RecalculateNormals();
        Mesh.RecalculateTangents();
        // if (Normals.Count != 0) Mesh.SetNormals(Normals);
        // if (Tangents.Count != 0) Mesh.SetTangents(Tangents);

        if (hasMeshCollider) MeshCollider.sharedMesh = Mesh;
    }

    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, bool isDoubleSided = false)
    {
        int vertexIndex = Vertices.Count;

        Vertices.Add(v1);
        Vertices.Add(v2);
        Vertices.Add(v3);

        Triangles.Add(vertexIndex);
        Triangles.Add(vertexIndex + 1);
        Triangles.Add(vertexIndex + 2);

        if (isDoubleSided) AddTriangle(v3, v2, v1);
    }

    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, bool isDoubleSided = false)
    {
        int vertexIndex = Vertices.Count;

        Vertices.Add(v1);
        Vertices.Add(v2);
        Vertices.Add(v3);
        Vertices.Add(v4);

        Triangles.Add(vertexIndex);
        Triangles.Add(vertexIndex + 1);
        Triangles.Add(vertexIndex + 2);
        Triangles.Add(vertexIndex + 2);
        Triangles.Add(vertexIndex + 3);
        Triangles.Add(vertexIndex);

        if (isDoubleSided) AddQuad(v4, v3, v2, v1);
    }

    public void AddTriangleUV (Vector2 uv1, Vector2 uv2, Vector3 uv3, bool isDoubleSided = false) 
    {
		UVs.Add(uv1);
		UVs.Add(uv2);
		UVs.Add(uv3);

        if (isDoubleSided) AddTriangleUV(uv3, uv2, uv1);
	}

    public void AddQuadUV (Vector2 uv1, Vector2 uv2, Vector3 uv3, Vector3 uv4, bool isDoubleSided = false) 
    {
		UVs.Add(uv1);
		UVs.Add(uv2);
		UVs.Add(uv3);
		UVs.Add(uv4);

        if (isDoubleSided) AddQuadUV(uv4, uv3, uv2, uv1);
	}

    #endregion

    #region Other Functions

    private void TriangulateMeshShape()
    {
        switch (meshShape)
        {
            case MeshShape.Triangle:
                this.TriangulateTriangle(5);
                break;
            case MeshShape.Quad:
                this.TriangulateQuad(5);
                break;
            case MeshShape.Cube:
                this.TriangulateCube(5);
                break;
            case MeshShape.Tetrahedron:
                this.TriangulateTetrahedron(5);
                break;
            case MeshShape.Sphere:
                this.TriangulateSphere(5);
                break;
        }
    }

    #endregion

    #endregion
    /************************************************************/
    #region Debug

    private void OnDrawGizmos()
    {
        float shade = 0;
        for (int i = 0; i < Vertices.Count; i++) 
        {
            shade = (float) i / Vertices.Count;
            Gizmos.color = new Color(shade, shade, shade);
            Gizmos.DrawSphere(transform.TransformPoint(Vertices[i]), 0.1f);
        }
    }

    #endregion
    /************************************************************/
}
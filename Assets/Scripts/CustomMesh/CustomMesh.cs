/**
 * File Name: CustomMesh.cs
 * Description: Script that handles the functionality of building / triangulating a custom mesh
 * 
 * Authors: Will Lacey
 * Date Created: January 15, 2022
 * 
 * Additional Comments: 
 *      File Line Length: 120
 *      
 *      This script has also been created in Project-Fort; although it has been adapted to better fit this package
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class CustomMesh : MonoBehaviour
{
    /************************************************************/
    #region Class Enums

    private enum MeshShape { Custom, Triangle, Quad, Cube, Tetrahedron, Sphere }

    #endregion
    /************************************************************/
    #region Fields

    [SerializeField] MeshShape meshShape;

    #endregion
    /************************************************************/
    #region Properties

    public Mesh Mesh { get; set; }
    
    public List<Vector3> Vertices { get; set; } = new List<Vector3>();

    public List<int> Triangles { get; set; } = new List<int>();

    public MeshFilter MeshFilter => GetComponent<MeshFilter>();

    public MeshRenderer MeshRenderer => GetComponent<MeshRenderer>();

    public MeshCollider MeshCollider => GetComponent<MeshCollider>();

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Awake()
    {
        Mesh = new Mesh();
        MeshFilter.mesh = Mesh;

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

    public void Apply(bool hasCollider = false)
    {
        Mesh.SetVertices(Vertices);
        Mesh.SetTriangles(Triangles, 0);
        Mesh.RecalculateNormals();

        if (hasCollider) MeshCollider.sharedMesh = Mesh;
    }

    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, int subdivides = 0, bool isDoubleSided = false)
    {
        int currentVertexIndex = Vertices.Count;

        Vertices.Add(v1);
        Vertices.Add(v2);
        Vertices.Add(v3);

        Triangles.Add(currentVertexIndex);
        Triangles.Add(currentVertexIndex + 1);
        Triangles.Add(currentVertexIndex + 2);

        if (isDoubleSided) AddTriangle(v3, v2, v1);
    }

    public void Test(Vector3 v1, Vector3 v2, Vector3 v3, int subdivides)
    {
        int currentVertexIndex = Vertices.Count;;

        // add vertices
        int vertexCount = GetVertexCountAfterNumberOfSubdivides(subdivides);
        for (int v = 0; v < vertexCount; v++)
        {
            Vertices.Add(GetVertexFromSubdividedTriangle(v1, v2, v3, subdivides, v));
        }

        // add triangles
        int triangleCount = GetTriangleCountAfterNumberOfSubdivides(subdivides);
        for (int t = 0; t < triangleCount; t++)
        {
            // Triangles.Add(currentVertexIndex + t);
            // Triangles.Add(currentVertexIndex + t + 1);
            // Triangles.Add(currentVertexIndex + t + 2);
        }

        // int triangleLayer = GetTriangleLayerCountAfterNumberOfSubdivides(subdivides);
    }

    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, int subdivides = 0, bool isDoubleSided = false)
    {
        int currentVertexIndex = Vertices.Count;

        Vertices.Add(v1);
        Vertices.Add(v2);
        Vertices.Add(v3);
        Vertices.Add(v4);

        Triangles.Add(currentVertexIndex);
        Triangles.Add(currentVertexIndex + 1);
        Triangles.Add(currentVertexIndex + 2);
        Triangles.Add(currentVertexIndex + 2);
        Triangles.Add(currentVertexIndex + 3);
        Triangles.Add(currentVertexIndex);

        if (isDoubleSided) AddQuad(v4, v3, v2, v1);
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

    private Vector3 GetVertexFromSubdividedTriangle(Vector3 v1, Vector3 v2, Vector3 v3, int subdivides, int vertexIndex)
    {
        return new Vector3();
    }

    private int GetTriangleCountAfterNumberOfSubdivides(int subdivides)
    {
        if (subdivides < 0) return 0;
        return (int) Mathf.Pow(4, subdivides);
    }

    private int GetVertexCountAfterNumberOfSubdivides(int subdivides)
    {
        if (subdivides < 0) return 0;
        return (int) ((Mathf.Pow(2, subdivides) + 1) * (Mathf.Pow(2, subdivides) + 2) / 2);
    }

    private int GetTriangleLayerCountAfterNumberOfSubdivides(int subdivides)
    {
        if (subdivides < 0) return 0;
        return (int) Mathf.Pow(2, subdivides) + 1;
    }

    #endregion

    #endregion
    /************************************************************/
}
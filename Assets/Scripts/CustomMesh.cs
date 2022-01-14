/**
 * File Name: CustomMesh.cs
 * Description: Script that handles the functionality of building a custom mesh
 * 
 * Authors: Will Lacey
 * Date Created: November 9, 2021
 * 
 * Additional Comments: 
 *      File Line Length: 120
 *      
 *      This script has also been created in Project-Fort; although it has been adapted to better fit this project; in
 *      the future, it might be worth it to extract this class and associated classes into their own package
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomMesh : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Header("Settings")]
    [Tooltip("Mesh shape type for the MeshHandler to triangulate or handle.")]
    [SerializeField] MeshType meshType;

    List<Vector3> vertices;
    List<int> triangles;

    #endregion
    /************************************************************/
    #region Properties

    public Mesh mesh { get; set; }

    private MeshFilter meshFilter => GetComponent<MeshFilter>();

    private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();

    public Material material
    {
        get => meshRenderer.sharedMaterial;
        set => meshRenderer.sharedMaterial = value;
    }

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Awake()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<MeshRenderer>());
    }

    #endregion

    #region Instantiation Functions

    public static CustomMesh Instantiate(string name, Transform parent)
    {
        // initialize GameObject
        GameObject obj = new GameObject(name);
        obj.transform.parent = parent;
        obj.transform.localPosition = new Vector3();
        obj.transform.localRotation = new Quaternion();

        // initialize CustomMesh
        CustomMesh customMesh = obj.AddComponent<CustomMesh>();
        customMesh.Initialize();

        return customMesh;
    }

    private void Initialize()
    {
        mesh = new Mesh();
        vertices = new List<Vector3>();
        triangles = new List<int>();
        meshFilter.mesh = mesh;

        switch (meshType)
        {
            case MeshType.Triangle:
                mesh.name = "Custom Triangle";
                TriangulateTriangle();
                break;

            case MeshType.Quad:
                mesh.name = "Custom Quad";
                TriangulateQuad();
                break;

            case MeshType.Cube:
                mesh.name = "Custom Cube";
                TriangulateCube(1);
                break;
        }
    }

    #endregion

    #region Mesh Functions

    public void Clear()
    {
        mesh.Clear();
        vertices.Clear();
        triangles.Clear();
    }

    public void Apply()
    {
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
    }

    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, bool doubleSided = false)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);

        if (doubleSided) AddTriangle(v3, v2, v1);
    }

    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, bool doubleSided = false)
    {
        int vertexIndex = vertices.Count;

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);

        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
        triangles.Add(vertexIndex);

        if (doubleSided) AddQuad(v4, v3, v2, v1);
    }

    #endregion

    #region Shape Triangulation Functions

    public void TriangulateTriangle()
    {
        Clear();
        Vector3 v1 = new Vector3(0, 0, 0);
        Vector3 v2 = new Vector3(0, 0, 1);
        Vector3 v3 = new Vector3(1, 0, 0);
        AddTriangle(v1, v2, v3);
        Apply();
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    public void TriangulateQuad()
    {
        Clear();
        Vector3 v1 = new Vector3(0, 0, 0);
        Vector3 v2 = new Vector3(0, 0, 1);
        Vector3 v3 = new Vector3(1, 0, 1);
        Vector3 v4 = new Vector3(1, 0, 0);
        AddQuad(v1, v2, v3, v4);
        Apply();
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    public void TriangulateCube(float length)
    {
        Vector3 center = new Vector3();

        Vector3 v1 = new Vector3
        {
            x = center.x + 0.5f * length,
            y = center.y + 0.5f * length,
            z = center.z + 0.5f * length
        };

        Vector3 v2 = v1;
        v2.x -= length;

        Vector3 v3 = v2;
        v3.y -= length;

        Vector3 v4 = v3;
        v4.x += length;

        Vector3 v5 = v1;
        v5.z -= length;

        Vector3 v6 = v2;
        v6.z -= length;

        Vector3 v7 = v3;
        v7.z -= length;

        Vector3 v8 = v4;
        v8.z -= length;

        Clear();

        // sides
        AddQuad(v1, v2, v3, v4);
        AddQuad(v2, v6, v7, v3);
        AddQuad(v6, v5, v8, v7);
        AddQuad(v5, v1, v4, v8);
        // bases
        AddQuad(v1, v5, v6, v2);
        AddQuad(v4, v3, v7, v8);

        Apply();

        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }

    public void TriangulateSphere(float radius, float numberOfSubDivisions)
    {
        // TODO: [CGOL-55] add resolution/number of subdivisions to function AddTriangle
        // TriangulateCube(radius?, numberOfSubDivisions);
        // foreach mesh.vertices, convert w/PointOnCubeToPointOnSphere(vertex)
        // recalculate normals()
    }

    private static Vector3 PointOnCubeToPointOnSphere(Vector3 p)
    {
        // Thanks to http://mathproofs.blogspot.com/2005/07/mapping-cube-to-sphere.html !

        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;

        float x = p.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 + z2) / 3);
        float y = p.y * Mathf.Sqrt(1 - (z2 + x2) / 2 + (z2 + x2) / 3);
        float z = p.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 + y2) / 3);

        return new Vector3(x, y, z);
    }

    #endregion

    #endregion
    /************************************************************/
    #region Enums

    private enum MeshType
    {
        // base surfaces; for debugging purposes
        Triangle,
        Quad,

        // CustomMesh shapes
        Cube
    }

    #endregion
    /************************************************************/
}
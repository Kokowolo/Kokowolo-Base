/**
 * File Name: CustomMeshExtensions.cs
 * Description: Script that contains the extension functionality for the CustomMesh class; in particular, these  
 *                  extension functions allow for various custom mesh triangulations or shape configurations
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

public static class CustomMeshExtensions
{
    #region Triangle & Quad Functions

    public static void TriangulateTriangle(this CustomMesh customMesh, float edgeLength)
    {
        Vector3 centroid = customMesh.transform.localPosition;
        float height = Mathf.Sqrt(2f/3f) * edgeLength;
        float v2z = Mathf.Sqrt(edgeLength * edgeLength - height * height);
        float halfEdgeLength = 0.5f * edgeLength;
        float v34z = -Mathf.Sqrt(v2z * v2z - halfEdgeLength * halfEdgeLength);

        Vector3 v1 = new Vector3(0, 0, v2z);
        Vector3 v2 = new Vector3(halfEdgeLength, 0, v34z);
        Vector3 v3 = new Vector3(-halfEdgeLength, 0, v34z);

        customMesh.AddTriangle(v1, v2, v3, isDoubleSided: true);
        customMesh.Apply(hasCollider: true);
    }

    public static void TriangulateQuad(this CustomMesh customMesh, float edgeLength)
    {
        float halfEdgeLength = 0.5f * edgeLength;

        Vector3 v1 = new Vector3(halfEdgeLength, 0, halfEdgeLength);
        Vector3 v2 = new Vector3(halfEdgeLength, 0, -halfEdgeLength);
        Vector3 v3 = new Vector3(-halfEdgeLength, 0, halfEdgeLength);
        Vector3 v4 = new Vector3(-halfEdgeLength, 0, -halfEdgeLength);

        customMesh.AddQuad(v1, v2, v3, v4, isDoubleSided: true);
        customMesh.Apply(hasCollider: true);
    }

    #endregion

    #region Cube & Tetrahedron Functions

    public static void TriangulateCube(this CustomMesh customMesh, float edgeLength)
    {
        AddCubePointsToCustomMesh(customMesh, edgeLength);
        customMesh.Apply();
    }

    private static void AddCubePointsToCustomMesh(CustomMesh customMesh, float edgeLength)
    {
        float halfEdgeLength = 0.5f * edgeLength;

        Vector3 v1 = new Vector3(halfEdgeLength, halfEdgeLength, halfEdgeLength);
        Vector3 v2 = new Vector3(halfEdgeLength, halfEdgeLength, -halfEdgeLength);
        Vector3 v3 = new Vector3(halfEdgeLength, -halfEdgeLength, halfEdgeLength);
        Vector3 v4 = new Vector3(halfEdgeLength, -halfEdgeLength, -halfEdgeLength);
        Vector3 v5 = new Vector3(-halfEdgeLength, halfEdgeLength, halfEdgeLength);
        Vector3 v6 = new Vector3(-halfEdgeLength, halfEdgeLength, -halfEdgeLength);
        Vector3 v7 = new Vector3(-halfEdgeLength, -halfEdgeLength, halfEdgeLength);
        Vector3 v8 = new Vector3(-halfEdgeLength, -halfEdgeLength, -halfEdgeLength);

        // sides
        customMesh.AddQuad(v1, v3, v4, v2);
        customMesh.AddQuad(v6, v2, v4, v8);
        customMesh.AddQuad(v1, v5, v7, v3);
        customMesh.AddQuad(v5, v6, v8, v7);
        // bases
        customMesh.AddQuad(v1, v2, v6, v5);
        customMesh.AddQuad(v3, v7, v8, v4);
    }

    public static void TriangulateTetrahedron(this CustomMesh customMesh, float edgeLength)
    {
        Vector3 centroid = customMesh.transform.position;
        float height = Mathf.Sqrt(2f/3f) * edgeLength;
        float v2z = Mathf.Sqrt(edgeLength * edgeLength - height * height);
        float halfEdgeLength = 0.5f * edgeLength;
        float v34z = -Mathf.Sqrt(v2z * v2z - halfEdgeLength * halfEdgeLength);

        Vector3 v1 = centroid + new Vector3(0, 0.75f * height, 0);
        Vector3 v2 = centroid + new Vector3(0, -0.25f * height, v2z);
        Vector3 v3 = centroid + new Vector3(-halfEdgeLength, -0.25f * height, v34z);
        Vector3 v4 = centroid + new Vector3(halfEdgeLength, -0.25f * height, v34z);

        customMesh.AddTriangle(v1, v3, v2);
        customMesh.AddTriangle(v1, v2, v4);
        customMesh.AddTriangle(v1, v4, v3);
        customMesh.AddTriangle(v4, v2, v3);
        customMesh.Apply();
    }

    #endregion

    #region Sphere Functions

    public static void TriangulateSphere(this CustomMesh customMesh, float radius)
    {
        AddCubePointsToCustomMesh(customMesh, 1);

        for (int i = 0; i < customMesh.Vertices.Count; i++)
        {
            customMesh.Vertices[i] = radius * PointOnCubeToPointOnSphere(customMesh.Vertices[i]);
        }

        customMesh.Apply();
    }

    // Thanks to http://mathproofs.blogspot.com/2005/07/mapping-cube-to-sphere.html
    public static Vector3 PointOnCubeToPointOnSphere(Vector3 point)
    {
        float x2 = point.x * point.x;
        float y2 = point.y * point.y;
        float z2 = point.z * point.z;
        float x = point.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 + z2) / 3);
        float y = point.y * Mathf.Sqrt(1 - (x2 + z2) / 2 + (x2 + z2) / 3);
        float z = point.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 + y2) / 3);
        return new Vector3(x, y, z);
    }

    #endregion
}

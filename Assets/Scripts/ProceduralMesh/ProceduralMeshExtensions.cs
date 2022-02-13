/**
 * File Name: ProceduralMeshExtensions.cs
 * Description: Script that contains the extension functionality for the ProceduralMesh class; in particular, these  
 *                  extension functions allow for various procedural mesh triangulations or shape configurations
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

public static class ProceduralMeshExtensions
{
    #region Triangle & Quad Functions

    public static void TriangulateTriangle(this ProceduralMesh proceduralMesh, float edgeLength)
    {
        Vector3 centroid = proceduralMesh.transform.localPosition;
        float height = Mathf.Sqrt(2f/3f) * edgeLength;
        float v2z = Mathf.Sqrt(edgeLength * edgeLength - height * height);
        float halfEdgeLength = 0.5f * edgeLength;
        float v34z = -Mathf.Sqrt(v2z * v2z - halfEdgeLength * halfEdgeLength);

        Vector3 v1 = new Vector3(0, 0, v2z);
        Vector3 v2 = new Vector3(halfEdgeLength, 0, v34z);
        Vector3 v3 = new Vector3(-halfEdgeLength, 0, v34z);
        proceduralMesh.AddTriangle(v1, v2, v3, isDoubleSided: true);

        Vector2 uv1 = new Vector2(0, 0);
        Vector2 uv2 = new Vector2(0, 1);
        Vector2 uv3 = new Vector2(1, 0);   
        proceduralMesh.AddTriangleUV(uv1, uv2, uv3, isDoubleSided: true);

        proceduralMesh.HasUVs = true;
        proceduralMesh.Apply(hasMeshCollider: true);
    }

    public static void TriangulateQuad(this ProceduralMesh proceduralMesh, float edgeLength)
    {
        float halfEdgeLength = 0.5f * edgeLength;

        Vector3 v1 = new Vector3(halfEdgeLength, 0, halfEdgeLength);
        Vector3 v2 = new Vector3(halfEdgeLength, 0, -halfEdgeLength);
        Vector3 v3 = new Vector3(-halfEdgeLength, 0, halfEdgeLength);
        Vector3 v4 = new Vector3(-halfEdgeLength, 0, -halfEdgeLength);
        proceduralMesh.AddQuad(v1, v2, v3, v4, isDoubleSided: true);

        Vector2 uv1 = new Vector2(0, 0);
        Vector2 uv2 = new Vector2(0, 1);
        Vector2 uv3 = new Vector2(1, 0);
        Vector2 uv4 = new Vector2(1, 1);
        proceduralMesh.AddQuadUV(uv1, uv2, uv3, uv4, isDoubleSided: true);

        proceduralMesh.HasUVs = true;
        proceduralMesh.Apply(hasMeshCollider: true);
    }

    #endregion

    #region Cube & Tetrahedron Functions

    public static void TriangulateCube(this ProceduralMesh proceduralMesh, float edgeLength)
    {
        AddCubePointsToProceduralMesh(proceduralMesh, edgeLength);
        proceduralMesh.Apply();
    }

    private static void AddCubePointsToProceduralMesh(ProceduralMesh proceduralMesh, float edgeLength)
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
        proceduralMesh.AddQuad(v1, v3, v4, v2);
        proceduralMesh.AddQuad(v6, v2, v4, v8);
        proceduralMesh.AddQuad(v1, v5, v7, v3);
        proceduralMesh.AddQuad(v5, v6, v8, v7);
        // bases
        proceduralMesh.AddQuad(v1, v2, v6, v5);
        proceduralMesh.AddQuad(v3, v7, v8, v4);
    }

    public static void TriangulateTetrahedron(this ProceduralMesh proceduralMesh, float edgeLength)
    {
        Vector3 centroid = proceduralMesh.transform.position;
        float height = Mathf.Sqrt(2f/3f) * edgeLength;
        float v2z = Mathf.Sqrt(edgeLength * edgeLength - height * height);
        float halfEdgeLength = 0.5f * edgeLength;
        float v34z = -Mathf.Sqrt(v2z * v2z - halfEdgeLength * halfEdgeLength);

        Vector3 v1 = centroid + new Vector3(0, 0.75f * height, 0);
        Vector3 v2 = centroid + new Vector3(0, -0.25f * height, v2z);
        Vector3 v3 = centroid + new Vector3(-halfEdgeLength, -0.25f * height, v34z);
        Vector3 v4 = centroid + new Vector3(halfEdgeLength, -0.25f * height, v34z);

        proceduralMesh.AddTriangle(v1, v3, v2);
        proceduralMesh.AddTriangle(v1, v2, v4);
        proceduralMesh.AddTriangle(v1, v4, v3);
        proceduralMesh.AddTriangle(v4, v2, v3);
        proceduralMesh.Apply();
    }

    #endregion

    #region Sphere Functions

    public static void TriangulateSphere(this ProceduralMesh proceduralMesh, float radius)
    {
        AddCubePointsToProceduralMesh(proceduralMesh, 1);

        for (int i = 0; i < proceduralMesh.Vertices.Count; i++)
        {
            proceduralMesh.Vertices[i] = radius * PointOnCubeToPointOnSphere(proceduralMesh.Vertices[i]);
        }

        proceduralMesh.Apply();
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

    #region Temp Functions

    public static void TriangulateQuad(this ProceduralMesh proceduralMesh, 
        float lengthX, float lengthZ, int numberOfQuadsX, int numberOfQuadsZ)
    {
        proceduralMesh.HasUVs = true;

        Vector3 v1 = new Vector3();
        Vector3 v2 = new Vector3();
        Vector3 v3 = new Vector3();
        Vector3 v4 = new Vector3();

        Vector2 uv1 = new Vector2();
        Vector2 uv2 = new Vector2();
        Vector2 uv3 = new Vector2();
        Vector2 uv4 = new Vector2();

        for (int z = 0; z < numberOfQuadsZ; z++)
        {
            v2 = new Vector3(0, 0, z * lengthZ / numberOfQuadsZ);
            v4 = new Vector3(0, 0, (z + 1) * lengthZ / numberOfQuadsZ);

            uv2 = new Vector3(0, z * lengthZ / numberOfQuadsZ);
            uv4 = new Vector3(0, (z + 1) * lengthZ / numberOfQuadsZ);
            
            for (int x = 0; x < numberOfQuadsX; x++)
            {
                v1 = v2;
                v2 = new Vector3((x + 1) * lengthX / numberOfQuadsX, 0, z * lengthZ / numberOfQuadsZ);
                v3 = v4;
                v4 = new Vector3((x + 1) * lengthX / numberOfQuadsX, 0, (z + 1) * lengthZ / numberOfQuadsZ);
                proceduralMesh.AddQuad(v3, v4, v2, v1);

                uv1 = uv2;
                uv2 = new Vector2((x + 1) * lengthX / numberOfQuadsX, z * lengthZ / numberOfQuadsZ);
                uv3 = uv4;
                uv4 = new Vector2((x + 1) * lengthX / numberOfQuadsX, (z + 1) * lengthZ / numberOfQuadsZ);

                proceduralMesh.AddQuadUV(uv3, uv4, uv2, uv1);
            }
        }

        proceduralMesh.Apply();
    }

    #endregion
}

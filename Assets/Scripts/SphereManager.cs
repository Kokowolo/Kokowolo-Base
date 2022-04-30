/*
 * File Name: SphereManager.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 28, 2022
 * 
 * Additional Comments:
 *		While this file has been updated to better fit this project, the original version can be found here:
 *		https://catlikecoding.com/unity/tutorials/hex-map/
 *
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Kokowolo.Utilities;
using Kokowolo.ProceduralMesh;

public class SphereManager : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Header("Manager Settings")]
    [SerializeField, Min(1)] int countX = 10;
    [SerializeField, Min(1)] int countY = 10;
    [SerializeField, Min(1)] int countZ = 10;
    [Tooltip("distance between spheres")]
    [SerializeField, Range(0, 5)] float sphereDistance = 10;

    [Header("Sphere Settings")]
    [SerializeField, Range(0, 2)] float sphereRadius = 0.5f;
    [SerializeField, Range(0, 5)] int sphereSubdivisions = 5;
    [SerializeField] Material sphereMaterial = null;

    List<ProceduralMesh> spheres = new List<ProceduralMesh>();

    #endregion
    /************************************************************/
    #region Properties

    public static SphereManager Instance => Singleton<SphereManager>.Get();

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Awake()
    {
        Singleton<SphereManager>.Set(this);
        GenerateSpheres();
    }

    private void Start()
    {
        RegenerateSpheres();
    }

    #endregion

    #region Other Functions

    private void GenerateSpheres()
    {
        for (int x = 0; x < countX; x++)
        {
            for (int y = 0; y < countY; y++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    GenerateSphere(x, y, z);
                }
            }
        }
    }

    private void RegenerateSpheres()
    {
        int count = 0;
        for (int x = 0; x < countX; x++)
        {
            for (int y = 0; y < countY; y++)
            {
                for (int z = 0; z < countZ; z++)
                {
                    RegenerateSphere(spheres[count], x, y, z);
                    count++;
                }
            }
        }
    }

    private void GenerateSphere(int x, int y, int z)
    {
        // create GameObject
        GameObject gameObject = new GameObject();
        gameObject.name = $"Sphere ({spheres.Count}) - ({x},{y},{z})";
        gameObject.transform.parent = transform;

        // create Sphere
        ProceduralMesh sphere = gameObject.AddComponent<ProceduralMesh>();
        gameObject.AddComponent<MeshCollider>();
        sphere.MeshRenderer.material = sphereMaterial;
        spheres.Add(sphere);
    }

    private void RegenerateSphere(ProceduralMesh sphere, int x, int y, int z)
    {
        sphere.Clear();
        sphere.TriangulateSphere(sphereRadius, sphereSubdivisions, hasMeshCollider: true);

        Vector3 position = new Vector3(x, y, z);
        if (x % 2 == 0) position.z += 0.5f;
        if (y % 2 == 0) position.x += 0.5f;
        if (z % 2 == 0) position.y += 0.5f;
        sphere.transform.position = transform.position + position * sphereDistance;
    }

    #endregion

    #endregion
    /************************************************************/
    #region Debug
    #if UNITY_EDITOR

    [MenuItem("CONTEXT/SphereManager/Reset Spheres")]
    public static void ResetSpheres(MenuCommand menuCommand)
    {
        if (!Application.isPlaying) return;

        foreach (ProceduralMesh sphere in Instance.spheres) 
        {
            Destroy(sphere.gameObject);
        }
        Instance.spheres.Clear();

        Instance.GenerateSpheres();
        Instance.RegenerateSpheres();
    }

    #endif
    #endregion
    /************************************************************/
}
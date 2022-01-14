/**
 * File Name: MeshHandler.cs
 * Description: 
 * 
 * Authors: Will Lacey
 * Date Created: November 9, 2021
 * 
 * Additional Comments: 
 *      File Line Length: 120
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshHandler : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [SerializeField, Range(0, 2)] float perturbStrength = 0;

    Vector3[] originalVertices;

    #endregion
    /************************************************************/
    #region Properties

    public Mesh mesh => meshFilter.mesh;

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
        originalVertices = mesh.vertices;
    }

    #endregion

    #region Other Functions

    public void SetVertices(Vector3[] vertices)
    {
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    public void DistortMesh(float strength)
    {
        Vector3[] vertices = mesh.vertices; // HACK: verify this works, it might be worth it to create another temp var

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            // TODO: [CGOL-55] add perturb that does not show the exact same thing every time
            vertices[i] = MathUtils.Perturb(vertices[i], strength); 
        }

        SetVertices(vertices);
    }

    #endregion

    #endregion
    /************************************************************/
    #region Debug
    #if UNITY_EDITOR

    [Header("Debug Settings")]
    [SerializeField] bool useDebugMode = false;
    [SerializeField, Range(0, 10)] float perturbTime = 1;

    float t;
    bool isIncreasing = true;
    float perturbCooldown;
    float currentPerturbStrength;

    private void OnValidate()
    {
        if (Application.isPlaying && originalVertices != null)
        {
            SetVertices(originalVertices);
            DistortMesh(perturbStrength);
        }
    }

    private void LateUpdate()
    {
        if (!useDebugMode) return;

        if (perturbCooldown >= perturbTime)
        {
            perturbCooldown = 0f;
            isIncreasing = !isIncreasing;
        }

        t = (isIncreasing) ? perturbCooldown / perturbTime : (perturbTime - perturbCooldown) / perturbTime;
        currentPerturbStrength = Mathf.Lerp(0, perturbStrength, t);

        SetVertices(originalVertices);
        DistortMesh(currentPerturbStrength);

        perturbCooldown += Time.deltaTime;
    }

    #endif
    #endregion
    /************************************************************/
}
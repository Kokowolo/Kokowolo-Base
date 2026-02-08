/*
 * File Name: HexMesh.cs
 * Description: This script is for...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 3, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Grid;
using Kokowolo.ProceduralMesh;
using Kokowolo.Utilities;

public class HexMesh : CustomMesh
{
    /************************************************************/
    #region Fields

    [Header("Settings")]
    [SerializeField, Min(0.001f)] private float noiseStrength = 1;
    [SerializeField, Range(0, 10)] public int subdivisions = 0;

    #endregion
    /************************************************************/
    #region Properties

    protected override string MeshName
    {
        get => nameof(HexMesh);
    }

    #endregion
    /************************************************************/
    #region Functions

    private void Awake()
    {
        Reload();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Clear();
            Apply();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Reload();
        }
    }

    public void Reload()
    {
        Clear();
        List<GridCell> cells = new List<GridCell>();
        for (int z = 0; z < GridManager.TargetMapObject.Map.Structure.Zone.CountZ; z++)
        {
            for (int x = 0; x < GridManager.TargetMapObject.Map.Structure.Zone.CountX; x++)
            {
                cells.Add(GridManager.TargetMapObject.Map.GetCell(new GridCoordinates(0, z, x)));
            }
        }
        Triangulate(cells);
        Apply(MeshCollider);
    }

    public void Triangulate(List<GridCell> cells)
    {
        foreach (GridCell cell in cells)
        {
            Vector3 v0 = KokoRandom.Perturb(GridMetrics.GetCorner(GridDirectionExtensions.GetDirection(0)) + cell.SurfacePosition, noiseStrength, useY: false);
            Vector3 v1 = KokoRandom.Perturb(GridMetrics.GetCorner(GridDirectionExtensions.GetDirection(1)) + cell.SurfacePosition, noiseStrength, useY: false);
            Vector3 v2 = KokoRandom.Perturb(GridMetrics.GetCorner(GridDirectionExtensions.GetDirection(2)) + cell.SurfacePosition, noiseStrength, useY: false);
            Vector3 v3 = KokoRandom.Perturb(GridMetrics.GetCorner(GridDirectionExtensions.GetDirection(3)) + cell.SurfacePosition, noiseStrength, useY: false);
            Vector3 v4 = KokoRandom.Perturb(GridMetrics.GetCorner(GridDirectionExtensions.GetDirection(4)) + cell.SurfacePosition, noiseStrength, useY: false);
            Vector3 v5 = KokoRandom.Perturb(GridMetrics.GetCorner(GridDirectionExtensions.GetDirection(5)) + cell.SurfacePosition, noiseStrength, useY: false);

            AddTriangle(v0, v1, v5, subdivisions);
            AddQuad(v1, v2, v4, v5, subdivisions);
            AddTriangle(v3, v4, v2, subdivisions);

            // Vector2 uv0 = new Vector2(0, 1);    // 0,1      1,1
            // Vector2 uv1 = new Vector2(1, 1);    //  how textures
            // Vector2 uv2 = new Vector2(1, 0);    //  are UV'ed
            // Vector2 uv3 = new Vector2(0, 0);    // 0,0      1,0
            
            // AddTriangleUV(uv0, uv1, uv2, subdivisions);
            // AddQuadUV(uv0, uv1, uv2, uv3, subdivisions);
            // AddTriangleUV(uv0, uv1, uv2, subdivisions);
        }

        // Vector2 uv0 = new Vector2(0, 1);    // 0,1      1,1
        // Vector2 uv1 = new Vector2(1, 1);    //  how textures
        // Vector2 uv2 = new Vector2(1, 0);    //  are UV'ed
        // Vector2 uv3 = new Vector2(0, 0);    // 0,0      1,0
        // AddQuadUV(uv0, uv1, uv2, uv3, subdivisions);
    }

    #endregion
    /************************************************************/
    #region Debug
    #if UNITY_EDITOR

    // private void OnValidate() 
    // {
    //     if (Mesh) Refresh();
    // }

    #endif
    #endregion
    /************************************************************/
}
/*
 * File Name: TestGridPipeline.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 15, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;
using Kokowolo.Grid;
using Kokowolo.Pathfinding;

public class TestGridPipelineManager : MonoBehaviour, IGridPipeline
{
    /************************************************************/
    #region Fields

    [SerializeField] private LayerMask gridMapLayerMask;
    [SerializeField] private LayerMask gridSurfaceLayerMask;
    [SerializeField] private string gridTag;

    private GridCell cellA;
    private GridCell cellB;
    private NodePath searchPath = new NodePath();

    #endregion
    /************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cellA != null) GridMap.Instance.Visual.HideCursor(cellA.Coordinates);
            cellA = GridMap.Instance.GetCell(GridCursorController.Instance.Coordinates);
            GridMap.Instance.Visual.ShowCursor(cellA.Coordinates, Color.blue);
            Route();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (cellB != null) GridMap.Instance.Visual.HideCursor(cellB.Coordinates);
            cellB = GridMap.Instance.GetCell(GridCursorController.Instance.Coordinates);
            GridMap.Instance.Visual.ShowCursor(cellB.Coordinates, Color.red);
            Route();
        }
    }

    private void Route()
    {
        foreach (Node node in searchPath)
        {
            GridMap.Instance.Visual.HidePathfinding((node.Object as GridCell).Coordinates);
        }
        if (cellA != cellB && cellA != null && cellB != null)
        {
            TestGridPathfinding pathfinding = FindObjectOfType<TestGridPathfinding>(); 
            searchPath = AStarPathfinding.Search(pathfinding, cellA.Node, cellB.Node);

            foreach (Node node in searchPath)
            {
                GridMap.Instance.Visual.ShowPathfinding((node.Object as GridCell).Coordinates);
            }
        }
    }

    #region Interface Functions

    public LayerMask GetGridMapLayerMask()
    {
        return gridMapLayerMask;
    }

    public LayerMask GetGridSurfaceLayerMask()
    {
        return gridSurfaceLayerMask;
    }

    public string GetGridTag()
    {
        return gridTag;
    }

    // public IGridObject GetGridObject(GridCoordinates coordinates)
    // {
    //     return null;
    // }

    #endregion
    
    #endregion
    /************************************************************/
}
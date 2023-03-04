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
    [SerializeField] private LayerMask gridSurfaceSoftLayerMask;
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
            if (cellA != null) GridMapVisual.Instance.HideCursor(cellA.Coordinates);
            cellA = GridManager.Map.GetCell(GridCursorController.Instance.Coordinates) as GridCell;
            GridMapVisual.Instance.ShowCursor(cellA.Coordinates, Color.blue);
            Route();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (cellB != null) GridMapVisual.Instance.HideCursor(cellB.Coordinates);
            cellB = GridManager.Map.GetCell(GridCursorController.Instance.Coordinates) as GridCell;
            GridMapVisual.Instance.ShowCursor(cellB.Coordinates, Color.red);
            Route();
        }
    }

    private void Route()
    {
        foreach (Node node in searchPath)
        {
            GridMapVisual.Instance.HidePathfinding((node.Object as GridCell).Coordinates);
        }
        if (cellA != cellB && cellA != null && cellB != null)
        {
            searchPath = AStarPathfinding.Search(TestGridPathfinding.Instance, cellA.Node, cellB.Node);

            foreach (Node node in searchPath)
            {
                GridMapVisual.Instance.ShowPathfinding((node.Object as GridCell).Coordinates);
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

    public LayerMask GetGridSurfaceSoftLayerMask()
    {
        return gridSurfaceSoftLayerMask;
    }

    public string GetGridTag()
    {
        return gridTag;
    }

    public GridCell GetGridCell(GridCoordinates coordinates)
    {
        return new TestGridCell(coordinates);
    }

    public void ShowCursor(GridCoordinates coordinates)
    {
        GridMapVisual.Instance.ShowCursor(coordinates);
    }

    public void HideCursor(GridCoordinates coordinates)
    {
        GridMapVisual.Instance.HideCursor(coordinates);
    }

    #endregion

    #endregion
    /************************************************************/
}
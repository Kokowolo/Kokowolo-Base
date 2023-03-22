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

    [Header("Cached References")]
    [SerializeField] private GameObject debugGridTransformVisual;

    [Header("Grid Pipeline Settings")]
    [SerializeField] private LayerMask gridMapLayerMask;
    [SerializeField] private LayerMask gridSurfaceLayerMask;
    [SerializeField] private LayerMask gridSurfaceSoftLayerMask;
    [SerializeField] private string gridTag;

    [Header("Other Settings")]
    [SerializeField] private int range = 1;

    private GridCell cellA;
    private GridCell cellB;
    private GridCell cellC;
    private NodePath searchPath = new NodePath();

    GridTransform gridTransform;

    #endregion
    /************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void Awake() 
    {
        gridTransform = new GridTransform();
        gridTransform.Init(transform);
        SetDebugGridTransformVisual();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GridManager.SetActive(!GridManager.Instance.gameObject.activeSelf);
        }
        if (Input.GetMouseButtonDown(0))
        {
            ShowCell(ref cellA, Color.blue);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShowCell(ref cellB, Color.red);
        }
        if (Input.GetMouseButtonDown(2))
        {
            ShowCell(ref cellC, Color.yellow);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            gridTransform.SetCoordinates(GridCursorController.Instance.Coordinates);
            SetDebugGridTransformVisual();
            StartCoroutine(HighlightForwardCells());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gridTransform.Direction = gridTransform.Direction.Next();
            StartCoroutine(HighlightForwardCells());
            SetDebugGridTransformVisual();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Route();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DirectionCheck();
        }
    }

    private void ShowCell(ref GridCell cell, Color color)
    {
        if (cell != null) GridMapVisual.Instance.HideCursor(cell.Coordinates);
        cell = GridManager.Map.GetCell(GridCursorController.Instance.Coordinates) as GridCell;
        GridMapVisual.Instance.ShowCursor(cell.Coordinates, color);
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

    private void DirectionCheck()
    {
        GridDirection inDirection = GridCoordinates.GetDirectionToCoordinates(cellC.Coordinates, cellA.Coordinates);
        GridDirection outDirection = GridCoordinates.GetDirectionToCoordinates(cellA.Coordinates, cellB.Coordinates);
        Debug.Log($"In: {inDirection}, Out: {outDirection}, ${inDirection.Distance(outDirection)}");
    }

    private void SetDebugGridTransformVisual()
    {
        debugGridTransformVisual.transform.position = GridPositioning.GetPosition(gridTransform.Coordinates);
        debugGridTransformVisual.transform.rotation = GridMetrics.GetRotationFromDirection(gridTransform.Direction);
    }

    private IEnumerator HighlightForwardCells()
    {
        List<GridCell> cells = gridTransform.GetForwardCells(range);
        foreach (GridCell cell in cells)
        {
            GridMapVisual.Instance.ShowCursor(cell.Coordinates, Color.magenta);
        }
        yield return new WaitForSeconds(1f);
        foreach (GridCell cell in cells)
        {
            GridMapVisual.Instance.HideCursor(cell.Coordinates);
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
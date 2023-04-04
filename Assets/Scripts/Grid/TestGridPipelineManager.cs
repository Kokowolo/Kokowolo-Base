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

using Kokowolo.Grid;
using Kokowolo.Pathfinding;
using Kokowolo.Utilities;
using MathKoko = Kokowolo.Utilities.Math;

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

    GridMapVisualJob visualJob;
    List<GridMapVisualJob> visualJobs = new List<GridMapVisualJob>();
    List<GridCell> visualCells = new List<GridCell>();
 
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

        // GridManager.Visual.Show(cells);
    }

    private void HandleInput()
    {
        // INPUT SCEME 1
        if (Input.GetMouseButtonDown(0))
        {
            GridCell cell = GridCursorController.Instance.Cell;
            if (cell != null && !visualCells.Contains(cell)) 
            {
                visualCells.Add(cell);
            }

            visualJob = GridManager.Visual.CreateVisualJob(
                GridMapVisualJob.JobType.Group, 
                visualCells.ToCoordinatesList(), 
                color: MathKoko.GetRandomColor() * 0.8f
            );
        }
        else if (Input.GetMouseButton(0))
        {
            GridCell cell = GridCursorController.Instance.Cell;
            if (cell != null && !visualCells.Contains(cell)) 
            {
                visualCells.Add(cell);
                visualJob.Update(visualCells.ToCoordinatesList());
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            visualCells.Clear();
            visualJobs.Add(visualJob);
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (visualJobs.Count > 0)
            {
                visualJob = visualJobs[visualJobs.Count - 1];
                GridManager.Visual.DestroyVisualJob(visualJob);
                visualJobs.RemoveAt(visualJobs.Count - 1);
            }
        }

        // INPUT SCEME 2
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     GridManager.SetActive(!GridManager.Instance.gameObject.activeSelf);
        // }
        // if (Input.GetKeyDown(KeyCode.LeftArrow))
        // {
        //     range = Mathf.Max(0, range - 1);
        // }
        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     range++;
        // }
        // if (Input.GetMouseButtonDown(0))
        // {
        //     ShowCell(ref cellA, Color.blue);
        // }
        // if (Input.GetMouseButtonDown(1))
        // {
        //     ShowCell(ref cellB, Color.red);
        // }
        // if (Input.GetMouseButtonDown(2))
        // {
        //     ShowCell(ref cellC, Color.yellow);
        // }
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     gridTransform.SetCoordinates(GridCursorController.Instance.Coordinates);
        //     SetDebugGridTransformVisual();
        //     StartCoroutine(ShowForwardCells());
        // }
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     gridTransform.Direction = gridTransform.Direction.Next();
        //     StartCoroutine(ShowForwardCells());
        //     SetDebugGridTransformVisual();
        // }
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     Route();
        // }
        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     RouteAll();
        // }
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     DirectionCheck();
        // }
    }

    // private void ShowCell(ref GridCell cell, Color color)
    // {
    //     if (cell != null) GridMapVisual.Instance.HideCursor(cell.Coordinates);
    //     cell = GridManager.Map.GetCell(GridCursorController.Instance.Coordinates) as GridCell;
    //     GridMapVisual.Instance.ShowCursor(cell.Coordinates, color);
    // }

    // private void Route()
    // {
    //     ClearSearchPath();
    //     if (cellA != cellB && cellA != null && cellB != null)
    //     {
    //         searchPath = AStarPathfinding.GetPath(TestGridPathfinding.Instance, cellA.Node, cellB.Node, range);

    //         foreach (Node node in searchPath)
    //         {
    //             GridMapVisual.Instance.ShowPathfinding((node.Object as GridCell).Coordinates);
    //         }
    //     }
    // }

    // private void RouteAll()
    // {
    //     ClearSearchPath();
    //     StartCoroutine(ShowAllSearchedNodes());
    // }

    // private void ClearSearchPath()
    // {
    //     foreach (Node node in searchPath)
    //     {
    //         GridMapVisual.Instance.HidePathfinding((node.Object as GridCell).Coordinates);
    //     }
    //     searchPath.Clear();
    // }

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

    private IEnumerator ShowForwardCells()
    {
        List<GridCell> cells = gridTransform.GetForwardCells(range);
        var job = GridManager.Visual.CreateVisualJob(
            GridMapVisualJob.JobType.Singles, 
            cells.ToCoordinatesList(), 
            color: Color.magenta
        );
        yield return new WaitForSeconds(1f);
        GridManager.Visual.DestroyVisualJob(job);
    }

    private IEnumerator ShowAllSearchedNodes()
    {
        List<Node> nodes = AStarPathfinding.GetAllSearchedNodes(TestGridPathfinding.Instance, cellA.Node, range);
        List<GridCoordinates> coordinatesList = ListPool.Get<GridCoordinates>();
        foreach (Node node in nodes)
        {
            coordinatesList.Add((node.Object as GridCell).Coordinates);
        }

        var job = GridManager.Visual.CreateVisualJob(GridMapVisualJob.JobType.Minis, coordinatesList);
        yield return new WaitForSeconds(1f);
        GridManager.Visual.DestroyVisualJob(job);
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

    #endregion

    #endregion
    /************************************************************/
}
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

    private (GridCell, GridMapVisualJob)[] cellJobs = new (GridCell, GridMapVisualJob)[3];
    private NodePath searchPath = new NodePath();
    GridMapVisualJob searchPathVisualJob;

    GridTransform gridTransform;

    GridMapVisualJob visualJob;
    List<GridMapVisualJob> visualJobs = new List<GridMapVisualJob>();
    List<GridCell> visualCells = new List<GridCell>();

    private bool mode = true;
 
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
        // Switch Mode
        if (Input.GetKeyDown(KeyCode.Tab)) mode = !mode;

        if (mode)
        {
            HandleMode1Input();
        }
        else
        {
            HandleMode2Input();
        }
    }

    private void HandleMode1Input()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridCell cell = GridCursorController.Instance.Cell;
            if (cell != null && !visualCells.Contains(cell)) 
            {
                visualCells.Add(cell);
            }

            visualJob = GridManager.Visual.CreateVisualJob(
                GridMapVisualJob.JobType.Group, 
                visualCells, 
                color: MathKoko.GetRandomColor() * 0.8f
            );
        }
        else if (Input.GetMouseButton(0))
        {
            GridCell cell = GridCursorController.Instance.Cell;
            if (cell != null && !visualCells.Contains(cell)) 
            {
                visualCells.Add(cell);
                visualJob.Update(visualCells);
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
                GridManager.Visual.RemoveVisualJob(visualJob);
                visualJobs.RemoveAt(visualJobs.Count - 1);
            }
        }
    }

    private void HandleMode2Input()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GridManager.SetActive(!GridManager.Instance.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            range = Mathf.Max(0, range - 1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            range++;
        }
        if (Input.GetMouseButtonDown(0))
        {
            ShowCellJob(0, Color.blue);
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShowCellJob(1, Color.red);
        }
        if (Input.GetMouseButtonDown(2))
        {
            ShowCellJob(2, Color.yellow);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            gridTransform.SetCoordinates(GridCursorController.Instance.Coordinates);
            SetDebugGridTransformVisual();
            StartCoroutine(ShowForwardCells());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            gridTransform.SetDirection(gridTransform.Direction.Next());
            StartCoroutine(ShowForwardCells());
            SetDebugGridTransformVisual();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Route();
        }
        if (Input.GetKey(KeyCode.G))
        {
            RouteAll();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DirectionCheck();
        }
    }

    private void ShowCellJob(int index, Color color)
    {
        GridCell cell = cellJobs[index].Item1 = GridCursorController.Instance.Cell;

        if (cellJobs[index].Item2 == null)
        {
            cellJobs[index].Item2 = GridManager.Visual.CreateVisualJob(
                GridMapVisualJob.JobType.Singles, 
                cell.Coordinates, 
                color: color
            );
        }
        else
        {
            cellJobs[index].Item2.Update(cell.Coordinates);
        }
    }

    private void Route()
    {
        GridCell start = cellJobs[0].Item1;
        GridCell end = cellJobs[1].Item1;

        if (start != end && start != null && end != null)
        {
            searchPath = AStarPathfinding.GetPath(TestGridPathfinding.Instance, start.Node, end.Node, range);
            List<GridCell> cells = new List<GridCell>();
            foreach (Node node in searchPath)
            {
                cells.Add(node.GetCell());
            }

            if (searchPathVisualJob == null)
            {
                searchPathVisualJob = GridManager.Visual.CreateVisualJob(
                    GridMapVisualJob.JobType.Minis, 
                    cells
                );
            }
            else
            {
                searchPathVisualJob.Update(cells);
            }
        }
    }

    private void RouteAll()
    {
        StartCoroutine(ShowAllSearchedNodes());
    }

    private void DirectionCheck()
    {
        GridCell cellA = cellJobs[0].Item1;
        GridCell cellB = cellJobs[1].Item1;
        GridCell cellC = cellJobs[2].Item1;

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
            cells, 
            color: Color.magenta
        );
        yield return new WaitForSeconds(1f);
        GridManager.Visual.RemoveVisualJob(job);
    }

    private IEnumerator ShowAllSearchedNodes()
    {
        GridCell cell = GridCursorController.Instance.Cell;
        List<Node> nodes = AStarPathfinding.GetAllSearchedNodes(TestGridPathfinding.Instance, cell.Node, range);
        List<GridCell> cells = ListPool.Get<GridCell>();
        foreach (Node node in nodes)
        {
            cells.Add(node.GetCell());
        }

        var job = GridManager.Visual.CreateVisualJob(GridMapVisualJob.JobType.Minis, cells, color: MathKoko.GetRandomColor());
        yield return new WaitForSeconds(1f);
        GridManager.Visual.RemoveVisualJob(job);
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
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

public class TestGridPipelineManager : MonoBehaviour, IGridMapObject
{
    /*██████████████████████████████████████████████████████████*/
    #region Enum

    private enum InputMode
    {
        Mode1,
        Mode2,
        Mode3
    }
    
    #endregion
    /*██████████████████████████████████████████████████████████*/
    #region Fields

    [Header("References")]
    [SerializeField] private GameObject debugGridTransformVisual;
    [SerializeField] private TMPro.TextMeshProUGUI modeText;

    [Header("References: IGridMapObject")]
    [SerializeField] GridMetrics _Metrics;
    [SerializeField] GridMap _Map;
    
    [Header("Old Grid Pipeline Settings")]
    [SerializeField] private LayerMask gridMapLayerMask;
    [SerializeField] private LayerMask gridSurfaceLayerMask;
    [SerializeField] private LayerMask gridSurfaceSoftLayerMask;
    [SerializeField] private string gridTag;

    [Header("Other Settings")]
    [SerializeField] private int range = 1;
    [SerializeField] private GridTransform gridTransform;

    private (GridCell, GridMapVisualJob)[] cellJobs = new (GridCell, GridMapVisualJob)[3];
    private NodePath searchPath = new NodePath();
    GridMapVisualJob searchPathVisualJob;

    GridMapVisualJob visualJob;
    List<GridMapVisualJob> visualJobs = new List<GridMapVisualJob>();
    List<GridCell> visualCells = new List<GridCell>();

    GridMapVisualJob gridTransformVisualJob;

    private InputMode mode = InputMode.Mode1;
 
    #endregion
    /*██████████████████████████████████████████████████████████*/
    #region Properties

    public GridIO IO => throw new System.NotImplementedException();
    public GridMetrics Metrics => _Metrics;
    public IPathfinding Pathfinding => TestGridPathfinding.Instance;
    public GridMap Map => _Map;

    // Input
    private bool Input_SwitchMode => Input.GetKeyDown(KeyCode.Tab);

    // General
    private bool Input_Undo => Input.GetMouseButtonDown(1);
    private bool Input_Click => Input.GetMouseButtonDown(0);

    private bool Input_RotateClockwise => Input.GetKeyDown(KeyCode.E);
    private bool Input_RotateCounterClockwise => Input.GetKeyDown(KeyCode.Q);

    // Mode 1
    private bool Input_Click_Hold => Input.GetMouseButton(0);
    private bool Input_Click_Release => Input.GetMouseButtonUp(0);

    // Mode 2
    private bool Input_CreateMap => Input.GetKeyDown(KeyCode.Alpha0);
    private bool Input_ToggleGridManagerActive => Input.GetKeyDown(KeyCode.Space);

    private bool Input_RangeIncrease => Input.GetKeyDown(KeyCode.RightArrow);
    private bool Input_RangeDecrease => Input.GetKeyDown(KeyCode.LeftArrow);

    private bool Input_RoutePath => Input.GetKeyDown(KeyCode.R);
    private bool Input_RouteDijkstra => Input.GetKeyDown(KeyCode.F);
    private bool Input_DirectionCheck => Input.GetKeyDown(KeyCode.C);

    #endregion
    /*██████████████████████████████████████████████████████████*/
    #region Functions

    private void Start()
    {
        Map.SetSize();
        Map.UpdateNavigation();
        gridTransform.Init(debugGridTransformVisual.transform);
        RefreshGridTransform();

        modeText.text = $"{mode}";
    }

    private void Update()
    {
        // SHOW CENTER POINT
        {
            Vector3 point1 = GridManager.TargetMapObject.Map.ExplorableBounds.center;
            Vector3 point2 = GridManager.TargetMapObject.Map.StructureBounds.center;
            point1 = GridManager.Instance.transform.TransformPoint(point1);
            point2 = GridManager.Instance.transform.TransformPoint(point2);
            Debug.DrawLine(point1, point1 + GridManager.Instance.transform.rotation * new Vector3(0, 100, 0), Color.blue);
            Debug.DrawLine(point2, point2 + GridManager.Instance.transform.rotation * new Vector3(0, 100, 0), Color.green);
        }
        HandleInput();
    }

    private void HandleInput()
    {
        // Switch Mode
        if (Input_SwitchMode) 
        {
            mode = (InputMode) KokoMath.WrapClamp((int) mode + 1, 0, EnumExtensions.GetCount<InputMode>());
            modeText.text = $"{mode}";
        }

        switch (mode)
        {
            case InputMode.Mode1:
            {
                HandleInputMode1();
                break;
            }
            case InputMode.Mode2:
            {
                HandleInputMode2();
                break;
            }
            case InputMode.Mode3:
            {
                CustomFunction();
                break;
            }
        }
    }

    private void HandleInputMode1()
    {
        if (Input_Click)
        {
            visualCells.Clear();
            GridCell cell = GridCursorController.Instance.Cell;
            if (cell != null && !visualCells.Contains(cell)) 
            {
                visualCells.Add(cell);
            }

            visualJob = GridManager.Visual.CreateVisualJob(
                GridMapVisualJob.JobType.Group, 
                visualCells, 
                color: KokoRandom.Color() * 0.8f
            );
        }
        else if (Input_Click_Hold)
        {
            GridCell cell = GridCursorController.Instance.Cell;
            if (cell != null && !visualCells.Contains(cell)) 
            {
                visualCells.Add(cell);
                visualJob.Update(visualCells);
            }
        }
        if (Input_Click_Release)
        {
            visualJobs.Add(visualJob);
        }
        if (Input_Undo)
        {
            if (visualJobs.Count > 0)
            {
                visualJob = visualJobs[visualJobs.Count - 1];
                GridManager.Visual.RemoveVisualJob(visualJob);
                visualJobs.RemoveAt(visualJobs.Count - 1);
            }
        }
        if (Input_RotateClockwise || Input_RotateCounterClockwise)
        {
            // params
            GridDirection startDir = GridDirection.NW;
            // get data
            List<GridCoordinates> coordinatesList = new List<GridCoordinates>();
            foreach (var cell in visualCells) coordinatesList.Add(cell.Coordinates);
            // variables
            GridDirection direction = Input_RotateClockwise ? startDir.Next() : startDir.Previous();
            // call function
            GridPositioning.Rotate(ref coordinatesList, startDir, direction);
            // update visual
            for (int i = 0; i < visualCells.Count; i++) visualCells[i] = GridManager.TargetMapObject.Map.GetCell(coordinatesList[i]);
            visualJobs[visualJobs.Count - 1].Update(coordinatesList);
        }
    }

    private void HandleInputMode2()
    {
        if (Input_ToggleGridManagerActive)
        {
            throw new System.NotImplementedException();
            // GridManager.SetActive(!GridManager.Instance.gameObject.activeSelf);
        }
        if (Input_CreateMap)
        {
            throw new System.NotImplementedException();
            // GridManager.TargetMapObject.Map.Create();
        }
        if (Input_RangeDecrease)
        {
            range = Mathf.Max(0, range - 1);
        }
        if (Input_RangeIncrease)
        {
            range++;
        }
        if (Input_Click)
        {
            ShowCellJob(0, Color.blue);
        }
        if (Input_Undo)
        {
            ShowCellJob(1, Color.red);
        }
        if (Input.GetMouseButtonDown(2))
        {
            ShowCellJob(2, Color.yellow);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            RefreshGridTransform(coordinates: GridCursorController.Instance.Coordinates);
            StartCoroutine(ShowForwardCells());
        }
        if (Input_RotateClockwise ||Input_RotateCounterClockwise)
        {
            if (Input_RotateClockwise)
            {
                RefreshGridTransform(direction: gridTransform.Direction.Next());
            }
            else
            {
                RefreshGridTransform(direction: gridTransform.Direction.Previous());
            }
            StartCoroutine(ShowForwardCells());
        }
        if (Input_RoutePath)
        {
            Route();
        }
        if (Input_RouteDijkstra)
        {
            RouteAll();
        }
        if (Input_DirectionCheck)
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
            searchPath = AStarPathfinding.GetPath(TestGridPathfinding.Instance, start, end, range);
            List<GridCell> cells = new List<GridCell>();
            foreach (INode node in searchPath)
            {
                cells.Add(node as GridCell);
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

        GridDirection inDirection = cellC.Coordinates.GetDirectionToCoordinates(cellA.Coordinates);
        GridDirection outDirection = cellA.Coordinates.GetDirectionToCoordinates(cellB.Coordinates);
        Debug.Log($"In: {inDirection}, Out: {outDirection}, Rotations Away:{inDirection.Distance(outDirection)}");
    }

    private void RefreshGridTransform(GridCoordinates? coordinates = null, GridDirection? direction = null)
    {
        // set coordinates
        if (coordinates != null)
        {
            gridTransform.Coordinates = (GridCoordinates) coordinates;
        }

        // set direction
        if (direction != null)
        {
            gridTransform.Direction = (GridDirection) direction;
        }

        // set GridTransform visual
        if (gridTransformVisualJob == null)
        {
            gridTransformVisualJob = GridManager.Visual.CreateVisualJob(
                GridMapVisualJob.JobType.Group, 
                gridTransform.GetOccupyingCoordsList(), 
                color: Color.magenta
            );
        }
        else
        {
            gridTransformVisualJob.Update(gridTransform.GetOccupyingCoordsList());
        }

        // SetDebugGridTransformVisual
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
        List<INode> nodes = AStarPathfinding.GetAllSearchedNodes(TestGridPathfinding.Instance, cell, range);
        List<GridCell> cells = ListPool.Get<GridCell>();
        foreach (INode node in nodes)
        {
            cells.Add(node as GridCell);
        }

        var job = GridManager.Visual.CreateVisualJob(GridMapVisualJob.JobType.Minis, cells, color: KokoRandom.Color());
        yield return new WaitForSeconds(1f);
        GridManager.Visual.RemoveVisualJob(job);
    }

    private void CustomFunction()
    {
        // temp test code
    }

    #region Interface Functions

    LayerMask IGridMapObject.GetGridMapLayerMask()
    {
        return gridMapLayerMask;
    }

    LayerMask IGridMapObject.GetGridSurfaceLayerMask()
    {
        return gridSurfaceLayerMask;
    }

    LayerMask IGridMapObject.GetGridSurfaceSoftLayerMask()
    {
        return gridSurfaceSoftLayerMask;
    }

    string IGridMapObject.GetGridTag()
    {
        return gridTag;
    }

    GridCell IGridMapObject.CreateGridCell(GridCoordinates coordinates)
    {
        return new TestGridCell(coordinates);
    }

    #endregion

    #endregion
    /*██████████████████████████████████████████████████████████*/
#if UNITY_EDITOR
    #region Editor
    
    void OnDrawGizmos()
    {
        Map.DrawGizmos();
    }
    
    #endregion
#endif
    /*██████████████████████████████████████████████████████████*/
}
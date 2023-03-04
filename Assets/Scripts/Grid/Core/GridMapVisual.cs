/*
 * File Name: GridSystemVisual.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 22, 2022
 * 
 * Additional Comments:
 *      File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Grid
{
    public class GridMapVisual : MonoSingleton<GridMapVisual>
    {
        /************************************************************/
        #region Fields

        [Header("Cached References")] 
        [SerializeField] private GridCellVisual gridCellVisualPrefab = null;

        private GridStructure<GridCellVisual> gridCellVisuals;
        
        #endregion
        /************************************************************/
        #region Properties

        #endregion
        /************************************************************/
        #region Functions

        protected override void MonoSingleton_Awake() 
        {
            GridManager.OnGridEnabled += Handle_GridManager_OnGridEnabled;
            GridManager.OnGridDisabled += Handle_GridManager_OnGridDisabled;
        }

        protected override void MonoSingleton_OnDestroy()
        {
            GridManager.OnGridEnabled -= Handle_GridManager_OnGridEnabled;
            GridManager.OnGridDisabled -= Handle_GridManager_OnGridDisabled;
        }

        private void Start()
        {
            enabled = false;
            
            gridCellVisuals = new GridStructure<GridCellVisual>(
                GridManager.Map.CellCountY, GridManager.Map.CellCountZ, GridManager.Map.CellCountX
            );
            gridCellVisuals.Initialize(CreateGridObject);
        }

        private GridCellVisual CreateGridObject(GridCoordinates coordinates)
        {
            GridCell cell = GridManager.Map.GetCell(coordinates);
            // if (!cell.Node.IsExplorable) return null;
            GridCellVisual visual = Instantiate(
                gridCellVisualPrefab, 
                cell.SurfacePosition,
                Quaternion.identity,
                transform
            );
            visual.name = $"Visual {coordinates.Index}";
            visual.Coordinates = coordinates;
            return visual;
        }

        public void ShowCoordinatesWithinRange(GridCoordinates coordinates, int range, Color? color = null)
        {
            List<GridCellVisual> visuals = gridCellVisuals.GetGridObjects(coordinates, range);
            foreach (var visual in visuals)
            {
                visual.ShowOuter(color ?? Color.white);
            }
        }

        public void ShowCoordinatesWithinRange(Vector3 worldPosition, int range, Color? color = null)
        {
            List<GridCellVisual> visuals = gridCellVisuals.GetGridObjects(worldPosition, range);
            foreach (var visual in visuals)
            {
                visual.ShowOuter(color ?? Color.white);
            }
        }

        public void HideCoordinatesWithinRange(GridCoordinates coordinates, int range)
        {
            List<GridCellVisual> visuals = gridCellVisuals.GetGridObjects(coordinates, range);
            foreach (var visual in visuals)
            {
                visual.HideOuter();
            }
        }

        public void HideCoordinatesWithinRange(Vector3 worldPosition, int range)
        {
            List<GridCellVisual> visuals = gridCellVisuals.GetGridObjects(worldPosition, range);
            foreach (var visual in visuals)
            {
                visual.HideOuter();
            }
        }

        public void ShowCoordinatesFromCellList(List<GridCell> gridCells, Color? color = null)
        {
            foreach (GridCell cell in gridCells)
            {
                ShowCursor(cell.Coordinates, color);
            }
        }

        public void HideCoordinatesFromCellList(List<GridCell> gridCells)
        {
            foreach (GridCell cell in gridCells)
            {
                HideCursor(cell.Coordinates);
            }
        }

        public void ShowCursor(GridCoordinates coordinates, Color? color = null)
        {
            gridCellVisuals.GetGridObject(coordinates)?.ShowOuter(color ?? Color.white);
        }

        public void ShowPathfinding(GridCoordinates coordinates, float rotationSpeed = 0)
        {
            gridCellVisuals.GetGridObject(coordinates).ShowInner(Color.white, rotationSpeed);
        }

        public void HideCursor(GridCoordinates coordinates)
        {
            gridCellVisuals.GetGridObject(coordinates)?.HideOuter();
        }

        public void HidePathfinding(GridCoordinates coordinates)
        {
            gridCellVisuals.GetGridObject(coordinates).HideInner();
        }

        private void Handle_GridManager_OnGridEnabled(object sender, EventArgs e)
        {
            if (gridCellVisuals == null) return;

            gameObject.SetActive(true);
            foreach (var visual in gridCellVisuals)
            {
                visual.transform.position = GridManager.Map.GetCell(visual.Coordinates).SurfacePosition;
            }
        }

        private void Handle_GridManager_OnGridDisabled(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        #endregion
        /************************************************************/
    }
}
/*
 * File Name: GridCursorController.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 24, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Kokowolo.Utilities;

namespace Kokowolo.Grid
{
    public class GridCursorController : MonoSingleton<GridCursorController>
    {
        /************************************************************/
        #region Events

        public static event EventHandler<GridCursorControllerEventArgs> OnCoordinatesChanged;

        public class GridCursorControllerEventArgs : EventArgs
        {
            public GridCoordinates previous;
            public GridCoordinates current;
        }

        #endregion
        /************************************************************/
        #region Fields

        private GridCoordinates nextCoordinates = GridCoordinates.Invalid;
        private GridCursorControllerEventArgs eventArgs = new GridCursorControllerEventArgs();

        #endregion
        /************************************************************/
        #region Properties

        public GridCoordinates Coordinates { get; private set; } = GridCoordinates.Invalid;

        #endregion
        /************************************************************/
        #region Functions

        // private void Start() 
        // {
        //     CursorManager.LayerMask = GridPipelineManager.GetGridCursorLayerMask();
        // }

        private void LateUpdate() 
        {
            UpdateGridCoordinates();
        }

        private GridCoordinates GetCoordinatesFromScreenPoint()
        {
            GridCoordinates coordinates = GridCoordinates.Invalid;
            if (CursorManager.HasValidHitInfo)
            {
                // if (General.IsGameObjectInLayerMask(CursorManager.HitInfo.transform.gameObject, LayerManager.UnitLayerMask))
                // {
                //     coordinates = CursorManager.HitInfo.transform.GetComponentInParent<Unit>().GridTransform.Coordinates;
                // }
                // else
                // {
                    coordinates = GridCoordinates.GetGridCoordinates(CursorManager.HitInfo.point);
                // }
            }   
            return coordinates;
        }

        private void UpdateGridCoordinates()
        {
            nextCoordinates = GetCoordinatesFromScreenPoint();
            // Debug.Log($"GridCursorController {nextCoordinates}");
            if (GridManager.Map.Contains(nextCoordinates) && !GridManager.Map.GetCell(nextCoordinates).IsExplorable) 
            {
                nextCoordinates = GridCoordinates.Invalid;
            }

            if (BaseInputManager.IsMouseOverUI()) 
            {
                nextCoordinates = GridCoordinates.Invalid;
            }
            else
            {
                if (!GridManager.Map.Contains(nextCoordinates) || nextCoordinates == Coordinates) return;

                GridMapVisual.Instance.HideCursor(Coordinates); // FIXME: 
                eventArgs.previous = Coordinates;
                eventArgs.current = nextCoordinates;
                Coordinates = nextCoordinates;
                OnCoordinatesChanged?.Invoke(this, eventArgs);
                GridMapVisual.Instance.ShowCursor(Coordinates);
            }        
        }

        #endregion
        /************************************************************/
    }
}
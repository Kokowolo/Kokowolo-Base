// /* 
//  * Copyright (c) 2026 Kokowolo. All Rights Reserved.
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: April 4, 2023
//  * 
//  * Additional Comments:
//  *      File Line Length: ~140
//  */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using System;
// using System.Linq;

// namespace Kokowolo.Grid
// {
//     public class GridMapVisualJob
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Events

//         public static event EventHandler OnAnyJobUpdated;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Enums

//         public enum JobType
//         {
//             None = -1,
//             Singles, 
//             Minis, 
//             Group
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields

//         JobType jobType;
//         List<GridCoordinates> coordinatesList = null;
//         Color color;
//         int priority;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         // bool HasCoordinatesList => coordinatesList != null;

//         public bool IsShowing { get; private set; }
        
//         internal int Priority => priority;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         internal GridMapVisualJob(JobType jobType, GridCoordinates coordinates, Color color, int priority)
//         {
//             this.jobType = jobType;
//             this.coordinatesList = new List<GridCoordinates>{coordinates};
//             this.color = color;
//             this.priority = priority;
//         }

//         internal GridMapVisualJob(JobType jobType, IList<GridCoordinates> coordinatesList, Color color, int priority)
//         {
//             this.jobType = jobType;
//             this.coordinatesList = new List<GridCoordinates>(coordinatesList);
//             this.color = color;
//             this.priority = priority;
//         }

//         internal GridMapVisualJob(JobType jobType, IEnumerable<IGridCoordinates> collection, Color color, int priority)
//         {
//             this.jobType = jobType;
//             SetCoordinatesListFromCollection(collection);
//             this.color = color;
//             this.priority = priority;
//         }

//         void SetCoordinatesListFromCollection(IEnumerable<IGridCoordinates> collection)
//         {
//             if (coordinatesList == null) coordinatesList = new List<GridCoordinates>();
//             else coordinatesList.Clear();
//             foreach (IGridCoordinates element in collection)
//             {
//                 coordinatesList.Add(element.Coordinates);
//             }
//         }

//         internal void Show()
//         {
//             IsShowing = true;
//             GridMapVisualObject visual;
//             foreach (GridCoordinates coordinates in coordinatesList)
//             {
//                 GridManager.Visual.Structure.TryGet(coordinates, out visual);
//                 visual.Show(jobType, color);
//             }
//         }

//         internal void Hide()
//         {
//             if (!IsShowing) return;
//             IsShowing = false;

//             GridMapVisualObject visual;
//             foreach (GridCoordinates coordinates in coordinatesList)
//             {
//                 GridManager.Visual.Structure.TryGet(coordinates, out visual);
//                 visual.Hide(jobType);
//             }
//         }

//         public void Update(GridCoordinates coordinates, in Color? color = null)
//         {
//             Hide();
//             this.coordinatesList = new List<GridCoordinates>{coordinates};
//             this.color = color ?? this.color;
//             OnAnyJobUpdated?.Invoke(this, EventArgs.Empty);
//         }

//         public void Update(List<GridCoordinates> coordinatesList, in Color? color = null)
//         {
//             Hide();
//             this.coordinatesList = new List<GridCoordinates>(coordinatesList);
//             this.color = color ?? this.color;
//             OnAnyJobUpdated?.Invoke(this, EventArgs.Empty);
//         }

//         public void Update(IEnumerable<IGridCoordinates> collection, in Color? color = null)
//         {
//             Hide();
//             SetCoordinatesListFromCollection(collection);
//             this.color = color ?? this.color;
//             OnAnyJobUpdated?.Invoke(this, EventArgs.Empty);
//         }
        
//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }
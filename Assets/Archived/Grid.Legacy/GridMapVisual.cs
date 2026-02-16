// /* 
//  * Copyright (c) 2026 Kokowolo. All Rights Reserved.
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: August 22, 2022
//  * 
//  * Additional Comments:
//  *      File Line Length: ~140
//  */

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using System;
// using Kokowolo.Utilities;

// namespace Kokowolo.Grid
// {
//     public class GridMapVisual : MonoBehaviour
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields

//         [Header("References")] 
//         [SerializeField] GridMapVisualObject gridCellVisualPrefab = null;

//         List<GridMapVisualJob> jobs = new List<GridMapVisualJob>();

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         internal GridStructure<GridMapVisualObject> Structure { get; private set; }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         void Awake() 
//         {
//             enabled = false;
            
//             GridManager.TargetMapObject.Map.OnEnabled += Handle_GridMap_OnEnabled;
//             GridManager.TargetMapObject.Map.OnDisabled += Handle_GridMap_OnDisabled;

//             GridManager.TargetMapObject.Map.OnSizeSet += Handle_GridMap_OnSizeSet;
//             GridManager.TargetMapObject.Map.OnNavigationUpdated += Handle_GridMap_OnNavigationUpdated;

//             GridMapVisualJob.OnAnyJobUpdated += Handle_GridMapVisualJob_OnAnyJobUpdated;
//         }

//         void OnDestroy()
//         {
//             GridManager.TargetMapObject.Map.OnEnabled -= Handle_GridMap_OnEnabled;
//             GridManager.TargetMapObject.Map.OnDisabled -= Handle_GridMap_OnDisabled;

//             GridManager.TargetMapObject.Map.OnSizeSet -= Handle_GridMap_OnSizeSet;
//             GridManager.TargetMapObject.Map.OnNavigationUpdated -= Handle_GridMap_OnNavigationUpdated;

//             GridMapVisualJob.OnAnyJobUpdated -= Handle_GridMapVisualJob_OnAnyJobUpdated;
//         }

//         void LateUpdate() 
//         {
//             foreach (GridMapVisualJob job in jobs)
//             {
//                 job.Show();
//             }

//             enabled = false;
//         }

//         public GridMapVisualJob CreateVisualJob(GridMapVisualJob.JobType jobType, GridCoordinates coordinates, Color? color = null, int priority = 0)
//         {
//             GridMapVisualJob job = new GridMapVisualJob(jobType, coordinates, color ?? Color.white, priority);
//             AddVisualJob(job);
//             return job;
//         }

//         public GridMapVisualJob CreateVisualJob(GridMapVisualJob.JobType jobType, IList<GridCoordinates> coordinatesList, Color? color = null, int priority = 0)
//         {
//             GridMapVisualJob job = new GridMapVisualJob(jobType, coordinatesList, color ?? Color.white, priority);
//             AddVisualJob(job);
//             return job;
//         }

//         public GridMapVisualJob CreateVisualJob(GridMapVisualJob.JobType jobType, IEnumerable<IGridCoordinates> collection, Color? color = null, int priority = 0)
//         {
//             GridMapVisualJob job = new GridMapVisualJob(jobType, collection, color ?? Color.white, priority);
//             AddVisualJob(job);
//             return job;
//         }

//         void AddVisualJob(GridMapVisualJob job)
//         {
//             int index;
//             for (index = jobs.Count; index > 0; index--)
//             {
//                 if (jobs[index - 1].Priority <= job.Priority) break;
//             }
//             jobs.Insert(index, job);
//             Refresh();
//         }

//         public void RemoveVisualJob(GridMapVisualJob job)
//         {
//             if (job == null) return;

//             job.Hide();
//             jobs.Remove(job);
//             Refresh();
//         }

//         void Refresh()
//         {
//             enabled = true;
//         }

//         void Handle_GridMap_OnEnabled(object sender, EventArgs e)
//         {
//             gameObject.SetActive(true);
//         }

//         void Handle_GridMap_OnDisabled(object sender, EventArgs e)
//         {
//             gameObject.SetActive(false);
//         }

//         void Handle_GridMap_OnSizeSet(object sender, EventArgs e)
//         {
//             if (Structure == null)
//             {
//                 Structure = new GridStructure<GridMapVisualObject>();
//             }
//             else
//             {
//                 foreach (GridMapVisualJob job in jobs)
//                 {
//                     job.Hide();
//                 }
//             }
            
//             Structure.TrySetSize(GridManager.TargetMapObject.Map.Structure.Zone, CreateGridObject);

//             GridMapVisualObject CreateGridObject(GridCoordinates coordinates)
//             {
//                 // if (!cell.Node.IsExplorable) return null;
//                 GridMapVisualObject visual = Instantiate(
//                     gridCellVisualPrefab, 
//                     GridManager.TargetMapObject.Map.GetCell(coordinates).SurfacePosition,
//                     Quaternion.identity,
//                     transform
//                 );
//                 visual.name = $"Visual {coordinates.GetIndex()}";
//                 visual.Coordinates = coordinates;
//                 return visual;
//             }
//         }

//         void Handle_GridMap_OnNavigationUpdated(object sender, EventArgs e)
//         {
//             foreach (var visual in Structure)
//             {
//                 visual.transform.position = GridManager.TargetMapObject.Map.GetCell(visual.Coordinates).SurfacePosition;
//             }
//         }

//         void Handle_GridMapVisualJob_OnAnyJobUpdated(object sender, EventArgs e)
//         {
//             Refresh();
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }
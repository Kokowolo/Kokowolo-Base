// /* 
//  * Copyright (c) 2026 Kokowolo. All Rights Reserved.
//  * Author(s): Kokowolo, Will Lacey
//  * Date Created: August 22, 2022
//  * 
//  * Additional Comments:
//  *      File Line Length: ~140
//  */

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using Kokowolo.Utilities;

// namespace Kokowolo.Grid
// {
//     public class GridMapVisualObject : MonoBehaviour, IGridObject
//     {
//         /*██████████████████████████████████████████████████████████*/
//         #region Fields

//         [SerializeField] SpriteRenderer singleSpriteRenderer;
//         [SerializeField] SpriteRenderer miniSpriteRenderer;
//         [SerializeField] SpriteRenderer groupSpriteRenderer;

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Properties

//         public GridCoordinates Coordinates { get; set; }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//         #region Functions

//         void Awake() 
//         {
//             singleSpriteRenderer.transform.localScale *= GridMetrics.CellSize;
//             miniSpriteRenderer.transform.localScale *= GridMetrics.CellSize;

//             miniSpriteRenderer.transform.rotation = Quaternion.Euler(90, 0, UnityEngine.Random.Range(0, 360));

//             Hide();
//         }

//         bool destroyed = false;
//         void OnDestroy()
//         {
//             if (destroyed) return;
//             destroyed = true;
//             Dispose();
//         }

//         bool disposed;
//         ~GridMapVisualObject() => Dispose();
//         public void Dispose()
//         {
//             if (disposed) return;
//             disposed = true;
//             GC.SuppressFinalize(this);
//             if (!destroyed) Destroy(gameObject);
//         }

//         internal void Show(GridMapVisualJob.JobType jobType, Color color)
//         {
//             switch (jobType)
//             {
//                 case GridMapVisualJob.JobType.Singles:
//                     singleSpriteRenderer.enabled = true;
//                     singleSpriteRenderer.color = color;
//                     break;
//                 case GridMapVisualJob.JobType.Minis:
//                     miniSpriteRenderer.enabled = true;
//                     miniSpriteRenderer.color = color;
//                     break;
//                 case GridMapVisualJob.JobType.Group:
//                     groupSpriteRenderer.enabled = true;
//                     groupSpriteRenderer.color = color;
//                     break;
//             }
//         }

//         internal void Hide(GridMapVisualJob.JobType jobType)
//         {
//             switch (jobType)
//             {
//                 case GridMapVisualJob.JobType.Singles:
//                     singleSpriteRenderer.enabled = false;
//                     break;
//                 case GridMapVisualJob.JobType.Minis:
//                     miniSpriteRenderer.enabled = false;
//                     break;
//                 case GridMapVisualJob.JobType.Group:
//                     groupSpriteRenderer.enabled = false;
//                     break;
//             }
//         }

//         internal void Hide()
//         {
//             Hide(jobType: GridMapVisualJob.JobType.Singles);
//             Hide(jobType: GridMapVisualJob.JobType.Minis);
//             Hide(jobType: GridMapVisualJob.JobType.Group);
//         }

//         #endregion
//         /*██████████████████████████████████████████████████████████*/
//     }
// }
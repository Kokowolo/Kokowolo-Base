/* 
 * Copyright (c) 2026 Kokowolo. All Rights Reserved.
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 15, 2026
 * 
 * Additional Comments:
 *      File Line Length: ~140
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

namespace Kokowolo.Grid
{
    public class TestGridOverlayHandler : MonoBehaviour, IGridOverlayHandler
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        List<GridOverlay> overlays; // priority is sorted from least to greatest

        GridStructure<TestGridOverlayWrapper> structure;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        void Awake()
        {
            GridManager.TargetMapObject.Map.OnSizeSet += Handle_GridMap_OnSizeSet;

            overlays = new();
        }

        void OnDestroy()
        {
            GridManager.TargetMapObject.Map.OnSizeSet -= Handle_GridMap_OnSizeSet;

            GridOverlay overlay;
            for (int i = overlays.Count - 1; i >= 0 ; i--)
            {
                overlay = overlays[i];
                overlays.RemoveAt(i);
                DisposeInternal(overlay);
            }
            overlays.Clear();
        }

        void Refresh()
        {
            for (int i = 0; i < overlays.Count; i++)
            {
                Show(overlays[i]);
            }
        }

        public GridOverlay Overlay(GridOverlay.Type overlayType, ref List<GridCoordinates> coordsList, Color color, int priority)
        {
            GridOverlay overlay = new GridOverlay(overlayType, coordsList, color, priority);

            bool added = false;
            for (int i = 0; i < overlays.Count; i++)
            {
                if (overlays[i].Priority > overlay.Priority)
                {
                    overlays.Insert(i, overlay);
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                overlays.Add(overlay);
            }
            Refresh();
            return overlay;
        }

        void Show(GridOverlay overlay)
        {
            for (int i = 0; i < overlay.CoordinatesList.Count; i++)
            {
                structure.TryGet(overlay.CoordinatesList[i], out TestGridOverlayWrapper wrapper);
                if (!wrapper.TestGameObject)
                {
                    wrapper.TestGameObject = PoolManager.Get<TestGridOverlayGameObject>();
                }
                wrapper.TestGameObject.Show(overlay.OverlayType, overlay.Color);
            }
        }

        bool IGridOverlayHandler.Dispose(GridOverlay overlay)
        {
            if (!overlays.Contains(overlay)) return false;
            bool removed = overlays.Remove(overlay);
            DisposeInternal(overlay);
            Refresh();
            return removed;
        }

        void DisposeInternal(GridOverlay overlay)
        {
            for (int i = 0; i < overlay.CoordinatesList.Count; i++)
            {
                structure.TryGet(overlay.CoordinatesList[i], out TestGridOverlayWrapper wrapper);
                wrapper.TestGameObject.Hide();
                PoolManager.Add(wrapper.TestGameObject);
                wrapper.TestGameObject = null;
            }
            overlay.Dispose();
        }

        void Handle_GridMap_OnSizeSet(object sender, EventArgs e) => GenerateStructure();
        void GenerateStructure() => (structure ??= new()).TrySetSize(GridManager.TargetMapObject.Map.Structure.Zone, CreateTestGridOverlayWrapper);
        TestGridOverlayWrapper CreateTestGridOverlayWrapper(GridCoordinates coordinates)
        {
            return new TestGridOverlayWrapper(coordinates);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Subclasses

        class TestGridOverlayWrapper : IGridObject
        {
            TestGridOverlayGameObject _TestGameObject;
            public TestGridOverlayGameObject TestGameObject
            {
                get => _TestGameObject;
                set
                {
                    _TestGameObject = value;
                    if (!value) return;
                    _TestGameObject.transform.position = GridPositioning.GetPosition(Coordinates, GridPositioning.Space.World_Surface);
                }
            }

            public GridCoordinates Coordinates { get; set; }

            public TestGridOverlayWrapper(GridCoordinates coordinates)
            {
                Coordinates = coordinates;
            }

            bool disposed;
            ~TestGridOverlayWrapper() => Dispose();
            public void Dispose()
            {
                if (disposed) return;
                disposed = true;
                GC.SuppressFinalize(this);
                TestGameObject.Dispose();
            }
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}
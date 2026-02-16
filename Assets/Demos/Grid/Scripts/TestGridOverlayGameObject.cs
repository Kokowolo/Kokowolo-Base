/* 
 * Copyright (c) 2026 Kokowolo. All Rights Reserved.
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 22, 2022
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
    public class TestGridOverlayGameObject : MonoBehaviour, IPoolableMonoBehaviour
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        [SerializeField] SpriteRenderer singleSpriteRenderer;
        [SerializeField] SpriteRenderer miniSpriteRenderer;
        [SerializeField] SpriteRenderer groupSpriteRenderer;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        void Awake() 
        {
            // NOTE: this randomization is visible with pooling active
            singleSpriteRenderer.transform.localScale *= GridMetrics.CellSize;
            miniSpriteRenderer.transform.localScale *= GridMetrics.CellSize;
            miniSpriteRenderer.transform.rotation = Quaternion.Euler(90, 0, UnityEngine.Random.Range(0, 360));
            Hide();
        }

        bool destroyed = false;
        void OnDestroy()
        {
            if (destroyed) return;
            destroyed = true;
            Dispose();
        }

        bool disposed;
        ~TestGridOverlayGameObject() => Dispose();
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            GC.SuppressFinalize(this);
            if (!destroyed) Destroy(gameObject);
        }

        internal void Show(GridOverlay.Type style, Color color)
        {
            switch (style)
            {
                case GridOverlay.Type.Singles:
                    singleSpriteRenderer.enabled = true;
                    singleSpriteRenderer.color = color;
                    break;
                case GridOverlay.Type.Minis:
                    miniSpriteRenderer.enabled = true;
                    miniSpriteRenderer.color = color;
                    break;
                case GridOverlay.Type.Group:
                    groupSpriteRenderer.enabled = true;
                    groupSpriteRenderer.color = color;
                    break;
            }
        }

        internal void Hide(GridOverlay.Type style)
        {
            switch (style)
            {
                case GridOverlay.Type.Singles:
                    singleSpriteRenderer.enabled = false;
                    break;
                case GridOverlay.Type.Minis:
                    miniSpriteRenderer.enabled = false;
                    break;
                case GridOverlay.Type.Group:
                    groupSpriteRenderer.enabled = false;
                    break;
            }
        }

        internal void Hide()
        {
            Hide(style: GridOverlay.Type.Singles);
            Hide(style: GridOverlay.Type.Minis);
            Hide(style: GridOverlay.Type.Group);
        }

        void IPoolable.OnAddedToPool()
        {
            transform.SetParent(PoolManager.Instance.transform);
            // gameObject.SetActive(false);
        }

        void IPoolable.OnRemovedFromPool()
        {
            transform.SetParent(((TestGridOverlayHandler) GridManager.TargetMapObject.OverlayHandler).transform);
            // gameObject.SetActive(true);
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}
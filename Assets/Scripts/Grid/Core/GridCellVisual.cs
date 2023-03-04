/*
 * File Name: GridCellVisual.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: August 22, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

namespace Kokowolo.Grid
{
    public class GridCellVisual : MonoBehaviour
    {
        /************************************************************/
        #region Fields

        [SerializeField] private SpriteRenderer outerSpriteRenderer;
        [SerializeField] private SpriteRenderer innerSpriteRenderer;
        [SerializeField] private Rotator innerRotator;

        private int activeOuterCount = 1;
        private int activeInnerCount = 1;

        #endregion
        /************************************************************/
        #region Properties

        public GridCoordinates Coordinates { get; set; }

        // public bool IsVisible => ActiveOuterCount > 0 || ActiveInnerCount > 0;

        private int ActiveOuterCount
        {
            get => activeOuterCount;
            set
            {
                activeOuterCount = value;
                if (activeOuterCount < 0) 
                {
                    activeOuterCount = 0;
                    LogManager.LogWarning($"{name} had activeOuterCount less than 0");
                }
            }
        }

        private int ActiveInnerCount
        {
            get => activeInnerCount;
            set
            {
                activeInnerCount = value;
                if (activeInnerCount < 0) 
                {
                    activeInnerCount = 0;
                    LogManager.LogWarning($"{name} had activeInnerCount less than 0");
                }
            }
        }

        #endregion
        /************************************************************/
        #region Functions

        private void Awake() 
        {
            outerSpriteRenderer.transform.localScale *= GridMetrics.CellSize;
            innerSpriteRenderer.transform.localScale *= GridMetrics.CellSize;

            innerSpriteRenderer.transform.rotation = Quaternion.Euler(90, 0, Random.Range(0, 360));

            Hide();
        }

        public void ShowOuter(Color color)
        {
            ActiveOuterCount++;
            outerSpriteRenderer.enabled = true;
            outerSpriteRenderer.color = color;
        }

        public void HideOuter()
        {
            ActiveOuterCount--;
            if (ActiveOuterCount > 0) return;

            outerSpriteRenderer.enabled = false;
        }

        public void ShowInner(Color color, float rotationSpeed = 0)
        {
            ActiveInnerCount++;
            innerSpriteRenderer.enabled = true;
            innerSpriteRenderer.color = color;

            innerRotator.enabled = true;
            innerRotator.speed = new Vector3(0, 0, rotationSpeed);
        }

        public void HideInner()
        {
            ActiveInnerCount--;
            if (ActiveInnerCount > 0) return;

            innerSpriteRenderer.enabled = false;
            innerRotator.enabled = false;
        }

        public void Hide()
        {
            HideOuter();
            HideInner();
        }

        #endregion
        /************************************************************/
    }
}
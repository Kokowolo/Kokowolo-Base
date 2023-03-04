/*
 * File Name: GridObstacle.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: November 14, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kokowolo.Grid
{
    public class GridObstacle : MonoBehaviour
    {
        /************************************************************/
        #region Fields

        #endregion
        /************************************************************/
        #region Properties

        #endregion
        /************************************************************/
        #region Functions

        private void Awake() 
        {
            // GridCoordinates coordinates = GridCoordinates.GetGridCoordinates(transform.position);
            // GridCell cell = GridManager.Map.GetCell(coordinates);
            // transform.position = cell.WorldPosition;
        }

        private void Start()
        {
            GridCoordinates coordinates = GridCoordinates.GetGridCoordinates(transform.position);
            GridCell cell = GridManager.Map.GetCell(coordinates);
            if (cell != null) cell.Node.IsExplorable = false;
        }
        
        #endregion
        /************************************************************/
    }
}
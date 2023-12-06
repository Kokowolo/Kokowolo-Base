/*
 * File Name: TestRunner2.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: November 28, 2023
 * 
 * Additional Comments:
 *		File Line Length: 140
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;
using Kokowolo.Grid;
using System;

public class TestRunner2 : MonoBehaviour
{
    /************************************************************/
    #region Fields
    
    [SerializeField] bool useIndex = false;
    [SerializeField] bool useCubeCoordinates = false;
    [SerializeField] GridCoordinates a;
    [SerializeField] GridCoordinates b;
    [SerializeField] GridDirection d;
    [SerializeField] GridDirection shift;

    #endregion
    /************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     GridCoordinates c = b.RotateAround(a, d);
        //     LogManager.Log(c.ToString(useIndex, useCubeCoordinates));

        //     LogManager.Log(d.RotateBy(shift));
        // }
    }

    #endregion
    /************************************************************/
}
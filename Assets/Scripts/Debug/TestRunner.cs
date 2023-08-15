/*
 * File Name: TestRunner.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 29, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;
using Kokowolo.Grid;
using System;

public class TestRunner : MonoBehaviour//, IPoolable<TestRunner>
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
        List<int> ints = new List<int>();
        Debug.Log(ints.Capacity);
        ints.Add(0);
        ints.Add(1);
        ints.Add(2);
        ints.Add(3);
        ints.Add(4);
        Debug.Log(ints.Capacity);
        Debug.Log(ints[1]);
        ints.Clear();
        Debug.Log(ints.Capacity);
        Debug.Log(ints[1]);
        ints.Capacity = 3;
        Debug.Log(ints.Capacity);
        Debug.Log(ints[1]);
    }

    #endregion
    /************************************************************/
}
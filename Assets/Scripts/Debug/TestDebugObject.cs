/*
 * File Name: TestDebugObject.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: September 7, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebugObject : MonoBehaviour
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
        Debug.Log($"Hi {name}");
    }

    #endregion
    /************************************************************/
}
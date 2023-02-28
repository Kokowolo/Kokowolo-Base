/*
 * File Name: TestMonoSingleton.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: February 16, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

public class TestMonoSingleton : MonoSingleton<TestMonoSingleton>
{
    /************************************************************/
    #region Fields

    #endregion
	/************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    protected override void MonoSingleton_Awake()
    {
        Debug.Log("hi");
    }
    
    #endregion
    /************************************************************/
}
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

public class TestRunner : MonoBehaviour
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
        // Debug.LogWarning("asdasd");
        LogManager.Log("Hiiii", gameObject);
        LogManager.LogWarning("Warning Hiiii", gameObject);
        LogManager.LogError("Error Hiiii", gameObject);
        LogManager.Log($"<color=#00FF00>All files processed...</color>");

        Debug.Log("2Hiiii", gameObject);
        Debug.LogWarning("2Warning Hiiii", gameObject);
        Debug.LogError("2Error Hiiii", gameObject);
        Debug.Log($"2<color=#00FF00>All files processed...</color>");
    }

    #endregion
    /************************************************************/
}
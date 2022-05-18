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
        Debug.Log(transform.childCount);
        Transform bone = transform.Find("5");
        Debug.LogWarning(bone);
        
        Debug.Log(transform.hierarchyCount);
        bone = transform.RecursiveFind("5");
        Debug.LogWarning(bone);

        Debug.Log(transform.hierarchyCount);
        bone = transform.RecursiveFind("53");
        Debug.LogWarning(bone);
    }

    private void Start() 
    {
        
    }

    private void OnDestroy() 
    {
        
    }

    #endregion
    /************************************************************/
}
/*
 * File Name: TestRunner.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 29, 2022
 * 
 * Additional Comments:
 *		While this file has been updated to better fit this project, the original version can be found here:
 *		https://catlikecoding.com/unity/tutorials/hex-map/
 *
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

    MeshCollider _meshCollider;

    #endregion
	/************************************************************/
    #region Properties

    MeshCollider MeshCollider => _meshCollider;

    #endregion
    /************************************************************/
    #region Functions

    private void Awake() 
    {
        Debug.Log(MeshCollider.name);
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
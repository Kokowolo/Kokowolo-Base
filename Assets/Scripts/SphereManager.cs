/*
 * File Name: SphereManager.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 28, 2022
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

public class SphereManager : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Tooltip("this is a float")]
    [SerializeField, Range(0, 1)] float myFloat = 0f;

    private static SphereManager _singleton = null;

    #endregion
    /************************************************************/
    #region Properties

    public static SphereManager Singleton
    {
        get
        {
            if (!_singleton) _singleton = FindObjectOfType<SphereManager>();
            return _singleton;
        }
        private set => _singleton = value;
    }

    #endregion
    /************************************************************/
    #region Functions

    #region Unity Functions

    private void Awake()
    {
        //Subscribe();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        //Unsubscribe();
    }

    #endregion

    #endregion
    /************************************************************/
    #region Debug
    #if UNITY_EDITOR

    #endif
    #endregion
    /************************************************************/
}
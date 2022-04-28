/*
 * File Name: Singleton.cs
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

using Kokowolo.Utilities;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /************************************************************/
    #region Functions

    private static T _instance;

    #endregion
    /************************************************************/
    #region Properties

    public static T Instance
    {
        get
        {
            if (!_instance) _instance = FindObjectOfType<T>();
            return _instance;
        }
        set
        {
            General.GetCallingMethod();
        }
    }

    #endregion
    /************************************************************/
    #region Functions



    #endregion
    /************************************************************/
}
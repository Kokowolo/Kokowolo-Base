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
        List<int> ints = new List<int>();

        ints.Add(3);   
        ints.Add(2);
        ints.Add(1);

        LogList(ints);

        ints.Swap(0, 2);

        LogList(ints);
    }
    
    private void LogList<T>(List<T> list)
    {
        Debug.Log("Logging List");
        foreach (T t in list)
        {
            Debug.Log(t.ToString());
        }
    }

    #endregion
    /************************************************************/
}
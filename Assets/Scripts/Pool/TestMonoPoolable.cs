/*
 * File Name: TestMonoPoolable.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: April 11, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

public class TestMonoPoolable : MonoBehaviour, IPoolable<TestMonoPoolable>
{
    /************************************************************/
    #region Fields

    private static int count = 0;

    #endregion
	/************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void OnEnable() 
    {
        name = $"Mono {count++}";
        Debug.Log($"Hi, I'm {name}");
    }

    public static TestMonoPoolable Create()
    {
        return Instantiate(PrefabManager.Get<TestMonoPoolable>());
    }

    public void OnAddPoolable()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(PoolSystem.Instance.transform);
    }

    public void OnGetPoolable()
    {
        gameObject.SetActive(true);
        gameObject.transform.SetParent(FindObjectOfType<TestRunner>().transform);
    }

    #endregion
    /************************************************************/
}
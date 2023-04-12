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

    [SerializeField] private TestMonoPoolable prefab = null;

    List<TestMonoPoolable> monos = new List<TestMonoPoolable>();

    #endregion
    /************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestMonoPoolable poolable = PoolSystem.Get<TestMonoPoolable>();
            monos.Add(poolable);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestMonoPoolable poolable = Pop();
            if (poolable) PoolSystem.Add(poolable);
        }
    }

    private TestMonoPoolable Pop()
    {
        if (monos.Count == 0)
        {
            Debug.LogWarning("No more monos");
            return null;
        }
        else
        {
            TestMonoPoolable poolable = monos[monos.Count - 1];
            monos.RemoveAt(monos.Count - 1);
            return poolable;
        }
    }

    #endregion
    /************************************************************/
}
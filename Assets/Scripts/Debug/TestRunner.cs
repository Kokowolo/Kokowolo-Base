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

    [SerializeField] TestRunner prefab;

    #endregion
	/************************************************************/
    #region Properties

    TestRunner MonoBehaviourPrefab => PrefabManager.Get<TestRunner>();
    TestScriptableObject ScriptableObjectPrefab => PrefabManager.Get<TestScriptableObject>();

    #endregion
    /************************************************************/
    #region Functions

    private void Awake() 
    {
        Instantiate(PrefabManager.Get<TestDebugObject>());
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Instantiate(MonoBehaviourPrefab);
        } 
        else if (Input.GetMouseButtonDown(1)) 
        {
            Debug.Log(ScriptableObjectPrefab.val);
        }
    }

    private void OnDestroy() 
    {
        
    }

    #endregion
    /************************************************************/
}
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
    #region Properties

    public static TestRunner Instance => Singleton.Get<TestRunner>();

    #endregion
    /************************************************************/
    #region Functions

    private void Awake()
    {
        if (!Singleton.TrySet(this)) return; 

        // ^ handles possible second instance when reloading scene
        // continue...

        Debug.Log("Awake");
    }

    private void OnDestroy()
    {
        if (!Singleton.IsSingleton(this)) return;

        // ^ handles possible second instance when reloading scene
        // continue...

        Debug.Log("OnDestroy");
    }

    private void Start()
    {
        Debug.Log("Start");
    }

    private void Update()
    {
        Debug.Log("Update");
    }

    #endregion
    /************************************************************/
}
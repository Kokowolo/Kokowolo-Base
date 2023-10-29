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

public class TestRunner : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [Header("Cached References")]
    [SerializeField] List<GameObject> cubes;

    [Header("Settings")]
    [SerializeField] float min = - 1;
    [SerializeField] float max = 1;
    [SerializeField] float speed = 1;
    
    List<int> selectedCubeIndexes = new List<int>();
    List<ScheduledEvent> scheduledEvents = new List<ScheduledEvent>();

    float target;

    #endregion
    /************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void Awake()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            scheduledEvents.Add(null);
        }
    }

    private IEnumerator Long()
    {
        int count=0;
        while (count < 5)
        {
            LogManager.Log("Long");
            count++;
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator Short()
    {
        LogManager.Log("Short");
        yield return new WaitForSeconds(1);
        LogManager.Log("Short end");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectCubes();
            MoveSelectedCubes();
        }
    }

    private void SelectCubes()
    {
        selectedCubeIndexes.Clear();
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selectedCubeIndexes.Add(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            selectedCubeIndexes.Add(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            selectedCubeIndexes.Add(2);
        }
    }

    private void MoveSelectedCubes()
    {
        // Clear Previous Coroutines
        for (int i = 0; i < scheduledEvents.Count; i++)
        {
            ScheduledEventManager.StopEvent(scheduledEvents[i]);
        }

        // Tween Cubes
        for (int i = 0; i < selectedCubeIndexes.Count; i++)
        {
            int index = selectedCubeIndexes[i];
            // float target = cubes[index].transform.position.x > (max + min) / 2 ? min : max;
            Action<float> setMethod = (float x) =>
            {
                Vector3 pos = cubes[index].transform.position;
                pos.x = x;
                cubes[index].transform.position = pos;
            };
            float a = cubes[index].transform.position.x > Mathf.Abs(max + min) / 2 ? max : min;
            float b = cubes[index].transform.position.x > Mathf.Abs(max + min) / 2 ? min : max;
            if (index == 2)
            {
                scheduledEvents[index] = Tween.Loglerp(cubes[index].transform.position.x, a, b, speed, setMethod);
            }
            else
            {
                scheduledEvents[index] = Tween.Lerp(cubes[index].transform.position.x, a, b, speed, setMethod);
            }
            
        }
    }

    #endregion
    /************************************************************/
}
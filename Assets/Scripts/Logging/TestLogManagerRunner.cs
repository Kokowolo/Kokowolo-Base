/*
 * File Name: TestRunner.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: November 15, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.Utilities;

public class TestLogManagerRunner : MonoBehaviour
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
        Debug.Log("start! 1");

        LogManager.Log("start! 2");

        Logger logger = new Logger(Debug.unityLogger.logHandler);

        LogManager.Log(logger, "hi");

        LogManager.Log(logger, "hi but green", color: Color.green);

        logger.logEnabled = false;
        LogManager.Log(logger, "hi but you cant see me");
        LogManager.Log("hi but you CAN see me");

        logger.logEnabled = true;
        LogManager.LogWarning(logger, "hi again");

        LogManager.LogError("bye");

        System.Exception exception = new System.Exception("exception message!");
        LogManager.LogException(exception);
        LogManager.LogError("unreachable message");

        // CONSOLE SHOULD LOOK LIKE (ignoring tags)
        // start! 1
        // start! 2
        // hi
        // hi but green (green)
        // hi but you CAN see me <---- Just kidding this message isn't here
        // hi again (warning)
        // bye (error)
        // System.Exception: exception message! (exception)
        // Exception: exception message! (System.Exception)
    }

    #endregion
    /************************************************************/
}
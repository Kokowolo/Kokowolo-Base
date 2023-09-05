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
        Logger logger = new Logger(Debug.unityLogger.logHandler);
        // LogManager.LogWarning(combatLog, "hi", Color.green);
        LogManager.Log(logger, "hi");
        LogManager.Log(logger, "hi but green", color: Color.green);
        logger.logEnabled = false;
        LogManager.Log(logger, "hi but you cant see me");
        LogManager.Log("hi but you can see me");
        logger.logEnabled = true;
        LogManager.LogWarning(logger, "hi again");
        LogManager.LogError("bye");

        // CONSOLE SHOULD LOOK LIKE
        // hi
        // hi but green (green)
        // hi but you cant see me
        // hi again (warning)
        // bye (error)
    }

    #endregion
    /************************************************************/
}
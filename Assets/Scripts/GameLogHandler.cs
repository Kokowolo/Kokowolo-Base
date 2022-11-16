/*
 * File Name: GameLogHandler.cs
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

using System;
using System.IO;
using System.Globalization;

public class GameLogHandler : ILogHandler
{
    /************************************************************/
    #region Fields

    private bool showClass;

    private FileStream fileStream;
    private StreamWriter streamWriter;
    private ILogHandler defaultLogHandler;

    #endregion
	/************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    public GameLogHandler(bool showClass)
    {
        this.showClass = showClass;
        defaultLogHandler = Debug.unityLogger.logHandler;
        
        string filePath = Path.Combine(Application.persistentDataPath, "GameLog.txt");

        File.WriteAllText(filePath, string.Empty);
        fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        streamWriter = new StreamWriter(fileStream);
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        streamWriter.WriteLine(GetFormattedString(logType, context, format, args));
        streamWriter.Flush();
        format = GetFormattedString(LogType.Log, context, format, args);
        defaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        defaultLogHandler.LogException(exception, context);
    }

    private string GetFormattedString(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        switch (logType)
        {
            case LogType.Warning:
                if (showClass) return $"(Warning) {String.Format(format, args)}";
                else return $"(Warning) [{GetCallingClassName(6)}] {String.Format(format, args)}";
            case LogType.Error:
                if (showClass) return $"(Error) {String.Format(format, args)}";
                else return $"(Warning) [{GetCallingClassName(6)}] {String.Format(format, args)}";
            default:
                return $"[{GetCallingClassName(5)}] {String.Format(format, args)}";
        }
    }

    private static string GetCallingClassName(int count)
    {   
        string className = new System.Diagnostics.StackTrace().GetFrame(count).GetMethod().ReflectedType.Name;
        if (className == "LogManager")
        {
            return new System.Diagnostics.StackTrace().GetFrame(count + 1).GetMethod().ReflectedType.Name;
        }
        return className;
    }

    #endregion
    /************************************************************/
}
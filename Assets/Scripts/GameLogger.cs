/*
 * File Name: GameLogger.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: November 15, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120

// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Globalization;

public class GameLogger : ILogHandler
{
    /************************************************************/
    #region Fields

    private const string kNoTagFormat = "{0}";
    private const string kTagFormat = "{0}: {1}";

    #endregion
    /************************************************************/
    #region Properties

    public ILogHandler logHandler { get; set; }

    public bool logEnabled { get; set; }

    public LogType filterLogType { get; set; }

    #endregion
    /************************************************************/
    #region Fields

    public GameLogger(ILogHandler logHandler)
    {
        this.logHandler = logHandler;
        this.logEnabled = true;
        this.filterLogType = LogType.Log;
    }

    public bool IsLogTypeAllowed(LogType logType)
    {
        if (logEnabled)
        {
            if (logType == LogType.Exception) return true;

            if (filterLogType != LogType.Exception) return (logType <= filterLogType);
        }
        return false;
    }

    private static string GetString(object message)
    {
        if (message == null)
        {
            return "Null";
        }
        var formattable = message as IFormattable;
        if (formattable != null)
        {
            return "aaaa" + formattable.ToString(null, CultureInfo.InvariantCulture);
        }
        else
        {
            return "aaaa" + message.ToString();
        }
    }

    public void Log(LogType logType, object message)
    {
        if (IsLogTypeAllowed(logType)) 
        {
            logHandler.LogFormat(logType, null, kNoTagFormat, new object[] {GetString(message)});
        }
    }

    public void Log(LogType logType, object message, UnityEngine.Object context)
    {
        if (IsLogTypeAllowed(logType))
        {
            logHandler.LogFormat(logType, context, kNoTagFormat, new object[] {GetString(message)});
        }
    }

    public void Log(LogType logType, string tag, object message)
    {
        if (IsLogTypeAllowed(logType))
        {
            logHandler.LogFormat(logType, null, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
    {
        if (IsLogTypeAllowed(logType))
        {
            logHandler.LogFormat(logType, context, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void Log(object message)
    {
        if (IsLogTypeAllowed(LogType.Log))
        {
            logHandler.LogFormat(LogType.Log, null, kNoTagFormat, new object[] {GetString(message)});
        }
    }

    public void Log(string tag, object message)
    {
        if (IsLogTypeAllowed(LogType.Log))
        {
            logHandler.LogFormat(LogType.Log, null, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void Log(string tag, object message, UnityEngine.Object context)
    {
        if (IsLogTypeAllowed(LogType.Log))
        {
            logHandler.LogFormat(LogType.Log, context, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void LogWarning(string tag, object message)
    {
        if (IsLogTypeAllowed(LogType.Warning))
        {
            logHandler.LogFormat(LogType.Warning, null, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context)
    {
        if (IsLogTypeAllowed(LogType.Warning))
        {
            logHandler.LogFormat(LogType.Warning, context, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void LogError(string tag, object message)
    {
        if (IsLogTypeAllowed(LogType.Error))
        {
            logHandler.LogFormat(LogType.Error, null, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void LogError(string tag, object message, UnityEngine.Object context)
    {
        if (IsLogTypeAllowed(LogType.Error))
        {
            logHandler.LogFormat(LogType.Error, context, kTagFormat, new object[] {tag, GetString(message)});
        }
    }

    public void LogException(Exception exception)
    {
        if (logEnabled)
        {
            logHandler.LogException(exception, null);
        }
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if (logEnabled)
        {
            logHandler.LogException(exception, context);
        }
    }

    public void LogFormat(LogType logType, string format, params object[] args)
    {
        if (IsLogTypeAllowed(logType))
        {
            logHandler.LogFormat(logType, null, format, args);
        }
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (IsLogTypeAllowed(logType))
        {
            logHandler.LogFormat(logType, context, format, args);
        }
    }

    #endregion
    /************************************************************/
}
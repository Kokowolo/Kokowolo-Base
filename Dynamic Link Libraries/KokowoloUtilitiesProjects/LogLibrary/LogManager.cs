/*
 * File Name: LogManager.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: November 14, 2022
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

// #define USE_LOGGER 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Globalization;
using System.Text;

namespace Kokowolo.Utilities//.Analytics
{
    public static class LogManager
    {
        /************************************************************/
        #region Fields

        public const string LogManagerProfileString = "LogManager Profile";
        private static readonly Logger defaultLogger = new Logger(Debug.unityLogger.logHandler);

        private static bool hasAttemptedLoad;
        private static int stackTraceDepth;

        #endregion
        /************************************************************/
        #region Properties

        private static StringBuilder _StringBuilder;
        private static StringBuilder StringBuilder 
        {
            get
            {
                _StringBuilder ??= new StringBuilder(capacity: 256);
                return _StringBuilder;
            }
        }

        private static LogManagerProfile _Profile;
        private static LogManagerProfile Profile
        {
            get
            {
                if (!_Profile && !hasAttemptedLoad)
                {
                    hasAttemptedLoad = true;
                    _Profile = Resources.Load<LogManagerProfile>(LogManagerProfileString);
                }
                return _Profile;
            }
        }

        // default properties without an initialized LogManagerProfile
        private static bool LogMessageWithClassTag => !Profile || Profile.LogMessageWithClassTag;
        private static bool ThrowWhenLoggingException => Profile && Profile.ThrowWhenLoggingException;

        // other properties
        private static bool LogMessageWithColorTag => Application.isEditor;

        #endregion
        /************************************************************/
        #region Functions

        public static void Log(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Log, defaultLogger, message, context, color);
        }

        public static void Log(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Log, logger, message, context, color);
        }

        public static void LogWarning(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Warning, defaultLogger, message, context, color);
        }

        public static void LogWarning(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Warning, logger, message, context, color);
        }

        public static void LogError(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Error, defaultLogger, message, context, color);
        }

        public static void LogError(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Error, logger, message, context, color);
        }

        public static void LogException(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Exception, defaultLogger, message, context, color);
            if (ThrowWhenLoggingException)
            {
                throw new Exception(StringBuilder.ToString());
            }
        }

        public static void LogException(Exception exception, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Exception, defaultLogger, exception, context, color);
            if (ThrowWhenLoggingException)
            {
                throw exception;
            }
        }

        public static void LogException(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Exception, logger, message, context, color);
            if (ThrowWhenLoggingException)
            {
                throw new Exception(StringBuilder.ToString());
            }
        }

        public static void LogException(Logger logger, Exception exception, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Exception, logger, exception, context, color);
            if (ThrowWhenLoggingException)
            {
                throw exception;
            }
        }

        // [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void Log(LogType logType, Logger logger, object message, UnityEngine.Object context, Color? color)
        {
            stackTraceDepth = 4;
            // stackTraceDepth = stackTraceDepth > 3 ? stackTraceDepth : 3;

            StringBuilder.Clear();
            BuildMessageWithClassTag(message);
            BuildMessageWithColorTag(color);

            Debug.unityLogger.logHandler = logger;
            logger.Log(logType, StringBuilder.ToString(), context: context);
            Debug.unityLogger.logHandler = defaultLogger;
            stackTraceDepth = 0;
        }

        private static void BuildMessageWithClassTag(object message)
        {
            if (!LogMessageWithClassTag)
            {
                StringBuilder.Append(message);
            }
            else if (message == null) 
            {
                StringBuilder.Append($"[{GetCallingClassName(stackTraceDepth)}] Null");
            }
            else if (message is IFormattable formattable)
            {
                StringBuilder.Append(
                    $"[{GetCallingClassName(stackTraceDepth)}] {formattable.ToString(null, CultureInfo.InvariantCulture)}"
                );
            }
            else
            {
                StringBuilder.Append($"[{GetCallingClassName(stackTraceDepth)}] {message}");
            }
        }

        private static void BuildMessageWithColorTag(Color? color = null)
        {
            if (!LogMessageWithColorTag || color == null) return;
            StringBuilder.Insert(0, $"<color=#{ColorUtility.ToHtmlStringRGB((Color) color)}>");
            StringBuilder.Append("</color>");
        }

        private static string GetCallingClassName(int stackTraceDepth)
        {   
            return new System.Diagnostics.StackTrace().GetFrame(stackTraceDepth).GetMethod().ReflectedType.Name;
        }

        #endregion
        /************************************************************/
    }
}
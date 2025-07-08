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
// using System.Reflection;
using System.Globalization;
using System.Text;

namespace Kokowolo.Utilities//.Analytics
{
    public static class LogManager
    {
        /*██████████████████████████████████████████████████████████*/
        #region Fields

        // cached references
        static Logger unityLogger;

        // variables
        static int stackTraceDepth;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Properties

        static StringBuilder _StringBuilder;
        static StringBuilder StringBuilder => _StringBuilder ??= new StringBuilder(capacity: 256);

        static LogManagerProfile _Profile;
        public static LogManagerProfile Profile
        {
            get
            {
                TryInitialize();
                return _Profile;
            }
            set
            {
                _Profile = value;
                if (_Profile) _Profile.OnLoad();
                else LogWarning("could not propertly initialize");
            }
        }

        public static Logger UnityLogger// => Debug.unityLogger.logHandler; this doesn't work and i don't get it :(
        {
            get
            {
                TryInitialize();
                return unityLogger;
            }
        }

        // default properties without an initialized LogManagerProfile
        static bool LogMessageWithClassTag => !Profile || Profile.LogMessageWithClassTag;
        static bool ThrowWhenLoggingException => Profile && Profile.ThrowWhenLoggingException;

        // other properties
        static bool LogMessageWithColorTag => Application.isEditor;

        #endregion
        /*██████████████████████████████████████████████████████████*/
        #region Functions

        public static void Log(object message, UnityEngine.Object context = null, Color? color = null) => Log(LogType.Log, UnityLogger, message, context, color);
        public static void Log(object message, UnityEngine.Object context) => Log(LogType.Log, UnityLogger, message, context, null);
        public static void Log(object message, Color? color) => Log(LogType.Log, UnityLogger, message, null, color);

        public static void Log(Logger logger, object message, UnityEngine.Object context = null, Color? color = null) => Log(LogType.Log, logger, message, context, color);
        public static void Log(Logger logger, object message, UnityEngine.Object context) => Log(LogType.Log, logger, message, context, null);
        public static void Log(Logger logger, object message, Color? color) => Log(LogType.Log, logger, message, null, color);

        public static void LogWarning(object message, UnityEngine.Object context = null, Color? color = null) => Log(LogType.Warning, UnityLogger, message, context, color);
        public static void LogWarning(object message, UnityEngine.Object context) => Log(LogType.Warning, UnityLogger, message, context, null);
        public static void LogWarning(object message, Color? color) => Log(LogType.Warning, UnityLogger, message, null, color);

        public static void LogWarning(Logger logger, object message, UnityEngine.Object context = null, Color? color = null) => Log(LogType.Warning, logger, message, context, color);
        public static void LogWarning(Logger logger, object message, UnityEngine.Object context) => Log(LogType.Warning, logger, message, context, null);
        public static void LogWarning(Logger logger, object message, Color? color) => Log(LogType.Warning, logger, message, null, color);

        public static void LogError(object message, UnityEngine.Object context = null, Color? color = null) => Log(LogType.Error, UnityLogger, message, context, color);
        public static void LogError(object message, UnityEngine.Object context) => Log(LogType.Error, UnityLogger, message, context, null);
        public static void LogError(object message, Color? color) => Log(LogType.Error, UnityLogger, message, null, color);

        public static void LogError(Logger logger, object message, UnityEngine.Object context = null, Color? color = null) => Log(LogType.Error, logger, message, context, color);
        public static void LogError(Logger logger, object message, UnityEngine.Object context) => Log(LogType.Error, logger, message, context, null);
        public static void LogError(Logger logger, object message, Color? color) => Log(LogType.Error, logger, message, null, color);

        public static void LogException(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Exception, UnityLogger, message, context, color);
            if (ThrowWhenLoggingException)
            {
                throw new Exception(StringBuilder.ToString());
            }
        }

        public static void LogException(Exception exception, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Exception, UnityLogger, exception, context, color);
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
        static void Log(LogType logType, Logger logger, object message, UnityEngine.Object context, Color? color)
        {
            TryInitialize();

            // set stack trace depth
            stackTraceDepth = 4; // NOTE: use uncommented line if we use nested function calls in the future
            // stackTraceDepth = stackTraceDepth > 3 ? stackTraceDepth : 3;

            // build message
            StringBuilder.Clear();
            BuildMessageWithClassTag(message);
            BuildMessageWithColorTag(color);

            // swap out original logger for given logger, log message, then replace original logger
            SetLogger(logger);
            logger.Log(logType, StringBuilder.ToString(), context: context);
            SetLogger(UnityLogger);

            // reset stack trace depth
            stackTraceDepth = 0;
        }

        static void TryInitialize()
        {
            if (unityLogger != null) return;

            // initialize Unity Logger
            unityLogger = new Logger(Debug.unityLogger.logHandler);

            // initialize LogManager Profile
            Profile = Resources.Load<LogManagerProfile>(path: "LogManagerProfile");
        }

        static void SetLogger(Logger logger)
        {
            Debug.unityLogger.logHandler = logger.logHandler;
        }

        static void BuildMessageWithClassTag(object message)
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

        static void BuildMessageWithColorTag(Color? color = null)
        {
            if (!LogMessageWithColorTag || color == null) return;
            StringBuilder.Insert(0, $"<color=#{ColorUtility.ToHtmlStringRGB((Color) color)}>");
            StringBuilder.Append("</color>");
        }

        static string GetCallingClassName(int stackTraceDepth)
        {   
            return new System.Diagnostics.StackTrace().GetFrame(stackTraceDepth).GetMethod().ReflectedType.Name;
        }

        #endregion
        /*██████████████████████████████████████████████████████████*/
    }
}
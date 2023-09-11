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
        /************************************************************/
        #region Fields

        // data
        internal const string LogManagerProfileString = "LogManager Profile";

        // cached references
        private static Logger unityLogger;
        private static LogManagerProfile profile;

        // variables
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

        public static LogManagerProfile Profile
        {
            get
            {
                TryInitialize();
                return profile;
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
        private static bool LogMessageWithClassTag => !Profile || Profile.LogMessageWithClassTag;
        private static bool ThrowWhenLoggingException => Profile && Profile.ThrowWhenLoggingException;

        // other properties
        private static bool LogMessageWithColorTag => Application.isEditor;

        #endregion
        /************************************************************/
        #region Functions

        public static void Log(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Log, UnityLogger, message, context, color);
        }

        public static void Log(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Log, logger, message, context, color);
        }

        public static void LogWarning(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Warning, UnityLogger, message, context, color);
        }

        public static void LogWarning(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Warning, logger, message, context, color);
        }

        public static void LogError(object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Error, UnityLogger, message, context, color);
        }

        public static void LogError(Logger logger, object message, UnityEngine.Object context = null, Color? color = null)
        {
            Log(LogType.Error, logger, message, context, color);
        }

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
        private static void Log(LogType logType, Logger logger, object message, UnityEngine.Object context, Color? color)
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

        private static void TryInitialize()
        {
            if (unityLogger != null) return;

            // initialize Unity Logger
            unityLogger = new Logger(Debug.unityLogger.logHandler);

            // initialize LogManager Profile
            profile = Resources.Load<LogManagerProfile>(LogManagerProfileString);
            if (profile) profile.OnLoad();
        }

        private static void SetLogger(Logger logger)
        {
            Debug.unityLogger.logHandler = logger.logHandler;
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
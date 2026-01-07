using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using UnityEngine;

using Kokowolo.Utilities;

namespace Kokowolo.Base
{
    public class StackTraceReader : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Func();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void Func() => Func1();

        void Func1()
        {
            LogStackTrace();
        }

        void SayHi()
        {
            StackTrace stackTrace = new StackTrace(true);
            string str = $"{stackTrace.GetFrame(0).GetMethod().Name}-({stackTrace.GetFrame(0).GetFileLineNumber()}, {stackTrace.GetFrame(0).GetFileColumnNumber()})";
            UnityEngine.Debug.Log(str);
        }

        void LogStackTrace()
        {
            StackTrace stackTrace = new StackTrace(true);
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                StackFrame sf = stackTrace.GetFrame(i);
                UnityEngine.Debug.Log($"{sf.GetMethod()}({stackTrace.GetFrame(0).GetFileLineNumber()}, {stackTrace.GetFrame(0).GetFileColumnNumber()})");
            }
        }
    }
}

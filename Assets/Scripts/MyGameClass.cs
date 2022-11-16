using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class MyFileLogHandler : ILogHandler
{
    private FileStream m_FileStream;
    private StreamWriter m_StreamWriter;
    private ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;

    public MyFileLogHandler()
    {
        string filePath = Application.persistentDataPath + "/MyLogs.txt";

        m_FileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        m_StreamWriter = new StreamWriter(m_FileStream);

        // Replace the default debug log handler
        Debug.unityLogger.logHandler = this;
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        m_StreamWriter.WriteLine(String.Format(format, args));
        m_StreamWriter.Flush();
        m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        m_DefaultLogHandler.LogException(exception, context);
    }
}

public class MyGameClass : MonoBehaviour
{
    void Start()
    {
        string kTAG = "MyGameTag";
        MyFileLogHandler myFileLogHandler = new MyFileLogHandler();
        ILogger logger = Debug.unityLogger;
        // GameLogger logger = new GameLogger(myFileLogHandler);

        logger.LogWarning(kTAG, "MyGameClass Start.");
        Debug.LogWarning("MyGameClass Start. 2");
    }
}
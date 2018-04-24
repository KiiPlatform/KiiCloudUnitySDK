using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class ConsoleLog : MonoBehaviour
{
    const int LOG_MAX_LINE = 200;

    public enum Level
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error
    }

    static Dictionary<Level, Color> LOG_COLOR = new Dictionary<Level,Color>()
    {
        { Level.Verbose, Color.gray },
        { Level.Debug, Color.white },
        { Level.Information, Color.cyan },
        { Level.Warning, Color.yellow },
        { Level.Error, Color.red }
    };

    public struct LogData
    {
        public Level Level { get; set; }

        public DateTime Time { get; set; }

        public string Message { get; set; }
    }

    readonly static Queue<LogData> _logQue = new Queue<LogData>(LOG_MAX_LINE);

    public static void V(string message)
    {
        _PushLog(Level.Verbose, message); 
    }

    public static void D(string message)
    {
        _PushLog(Level.Debug, message);
    }

    public static void I(string message)
    {
        _PushLog(Level.Information, message); 
    }

    public static void W(string message)
    {
        _PushLog(Level.Warning, message);
    }

    public static void E(string message)
    {
        _PushLog(Level.Error, message);
    }

    public static void Clear()
    {
        _logQue.Clear();
    }

    public static LogData[] GetLogs()
    {
        return _logQue.ToArray();
    }

    public static string[] GetLogsByString()
    {
        ArrayList logs = new ArrayList();
        foreach (LogData d in _logQue)
        {
            logs.Add(d.Time.ToLongTimeString() + " " + d.Message);
        }
        return (string[])logs.ToArray(typeof(string));
    }

    static void _PushLog(Level level, string message)
    {
        if (_logQue.Count >= LOG_MAX_LINE)
        {
            _logQue.Dequeue();
        }

        LogData data = new LogData();
        {
            data.Level = level;
            data.Message = message;
            data.Time = DateTime.Now;
        }
        _logQue.Enqueue(data);
    }
}

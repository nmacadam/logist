using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogistInternal;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Extends Unity's logging system by outputting a JSON log file with additional features
/// for use with the logist web-app
/// </summary>
public static class Logist
{
    [Flags]
    public enum Tag
    {
        Assert      = 1 << 0,
        Error       = 1 << 1,
        Exception   = 1 << 2,
        Warning     = 1 << 3,
        System      = 1 << 4,
        Performance = 1 << 5,
        Log         = 1 << 6,
        Graphics    = 1 << 7,
        AI          = 1 << 8,
        Audio       = 1 << 9,
        Content     = 1 << 10,
        Logic       = 1 << 11,
        GUI         = 1 << 12,
        Input       = 1 << 13,
        Network     = 1 << 14,
        Animation   = 1 << 15,
        Physics     = 1 << 16,
        Event       = 1 << 17
    }

	//private static List<LogContent> _logs = new List<LogContent>();

    private static DateTime _startTime;
    private static DateTime _endTime;

    private static LoggingSession _session = new LoggingSession();

    private struct LoggingSession
    {
        public SessionData SessionData;
        public List<LogContent> Logs;
    }

	private struct LogContent
    {
        public float TimeStamp;
        public string Log;
        public string[] Categories;
        public string[] StackTrace;
    }

    private struct SessionData
    {
        public string ProductName;
        public string Version;
        public string Platform;
        public string CompanyName;
        public string SessionStartTime;
        public string SessionEndTime;
        public string SessionDuration;
    }

	private static void WriteToFile()
	{
        if (!LogistSettings.GetOrCreateSettings().Enabled) return;

        _endTime = DateTime.Now;
        _session.SessionData.SessionStartTime = _endTime.ToLongTimeString();
        _session.SessionData.SessionDuration = (_endTime - _startTime).ToString();
        
        //string directoryBasePath = Application.dataPath + "/Logs/";
        string directoryBasePath = LogistSettings.GetPersistentSavePath();
        if (!Directory.Exists(directoryBasePath))
        {
            Directory.CreateDirectory(directoryBasePath);
        }
        string date = DateTime.Now.ToShortDateString();
        date = date.Replace('/', '-');
        string datedDirectoryPath = directoryBasePath + date + '/';
        if (!Directory.Exists(datedDirectoryPath))
        {
            Directory.CreateDirectory(datedDirectoryPath);
        }
        
        string time = DateTime.Now.ToLongTimeString().Replace(':', ' ');
        using (var sw = new StreamWriter(datedDirectoryPath + $"log {time}.json"))
        {
            sw.Write(JsonConvert.SerializeObject(_session, LogistSettings.GetOrCreateSettings().FormatOutput ? Formatting.Indented : Formatting.None));
        }
	}

	[RuntimeInitializeOnLoadMethod]
	private static void RunOnStart()
	{
        if (!LogistSettings.GetOrCreateSettings().Enabled) return;
        
        _session.Logs = new List<LogContent>();

        _session.SessionData.ProductName = Application.productName;
        _session.SessionData.Version = Application.version;
        _session.SessionData.Platform = Application.platform.ToString();
        _session.SessionData.CompanyName = Application.companyName;

        _startTime = DateTime.Now;
        _session.SessionData.SessionStartTime = _startTime.ToLongTimeString();

        if (LogistSettings.GetOrCreateSettings().HandleUnityLogBy == LogistSettings.HandleUnityLog.UseAll)
            Application.logMessageReceived += HandleUnityLog;
            
		Application.quitting += WriteToFile;
	}

    /// <summary>
    /// Writes a log to the Logist output
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="category">(Optional) Flag for categorizing the logs, multiple allowed</param>
	public static void Log(string message, Tag category = Tag.Log)
	{
        if (!LogistSettings.GetOrCreateSettings().Enabled) return;
        
        LogContent content;
        content.TimeStamp = Time.time;
        content.Log = message;
        content.Categories = Array.ConvertAll(category.GetFlags().ToArray(), e => e.ToString().ToLower());
        string pattern = @"\r\n?|\n";
        string[] elements = System.Text.RegularExpressions.Regex.Split(UnityEngine.StackTraceUtility.ExtractStackTrace(), pattern);
        content.StackTrace = elements;
        _session.Logs.Add(content);
	}

    /// <summary>
    /// Writes a log to the Logist output
    /// </summary>
    /// <param name="message">String or object to be converted to string representation for display.</param>
    /// <param name="stackTrace">String representing stack trace</param>
    /// <param name="category">(Optional) Flag for categorizing the logs, multiple allowed</param>    
    public static void Log(string message, string stackTrace, Tag category = Tag.Log)
	{
        if (!LogistSettings.GetOrCreateSettings().Enabled) return;
        LogContent content;
        content.TimeStamp = Time.time;
        content.Log = message;
        content.Categories = Array.ConvertAll(category.GetFlags().ToArray(), e => e.ToString().ToLower());//category.ToString().ToLower();
        string pattern = @"\r\n?|\n";
        string[] elements = System.Text.RegularExpressions.Regex.Split(UnityEngine.StackTraceUtility.ExtractStackTrace(), pattern);
        content.StackTrace = elements;
        _session.Logs.Add(content);
	}

    private static void HandleUnityLog(string logString, string stackTrace, LogType type)
    {
        if (LogistSettings.GetOrCreateSettings().HandleUnityLogBy != LogistSettings.HandleUnityLog.IgnoreAll) return;

        switch (type)
        {
            case LogType.Assert:
                Log(logString, stackTrace, Tag.Assert);
                break;
            case LogType.Error:
                Log(logString, stackTrace, Tag.Error);
                break;
            case LogType.Exception:
                Log(logString, stackTrace, Tag.Exception);
                break;
            case LogType.Warning:
                Log(logString,stackTrace, Tag.Warning);
                break;
            case LogType.Log:
                if (LogistSettings.GetOrCreateSettings().HandleUnityLogBy != LogistSettings.HandleUnityLog.IgnoreLogs)
                    Log(logString, stackTrace, Tag.Log);
                break;
            default:
                break;
        }
    }
}
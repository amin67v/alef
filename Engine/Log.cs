using System.IO;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Simple logging class
    /// </summary>
    public static class Log
    {
        static readonly object sync_obj = new object();
        static StreamWriter writer = null;
        static LogLevel level;

        /// <summary>
        /// Get or set the logging level
        /// </summary>
        public static LogLevel Level
        {
            get => level;
            set => level = value;
        }

        public static event LogHandler LogEvent;

        /// <summary>
        /// prints a debug message to the output.
        /// </summary>
        public static void Debug(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Debug, msg, file, line);
        }

        /// <summary>
        /// prints an error message to the output.
        /// </summary>
        public static void Error(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Error, msg, file, line);
        }

        /// <summary>
        /// prints an info message to the output.
        /// </summary>
        public static void Info(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Info, msg, file, line);
        }

        /// <summary>
        /// prints a warning message to the output.
        /// </summary>
        public static void Warning(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Warning, msg, file, line);
        }

        static void write(LogLevel lvl, string message, string file, int line)
        {
            lock (sync_obj)
            {
                string msg = $"[{lvl.ToString()}] ({Path.GetFileName(file)}:{line}) {message}";
                if (Level <= lvl)
                {
                    // log to file
                    if (writer != null)
                    {
                        writer.WriteLine(msg);
                        writer.Flush();
                    }
                    // raise OnLogEvent
                    if (LogEvent != null)
                        LogEvent(lvl, msg);
                }
            }
        }

        internal static void init()
        {
            var fstream = File.Create("log.txt");
            writer = new StreamWriter(fstream);
        }

        internal static void shut_down()
        {
            writer.Dispose();
            writer = null;
        }
    }

    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    public delegate void LogHandler(LogLevel level, string msg);


}




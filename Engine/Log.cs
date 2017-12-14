using System.IO;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Simple logging class
    /// </summary>
    public class Log : Disposable
    {
        readonly object sync_obj = new object();
        StreamWriter writer = null;
        LogLevel level;

        public Log(string file)
        {
            var fstream = File.Create(file);
            writer = new StreamWriter(fstream);
        }

        /// <summary>
        /// Get or set the logging level
        /// </summary>
        public LogLevel Level
        {
            get => level;
            set => level = value;
        }

        public static event LogHandler LogEvent;

        /// <summary>
        /// prints a debug message to the output.
        /// </summary>
        public void Debug(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Debug, msg, file, line);
        }

        /// <summary>
        /// prints an error message to the output.
        /// </summary>
        public void Error(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Error, msg, file, line);
        }

        /// <summary>
        /// prints an info message to the output.
        /// </summary>
        public void Info(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Info, msg, file, line);
        }

        /// <summary>
        /// prints a warning message to the output.
        /// </summary>
        public void Warning(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Warning, msg, file, line);
        }

        void write(LogLevel lvl, string message, string file, int line)
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

        protected override void OnDisposeManaged() 
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




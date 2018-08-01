using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Engine
{
    public enum LogLevel { Debug, Info, Warning, Error }

    public delegate void LogHandler(LogLevel level, string msg);

    /// <summary>
    /// Simple logging class
    /// </summary>
    public sealed class Log : ObjectBase
    {
        readonly ConsoleColor[] colors =
        {
            ConsoleColor.Magenta,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Red
        };

        readonly object syncObj = new object();
        StreamWriter writer = null;
        LogLevel level;
        bool console = true;

        /// <summary>
        /// Get or set the logging level
        /// </summary>
        public LogLevel Level
        {
            get => level;
            set => level = value;
        }

        public bool OutputConsole
        {
            get => console;
            set => console = value;
        }

        public event LogHandler LogEvent;

        internal Log(string file)
        {
            var fstream = File.Create(file);
            writer = new StreamWriter(fstream);
        }

        /// <summary>
        /// prints a debug message to the output.
        /// </summary>
        public void Debug(object msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Debug, msg.ToString(), file, line);
        }

        /// <summary>
        /// prints an info message to the output.
        /// </summary>
        public void Info(object msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Info, msg.ToString(), file, line);
        }

        /// <summary>
        /// prints a warning message to the output.
        /// </summary>
        public void Warning(object msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Warning, msg.ToString(), file, line);
        }

        /// <summary>
        /// prints an error message to the output.
        /// </summary>
        public void Error(object msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            write(LogLevel.Error, msg.ToString(), file, line);
        }

        protected override void OnDestroy()
        {
            writer.Dispose();
            writer = null;
        }

        void write(LogLevel lvl, string message, string file, int line)
        {
            lock (syncObj)
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

                    // log to console
                    if (console)
                    {
                        var c = Console.ForegroundColor;
                        Console.ForegroundColor = colors[(int)lvl];
                        Console.WriteLine(msg);
                        Console.ForegroundColor = c;
                    }

                    // raise OnLogEvent
                    if (LogEvent != null)
                        LogEvent(lvl, msg);
                }
            }
        }
    }
}
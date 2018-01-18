using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Engine
{
    /// <summary>
    /// Simple logging class
    /// </summary>
    public sealed class Log
    {
        static Log instance;

        static readonly ConsoleColor[] console_color =
        {
            ConsoleColor.Magenta,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Red
        };

        readonly object sync_obj = new object();
        StreamWriter writer = null;
        LogLevel level;
        bool console = true;

        Log(string file)
        {
            var fstream = File.Create(file);
            writer = new StreamWriter(fstream);
        }

        /// <summary>
        /// Get or set the logging level
        /// </summary>
        public static LogLevel Level
        {
            get => instance.level;
            set => instance.level = value;
        }

        public static bool OutputConsole
        {
            get => instance.console;
            set => instance.console = value;
        }

        public static event LogHandler LogEvent;

        /// <summary>
        /// prints a debug message to the output.
        /// </summary>
        public static void Debug(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            instance.write(LogLevel.Debug, msg, file, line);
        }

        /// <summary>
        /// prints an info message to the output.
        /// </summary>
        public static void Info(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            instance.write(LogLevel.Info, msg, file, line);
        }

        /// <summary>
        /// prints a warning message to the output.
        /// </summary>
        public static void Warning(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            instance.write(LogLevel.Warning, msg, file, line);
        }

        /// <summary>
        /// prints an error message to the output.
        /// </summary>
        public static void Error(string msg, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            instance.write(LogLevel.Error, msg, file, line);
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

                    // log to console
                    if (console)
                    {
                        var c = Console.ForegroundColor;
                        Console.ForegroundColor = console_color[(int)lvl];
                        Console.WriteLine(msg);
                        Console.ForegroundColor = c;
                    }

                    // raise OnLogEvent
                    if (LogEvent != null)
                        LogEvent(lvl, msg);
                }
            }
        }

        internal static void init(string file)
        {
            instance = new Log(file);
        }

        internal static void shutdown()
        {
            instance.writer.Dispose();
            instance.writer = null;
        }
    }

    public enum LogLevel { Debug, Info, Warning, Error }

    public delegate void LogHandler(LogLevel level, string msg);


}




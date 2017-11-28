using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Engine
{
    public static class Assert
    {
        /// <summary>
        /// If the condition is false then throws an exception with the specified description.
        /// </summary>
        [DebuggerHidden]
        [Conditional("DEBUG")]
        public static void IsTrue(bool condition, string fail_msg = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            if (!condition)
            {
                file = Path.GetFileName(file);
                var msg = $"({file}:{line}) Assertion failed. {fail_msg}";
                Log.Error(msg);
                throw new Exception(msg);
            }
        }
    }
}
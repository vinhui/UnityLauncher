using System;
using JetBrains.Annotations;

namespace UnityLauncher
{
    public static class Logger
    {
        public static bool Verbose { get; set; }

        /// <summary>
        /// Log a debug message to the console
        /// </summary>
        /// <remarks>This message won't show up if <see cref="Verbose"/> is not set to true</remarks>
        /// <param name="format">Message to log</param>
        /// <param name="args">Arguments to format the message with</param>
        [StringFormatMethod("format")]
        public static void Debug(string format, params object[] args)
        {
            if (!Verbose)
                return;

            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Log an info message to the console
        /// </summary>
        /// <param name="format">Message to log</param>
        /// <param name="args">Arguments to format the message with</param>
        [StringFormatMethod("format")]
        public static void Info(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Log an error message to the console
        /// </summary>
        /// <param name="format">Message to log</param>
        /// <param name="args">Arguments to format the message with</param>
        [StringFormatMethod("format")]
        public static void Error(string format, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }
    }
}
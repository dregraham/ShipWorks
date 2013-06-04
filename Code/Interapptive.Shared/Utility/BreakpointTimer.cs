using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Diagnostics;

    ///
    /// This is in the global namespace to make it easy to use
    /// 


    /// <summary>
    /// Intended to be called by "Tracepoints" in the visual studio debugger for easy timings
    /// </summary>
    public static class BreakpointTimer
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BreakpointTimer));

        static Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Start timing
        /// </summary>
        public static string Start()
        {
            return Start("Starting timer");
        }

        /// <summary>
        /// Start timing
        /// </summary>
        public static string Start(string message)
        {
            stopwatch = Stopwatch.StartNew();

            return message;
        }

        /// <summary>
        /// Output the time, but keep the stop watch running
        /// </summary>
        public static string OutputTime()
        {
            return OutputTime("Time so far");
        }

        /// <summary>
        /// Output the current time with the given message
        /// </summary>
        public static string OutputTime(string message)
        {
            return string.Format("{0}: {1}s", message, stopwatch.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// Stop the timer and output the total time
        /// </summary>
        public static string Stop()
        {
            return Stop("Total time");
        }

        /// <summary>
        /// Stop the timer and output the total time elapsed
        /// </summary>
        public static string Stop(string message)
        {
            stopwatch.Stop();

            return OutputTime(message);
        }
    }

using System;
using System.Linq;
using log4net;
using System.Diagnostics;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Utility class for 
    /// </summary>
    public class LoggedStopwatch : IDisposable
    {
        ILog log;

        string name;
        Stopwatch stopwatch;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoggedStopwatch(ILog log, string name)
        {
            this.log = log;
            this.name = name;
            log.Debug($"Stopwatch {name} starting...");

            stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// Stop the stopwatch and cleanup
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

         /// <summary>
        /// Stop the stopwatch
        /// </summary>
        private void Stop()
        {
            stopwatch.Stop();

            LogElapsedTime("Finished");
        }

        /// <summary>
        /// Log the current running elapsed time
        /// </summary>
        public void LogElapsedTime(string description)
        {
            if (log == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                description = string.Empty;
            }
            else
            {
                description = string.Format(" - {0}", description);
            }

            string logEntry = string.Format("Stopwatch {0} {1}: Elapsed {2} ms.", name, description, stopwatch.ElapsedMilliseconds);

            log.Debug(logEntry);
        }
    }
}

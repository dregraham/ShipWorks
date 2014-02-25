﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Logging
{
    public class LogEntryFactory : ILogEntryFactory
    {
        /// <summary>
        /// Gets the log entry.
        /// </summary>
        public IApiLogEntry GetLogEntry(ApiLogSource source, string name, LogActionType logActionType)
        {
            IApiLogEntry createdLogEntry;

            if (LogSession.IsApiLogActionTypeEnabled(logActionType))
            {
                createdLogEntry = new ApiLogEntry(source, name);

            }
            else
            {
                createdLogEntry = new NullApiLogEntry();
            }

            return createdLogEntry;
        }
    }
}

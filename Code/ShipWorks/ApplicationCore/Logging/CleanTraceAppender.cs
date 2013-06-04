using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;
using System.Diagnostics;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// log4net provices a TraceAppender, but it hardcoded outputs the logger name as the first thing on the line.
    /// This one is "clean" and outputs the trace pattern as configured.
    /// </summary>
    public class CleanTraceAppender : AppenderSkeleton
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CleanTraceAppender()
        {

        }

        /// <summary>
        /// Overridden to doe the Append
        /// </summary>
        protected override void Append(LoggingEvent loggingEvent)
        {
            Trace.Write(base.RenderLoggingEvent(loggingEvent));
            Trace.Flush();
        }

        /// <summary>
        /// Required overide
        /// </summary>
        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }
    }

}

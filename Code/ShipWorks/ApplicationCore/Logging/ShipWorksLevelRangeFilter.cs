using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using log4net.Filter;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Log filter for ShipWorks based on log level
    /// </summary>
    public class ShipWorksLevelRangeFilter : LevelRangeFilter
    {
        /// <summary>
        /// Filter out api logging and use base LevelRangeFilter
        /// </summary>
        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent.LoggerName.Equals("ApiMiddleware", StringComparison.InvariantCulture))
            {
                return FilterDecision.Deny;
            }

            return base.Decide(loggingEvent);
        }
    }
}

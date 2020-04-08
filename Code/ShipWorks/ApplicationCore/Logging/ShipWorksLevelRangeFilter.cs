using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using log4net.Filter;

namespace ShipWorks.ApplicationCore.Logging
{
    public class ShipWorksLevelRangeFilter : LevelRangeFilter
    {
        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (!loggingEvent.LoggerName.Contains("ShipWorks.Api"))
            {
                return base.Decide(loggingEvent);
            }

            return FilterDecision.Deny;
        }
    }
}

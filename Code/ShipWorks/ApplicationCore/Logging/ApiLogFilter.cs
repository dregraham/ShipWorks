using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using log4net.Filter;

namespace ShipWorks.ApplicationCore.Logging
{
    public class ApiLogFilter : FilterSkeleton
    {
        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent.LoggerName.Equals("ApiMiddleware", StringComparison.InvariantCulture))
            {
                return FilterDecision.Accept;
            }

            return FilterDecision.Deny;
        }
    }
}

using System;
using log4net;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.AppDomainHelpers
{
    /// <summary>
    /// Wrap the ApiLogger in a marshalable class so logging can happen accross Appdomains
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public class MarketplaceAdvisorLog : MarshalByRefObject
    {
        readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerType"></param>
        public MarketplaceAdvisorLog(Type loggerType)
        {
            log = LogManager.GetLogger(loggerType);
        }

        /// <summary>
        /// Write a warning
        /// </summary>
        public void WarnFormat(string message, params object[] args)
        {
            log.WarnFormat(message, args);
        }

        /// <summary>
        /// Write an informational message
        /// </summary>
        public void InfoFormat(string message, params object[] args)
        {
            log.InfoFormat(message, args);
        }

        /// <summary>
        /// Get an ApiLogEntry that can be used across app domains
        /// </summary>
        public MarketplaceAdvisorApiLogger CreateApiLogger(string action)
        {
            return new MarketplaceAdvisorApiLogger(action);
        }
    }
}
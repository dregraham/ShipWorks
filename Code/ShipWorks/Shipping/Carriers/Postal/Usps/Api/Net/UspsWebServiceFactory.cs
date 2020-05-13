﻿using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Factory for the USPS web client
    /// </summary>
    [Component]
    public class UspsWebServiceFactory : IUspsWebServiceFactory
    {
        readonly Lazy<int> requestTimeout = new Lazy<int>(() =>
            new RegistryHelper(@"Software\Interapptive\ShipWorks\Options")
                .GetValue("uspsRequestTimeout", -1));
        readonly ILogEntryFactory logEntryFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logEntryFactory"></param>
        public UspsWebServiceFactory(ILogEntryFactory logEntryFactory)
        {
            this.logEntryFactory = logEntryFactory;
        }

        /// <summary>
        /// Gets the service URL to use when contacting the USPS API.
        /// </summary>
        private static string ServiceUrl =>
            UspsWebClient.UseTestServer ?
                "https://swsim.testing.stamps.com/swsim/swsimv90.asmx" :
                "https://swsim.stamps.com/swsim/swsimv90.asmx";

        /// <summary>
        /// Create the web service
        /// </summary>
        public IExtendedSwsimV90 Create(string logName, LogActionType logActionType)
        {
            var client = new SwsimV90(logEntryFactory.GetLogEntry(ApiLogSource.Usps, logName, logActionType))
            {
                Url = ServiceUrl
            };

            if (requestTimeout.Value > 0)
            {
                client.Timeout = requestTimeout.Value;
            }

            return client;
        }
    }
}

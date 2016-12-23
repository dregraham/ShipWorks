using ShipWorks.ApplicationCore.ComponentRegistration;
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
                "https://swsim.testing.stamps.com/swsim/SwsimV55.asmx" :
                "https://swsim.stamps.com/swsim/SwsimV55.asmx";

        /// <summary>
        /// Create the web service
        /// </summary>
        public ISwsimV55 Create(string logName, LogActionType logActionType)
        {
            return new SwsimV55(logEntryFactory.GetLogEntry(ApiLogSource.Usps, logName, logActionType))
            {
                Url = ServiceUrl
            };
        }
    }
}

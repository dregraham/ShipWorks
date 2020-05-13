using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net
{
    /// <summary>
    /// Factory for the USPS web client
    /// </summary>
    public interface IUspsWebServiceFactory
    {
        /// <summary>
        /// Create the web service
        /// </summary>
        IExtendedSwsimV90 Create(string logName, LogActionType logActionType);
    }
}

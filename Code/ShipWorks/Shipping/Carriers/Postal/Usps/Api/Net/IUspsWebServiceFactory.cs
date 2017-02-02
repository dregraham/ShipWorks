using ShipWorks.ApplicationCore.Logging;

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
        ISwsimV55 Create(string logName, LogActionType logActionType);
    }
}

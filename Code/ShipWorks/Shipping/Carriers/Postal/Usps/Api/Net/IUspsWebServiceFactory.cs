using System.Web.UI.WebControls;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Usps.Net;
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
        IExtendedSwsimV135 Create(string logName, LogActionType logActionType);

        /// <summary>
        /// Create the webservice for FinishAccountVerification
        /// </summary>
        ISwimFinishAccountVerification CreateFinishAccountVerification(string logName, LogActionType logActionType,
            string smsVerificationPhoneNumber);
    }
}

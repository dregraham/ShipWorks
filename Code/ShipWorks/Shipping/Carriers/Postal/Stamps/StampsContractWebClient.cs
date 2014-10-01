using System;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices.Contract;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// This is hitting v38 of the Stamps.com API to change the plan. At the time this class was created
    /// the StampsApiSession was using v29 of the API. In order to not have to retest both Stamps.com and
    /// Express1 to use the functionality to change plan, this class was created that gets called by the 
    /// StampsApiSession.
    /// </summary>
    public class StampsContractWebClient
    {
        // This value came from Stamps.com (the "standard" account value is 88)
        private const int ExpeditedPlanID = 236;

        private readonly bool useTestServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsContractWebClient"/> class.
        /// </summary>
        /// <param name="useTestServer">if set to <c>true</c> [use test server].</param>
        public StampsContractWebClient(bool useTestServer)
        {
            this.useTestServer = useTestServer;
        }

        /// <summary>
        /// Gets the service URL.
        /// </summary>
        private string ServiceUrl
        {
            get { return useTestServer ? "https://swsim.testing.stamps.com/swsim/SwsimV39.asmx" : "https://swsim.stamps.com/swsim/swsimv39.asmx"; }
        }

        /// <summary>
        /// Makes request to Stamps.com API to change plan associated with the account referenced by the authenticator to be 
        /// an Expedited plan. This requires an authentication call to the Stamps.com API prior to changing the plan.
        /// </summary>
        public void ChangeToExpeditedPlan(string authenticator, string promoCode)
        {
            // Output parameters for web service call
            PurchaseStatus purchaseStatus;
            int transactionID;
            string rejectionReason = string.Empty;
            
            try
            {
                using (SwsimV39 webService = new SwsimV39(new LogEntryFactory().GetLogEntry(ApiLogSource.UspsStamps, "ChangePlan", LogActionType.Other)))
                {
                    webService.Url = ServiceUrl;
                    webService.ChangePlan(authenticator, ExpeditedPlanID, promoCode, out purchaseStatus, out transactionID, out rejectionReason);
                }
            }
            catch (SoapException soapException)
            {
                string message = string.Format("ShipWorks was unable to change the Stamps.com plan. {0}{1}", rejectionReason ?? string.Empty, soapException.Message);
                throw new StampsException(message, soapException);
            }
            catch (Exception exception)
            {
                WebHelper.TranslateWebException(exception, typeof (StampsException));
            }
        }
    }
}

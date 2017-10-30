using Interapptive.Shared;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Web client for Magento, extends the generic client to add Action functionality
    /// </summary>
    public class MagentoWebClient : GenericStoreWebClient, IMagentoWebClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoWebClient(MagentoStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Executes an action on an order and returns the new order status
        /// </summary>
        public string ExecuteAction(MagentoUploadAction action)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            
            request.Variables.Add("order", action.OrderNumber.ToString());
            request.Variables.Add("command", action.Action);
            request.Variables.Add("comments", action.Comments);
            request.Variables.Add("carrier", action.Carrier);
            request.Variables.Add("tracking", action.TrackingNumber);

            // if we're supposed to send emails, send the flag
            if (action.SendEmail)
            {
                request.Variables.Add("sendemail", "1");
            }

            GenericModuleResponse response = ProcessRequest(request, "updateorder");

            // look for the resulting status
            return XPathUtility.Evaluate(response.XPath, "//OrderStatus", "fail_after_action");
        }

    }
}

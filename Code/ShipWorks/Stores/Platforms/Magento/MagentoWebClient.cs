using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class MagentoWebClient : GenericStoreWebClient
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
        [NDependIgnoreTooManyParams]
        public string ExecuteAction(long orderNumber, string action, string comments, string carrier, string trackingNumber, bool magentoEmails)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            request.Variables.Add("order", orderNumber.ToString());
            request.Variables.Add("command", action);
            request.Variables.Add("comments", comments);
            request.Variables.Add("carrier", carrier);
            request.Variables.Add("tracking", trackingNumber);

            // if we're supposed to send emails, send the flag
            if (magentoEmails)
            {
                request.Variables.Add("sendemail", "1");
            }

            GenericModuleResponse response = ProcessRequest(request, "updateorder");

            // look for the resulting status
            return XPathUtility.Evaluate(response.XPath, "//OrderStatus", "fail_after_action");
        }
    }
}

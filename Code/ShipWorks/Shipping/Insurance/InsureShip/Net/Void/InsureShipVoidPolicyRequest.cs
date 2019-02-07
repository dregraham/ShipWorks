using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// InsureShip request class for voiding a policy
    /// </summary>
    [Component]
    public class InsureShipVoidPolicyRequest : IInsureShipVoidPolicyRequest
    {
        private readonly IInsureShipWebClient webClient;
        private readonly ILog log;
        private readonly IInsureShipVoidValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipVoidPolicyRequest(IInsureShipWebClient webClient, IInsureShipVoidValidator validator, Func<Type, ILog> createLog)
        {
            this.validator = validator;
            this.webClient = webClient;
            log = createLog(GetType());
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public Result VoidInsurancePolicy(ShipmentEntity shipment) =>
            validator.IsVoidable(shipment)
                .Bind(x => PerformVoid(x, shipment));

        /// <summary>
        /// Perform the actual void
        /// </summary>
        private Result PerformVoid(bool isVoidable, ShipmentEntity shipment) =>
            isVoidable ?
                webClient.Submit<InsureShipVoidPolicyResponse>("void_policy", shipment.Order.Store, CreatePostData(shipment)) :
                Result.FromSuccess();

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private Dictionary<string, string> CreatePostData(ShipmentEntity shipment)
        {
            // PopulateShipmentOrder();

            Dictionary<string, string> postData = new Dictionary<string, string>();

            postData.Add("policy_id", shipment.InsurancePolicy.InsureShipPolicyID.ToString());
            postData.Add("email", shipment.ShipEmail);
            postData.Add("order_number", InsureShipShipmentIdentifier.GetUniqueShipmentId(shipment));

            return postData;
        }
    }
}

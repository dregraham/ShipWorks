using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// InsureShip request class for submitting a claim
    /// </summary>
    [Component]
    public class InsureShipSubmitClaimRequest : IInsureShipSubmitClaimRequest
    {
        private readonly IInsureShipWebClient webClient;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipSubmitClaimRequest(IInsureShipWebClient webClient, Func<Type, ILog> createLog)
        {
            this.webClient = webClient;
            log = createLog(GetType());
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public Result CreateInsuranceClaim(ShipmentEntity shipment) =>
            webClient.Submit<InsureShipSubmitClaimResponse>("submit_claim", shipment.Order.Store, CreatePostData(shipment))
                .Do(x => shipment.InsurancePolicy.ClaimID = x.ClaimID);

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private Dictionary<string, string> CreatePostData(ShipmentEntity shipment)
        {
            // PopulateShipmentOrder();

            Dictionary<string, string> postData = new Dictionary<string, string>();

            postData.Add("policy_id", shipment.InsurancePolicy.InsureShipPolicyID.ToString());
            postData.Add("customer_name", shipment.ShipPerson.ParsedName.FullName);
            postData.Add("items_purchased", string.Join(",", shipment.Order.OrderItems.Select(oi => oi.Name)));
            postData.Add("email", shipment.InsurancePolicy.EmailAddress);
            postData.Add("issue_type", ((InsureShipClaimType?) shipment.InsurancePolicy.ClaimType).GetValueOrDefault().ToString());
            postData.Add("description", shipment.InsurancePolicy.Description);
            postData.Add("date_of_issue", shipment.InsurancePolicy.DateOfIssue.GetValueOrDefault().ToString("yyyy-MM-DD"));
            postData.Add("claim_amount", shipment.InsurancePolicy.DamageValue.GetValueOrDefault().ToString());
            postData.Add("country", shipment.ShipPerson.AdjustedCountryCode(ShipmentTypeCode.None));
            postData.Add("phone", shipment.ShipPerson.Phone10Digits);
            postData.Add("address1", shipment.ShipStreet1);
            postData.Add("address2", shipment.ShipStreet2);
            postData.Add("city", shipment.ShipCity);
            postData.Add("state", shipment.ShipStateProvCode);
            postData.Add("zip", shipment.ShipPostalCode);
            postData.Add("order_number", InsureShipShipmentIdentifier.GetUniqueShipmentId(shipment));
            postData.Add("tracking_number", shipment.TrackingNumber);
            postData.Add("date_of_purchase", shipment.Order.OrderDate.ToString(CultureInfo.InvariantCulture));
            postData.Add("purchase_amount", shipment.Order.SubTotal.ToString("#0.##"));
            postData.Add("currency", "USD");

            return postData;
        }
    }
}

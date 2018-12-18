using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Insure
{
    /// <summary>
    /// InsureShip request class for insuring a shipment
    /// </summary>
    [Component]
    public class InsureShipInsureShipmentRequest : IInsureShipInsureShipmentRequest
    {
        private readonly IInsureShipWebClient webClient;
        private readonly ILog log;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IInsuranceUtility insuranceUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipInsureShipmentRequest(
            IInsureShipWebClient webClient,
            IShipmentTypeManager shipmentTypeManager,
            IInsuranceUtility insuranceUtility,
            Func<Type, ILog> createLog)
        {
            this.insuranceUtility = insuranceUtility;
            this.shipmentTypeManager = shipmentTypeManager;
            this.webClient = webClient;
            log = createLog(GetType());
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public Result CreateInsurancePolicy(ShipmentEntity shipment)
        {
            shipment.InsurancePolicy = shipment.InsurancePolicy ?? new InsurancePolicyEntity
            {
                InsureShipStoreName = string.Empty,
                CreatedWithApi = false
            };

            return webClient.Submit<InsureShipNewPolicyResponse>("new_policy", shipment.Order.Store, CreatePostData(shipment))
                .Do(x =>
                {
                    shipment.InsurancePolicy.InsureShipPolicyID = x.PolicyID;
                    shipment.InsurancePolicy.CreatedWithApi = true;
                });
        }

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private Dictionary<string, string> CreatePostData(ShipmentEntity shipment)
        {
            var insuredAmount = insuranceUtility.GetInsuredValue(shipment);

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("customer_name", shipment.ShipPerson.ParsedName.FullName);
            postData.Add("firstname", shipment.ShipFirstName);
            postData.Add("lastname", shipment.ShipLastName);
            postData.Add("items_ordered", string.Join(",", shipment.Order.OrderItems.Select(oi => oi.Name)));
            postData.Add("order_total", shipment.Order.OrderTotal.ToString("#0.##"));
            postData.Add("subtotal", insuredAmount.ToString("#0.##"));
            postData.Add("currency", "USD");
            postData.Add("coverage_amount", insuranceUtility.GetInsuranceCost(shipment, insuredAmount).ShipWorks?.ToString("#0.##"));
            postData.Add("shipping_amount", shipment.ShipmentCost.ToString("#0.##"));
            postData.Add("order_number", InsureShipShipmentIdentifier.GetUniqueShipmentId(shipment));
            postData.Add("offer_id", "57611"); // This is the type of insurance coverage, and the value came from InsureShip
            postData.Add("email", shipment.ShipEmail);
            postData.Add("phone", shipment.ShipPhone);
            postData.Add("carrier", shipmentType.ShipmentTypeName);
            postData.Add("tracking_number", shipment.TrackingNumber);
            postData.Add("order_date", shipment.Order.OrderDate.ToString(CultureInfo.InvariantCulture));
            postData.Add("ship_date", shipment.ShipDate.ToString(CultureInfo.InvariantCulture));

            AddAddress("shipping", shipment.ShipPerson, postData);
            AddAddress("billing", shipment.Order.BillPerson, postData);

            return postData;
        }

        /// <summary>
        /// Add an address to the post data
        /// </summary>
        private static void AddAddress(string prefix, PersonAdapter person, Dictionary<string, string> postData)
        {
            postData.Add(prefix + "_address1", person.Street1);
            postData.Add(prefix + "_address2", person.Street2);
            postData.Add(prefix + "_city", person.City);
            postData.Add(prefix + "_state", person.StateProvCode);
            postData.Add(prefix + "_zip", person.PostalCode);
            postData.Add(prefix + "_country", person.AdjustedCountryCode(ShipmentTypeCode.None));
        }
    }
}

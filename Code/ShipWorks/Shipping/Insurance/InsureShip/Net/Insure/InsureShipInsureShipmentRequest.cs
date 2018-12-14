using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

            return webClient.Submit<InsureShipNewPolicyResponse>("new_policy", CreatePostData(shipment))
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
            // TODO: Verify that iParcel always saves to InsuranceValue

            // PopulateShipmentOrder();

            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("customer_name", shipment.ShipPerson.ParsedName.FullName);
            postData.Add("firstname", shipment.ShipFirstName);
            postData.Add("lastname", shipment.ShipLastName);
            postData.Add("items_ordered", string.Join(",", shipment.Order.OrderItems.Select(oi => oi.Name)));
            postData.Add("order_total", shipment.Order.OrderTotal.ToString("#0.##"));
            postData.Add("subtotal", shipment.Order.SubTotal.ToString("#0.##"));
            postData.Add("currency", "USD");
            postData.Add("coverage_amount", insuranceUtility.GetInsuredValue(shipment).ToString(CultureInfo.InvariantCulture));
            postData.Add("shipping_amount", shipment.ShipmentCost.ToString("#0.##"));
            postData.Add("order_number", InsureShipShipmentIdentifier.GetUniqueShipmentId(shipment));
            //postData.Add("offer_id", ""); // Is this the same as rate_id??
            //postData.Add("rate_id", "11");
            postData.Add("email", shipment.ShipEmail);
            postData.Add("phone", shipment.ShipPhone);
            postData.Add("carrier", shipmentType.ShipmentTypeName);
            postData.Add("tracking_number", shipment.TrackingNumber);
            postData.Add("order_date", shipment.Order.OrderDate.ToString(CultureInfo.InvariantCulture));
            postData.Add("ship_date", shipment.ShipDate.ToString(CultureInfo.InvariantCulture));

            postData.Add("shipping_address1", shipment.ShipStreet1);
            postData.Add("shipping_address2", shipment.ShipStreet2);
            postData.Add("shipping_city", shipment.ShipCity);
            postData.Add("shipping_state", shipment.ShipStateProvCode);
            postData.Add("shipping_zip", shipment.ShipPostalCode);
            postData.Add("shipping_country", shipment.ShipPerson.AdjustedCountryCode(ShipmentTypeCode.None));
            postData.Add("billing_address1", shipment.Order.BillStreet1);
            postData.Add("billing_address2", shipment.Order.BillStreet2);
            postData.Add("billing_city", shipment.Order.BillCity);
            postData.Add("billing_state", shipment.Order.BillStateProvCode);
            postData.Add("billing_zip", shipment.Order.BillPostalCode);
            postData.Add("billing_country", shipment.Order.BillPerson.AdjustedCountryCode(ShipmentTypeCode.None));

            return postData;
        }
    }
}

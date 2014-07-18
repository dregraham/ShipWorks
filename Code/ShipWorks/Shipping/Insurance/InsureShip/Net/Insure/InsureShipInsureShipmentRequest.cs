using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Insure
{
    /// <summary>
    /// InsureShip request class for insuring a shipment
    /// </summary>
    public class InsureShipInsureShipmentRequest : InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipInsureShipmentRequest(ShipmentEntity shipment, InsureShipAffiliate affiliate) :
            this(new InsureShipResponseFactory(), shipment, affiliate, new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipInsureShipmentRequest)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipInsureShipmentRequest(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log) : 
            base(responseFactory, shipment, affiliate, insureShipSettings, log)
        { }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public override IInsureShipResponse Submit()
        {
            Uri uri = new Uri(string.Format("{0}distributors/{1}/policies", Settings.Url.AbsoluteUri, Settings.DistributorID));
            return SubmitPost(uri, CreatePostData());
        }

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private Dictionary<string, string> CreatePostData()
        {
            // TODO: Verify that iParcel always saves to InsuranceValue

            PopulateShipmentOrder();

            ShipmentType shipmentType = ShipmentTypeManager.GetType(Shipment);

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("distributor_id", Settings.DistributorID);
            postData.Add("store_id", Affiliate.InsureShipStoreID);
            postData.Add("store_name", Affiliate.InsureShipPolicyID);
            postData.Add("rate_id", "11");
            postData.Add("email", Shipment.OriginEmail);
            postData.Add("firstname", Shipment.OriginFirstName);
            postData.Add("lastname", Shipment.OriginLastName);
            postData.Add("shipping_city", Shipment.ShipCity);
            postData.Add("shipping_state", Shipment.ShipStateProvCode);
            postData.Add("shipping_zip", Shipment.ShipPostalCode);
            postData.Add("shipping_country ", Shipment.ShipCountryCode);
            postData.Add("shipment_value", GetInsuredValue().ToString(CultureInfo.InvariantCulture));
            postData.Add("order_id", GetUniqueShipmentId());
            postData.Add("shipment_id", GetUniqueShipmentId());
            postData.Add("tracking_id", Shipment.TrackingNumber);
            postData.Add("item_name", string.Join(",", Shipment.Order.OrderItems.Select(oi => oi.Name)));
            postData.Add("carrier", shipmentType.ShipmentTypeName);
            postData.Add("carrier_code", InsureShipCarrierCode.GetCarrierCode(Shipment));
            
            // If using the test server, append the test affiliate.
            if (Settings.UseTestServer)
            {            
                postData.Add("affiliate_id", "A0000000003");
            }

            return postData;
        }

        /// <summary>
        /// Determines the insured value for the shipment.
        /// </summary>
        private decimal GetInsuredValue()
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(Shipment);

            return
                Enumerable.Range(0, shipmentType.GetParcelCount(Shipment))
                    .Select(parcelIndex => shipmentType.GetParcelDetail(Shipment, parcelIndex).Insurance)
                    .Where(
                        choice =>
                            choice.Insured && choice.InsuranceProvider == InsuranceProvider.ShipWorks &&
                            choice.InsuranceValue > 0)
                    .Select(insuredPackages => insuredPackages.InsuranceValue)
                    .DefaultIfEmpty(0)
                    .Sum();
        }
    }
}

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
            base(responseFactory, shipment, affiliate, insureShipSettings, log, "InsureShipment")
        { }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public override IInsureShipResponse Submit()
        {
            Shipment.InsurancePolicy = new InsurancePolicyEntity()
            {
                InsureShipStoreName = Affiliate.InsureShipStoreID,
                CreatedWithApi = false
            };

            Uri uri = new Uri(string.Format("{0}distributors/{1}/policies", Settings.ApiUrl.AbsoluteUri, Settings.DistributorID));
            SubmitPost(uri, CreatePostData());
            return ResponseFactory.CreateInsureShipmentResponse(this);
        }

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private Dictionary<string, string> CreatePostData()
        {
            // TODO: Verify that iParcel always saves to InsuranceValue

            PopulateShipmentOrder();

            ShipmentType shipmentType = ShipmentTypeManager.GetType(Shipment);
            string unqiueShipmentId = new InsureShipShipmentIdentifier(Shipment).GetUniqueShipmentId();

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("distributor_id", Settings.DistributorID);
            postData.Add("store_id", Affiliate.InsureShipStoreID);
            postData.Add("store_name", Affiliate.InsureShipPolicyID);
            postData.Add("rate_id", "11");
            postData.Add("email", Shipment.ShipEmail);
            postData.Add("firstname", Shipment.ShipFirstName);
            postData.Add("lastname", Shipment.ShipLastName);
            postData.Add("phone", Shipment.ShipPhone);
            postData.Add("date", Shipment.Order.OrderDate.ToString(CultureInfo.InvariantCulture));
            postData.Add("shipping_address", string.Format("{0}{1}{2}{3}{4}", Shipment.ShipStreet1, Environment.NewLine, Shipment.ShipStreet2, Environment.NewLine, Shipment.ShipStreet3));
            postData.Add("shipping_city", Shipment.ShipCity);
            postData.Add("shipping_state", Shipment.ShipStateProvCode);
            postData.Add("shipping_zip", Shipment.ShipPostalCode);
            postData.Add("shipping_country", Shipment.ShipPerson.AdjustedCountryCode(ShipmentTypeCode.None));
            postData.Add("shipment_value", InsuranceUtility.GetInsuredValue(Shipment).ToString(CultureInfo.InvariantCulture));
            postData.Add("order_id", unqiueShipmentId);
            postData.Add("shipment_id", unqiueShipmentId);
            postData.Add("tracking_id", Shipment.TrackingNumber);
            postData.Add("item_name", string.Join(",", Shipment.Order.OrderItems.Select(oi => oi.Name)));
            postData.Add("carrier", shipmentType.ShipmentTypeName);
            postData.Add("carrier_code", InsureShipCarrierCode.GetCarrierCode(Shipment));

            return postData;
        }
    }
}

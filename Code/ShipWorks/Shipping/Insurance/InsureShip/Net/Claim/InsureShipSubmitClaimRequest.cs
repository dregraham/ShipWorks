using System;
using System.Collections.Generic;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// InsureShip request class for submitting a claim
    /// </summary>
    public class InsureShipSubmitClaimRequest : InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipSubmitClaimRequest(ShipmentEntity shipment, InsureShipAffiliate affiliate) :
            this(new InsureShipResponseFactory(), shipment, affiliate, new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipSubmitClaimRequest)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipSubmitClaimRequest(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log)
            : base(responseFactory, shipment, affiliate, insureShipSettings, log, "SubmitClaim")
        {
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public override IInsureShipResponse Submit()
        {
            Uri uri = new Uri(string.Format("{0}claims/submit/{1}", Settings.ApiUrl.AbsoluteUri, Settings.DistributorID));
            SubmitPost(uri, CreatePostData());
            return ResponseFactory.CreateSubmitClaimResponse(this);
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
            postData.Add("email", Shipment.InsurancePolicy.EmailAddress);
            postData.Add("firstname", Shipment.ShipFirstName);
            postData.Add("lastname", Shipment.ShipLastName);
            postData.Add("item_name", Shipment.InsurancePolicy.ItemName);
            postData.Add("description", Shipment.InsurancePolicy.Description);
            postData.Add("type", Shipment.InsurancePolicy.ClaimType.Value.ToString());
            postData.Add("damage_value", Shipment.InsurancePolicy.DamageValue.Value.ToString());
            postData.Add("shipping_date", Shipment.ShipDate.ToString("MM/dd/yyyy"));
            postData.Add("shipping_city", Shipment.ShipCity);
            postData.Add("shipping_state", Shipment.ShipStateProvCode);
            postData.Add("shipping_zip", Shipment.ShipPostalCode);
            postData.Add("shipping_country", Shipment.ShipPerson.AdjustedCountryCode(ShipmentTypeCode.None));
            postData.Add("billing_city", Shipment.Order.BillCity);
            postData.Add("billing_state", Shipment.Order.BillStateProvCode);
            postData.Add("billing_zip", Shipment.Order.BillPostalCode);
            postData.Add("billing_country", Shipment.Order.BillPerson.AdjustedCountryCode(ShipmentTypeCode.None));
            postData.Add("order_id", new InsureShipShipmentIdentifier(Shipment).GetUniqueShipmentId());
            postData.Add("tracking_id", Shipment.TrackingNumber);
            postData.Add("carrier", shipmentType.ShipmentTypeName);

            return postData;
        }
    }
}

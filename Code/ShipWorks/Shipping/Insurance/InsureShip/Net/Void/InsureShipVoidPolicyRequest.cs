using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using log4net;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Void
{
    /// <summary>
    /// InsureShip request class for voiding a policy
    /// </summary>
    public class InsureShipVoidPolicyRequest : InsureShipRequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipVoidPolicyRequest"/> class.
        /// </summary>
        public InsureShipVoidPolicyRequest(ShipmentEntity shipment, InsureShipAffiliate affiliate) :
            this(new InsureShipResponseFactory(), shipment, affiliate, new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipVoidPolicyRequest)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipVoidPolicyRequest"/> class.
        /// </summary>
        public InsureShipVoidPolicyRequest(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log) : 
            base(responseFactory, shipment, affiliate, insureShipSettings, log, "VoidPolicy")
        { }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public override IInsureShipResponse Submit()
        {
            Uri uri = new Uri(string.Format("{0}distributors/{1}/void_policy", Settings.ApiUrl.AbsoluteUri, Settings.DistributorID));
            SubmitPost(uri, CreatePostData());
            return ResponseFactory.CreateVoidPolicyResponse(this);
        }

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to void a policy.
        /// </summary>
        private Dictionary<string, string> CreatePostData()
        {
            PopulateShipmentOrder();

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("distributor_id", Settings.DistributorID);
            postData.Add("store_id", Affiliate.InsureShipStoreID);
            postData.Add("store_name", Affiliate.InsureShipPolicyID);
            postData.Add("order_id", new InsureShipShipmentIdentifier(Shipment).GetUniqueShipmentId());
            postData.Add("firstname", Shipment.ShipFirstName);
            postData.Add("lastname", Shipment.ShipLastName);
            
            return postData;
        }
    }
}

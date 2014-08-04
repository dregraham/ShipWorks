using System;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using log4net;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// InsureShip request class for checking the status of a claim
    /// </summary>
    public class InsureShipClaimStatusRequest : InsureShipRequestBase
    {
        private readonly Uri uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipClaimStatusRequest"/> class.
        /// </summary>
        public InsureShipClaimStatusRequest(ShipmentEntity shipment, InsureShipAffiliate affiliate) :
            this(new InsureShipResponseFactory(), shipment, affiliate, new InsureShipSettings(), LogManager.GetLogger(typeof(InsureShipSubmitClaimRequest)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipClaimStatusRequest"/> class.
        /// </summary>
        public InsureShipClaimStatusRequest(IInsureShipResponseFactory responseFactory, ShipmentEntity shipment, InsureShipAffiliate affiliate, IInsureShipSettings insureShipSettings, ILog log)
            : base(responseFactory, shipment, affiliate, insureShipSettings, log, "ClaimStatus")
        {
            uri = new Uri(string.Format("{0}/claims/status{1}/{2}", Settings.ApiUrl.AbsoluteUri, Settings.DistributorID, Shipment.InsurancePolicy.ClaimID));
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        /// <returns>An instance of IInsureShipResponse for processing the response.</returns>
        public override IInsureShipResponse Submit()
        {
            EnsureSecureConnection();
            
            ConfigureNewRequestSubmitter(uri);
            RequestSubmitter.Verb = HttpVerb.Get;

            // Log the request before submitting to InsureShip
            LogRequest(RequestSubmitter);
            HttpWebResponse webResponse = RequestSubmitter.GetResponse().HttpWebResponse;

            // Read the raw response and log 
            ReadResponse(webResponse);
            LogInsureShipResponse(webResponse);
            
            return ResponseFactory.CreateClaimStatusResponse(this);
        }
    }
}

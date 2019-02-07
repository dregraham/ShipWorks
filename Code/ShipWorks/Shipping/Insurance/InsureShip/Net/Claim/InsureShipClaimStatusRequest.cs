using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net.Claim
{
    /// <summary>
    /// InsureShip request class for checking the status of a claim
    /// </summary>
    [Component]
    public class InsureShipClaimStatusRequest : IInsureShipClaimStatusRequest
    {
        private readonly IInsureShipWebClient webClient;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsureShipRequestBase"/> class.
        /// </summary>
        public InsureShipClaimStatusRequest(IInsureShipWebClient webClient, Func<Type, ILog> createLog)
        {
            this.webClient = webClient;
            log = createLog(GetType());
        }

        /// <summary>
        /// Submits this request to InsureShip
        /// </summary>
        public GenericResult<string> GetClaimStatus(IShipmentEntity shipment) =>
            webClient.Submit<InsureShipGetClaimStatusResponse>("get_claim_status", shipment.Order.Store, CreatePostData(shipment))
                .Map(x => x.Status);

        /// <summary>
        /// Builds a string of all the data that needs to be sent to InsureShip to insure a shipment.
        /// </summary>
        private Dictionary<string, string> CreatePostData(IShipmentEntity shipment) =>
            new Dictionary<string, string>
            {
                { "claim_id", shipment.InsurancePolicy?.ClaimID?.ToString() }
            };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Void.Response
{
    /// <summary>
    /// An implementation of the ICarrierRequest representing the response from a FedEx  void request.
    /// </summary>
    public class FedExVoidResponse : ICarrierResponse
    {
        private readonly ShipmentReply voidReply;
        private readonly CarrierRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExVoidResponse" /> class.
        /// </summary>
        /// <param name="voidReply">The void reply.</param>
        /// <param name="request">The request.</param>
        public FedExVoidResponse(ShipmentReply voidReply, CarrierRequest request)
        {
            this.voidReply = voidReply;
            this.request = request;
        }

        /// <summary>
        /// Gets the request the was used to generate the response.
        /// </summary>
        /// <value>The CarrierRequest object.</value>
        public CarrierRequest Request
        {
            get { return request; }
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse
        {
            get { return voidReply; }
        }

        /// <summary>
        /// Function that tells CarrierResponse to process a request's response
        /// </summary>
        public void Process()
        {
            Validate();
        }

        /// <summary>
        /// Validate the reply
        /// </summary>
        private void Validate()
        {
            // Check for errors
            if (voidReply.HighestSeverity == NotificationSeverityType.ERROR || voidReply.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiException(voidReply.Notifications);
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}

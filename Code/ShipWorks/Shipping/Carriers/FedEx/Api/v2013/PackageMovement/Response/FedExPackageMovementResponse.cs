using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.PackageMovement.Response
{
    /// <summary>
    /// This object is used to process the FedExPackageMovementResponse. 
    /// It is populated with the actual WSDL respone object.
    /// </summary>
    public class FedExPackageMovementResponse : ICarrierResponse
    {
        PostalCodeInquiryReply nativeResponse = new PostalCodeInquiryReply();
        private CarrierRequest request;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExPackageMovementResponse(PostalCodeInquiryReply reply, CarrierRequest request)
        {
            this.nativeResponse = reply;
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
        /// LocationID set after Process() called
        /// </summary>
        public string LocationID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the native response received from the carrier API.
        /// </summary>
        /// <value>The native response.</value>
        public object NativeResponse
        {
            get { return nativeResponse; }
        }

        /// <summary>
        /// Check for errors and set LocationID
        /// </summary>
        public void Process()
        {
            Verify();

            LocationID = nativeResponse.ExpressDescription.LocationId;
        }

        /// <summary>
        /// Check for errors
        /// </summary>
        private void Verify()
        {
            if (nativeResponse.HighestSeverity == NotificationSeverityType.ERROR || nativeResponse.HighestSeverity == NotificationSeverityType.FAILURE)
            {
                throw new FedExApiException(nativeResponse.Notifications);
            }
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public IEntity2 CarrierAccountEntity { get; set; }
    }
}

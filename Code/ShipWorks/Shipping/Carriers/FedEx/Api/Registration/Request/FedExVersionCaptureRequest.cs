using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request
{
    /// <summary>
    /// Encapsulates Version Capture Request
    /// </summary>
    public class FedExVersionCaptureRequest : CarrierRequest
    {
        private readonly string accountLocationID;
        private readonly IFedExServiceGateway fedExService;
        private readonly FedExAccountEntity accountEntity;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestManipulators">The request manipulators.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="accountLocationID">The locationID is grabbed by package movement</param>
        /// <param name="fedExService">Service that actually makes the FedEx call</param>
        /// <param name="accountEntity">The account entity.</param>
        public FedExVersionCaptureRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipmentEntity, string accountLocationID, IFedExServiceGateway fedExService, FedExAccountEntity accountEntity)
            : base(requestManipulators, shipmentEntity)
        {
            this.accountLocationID = accountLocationID;
            this.fedExService = fedExService;
            this.accountEntity = accountEntity;
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity
        {
            get { return accountEntity; }
        }

        public override ICarrierResponse Submit()
        {
            NativeRequest = new VersionCaptureRequest
            {
                OriginLocationId = accountLocationID,
                VendorProductPlatform = "WINDOWS"
            };

            ApplyManipulators();

            VersionCaptureReply reply = fedExService.VersionCapture(NativeRequest as VersionCaptureRequest);

            return new FedExVersionCaptureResponse(reply, this);
        }
    }
}

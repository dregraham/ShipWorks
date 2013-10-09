using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request
{
    /// <summary>
    /// FedEx PackageMovement Request
    /// </summary>
    public class FedExPackageMovementRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway fedExService;
        private readonly FedExAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExPackageMovementRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipment, FedExAccountEntity account, IFedExServiceGateway fedExService)
            : base(requestManipulators, shipment)
        {
            this.fedExService = fedExService;
            this.account = account;
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity
        {
            get { return account; }
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>
        /// An ICarrierResponse containing the carrier-specific results of the request.
        /// </returns>
        public override ICarrierResponse Submit()
        {
            NativeRequest = new PostalCodeInquiryRequest
            {
                PostalCode = account.PostalCode,
                CountryCode = account.CountryCode
            };

            ApplyManipulators();

            PostalCodeInquiryReply reply = fedExService.PostalCodeInquiry(NativeRequest as PostalCodeInquiryRequest);
            return new FedExPackageMovementResponse(reply, this);
        }
    }
}

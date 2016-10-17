using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request
{
    /// <summary>
    /// GlobalShipAddress Request - Used to get Hold At Location addresses.
    /// </summary>
    public class FedExGlobalShipAddressRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway fedExService;
        private readonly IFedExResponseFactory responseFactory;
        private readonly FedExAccountEntity accountEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressRequest" /> class.
        /// </summary>
        public FedExGlobalShipAddressRequest(IEnumerable<ICarrierRequestManipulator> requestManipulators, ShipmentEntity shipment, IFedExResponseFactory responseFactory, IFedExServiceGateway fedExService, FedExAccountEntity accountEntity)
            : base(requestManipulators, shipment)
        {
            this.fedExService = fedExService;
            this.responseFactory = responseFactory;
            this.accountEntity = accountEntity;

            NativeRequest = new SearchLocationsRequest();
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity
        {
            get { return accountEntity; }
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>
        /// An ICarrierResponse containing the carrier-specific results of the request.
        /// </returns>
        public override ICarrierResponse Submit()
        {
            // Allow the manipulators to build the raw request for the FedEx service
            ApplyManipulators();

            SearchLocationsReply searchLocationsReply = fedExService.GlobalShipAddressInquiry(NativeRequest as SearchLocationsRequest);

            // Defer to the response factory to create the ship response that wraps the raw/native response obtained from the service
            return responseFactory.CreateGlobalShipAddressResponse(searchLocationsReply, this);
        }
    }
}

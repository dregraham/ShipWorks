using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx RateRequest request types.
    /// </summary>
    public class FedExRateRequest : CarrierRequest
    {
        private readonly IFedExServiceGateway serviceGateway;
        private readonly ICarrierResponseFactory responseFactory;
        private readonly ICarrierSettingsRepository settingsRepository;

        ///// <summary>
        ///// Initializes a new instance of the <see cref="FedExRateRequest" /> class using the
        ///// the settings repository that is provided and the "live" implementations for the
        ///// service gateway, response factory.
        ///// </summary>
        ///// <param name="manipulators">The manipulators.</param>
        ///// <param name="shipmentEntity">The shipment entity.</param>
        ///// <param name="settingsRepository">Repository that should be used for settings</param>
        //public FedExRateRequest(IEnumerable<ICarrierRequestManipulator> manipulators, ShipmentEntity shipmentEntity, ICarrierSettingsRepository settingsRepository)
        //    : this(manipulators, shipmentEntity, new FedExServiceGateway(settingsRepository), new FedExResponseFactory(new FedExLabelRepository()), settingsRepository)
        //{ }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateRequest" /> class.
        /// </summary>
        /// <param name="manipulators">The manipulators.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="serviceGateway">The service gateway.</param>
        /// <param name="responseFactory">The response factory.</param>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExRateRequest(IEnumerable<ICarrierRequestManipulator> manipulators, ShipmentEntity shipmentEntity, IFedExServiceGateway serviceGateway, ICarrierResponseFactory responseFactory, ICarrierSettingsRepository settingsRepository)
            : base(manipulators, shipmentEntity)
        {
            this.serviceGateway = serviceGateway;
            this.responseFactory = responseFactory;
            this.settingsRepository = settingsRepository;

            NativeRequest = new RateRequest();
        }

        /// <summary>
        /// Gets the carrier account entity.
        /// </summary>
        /// <value>The carrier account entity.</value>
        public override IEntity2 CarrierAccountEntity
        {
            get { return settingsRepository.GetAccount(ShipmentEntity); }
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public override ICarrierResponse Submit()
        {
            // Allow the manipulators to build the raw request for the FedEx service
            ApplyManipulators();

            // The request is ready to be sent to FedEx; we're sure the native request will be a RateRequest
            // (since we assigned it as such in the constructor) so we can safely cast it here
            RateReply nativeResponse = serviceGateway.GetRates(NativeRequest as RateRequest, ShipmentEntity);
            return responseFactory.CreateRateResponse(nativeResponse, this);
        }
    }
}

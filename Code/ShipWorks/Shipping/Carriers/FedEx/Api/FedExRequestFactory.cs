using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// FedEx Request Factory
    /// </summary>
    public class FedExRequestFactory : IFedExRequestFactory
    {
        private readonly IFedExServiceGateway defaultFedExServiceGateway;
        private readonly IFedExServiceGateway openShipFedExServiceGateway;
        private readonly ICarrierResponseFactory responseFactory;
        private readonly ICarrierSettingsRepository settingsRepository;
        private readonly IFedExShipmentTokenProcessor tokenProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRequestFactory" /> class.
        /// </summary>
        public FedExRequestFactory(ICarrierSettingsRepository settingsRepository)
            : this(new FedExServiceGateway(settingsRepository), new FedExOpenShipGateway(settingsRepository), settingsRepository, new FedExShipmentTokenProcessor(), new FedExResponseFactory())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRequestFactory" /> class. This
        /// constructor is primarily for testing purposes.
        /// </summary>
        public FedExRequestFactory(IFedExServiceGateway defaultFedExServiceGateway, IFedExServiceGateway openShipFedExServiceGateway, ICarrierSettingsRepository settingsRepository, IFedExShipmentTokenProcessor tokenProcessor, ICarrierResponseFactory responseFactory)
        {
            this.defaultFedExServiceGateway = defaultFedExServiceGateway;
            this.openShipFedExServiceGateway = openShipFedExServiceGateway;
            this.responseFactory = responseFactory;
            this.settingsRepository = settingsRepository;
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Creates the carrier-specific request to ship an order/create a label.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting requests to a carrier API to
        /// ship an order/create a label.
        /// </returns>
        public virtual CarrierRequest CreateShipRequest(ShipmentEntity shipmentEntity)
        {
            if (shipmentEntity == null)
            {
                throw new ArgumentNullException("shipmentEntity");
            }

            if (shipmentEntity.FedEx == null)
            {
                throw new ArgumentException("ShipmentEntity not associated with a FedEx shipment.");
            }

            // Load up all of the dependencies for a FedExShipRequest and provide them to the constructor
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>()
            {
                // This list will grow as shipping request manipulators are implemented
                new FedExShipperManipulator(),
                new FedExRecipientManipulator(),
                new FedExShipmentSpecialServiceTypeManipulator(),
                new FedExRateTypeManipulator(settingsRepository),
                new FedExLabelSpecificationManipulator(settingsRepository),
                new FedExTotalWeightManipulator(),
                new FedExTotalInsuredValueManipulator(),
                new FedExShippingChargesManipulator(),
                new FedExCertificationManipulator(tokenProcessor, settingsRepository),
                new FedExPackagingTypeManipulator(),
                new FedExPickupManipulator(),
                new FedExServiceTypeManipulator(),
                new FedExPackageSpecialServicesManipulator(),
                new FedExShippingWebAuthenticationDetailManipulator(),
                new FedExShippingClientDetailManipulator(settingsRepository),
                new FedExShippingVersionManipulator(), 
                new FedExReferenceManipulator(tokenProcessor, settingsRepository),
                new FedExPackageDetailsManipulator(),
                new FedExEmailNotificationsManipulator(),
                new FedExPriorityAlertManipulator(),
                new FedExDryIceManipulator(),
                new FedExMasterTrackingManipulator(),
                new FedExCodOptionsManipulator(settingsRepository),
                new FedExCustomsManipulator(),
                new FedExHoldAtLocationManipulator(),
                new FedExAdmissibilityManipulator(),
                new FedExBrokerManipulator(),
                new FedExCommercialInvoiceManipulator(),
                new FedExHomeDeliveryManipulator(),
                new FedExFreightManipulator(settingsRepository),
                new FedExDangerousGoodsManipulator(),
                new FedExSmartPostManipulator(),
                new FedExReturnsManipulator(),
                new FedExTrafficInArmsManipulator(),
                new FedExInternationalControlledExportManipulator(),
                new FedExOneRateManipulator()
            };

            IFedExNativeShipmentRequest nativeShipmentRequest;

            nativeShipmentRequest = new ProcessShipmentRequest();

            return new FedExShipRequest(manipulators, shipmentEntity, ChooseFedExServiceGateway(shipmentEntity), responseFactory, settingsRepository, nativeShipmentRequest);
        }

        /// <summary>
        /// Creates the version capture request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="accountLocationId">The account location ID.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to do the version capture.</returns>
        public CarrierRequest CreateVersionCaptureRequest(ShipmentEntity shipmentEntity, string accountLocationId, FedExAccountEntity account)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExRegistrationWebAuthenticationDetailManipulator(settingsRepository),
                new FedExRegistrationClientDetailManipulator(settingsRepository),
                new FedExRegistrationVersionManipulator()
            };

            // TODO: Look into injecting the response factory here like that used in the CreateShipResponse method
            return new FedExVersionCaptureRequest(manipulators, shipmentEntity, accountLocationId, defaultFedExServiceGateway, account);
        }

        /// <summary>
        /// Creates the package movement request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="account">The account.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx for package movements.</returns>
        public CarrierRequest CreatePackageMovementRequest(ShipmentEntity shipmentEntity, FedExAccountEntity account)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExPackageMovementWebAuthenticationDetailManipulator(settingsRepository),
                new FedExPackageMovementClientDetailManipulator(settingsRepository),
                new FedExPackageMovementVersionManipulator()
            };

            // TODO: Look into injecting the response factory here like that used in the CreateShipResponse method
            return new FedExPackageMovementRequest(manipulators, shipmentEntity, account, defaultFedExServiceGateway);
        }

        /// <summary>
        /// Creates the Search Location request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx searching dropoff location.</returns>
        public CarrierRequest CreateSearchLocationsRequest(ShipmentEntity shipmentEntity, FedExAccountEntity accountEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExGlobalShipAddressAddressManipulator(),
                new FedExGlobalShipAddressConstraintManipulator(),
                new FedExGlobalShipAddressWebAuthenticationDetailManipulator(settingsRepository),
                new FedExGlobalShipAddressVersionManipulator(),
                new FedExGlobalShipAddressClientDetailManipulator(settingsRepository)
            };

            return new FedExGlobalShipAddressRequest(manipulators, shipmentEntity, new FedExResponseFactory(), defaultFedExServiceGateway, accountEntity);
        }


        /// <summary>
        /// Creates the ground close request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to do the end of day close.</returns>
        public CarrierRequest CreateGroundCloseRequest(FedExAccountEntity accountEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExCloseWebAuthenticationDetailManipulator(),
                new FedExCloseClientDetailManipulator(),
                new FedExCloseVersionManipulator(),
                new FedExCloseDateManipulator()
            };

            return new FedExGroundCloseRequest(manipulators, null, defaultFedExServiceGateway, new FedExResponseFactory(), accountEntity);
        }

        /// <summary>
        /// Creates the smart post close request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to do the end of day close.</returns>
        public CarrierRequest CreateSmartPostCloseRequest(FedExAccountEntity accountEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExCloseWebAuthenticationDetailManipulator(),
                new FedExCloseClientDetailManipulator(),
                new FedExCloseVersionManipulator(),
                new FedExPickupCarrierManipulator()
            };

            return new FedExSmartPostCloseRequest(manipulators, null, defaultFedExServiceGateway, new FedExResponseFactory(), accountEntity);
        }

        /// <summary>
        /// Creates the void request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="shipmentEntity">The shipment entity to void</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to void a shipment.</returns>
        public CarrierRequest CreateVoidRequest(FedExAccountEntity accountEntity, ShipmentEntity shipmentEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExVoidWebAuthenticationDetailManipulator(),
                new FedExVoidClientDetailManipulator(settingsRepository),
                new FedExVoidVersionManipulator(),
                new FedExVoidParametersManipulator()
            };

            return new FedExVoidRequest(manipulators, shipmentEntity, ChooseFedExServiceGateway(shipmentEntity), new FedExResponseFactory(), accountEntity);
        }

        /// <summary>
        /// Creates the register CSP user request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to register a CSP user.</returns>
        public CarrierRequest CreateRegisterCspUserRequest(FedExAccountEntity accountEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExRegistrationWebAuthenticationDetailManipulator(),
                new FedExRegistrationClientDetailManipulator(settingsRepository),
                new FedExRegistrationVersionManipulator(),
                new FedExCspContactManipulator()
            };

            return new FedExRegisterCspUserRequest(manipulators, accountEntity);
        }

        /// <summary>
        /// Creates the subscription request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to subscribe a shipper to use the FedEx API.</returns>
        public CarrierRequest CreateSubscriptionRequest(FedExAccountEntity accountEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExRegistrationWebAuthenticationDetailManipulator(),
                new FedExRegistrationClientDetailManipulator(settingsRepository),
                new FedExRegistrationVersionManipulator(),
                new FedExSubscriberManipulator()
            };

            return new FedExSubscriptionRequest(manipulators, accountEntity);
        }


        /// <summary>
        /// Creates the rate request.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <param name="specializedManipulators">Any specialized manipulators that should be added to the request in addition
        /// to the standard/basic manipulators of the rate request.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx for obtaining shipping rates.</returns>
        public CarrierRequest CreateRateRequest(ShipmentEntity shipmentEntity, IEnumerable<ICarrierRequestManipulator> specializedManipulators)
        {
            FedExSettings settings = new FedExSettings(settingsRepository);

            // Create the "standard" manipulators for a FedEx rate request
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExRateClientDetailManipulator(),
                new FedExRateWebAuthenticationManipulator(settings),
                new FedExRateVersionManipulator(),
                new FedExRateReturnTransitManipulator(),
                new FedExRateShipperManipulator(),
                new FedExRateRecipientManipulator(),
                new FedExRateShipmentSpecialServiceTypeManipulator(),
                new FedExRateTotalInsuredValueManipulator(settings),
                new FedExRateTotalWeightManipulator(),
                new FedExRateRateTypeManipulator(settingsRepository),
                new FedExRatePickupManipulator(),
                new FedExRatePackageDetailsManipulator(settings),
                new FedExRatePackageSpecialServicesManipulator(),
                new FedExRatePackagingTypeManipulator(),
                new FedExRateCodOptionsManipulator(settingsRepository),
                new FedExRateDryIceManipulator(settings),
                new FedExRateBrokerManipulator(settings)
            };

            if (specializedManipulators != null && specializedManipulators.Any())
            {
                // Add any special manipulators on top of the basic manipulators
                manipulators.AddRange(specializedManipulators);
            }

            return new FedExRateRequest(manipulators, shipmentEntity, settingsRepository);
        }

        /// <summary>
        /// Creates the track request.
        /// </summary>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to retrieve tracking data.</returns>
        public CarrierRequest CreateTrackRequest(FedExAccountEntity accountEntity, ShipmentEntity shipmentEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExTrackingWebAuthenticationDetailManipulator(),
                new FedExTrackingClientDetailManipulator(),
                new FedExTrackingVersionManipulator(),
                new FedExTrackingPackageIdentifierManipulator()
            };

            return new FedExTrackRequest(manipulators, shipmentEntity, defaultFedExServiceGateway, new FedExResponseFactory(), accountEntity);
        }

        /// <summary>
        /// Creates the certificate request.
        /// </summary>
        /// <param name="certificateInspector">The certificate inspector.</param>
        /// <returns>An instance of an ICertificateRequest that can be used to check the security level
        /// of a host's certificate.</returns>
        public ICertificateRequest CreateCertificateRequest(ICertificateInspector certificateInspector)
        {
            Uri fedExEndpoint = new Uri(new FedExSettings(settingsRepository).EndpointUrl);
            return new CertificateRequest(fedExEndpoint, certificateInspector);
        }

        /// <summary>
        /// If Email Return Shipment, return OpenShipFedExServiceGateway, else return default gateway.
        /// </summary>
        public IFedExServiceGateway ChooseFedExServiceGateway(ShipmentEntity shipmentEntity)
        {
            IFedExServiceGateway fedExServiceGateway = defaultFedExServiceGateway;
            if (shipmentEntity.ReturnShipment && shipmentEntity.FedEx.ReturnType == (int)FedExReturnType.EmailReturnLabel)
            {
                fedExServiceGateway = openShipFedExServiceGateway;
            }
            return fedExServiceGateway;
        }
    }
}

using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.UploadDocuments.Request.Manipulators;
using System;
using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// FedEx Request Factory
    /// </summary>
    [Component]
    public class FedExRequestFactory : IFedExRequestFactory
    {
        private readonly IFedExResponseFactory responseFactory;
        private readonly IFedExSettingsRepository settingsRepository;
        private readonly IFedExShipmentTokenProcessor tokenProcessor;
        private readonly IFedExServiceGatewayFactory serviceGatewayFactory;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRequestFactory" /> class
        /// </summary>
        /// <remarks>
        /// We're taking a dependency on ILifetimeScope for the Create methods that are now
        /// just passed through to Autofac.
        /// </remarks>
        public FedExRequestFactory(
            IFedExServiceGatewayFactory serviceGatewayFactory,
            IFedExSettingsRepository settingsRepository,
            IFedExShipmentTokenProcessor tokenProcessor,
            IFedExResponseFactory responseFactory,
            ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
            this.serviceGatewayFactory = serviceGatewayFactory;
            this.responseFactory = responseFactory;
            this.settingsRepository = settingsRepository;
            this.tokenProcessor = tokenProcessor;
        }

        /// <summary>
        /// Creates the rate request
        /// </summary>
        public IFedExShipRequest CreateShipRequest() =>
            lifetimeScope.Resolve<IFedExShipRequest>(TypedParameter.From(settingsRepository));

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
                new FedExRegistrationClientDetailManipulator(),
                new FedExRegistrationVersionManipulator()
            };

            // TODO: Look into injecting the response factory here like that used in the CreateShipResponse method
            return new FedExVersionCaptureRequest(manipulators, shipmentEntity, accountLocationId, serviceGatewayFactory.Create(settingsRepository), account);
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
            return new FedExPackageMovementRequest(manipulators, shipmentEntity, account, serviceGatewayFactory.Create(settingsRepository));
        }

        /// <summary>
        /// Creates the Search Location request
        /// </summary>
        public IFedExGlobalShipAddressRequest CreateSearchLocationsRequest() =>
            lifetimeScope.Resolve<IFedExGlobalShipAddressRequest>(TypedParameter.From(settingsRepository));

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

            return new FedExGroundCloseRequest(manipulators, null, serviceGatewayFactory.Create(settingsRepository), responseFactory, accountEntity);
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

            return new FedExSmartPostCloseRequest(manipulators, null, serviceGatewayFactory.Create(settingsRepository), responseFactory, accountEntity);
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
                new FedExVoidClientDetailManipulator(),
                new FedExVoidVersionManipulator(),
                new FedExVoidParametersManipulator()
            };

            return new FedExVoidRequest(manipulators, shipmentEntity, serviceGatewayFactory.Create(shipmentEntity, settingsRepository), responseFactory, accountEntity);
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
                new FedExRegistrationClientDetailManipulator(),
                new FedExRegistrationVersionManipulator(),
                new FedExCspContactManipulator()
            };

            return new FedExRegisterCspUserRequest(manipulators, serviceGatewayFactory.Create(settingsRepository), responseFactory, accountEntity);
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
                new FedExRegistrationClientDetailManipulator(),
                new FedExRegistrationVersionManipulator(),
                new FedExSubscriberManipulator()
            };

            return new FedExSubscriptionRequest(manipulators, serviceGatewayFactory.Create(settingsRepository), responseFactory, accountEntity);
        }

        /// <summary>
        /// Creates the rate request
        /// </summary>
        public IFedExRateRequest CreateRateRequest() =>
            lifetimeScope.Resolve<IFedExRateRequest>(TypedParameter.From(settingsRepository));

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

            return new FedExTrackRequest(manipulators, shipmentEntity, serviceGatewayFactory.Create(settingsRepository), responseFactory, accountEntity);
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
        /// Creates the UploadImages request.
        /// </summary>
        /// <param name="accountEntity"></param>
        /// <returns>A CarrierRequest object that can be used for submitting a request to
        /// FedEx to retrieve UploadImage data.</returns>
        public CarrierRequest CreateUploadImageRequest(FedExAccountEntity accountEntity)
        {
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>
            {
                new FedExUploadImagesWebAuthenticationDetailManipulator(),
                new FedExUploadImagesClientDetailManipulator(),
                new FedExUploadImagesTransactionDetailManipulator(),
                new FedExUploadImagesVersionManipulator(),
                new FedExUploadImagesImageDetailManipulator()
            };

            return new FedExUploadImagesRequest(manipulators, serviceGatewayFactory.Create(settingsRepository),
                responseFactory, accountEntity);
        }
    }
}

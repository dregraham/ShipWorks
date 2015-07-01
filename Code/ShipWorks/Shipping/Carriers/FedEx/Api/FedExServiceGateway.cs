using System;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Track;
using ProcessShipmentRequest = ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship.ProcessShipmentRequest;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Implmentation of the IFedExServiceGateway interface that is responsible for network
    /// communication with FedEx.
    /// </summary>
    public class FedExServiceGateway : IFedExServiceGateway
    {
        private readonly ILogEntryFactory logEntryFactory;
        private readonly FedExSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExServiceGateway" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExServiceGateway(ICarrierSettingsRepository settingsRepository) 
			: this(settingsRepository, new LogEntryFactory())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExServiceGateway" /> class.
        /// </summary>
        public FedExServiceGateway(ICarrierSettingsRepository settingsRepository, ILogEntryFactory logEntryFactory)
        {
            this.logEntryFactory = logEntryFactory;
            // Tell the FedEx settings which data source to use 
            settings = new FedExSettings(settingsRepository);
        }

        /// <summary>
        /// Communicates with the FedEx API to process a shipment.
        /// </summary>
        /// <param name="nativeShipmentRequest">The native shipment request.</param>
        /// <returns>
        /// The ProcessShipmentReply receivied from FedEx.
        /// </returns>
        /// <exception cref="System.ArgumentException">nativeShipmentRequest doesn't appear to be a ProcessShipmentRequest or a CreatePendingShipmentRequest.</exception>
        /// <exception cref="FedExSoapCarrierException"></exception>
        public virtual IFedExNativeShipmentReply Ship(IFedExNativeShipmentRequest nativeShipmentRequest)
        {
            using (ShipService service = new ShipService(new ApiLogEntry(ApiLogSource.FedEx, "Process")))
            {
                return Ship(nativeShipmentRequest, service);
            }
        }

        /// <summary>
        /// Communicates with the FedEx API to process a shipment.
        /// </summary>
        /// <returns>
        /// The ProcessShipmentReply receivied from FedEx.
        /// </returns>
        protected IFedExNativeShipmentReply Ship(IFedExNativeShipmentRequest nativeShipmentRequest, ShipService service)
        {
            try
            {
                IFedExNativeShipmentReply processReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // ShipService object here (i.e. no more abstractions can be made)

                // Point the service to the correct endpoint
                service.Url = settings.EndpointUrl;

                // The request should already be configured at this point, so we just need to send
                // it across the wire to FedEx
                ProcessShipmentRequest processShipmentRequest = nativeShipmentRequest as ProcessShipmentRequest;
                processReply = service.processShipment(processShipmentRequest);

                // If we are an Interapptive user, save for certification
                if (InterapptiveOnly.IsInterapptiveUser)
                {
                    string customerTransactionId = nativeShipmentRequest.TransactionDetail.CustomerTransactionId;
                    FedExUtility.SaveCertificationRequestAndResponseFiles(customerTransactionId, "Ship", service.RawSoap);
                }


                return processReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }
        
        /// <summary>
        /// Communicates with FedEx API to Validate postal code and obtain locationID
        /// </summary>
        /// <param name="postalCodeInquiryRequest">Initialized request</param>
        /// <returns>FedEx Reply</returns>
        public PostalCodeInquiryReply PostalCodeInquiry(PostalCodeInquiryRequest postalCodeInquiryRequest)
        {
            try
            {
                PostalCodeInquiryReply postalCodeInquiryReply;

                using (PackageMovementInformationService service = new PackageMovementInformationService(new ApiLogEntry(ApiLogSource.FedEx, "PackageMovement.PostalCodeInquiry")))
                {
                    service.Url = settings.EndpointUrl;
                    postalCodeInquiryReply = service.postalCodeInquiry(postalCodeInquiryRequest);
                }

                return postalCodeInquiryReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Communicates to FedEx the version of our CSP
        /// </summary>
        /// <param name="versionCaptureRequest">Initialized Request</param>
        /// <returns>FedEx Reply</returns>
        public VersionCaptureReply VersionCapture(VersionCaptureRequest versionCaptureRequest)
        {
            try
            {
                VersionCaptureReply versionCaptureReply;

                using (RegistrationService service = new RegistrationService(new ApiLogEntry(ApiLogSource.FedEx, "RegistrationService.VersionCapture")))
                {
                    service.Url = settings.EndpointUrl;
                    versionCaptureReply = service.versionCapture(versionCaptureRequest);
                }

                return versionCaptureReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }


        /// <summary>
        /// Communicates with FedEx getting drop off locations near the destination address.
        /// </summary>
        /// <exception cref="FedExSoapCarrierException"></exception>
        /// <exception cref="FedExException"></exception>
        public SearchLocationsReply GlobalShipAddressInquiry(SearchLocationsRequest searchLocationsRequest)
        {
            try
            {
                SearchLocationsReply searchLocationsReply;

                using (GlobalShipAddressService service = new GlobalShipAddressService(new ApiLogEntry(ApiLogSource.FedEx, "SearchLocations")))
                {
                    service.Url = settings.EndpointUrl;
                    searchLocationsReply = service.searchLocations(searchLocationsRequest);
                }

                return searchLocationsReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(CarrierException));
            }
        }
        
        /// <summary>
        /// Intended to interact with the FedEx API for performing an end of day ground close.
        /// </summary>
        /// <param name="groundCloseRequest">The ground close request.</param>
        /// <returns>The GroundCloseReply recevied from FedEx.</returns>
        /// <exception cref="FedExSoapCarrierException"></exception>
        public GroundCloseReply Close(GroundCloseRequest groundCloseRequest)
        {
            try
            {
                GroundCloseReply closeReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // CloseService object here (i.e. no more abstractions can be made)
                using (CloseService service = new CloseService(new ApiLogEntry(ApiLogSource.FedEx, "GroundClose")))
                {
                    // Point the service to the correct endpoint
                    service.Url = settings.EndpointUrl;

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to FedEx
                    closeReply = service.groundClose(groundCloseRequest);
                }

                return closeReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for performing an end of day SmartPost close.
        /// </summary>
        /// <param name="smartPostCloseRequest">The smart post close request.</param>
        /// <returns>The SmartPostCloseRequest recevied from FedEx.</returns>
        public SmartPostCloseReply Close(SmartPostCloseRequest smartPostCloseRequest)
        {
            try
            {
                SmartPostCloseReply closeReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // CloseService object here (i.e. no more abstractions can be made)
                using (CloseService service = new CloseService(new ApiLogEntry(ApiLogSource.FedEx, "SmartPostClose")))
                {
                    // Point the service to the correct endpoint
                    service.Url = settings.EndpointUrl;

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to FedEx
                    closeReply = service.smartPostClose(smartPostCloseRequest);
                }

                return closeReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for performing a shipment void.
        /// </summary>
        /// <param name="deleteShipmentRequest">The delete shipment request.</param>
        /// <returns>The ShipmentReply recevied from FedEx.</returns>
        public virtual ShipmentReply Void(DeleteShipmentRequest deleteShipmentRequest)
        {
            using (ShipService service = new ShipService(new ApiLogEntry(ApiLogSource.FedEx, "Void")))
            {
                return Void(deleteShipmentRequest, service);
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for performing a shipment void.
        /// </summary>
        protected ShipmentReply Void(DeleteShipmentRequest deleteShipmentRequest, ShipService service)
        {
            try
            {
                ShipmentReply voidShipmentReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // ShipService object here (i.e. no more abstractions can be made)

                // Point the service to the correct endpoint
                service.Url = settings.EndpointUrl;

                // The request should already be configured at this point, so we just need to send
                // it across the wire to FedEx
                voidShipmentReply = service.deleteShipment(deleteShipmentRequest);


                return voidShipmentReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }


        /// <summary>
        /// Intended to interact with the FedEx API for registering a CSP user.
        /// </summary>
        /// <param name="registerRequest">The register request.</param>
        /// <returns>The RegisterWebCspUserReply recevied from FedEx.</returns>
        public RegisterWebUserReply RegisterCspUser(RegisterWebUserRequest registerRequest)
        {
            try
            {
                RegisterWebUserReply registerReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // RegistrationService object here (i.e. no more abstractions can be made)
                using (RegistrationService service = new RegistrationService(new ApiLogEntry(ApiLogSource.FedEx, "RegisterCSPUser")))
                {
                    // Point the service to the correct endpoint
                    service.Url = settings.EndpointUrl;

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to FedEx
                    registerReply = service.registerWebUser(registerRequest);
                }

                return registerReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for subscribing a shipper to use the FedEx API.
        /// </summary>
        /// <param name="subscriptionRequest">The subscription request.</param>
        /// <returns>The SubscriptionReply received from FedEx.</returns>
        public SubscriptionReply SubscribeShipper(SubscriptionRequest subscriptionRequest)
        {
            try
            {
                SubscriptionReply subscriptionReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // RegistrationService object here (i.e. no more abstractions can be made)
                using (RegistrationService service = new RegistrationService(new ApiLogEntry(ApiLogSource.FedEx, "Subscribe")))
                {
                    // Point the service to the correct endpoint
                    service.Url = settings.EndpointUrl;

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to FedEx
                    subscriptionReply = service.subscription(subscriptionRequest);
                }

                return subscriptionReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for obtaining shipping rates.
        /// </summary>
        /// <param name="rateRequest">The rate request.</param>
        /// <param name="shipmentEntity"></param>
        /// <returns>The RateReply received from FedEx.</returns>
        public RateReply GetRates(RateRequest rateRequest, ShipmentEntity shipmentEntity)
        {
            try
            {
                RateReply rateReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // RateService object here (i.e. no more abstractions can be made)
                using (RateService service = new RateService(logEntryFactory.GetLogEntry(ApiLogSource.FedEx, "Rates", LogActionType.GetRates)))
                {
                    // Point the service to the correct endpoint
                    service.Url = settings.EndpointUrl;

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to FedEx
                    rateReply = service.getRates(rateRequest);

                    // If we are an Interapptive user, save for certification
                    if (InterapptiveOnly.IsInterapptiveUser)
                    {
                        string uniqueId = string.IsNullOrEmpty(shipmentEntity.FedEx.ReferencePO) ? Guid.NewGuid().ToString() : shipmentEntity.FedEx.ReferencePO;
                        try
                        {
                            // Now that we are doing get rates for multiple accounts in parallel, this call can try to write to the same file at the same time
                            // and throw an error.  However, if it does, we don't care because this is only for certification purposes.
                            FedExUtility.SaveCertificationRequestAndResponseFiles(uniqueId, "Rates", service.RawSoap);
                        }
                        catch
                        {
                        }
                    }
                }

                return rateReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for retrieving tracking data.
        /// </summary>
        /// <param name="trackRequest">The tracking request.</param>
        /// <returns>The TrackReply recevied from FedEx.</returns>
        public TrackReply Track(TrackRequest trackRequest)
        {
            try
            {
                TrackReply trackReply;

                // This is where we actually communicate with FedEx, so it's okay to explicitly create the 
                // TrackService object here (i.e. no more abstractions can be made)
                using (TrackService service = new TrackService(new ApiLogEntry(ApiLogSource.FedEx, "Track")))
                {
                    // Point the service to the correct endpoint
                    service.Url = settings.EndpointUrl;

                    // The request should already be configured at this point, so we just need to send
                    // it across the wire to FedEx
                    trackReply = service.track(trackRequest);
                }

                return trackReply;
            }
            catch (SoapException ex)
            {
                throw new FedExSoapCarrierException(ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(FedExException));
            }
        }
    }
}

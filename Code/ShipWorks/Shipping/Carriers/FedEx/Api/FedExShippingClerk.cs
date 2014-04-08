using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Tracking.Response;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Tracking;
using log4net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// A FedEx implementation of the IShippingClerk interface.
    /// </summary>
    public class FedExShippingClerk : IFedExShippingClerk
    {
        private static bool hasDoneVersionCapture;
        private readonly bool forceVersionCapture;
        private readonly ILabelRepository labelRepository;

        private readonly IFedExRequestFactory requestFactory;
        private readonly ICarrierSettingsRepository settingsRepository;
        private readonly ICertificateInspector certificateInspector;

        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingClerk" /> class with default
        /// values for the "live" FedEx settings repository and FedEx request factory.
        /// </summary>
        /// <param name="certificateInspector">The certificate inspector.</param>
        public FedExShippingClerk(ICertificateInspector certificateInspector)
            : this(new FedExSettingsRepository(), certificateInspector)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingClerk" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        public FedExShippingClerk(ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector)
            : this(settingsRepository, certificateInspector, new FedExRequestFactory(settingsRepository), LogManager.GetLogger(typeof(FedExShippingClerk)), false, new FedExLabelRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShippingClerk" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        /// <param name="certificateInspector">The certificate inspector.</param>
        /// <param name="requestFactory">The request factory.</param>
        /// <param name="log">The log.</param>
        /// <param name="forceVersionCapture">if set to <c>true</c> [force version capture] to occur rather than only performing the version capture once.</param>
        /// <param name="labelRepository">Label repository for clearing old shipment references.</param>
        public FedExShippingClerk(ICarrierSettingsRepository settingsRepository, ICertificateInspector certificateInspector, IFedExRequestFactory requestFactory, ILog log, bool forceVersionCapture, ILabelRepository labelRepository)
        {
            this.settingsRepository = settingsRepository;
            this.certificateInspector = certificateInspector;
            this.forceVersionCapture = forceVersionCapture;
            this.requestFactory = requestFactory;
            this.log = log;
            this.labelRepository = labelRepository;
        }

        /// <summary>
        /// Gets a value indicating whether version capture has been performed.
        /// </summary>
        /// <value>
        /// <c>true</c> if version capture has been performed; otherwise, <c>false</c>.
        /// </value>
        public bool HasDoneVersionCapture
        {
            get
            {
                return hasDoneVersionCapture;
            }
        }

        /// <summary>
        /// Sends the shipment entity to the carrier so a shipment is created  in the carrier's system,
        /// and the resulting data (label, tracking info, etc.) is saved and/or updated on the shipment
        /// entity accordingly.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <exception cref="FedExSoapCarrierException"></exception>
        /// <exception cref="FedExException"></exception>
        public void Ship(ShipmentEntity shipmentEntity)
        {
            try
            {
                // Make sure the shipment has a valid account associated with it
                ValidateFedExAccount(shipmentEntity);

                //Make sure the addresses only have two lines
                ValidateTwoLineAddress(shipmentEntity);

                // Clean the shipment for the specified shipment service type.  
                CleanShipmentForShipmentServiceType(shipmentEntity);

                PerformVersionCapture(shipmentEntity);

                int packageCount = shipmentEntity.FedEx.Packages.Count();

                // Clear out any previously saved labels for this shipment (in case there was an error shipping the first time (MPS))
                labelRepository.ClearReferences(shipmentEntity);

                // Each package in the shipment must be submitted to FedEx in an individual request
                for (int packageSequenceNumber = 0; packageSequenceNumber < packageCount; packageSequenceNumber++)
                {
                    CarrierRequest shippingRequest = requestFactory.CreateShipRequest(shipmentEntity);
                    shippingRequest.SequenceNumber = packageSequenceNumber;

                    // Submit the request and have the response save the labels and update the shipment entity based on the data from FedEx
                    ICarrierResponse response = shippingRequest.Submit();
                    response.Process();
                }
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Cleans a shipment for specified service type.  
        /// For example, if the user makes a copy of a shipment that contains alcohol, then switches
        /// to SmartPost, the Alcohol check box will be hidden, but still checked and the shipment will
        /// be marked as containing Alcohol even though SmartPost does not allow this.
        /// This method removes invalid properties on the shipment for the service type.
        /// </summary>
        /// <param name="shipmentEntity"></param>
        private static void CleanShipmentForShipmentServiceType(ShipmentEntity shipmentEntity)
        {
            FedExShipmentEntity fedExShipmentEntity = shipmentEntity.FedEx;
            FedExServiceType serviceType = (FedExServiceType) fedExShipmentEntity.Service;

            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.StandardOvernight:
                case FedExServiceType.FirstOvernight:
                case FedExServiceType.FedEx2Day:
                case FedExServiceType.FedExExpressSaver:
                case FedExServiceType.InternationalPriority:
                case FedExServiceType.InternationalEconomy:
                case FedExServiceType.InternationalFirst:
                case FedExServiceType.FedExGround:
                case FedExServiceType.GroundHomeDelivery:
                case FedExServiceType.FedExEuropeFirstInternationalPriority:
                case FedExServiceType.FedEx2DayAM:
                    CleanShipmentForNonFreight(fedExShipmentEntity);
                    CleanShipmentForNonSmartPost(fedExShipmentEntity);
                    break;
                case FedExServiceType.FedEx1DayFreight:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.InternationalPriorityFreight:
                case FedExServiceType.InternationalEconomyFreight:
                case FedExServiceType.FirstFreight:
                    CleanShipmentForNonSmartPost(fedExShipmentEntity);
                    break;
                case FedExServiceType.SmartPost:
                    CleanShipmentForNonFreight(fedExShipmentEntity);
                    CleanAndValidateShipmentForSmartPost(fedExShipmentEntity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("shipmentEntity");
            }
        }

        /// <summary>
        /// Cleans the shipment, clearing out any Freight properties.
        /// </summary>
        /// <param name="fedExShipmentEntity"></param>
        private static void CleanShipmentForNonFreight(FedExShipmentEntity fedExShipmentEntity)
        {
            fedExShipmentEntity.FreightBookingNumber = string.Empty;
            fedExShipmentEntity.FreightInsideDelivery = false;
            fedExShipmentEntity.FreightInsidePickup = false;
            fedExShipmentEntity.FreightLoadAndCount = 0;

            // Fix each package(s)
            foreach (FedExPackageEntity fedExPackageEntity in fedExShipmentEntity.Packages)
            {
                fedExPackageEntity.SkidPieces = 0;
            }
        }

        /// <summary>
        /// Cleans the shipment and package so that any non-SmartPost fields are set to defaults that are
        /// valid for SmartPost
        /// </summary>
        /// <param name="fedExShipmentEntity"></param>
        private static void CleanAndValidateShipmentForSmartPost(FedExShipmentEntity fedExShipmentEntity)
        {
            if (fedExShipmentEntity.Shipment.Insurance && fedExShipmentEntity.Shipment.InsuranceProvider == (int)Insurance.InsuranceProvider.Carrier)
            {
                throw new FedExException("FedEx declared value is not supported for Smart Post shipments. For insurance coverage, go to Shipping Settings and enable Shipworks Insurance for this carrier.");
            }

            // Clear out COD
            fedExShipmentEntity.CodAddFreight = false;
            fedExShipmentEntity.CodAmount = 0;
            fedExShipmentEntity.CodCity = string.Empty;
            fedExShipmentEntity.CodCompany = string.Empty;
            fedExShipmentEntity.CodCompany = string.Empty;
            fedExShipmentEntity.CodEnabled = false;
            fedExShipmentEntity.CodFirstName = string.Empty;
            fedExShipmentEntity.CodLastName = string.Empty;
            fedExShipmentEntity.CodOriginID = 0;
            fedExShipmentEntity.CodPaymentType = 0;
            fedExShipmentEntity.CodPhone = string.Empty;
            fedExShipmentEntity.CodStateProvCode = string.Empty;
            fedExShipmentEntity.CodStreet1 = string.Empty;
            fedExShipmentEntity.CodStreet2 = string.Empty;
            fedExShipmentEntity.CodStreet3 = string.Empty;
            fedExShipmentEntity.CodPostalCode = string.Empty;
            fedExShipmentEntity.CodCountryCode = string.Empty;
            fedExShipmentEntity.CodTIN = string.Empty;
            fedExShipmentEntity.CodTrackingFormID = string.Empty;
            fedExShipmentEntity.CodTrackingNumber = string.Empty;

            // Clear out Hold At Location
            fedExShipmentEntity.FedExHoldAtLocationEnabled = false;
            fedExShipmentEntity.HoldCity = null;
            fedExShipmentEntity.HoldCompanyName = null;
            fedExShipmentEntity.HoldContactId = null;
            fedExShipmentEntity.HoldCountryCode = null;
            fedExShipmentEntity.HoldEmailAddress = null;
            fedExShipmentEntity.HoldFaxNumber = null;
            fedExShipmentEntity.HoldLocationId = null;
            fedExShipmentEntity.HoldLocationType = null;
            fedExShipmentEntity.HoldPagerNumber = null;
            fedExShipmentEntity.HoldPersonName = null;
            fedExShipmentEntity.HoldPhoneExtension = null;
            fedExShipmentEntity.HoldPhoneNumber = null;
            fedExShipmentEntity.HoldPostalCode = null;
            fedExShipmentEntity.HoldResidential = null;
            fedExShipmentEntity.HoldStateOrProvinceCode = null;
            fedExShipmentEntity.HoldStreet1 = null;
            fedExShipmentEntity.HoldStreet2 = null;
            fedExShipmentEntity.HoldStreet3 = null;
            fedExShipmentEntity.HoldTitle = null;
            fedExShipmentEntity.HoldUrbanizationCode = null;

            // Fix each package
            foreach (FedExPackageEntity fedExPackageEntity in fedExShipmentEntity.Packages)
            {
                fedExPackageEntity.ContainsAlcohol = false;
                fedExPackageEntity.DryIceWeight = 0;
                fedExPackageEntity.PriorityAlert = false;
                fedExPackageEntity.PriorityAlertDetailContent = string.Empty;
                fedExPackageEntity.PriorityAlertEnhancementType = 0;
                fedExPackageEntity.DangerousGoodsAccessibilityType = 0;
                fedExPackageEntity.DangerousGoodsCargoAircraftOnly = false;
                fedExPackageEntity.DangerousGoodsEmergencyContactPhone = string.Empty;
                fedExPackageEntity.DangerousGoodsEnabled = false;
                fedExPackageEntity.DangerousGoodsOfferor = string.Empty;
                fedExPackageEntity.DangerousGoodsPackagingCount = 0;
                fedExPackageEntity.DangerousGoodsType = 0;
                fedExPackageEntity.HazardousMaterialClass= string.Empty;
                fedExPackageEntity.HazardousMaterialNumber = string.Empty;
                fedExPackageEntity.HazardousMaterialPackingGroup = 0;
                fedExPackageEntity.HazardousMaterialProperName = string.Empty;
                fedExPackageEntity.HazardousMaterialQuanityUnits = 0;
                fedExPackageEntity.HazardousMaterialQuantityValue = 0;
            }
        }

        /// <summary>
        /// Cleans the shipment, clearing out any SmartPost properties.
        /// </summary>
        /// <param name="fedExShipmentEntity"></param>
        private static void CleanShipmentForNonSmartPost(FedExShipmentEntity fedExShipmentEntity)
        {
            fedExShipmentEntity.SmartPostConfirmation = false;
            fedExShipmentEntity.SmartPostCustomerManifest = string.Empty;
            fedExShipmentEntity.SmartPostEndorsement = 0;
            fedExShipmentEntity.SmartPostHubID = string.Empty;
            fedExShipmentEntity.SmartPostIndicia = 0;
        }

        /// <summary>
        /// Void/Cancel/Delete a FedEx shipment
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        public void Void(ShipmentEntity shipmentEntity)
        {
            try
            {
                // Make sure the shipment has a valid account associated with it
                ValidateFedExAccount(shipmentEntity);
                FedExAccountEntity account = (FedExAccountEntity) settingsRepository.GetAccount(shipmentEntity);

                PerformVersionCapture(shipmentEntity);

                CarrierRequest shippingRequest = requestFactory.CreateVoidRequest(account, shipmentEntity);

                // Submit the request and have the response save the labels and update the shipment entity based on the data from FedEx
                ICarrierResponse response = shippingRequest.Submit();
                response.Process();
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Processes the end of day close for smart post shipments.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A FedExEndOfDayCloseEntity object containing the details of each closing; a null value is returned if there weren't any shipments closed.</returns>
        /// <exception cref="FedExSoapCarrierException"></exception>
        /// <exception cref="FedExException">An error occurred while closing ground shipments.</exception>
        public FedExEndOfDayCloseEntity CloseGround(FedExAccountEntity account)
        {
            try
            {
                FedExEndOfDayCloseEntity closeEntity = null;

                // Obtain a handle to the ground close request to submit the request and process the response
                CarrierRequest request = requestFactory.CreateGroundCloseRequest(account);
                ICarrierResponse response = request.Submit();
                response.Process();

                FedExGroundCloseResponse groundResponse = response as FedExGroundCloseResponse;
                if (groundResponse == null)
                {
                    // We don't have a ground response for some reason, so we can't assign the close enity
                    log.Info(string.Format("An unexpected response type was received when trying to process the end of day close: {0} type was received.", response.GetType().FullName));
                }
                else
                {
                    // We have a valid ground response, so we can access the close entity
                    closeEntity = groundResponse.CloseEntity;
                }

                return closeEntity;
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Processes the end of day close for ground shipments.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A FedExEndOfDayCloseEntity object containing the details of each closing; a null value is returned if there weren't any shipments closed.</returns>
        /// <exception cref="FedExSoapCarrierException"></exception>
        /// <exception cref="FedExException">An error occurred while closing ground shipments.</exception>
        public FedExEndOfDayCloseEntity CloseSmartPost(FedExAccountEntity account)
        {
            try
            {
                FedExEndOfDayCloseEntity closeEntity = null;

                // Obtain a handle to the smart post close request to submit the request and process the response
                CarrierRequest request = requestFactory.CreateSmartPostCloseRequest(account);
                ICarrierResponse response = request.Submit();
                response.Process();

                FedExSmartPostCloseResponse smartPostResponse = response as FedExSmartPostCloseResponse;
                if (smartPostResponse == null)
                {
                    // We don't have a smart post response for some reason, so we can't assign the close enity
                    log.Info(string.Format("An unexpected response type was received when trying to process the end of day close: {0} type was received.", response.GetType().FullName));
                }
                else
                {
                    // We have a valid smart post response, so we can access the close entity
                    closeEntity = smartPostResponse.CloseEntity;
                }

                return closeEntity;
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Registers a FedEx account account for use with the FedEx API.
        /// </summary>
        /// <param name="account">The account.</param>
        public void RegisterAccount(EntityBase2 account)
        {
            FedExAccountEntity fedExAccount = account as FedExAccountEntity;

            // The act of registering an account with FedEx is potentially a two step process: registering
            // the account with the FedEx API to obtain a username/password that is generated by FedEx and
            // subscribing the shipper account to FedEx API where the meter number is obtained.
            try
            {
                // Grab the shipping settings to see if we already have a username
                ShippingSettingsEntity shippingSettings = settingsRepository.GetShippingSettings();
                if (shippingSettings.FedExUsername == null)
                {
                    // The account hasn't been registered, so we need to get a username and password from FedEx
                    CarrierRequest registrationRequest = requestFactory.CreateRegisterCspUserRequest(fedExAccount);

                    // Submit the request and process the response to save the username/password back to the settings
                    ICarrierResponse registrationResponse = registrationRequest.Submit();
                    registrationResponse.Process();
                }

                // The account has been registered with FedEx, so we're ready to subscribe
                CarrierRequest subscriptionRequest = requestFactory.CreateSubscriptionRequest(fedExAccount);

                // Submit the request to FedEx and process the response so the meter number gets updated
                // on the account entity that was provided to this method
                ICarrierResponse subscriptionResponse = subscriptionRequest.Submit();
                subscriptionResponse.Process();
            }
            catch (Exception ex)
            {
				throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Performs the FedEx version capture calls for FedEx that are required for end of day close functionality. This should
        /// only run once per run of ShipWorks.
        /// </summary>
        /// <exception cref="FedExException">An unexpected response type was received from the package movement request; expected
        /// type: FedExPackageMovementResponse.</exception>
        public void PerformVersionCapture(ShipmentEntity shipmentEntity)
        {
            if (!hasDoneVersionCapture || forceVersionCapture)
            {
                // Log the version capture and whether it was forced or not
                log.Info(string.Format("Performing FedEx version capture{0}", forceVersionCapture ? " (forced)" : string.Empty));

                // This is made up of two requests: perform the package movement and the next to perform the version capture 
                // based on the location ID received in the package movement request
                foreach (FedExAccountEntity account in settingsRepository.GetAccounts().Where(a => !((FedExAccountEntity) a).Is2xMigrationPending))
                {
                    CarrierRequest packageMovementRequest = requestFactory.CreatePackageMovementRequest(shipmentEntity, account);
                    FedExPackageMovementResponse packageMovementResponse = packageMovementRequest.Submit() as FedExPackageMovementResponse;
                    
                    if (packageMovementResponse == null)
                    {
                        throw new FedExException("An unexpected response type was received from the package movement request; expected type: FedExPackageMovementResponse.");
                    }

                    // Process the movement response and use the location ID of the movement response to create the version capture request
                    packageMovementResponse.Process();
                    CarrierRequest versionCaptureRequest = requestFactory.CreateVersionCaptureRequest(shipmentEntity, packageMovementResponse.LocationID, account);

                    versionCaptureRequest.Submit();
                }

                // Make a note that version capture has been performed, so we don't do it again
                hasDoneVersionCapture = true;                
            }
        }

        /// <summary>
        /// Queries FedEx for HoldAtLocations near the destination address.
        /// </summary>
        public DistanceAndLocationDetail[] PerformHoldAtLocationSearch(ShipmentEntity shipment)
        {
            FedExAccountEntity account = (FedExAccountEntity)settingsRepository.GetAccount(shipment);

            FedExRequestFactory fedExRequestFactory = new FedExRequestFactory(settingsRepository);
            FedExGlobalShipAddressRequest searchLocationsRequest = (FedExGlobalShipAddressRequest) fedExRequestFactory.CreateSearchLocationsRequest(shipment, account);

            FedExGlobalShipAddressResponse carrierResponse = (FedExGlobalShipAddressResponse) searchLocationsRequest.Submit();

            carrierResponse.Process();

            return carrierResponse.DistanceAndLocationDetails;
        }

        /// <summary>
        /// Validates the FedEx account associated with the shipment entity to make sure it is not null 
        /// and that it does not need to be configured as a result of a ShipWorks 2.x migration.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <exception cref="FedExException">No FedEx account is selected for the shipment.</exception>
        private void ValidateFedExAccount(ShipmentEntity shipmentEntity)
        {
            FedExAccountEntity account = (FedExAccountEntity)settingsRepository.GetAccount(shipmentEntity);
            if (account == null)
            {
                log.Error(string.Format("Shipment ID {0} does not have a FedEx account selected. Select a valid FedEx account that is available in ShipWorks.", shipmentEntity.ShipmentID));
                throw new FedExException("No FedEx account is selected for the shipment.");
            }
            
            if (account.Is2xMigrationPending)
            {
                log.Error(string.Format("Attempt to use a FedEx account migrated from ShipWorks 2 that has not been configured for ShipWorks 3. The FedEx account (account number {0}) needs to be configured for ShipWorks3.", account.AccountNumber));
                throw new FedExException("The FedEx account selected for the shipment was migrated from ShipWorks 2, but has not yet been configured for ShipWorks 3.");
            }
        }

        /// <summary>
        /// Validates the Addresses to insure they only have 2 lines in the origin address
        /// </summary>
        private void ValidateTwoLineAddress(ShipmentEntity shipmentEntity)
        {
            if (!string.IsNullOrEmpty(shipmentEntity.OriginStreet3))
            {
                log.Error(string.Format("Shipment ID {0} cannot have three lines in the From Street Address.", shipmentEntity.ShipmentID));
                throw new FedExException("The \"From\" Street Address cannot be longer than 2 lines.");
            }
            if (!string.IsNullOrEmpty(shipmentEntity.ShipStreet3))
            {
                log.Error(string.Format("Shipment ID {0} cannot have three lines in the To Street Address.", shipmentEntity.ShipmentID));
                throw new FedExException("The \"To\" Street Address cannot be longer than 2 lines.");
            }
        }


        /// <summary>
        /// Gets the shipping rates from FedEx for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A RateGroup containing the rates received from FedEx.</returns>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                //Make sure the addresses only have two lines
                ValidateTwoLineAddress(shipment);

                // Make sure we have a trusted connection with FedEx before making any requests that would
                // send credentials over the network
                ICertificateRequest certificateRequest = requestFactory.CreateCertificateRequest(certificateInspector);
                if (certificateRequest.Submit() != CertificateSecurityLevel.Trusted)
                {
                    log.Warn("The FedEx certificate did not pass inspection and could not be trusted.");
                    throw new FedExException("ShipWorks is unable to make a secure connection to FedEx.");
                }

                // Ensure that the version capture has been performed
                PerformVersionCapture(shipment);
                
                List<RateResult> overallResults = new List<RateResult>();
                
                // Retrieve the rates from FedEx
                overallResults.AddRange(GetBasicRates(shipment));
                overallResults.AddRange(GetSmartPostRates(shipment));

                return new RateGroup(overallResults);
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Gets the basic rates (non-smart post) for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A List of RateResult objects.</returns>
        private List<RateResult> GetBasicRates(ShipmentEntity shipment)
        {
            List<RateResult> rates = new List<RateResult>();

            // Submit the rate request to FedEx and perform any additional processing on it
            CarrierRequest request = requestFactory.CreateRateRequest(shipment, null);
            ICarrierResponse response = request.Submit();
            response.Process();

            RateReply nativeResponse = response.NativeResponse as RateReply;
            if (nativeResponse == null)
            {
                // We don't have the correct response type to continue processing
                log.Info(string.Format("An unexpected response type was received when trying to process the end of day close: {0} type was received.", response.GetType().FullName));
            }
            else
            {
                // We have the appropriate response type, so we can build the list of rate results
                rates = BuildRateResults(shipment, new List<RateReplyDetail>(nativeResponse.RateReplyDetails));
            }

            return rates;
        }

        /// <summary>
        /// Gets the smart post rates for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A List of RateResult objects.</returns>
        private List<RateResult> GetSmartPostRates(ShipmentEntity shipment)
        {
            List<RateResult> rates = new List<RateResult>();

            try
            {
                if (FedExUtility.IsSmartPostEnabled(shipment))
                {
                    // Create a request that will retrieve smart post rates by supplying a smart post manipulator
                    CarrierRequest smartPostRequest = requestFactory.CreateRateRequest(shipment, new List<ICarrierRequestManipulator> {new FedExRateSmartPostManipulator()});
                    ICarrierResponse smartPostResponse = smartPostRequest.Submit();
                    smartPostResponse.Process();

                    RateReply smartPostNativeResponse = smartPostResponse.NativeResponse as RateReply;
                    if (smartPostNativeResponse == null)
                    {
                        // We don't have the correct response type to continue processing
                        log.Info(string.Format("An unexpected response type was received when trying to process the end of day close: {0} type was received.", smartPostResponse.GetType().FullName));
                    }
                    else
                    {
                        // We have the appropriate response type, so we can build the list of rate results
                        rates = BuildRateResults(shipment, new List<RateReplyDetail>(smartPostNativeResponse.RateReplyDetails));
                    }
                }
            }
            catch (FedExException ex)
            {
                // Just eat any FedEx exception, so we can still display the basic rates
                log.Warn("Error getting SmartPost rates: " + ex.Message);
            }
            catch (FedExApiCarrierException ex)
            {
                // Just eat the FedEx API exception, so we can still display the basic rates
                log.Warn("Error getting SmartPost rates: " + ex.Message);
            }

            return rates;
        }

        /// <summary>
        /// Builds a list of ShipWorks RateResult objects from the FedEx rate details.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="rateDetails">The rate details.</param>
        /// <returns>A List of RateResult objects.</returns>
        private static List<RateResult> BuildRateResults(ShipmentEntity shipment, List<RateReplyDetail> rateDetails)
        {
            List<RateResult> results = new List<RateResult>();

            // We're not supporting priority freight and economy freight
            rateDetails.RemoveAll(r => r.ServiceType == ServiceType.FEDEX_FREIGHT_PRIORITY || r.ServiceType == ServiceType.FEDEX_FREIGHT_ECONOMY || r.ServiceType == ServiceType.FEDEX_FIRST_FREIGHT);

            // Translate them to rate results
            foreach (RateReplyDetail rateDetail in rateDetails)
            {
                FedExServiceType serviceType;

                serviceType = GetFedExServiceType(rateDetail.ServiceType);

                int transitDays = 0;
                DateTime? deliveryDate = null;

                if (rateDetail.DeliveryTimestampSpecified)
                {
                    // Transite time
                    deliveryDate = rateDetail.DeliveryTimestamp;
                    transitDays = (deliveryDate.Value.Date - shipment.ShipDate.Date).Days;
                }
                else if (rateDetail.TransitTimeSpecified)
                {
                    transitDays = GetTransitDays(rateDetail.TransitTime);
                    
                    if (serviceType == FedExServiceType.GroundHomeDelivery)
                    {
                        deliveryDate = ShippingManager.CalculateExpectedDeliveryDate(transitDays, DayOfWeek.Sunday, DayOfWeek.Monday);
                    }
                    else
                    {
                        deliveryDate = ShippingManager.CalculateExpectedDeliveryDate(transitDays, DayOfWeek.Saturday, DayOfWeek.Sunday);    
                    }
                }

                // Cost
                decimal cost = rateDetail.RatedShipmentDetails[0].ShipmentRateDetail.TotalNetCharge.Amount;
                if (shipment.OriginCountryCode.ToUpper() == "CA" && rateDetail.RatedShipmentDetails[0].ShipmentRateDetail.TotalNetFedExCharge.AmountSpecified)
                {
                    cost = rateDetail.RatedShipmentDetails[0].ShipmentRateDetail.TotalNetFedExCharge.Amount;
                }

                // Add the shipworks rate object
                results.Add(new RateResult(
                                EnumHelper.GetDescription(serviceType),
                                transitDays == 0 ? string.Empty : transitDays.ToString(),
                                cost,
                                new FedExRateSelection(serviceType))
                {
                    ExpectedDeliveryDate = deliveryDate,
                    ServiceLevel = GetServiceLevel(serviceType, transitDays),
                    ShipmentType = ShipmentTypeCode.FedEx,
                    ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.FedEx)
                });
            }

            return results;
        }

        /// <summary>
        /// Gets the service level.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="transitDays">The transit days.</param>
        private static ServiceLevelType GetServiceLevel(FedExServiceType serviceType, int transitDays)
        {
            switch (serviceType)
            {
                case FedExServiceType.PriorityOvernight:
                case FedExServiceType.StandardOvernight:
                case FedExServiceType.InternationalFirst:
                case FedExServiceType.FirstOvernight:
                case FedExServiceType.FedExEuropeFirstInternationalPriority:
                case FedExServiceType.FedEx1DayFreight:
                    return ServiceLevelType.OneDay;

                case FedExServiceType.FedEx2Day:
                case FedExServiceType.FedEx2DayFreight:
                case FedExServiceType.FedEx2DayAM:
                    return ServiceLevelType.TwoDays;

                case FedExServiceType.FedEx3DayFreight:
                case FedExServiceType.FedExExpressSaver:
                    return ServiceLevelType.ThreeDays;

                case FedExServiceType.InternationalEconomy:
                    return ServiceLevelType.FourToSevenDays;

                default:
                case FedExServiceType.InternationalPriority:
                case FedExServiceType.FedExGround:
                case FedExServiceType.GroundHomeDelivery:
                case FedExServiceType.InternationalPriorityFreight:
                case FedExServiceType.InternationalEconomyFreight:
                case FedExServiceType.SmartPost:
                    return ShippingManager.GetServiceLevel(transitDays);
            }
        }

        /// <summary>
        /// Get the integer number of days for the given fedex transit time value
        /// </summary>
        private static int GetTransitDays(TransitTimeType transitTime)
        {
            switch (transitTime)
            {
                case TransitTimeType.ONE_DAY: return 1;
                case TransitTimeType.TWO_DAYS: return 2;
                case TransitTimeType.THREE_DAYS: return 3;
                case TransitTimeType.FOUR_DAYS: return 4;
                case TransitTimeType.FIVE_DAYS: return 5;
                case TransitTimeType.SIX_DAYS: return 6;
                case TransitTimeType.SEVEN_DAYS: return 7;
                case TransitTimeType.EIGHT_DAYS: return 8;
                case TransitTimeType.NINE_DAYS: return 9;
                case TransitTimeType.TEN_DAYS: return 10;
                case TransitTimeType.ELEVEN_DAYS: return 11;
                case TransitTimeType.TWELVE_DAYS: return 12;
                case TransitTimeType.THIRTEEN_DAYS: return 13;
                case TransitTimeType.FOURTEEN_DAYS: return 14;
                case TransitTimeType.FIFTEEN_DAYS: return 15;
                case TransitTimeType.SIXTEEN_DAYS: return 16;
                case TransitTimeType.SEVENTEEN_DAYS: return 17;
                case TransitTimeType.EIGHTEEN_DAYS: return 18;
                case TransitTimeType.NINETEEN_DAYS: return 19;
                case TransitTimeType.TWENTY_DAYS: return 20;
            }

            return 0;
        }

        /// <summary>
        /// Get our own FedExServiceType value for the given rate ServiceType
        /// </summary>
        private static FedExServiceType GetFedExServiceType(ServiceType serviceType)
        {
            switch (serviceType)
            {
                case ServiceType.PRIORITY_OVERNIGHT: return FedExServiceType.PriorityOvernight;
                case ServiceType.STANDARD_OVERNIGHT: return FedExServiceType.StandardOvernight;
                case ServiceType.FIRST_OVERNIGHT: return FedExServiceType.FirstOvernight;
                case ServiceType.FEDEX_2_DAY: return FedExServiceType.FedEx2Day;
                case ServiceType.FEDEX_EXPRESS_SAVER: return FedExServiceType.FedExExpressSaver;
                case ServiceType.INTERNATIONAL_PRIORITY: return FedExServiceType.InternationalPriority;
                case ServiceType.INTERNATIONAL_ECONOMY: return FedExServiceType.InternationalEconomy;
                case ServiceType.INTERNATIONAL_FIRST: return FedExServiceType.InternationalFirst;
                case ServiceType.FEDEX_1_DAY_FREIGHT: return FedExServiceType.FedEx1DayFreight;
                case ServiceType.FEDEX_2_DAY_FREIGHT: return FedExServiceType.FedEx2DayFreight;
                case ServiceType.FEDEX_3_DAY_FREIGHT: return FedExServiceType.FedEx3DayFreight;
                case ServiceType.FEDEX_GROUND: return FedExServiceType.FedExGround;
                case ServiceType.GROUND_HOME_DELIVERY: return FedExServiceType.GroundHomeDelivery;
                case ServiceType.INTERNATIONAL_PRIORITY_FREIGHT: return FedExServiceType.InternationalPriorityFreight;
                case ServiceType.INTERNATIONAL_ECONOMY_FREIGHT: return FedExServiceType.InternationalEconomyFreight;
                case ServiceType.SMART_POST: return FedExServiceType.SmartPost;
                case ServiceType.FEDEX_2_DAY_AM: return FedExServiceType.FedEx2DayAM;
                case ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY: return FedExServiceType.FedExEuropeFirstInternationalPriority;
                case ServiceType.FEDEX_FIRST_FREIGHT: return FedExServiceType.FirstFreight;
            }

            throw new CarrierException("Invalid FedEx Service Type " + serviceType);
        }

        /// <summary>
        /// A helper method for inspecting and throwing a more specific exception for any exceptions 
        /// that are generated by the FedEx shipping clerk while communicating with the FedEx API.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private Exception HandleException(Exception exception)
        {
            if (exception is SoapException)
            {
                log.Error(exception.Message);
                return new FedExSoapCarrierException(exception as SoapException);
            }
            else if (exception is CarrierException)
            {
                CarrierException carrierException = exception as CarrierException;

                // There was an exception communicating with the API - the request went through and a response
                // was received, but the response most likely had an error result.
                log.Error(carrierException.Message);
                
                string errorMessage = string.Format("An error occurred while communicating with FedEx. {0}", carrierException.Message);

                if (!errorMessage.EndsWith("."))
                {
                    errorMessage = string.Format("{0}.", errorMessage);
                }

                return new FedExException(errorMessage, carrierException);
            }
            else
            {
                log.Error(exception.Message);
                return WebHelper.TranslateWebException(exception, typeof(FedExException));
            }
        }

        public TrackingResult Track(ShipmentEntity shipmentEntity)
        {
            try
            {
                // Make sure the shipment has a valid account associated with it
                ValidateFedExAccount(shipmentEntity);
                FedExAccountEntity account = (FedExAccountEntity)settingsRepository.GetAccount(shipmentEntity);

                PerformVersionCapture(shipmentEntity);

                CarrierRequest shippingRequest = requestFactory.CreateTrackRequest(account, shipmentEntity);

                // Submit the request and have the response save the labels and update the shipment entity based on the data from FedEx
                ICarrierResponse response = shippingRequest.Submit();
                response.Process();

                FedExTrackingResponse trackingResponse = response as FedExTrackingResponse;

                return trackingResponse.TrackingResult;
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
		}
		
		
    }
}

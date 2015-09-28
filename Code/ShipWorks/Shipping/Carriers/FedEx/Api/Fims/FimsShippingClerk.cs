using System;
using System.Linq;
using System.Web.Services.Protocols;
using Interapptive.Shared.Net;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// A FedEx FIMS implementation of the IShippingClerk interface.
    /// </summary>
    public class FimsShippingClerk : IFedExShippingClerk
    {
        private readonly IFimsLabelRepository labelRepository;
        private readonly ICarrierSettingsRepository settingsRepository;
        private readonly ILog log;
        private readonly IFimsWebClient webClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="FimsShippingClerk" /> class with default
        /// values for the "live" FedEx settings repository and FedEx request factory.
        /// </summary>
        /// <param name="webClient">The FIMS web client to use.</param>
        /// <param name="labelRepository">The FIMS label repo to use.</param>
        public FimsShippingClerk(IFimsWebClient webClient, IFimsLabelRepository labelRepository)
            : this(webClient, labelRepository, new FedExSettingsRepository(), LogManager.GetLogger(typeof(FimsShippingClerk)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FimsShippingClerk" /> class.
        /// </summary>
        /// <param name="webClient">The FIMS web client to use.</param>
        /// <param name="labelRepository">The FIMS label repo to use.</param>
        /// <param name="settingsRepository">The settings repository.</param>
        /// <param name="log">The log.</param>
        public FimsShippingClerk(IFimsWebClient webClient, IFimsLabelRepository labelRepository, ICarrierSettingsRepository settingsRepository, ILog log)
        {
            this.webClient = webClient;
            this.settingsRepository = settingsRepository;
            this.log = log;
            this.labelRepository = labelRepository;
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
                ShippingSettingsEntity settings = settingsRepository.GetShippingSettings();

                // Make sure it is a valid FedEx FIMS Shipment.
                ValidateShipment(shipmentEntity);

                // Throw exception if the shipment is configured as a return
                if (shipmentEntity.ReturnShipment)
                {
                    throw new FedExException("The FIMS service does not support returns");
                }

                // Make sure the FIMS username/password are valid
                ValidateFedExFimsAccount(settings);

                // Clear out any previously saved labels for this shipment (in case there was an error shipping the first time (MPS))
                labelRepository.ClearReferences(shipmentEntity);

                FimsShipRequest fimsShipRequest = new FimsShipRequest(shipmentEntity, settings.FedExFimsUsername, settings.FedExFimsPassword);

                IFimsShipResponse fimsShipResponse = webClient.Ship(fimsShipRequest);

                shipmentEntity.TrackingNumber = fimsShipResponse.ParcelID;
                shipmentEntity.FedEx.Packages[0].TrackingNumber = fimsShipResponse.ParcelID;
                shipmentEntity.FedEx.MasterFormID = String.Empty;
                shipmentEntity.ShipmentCost = 0;

                shipmentEntity.RequestedLabelFormat = (int) ThermalLanguage.None;
                shipmentEntity.ActualLabelFormat = null;
                shipmentEntity.FedEx.RequestedLabelFormat = (int)ThermalLanguage.None;

                labelRepository.SaveLabel(fimsShipResponse, shipmentEntity.FedEx.Packages[0].FedExPackageID);
            }
            catch (Exception ex)
            {
                throw (HandleException(ex));
            }
        }

        /// <summary>
        /// Make sure it is a valid FedEx FIMS Shipment.
        /// </summary>
        private static void ValidateShipment(ShipmentEntity shipmentEntity)
        {
            if (!FedExUtility.IsFimsService((FedExServiceType) shipmentEntity.FedEx.Service))
            {
                throw new FedExException("FedEX FIMS shipments require selecting a FIMS service type.");
            }

            if (shipmentEntity.ShipCountryCode == "US")
            {
                throw new FedExException("FedEX FIMS shipments cannot be shipped domestically.");
            }

            if (shipmentEntity.CustomsItems == null || !shipmentEntity.CustomsItems.Any())
            {
                throw new FedExException("FedEX FIMS shipments require customs information to be entered.");
            }

            if (shipmentEntity.FedEx.Packages == null || shipmentEntity.FedEx.Packages.Count > 1)
            {
                throw new FedExException("FedEX FIMS shipments allow only 1 package.");
            }
        }

        /// <summary>
        /// Make sure it is a valid FedEx FIMS Shipment.
        /// </summary>
        private static void ValidateFedExFimsAccount(ShippingSettingsEntity settings)
        {
            if (string.IsNullOrWhiteSpace(settings.FedExFimsUsername))
            {
                throw new FedExException("FedEX FIMS Username is missing.");
            }

            if (string.IsNullOrWhiteSpace(settings.FedExFimsPassword))
            {
                throw new FedExException("FedEX FIMS Password is missing.");
            }
        }

        /// <summary>
        /// A helper method for inspecting and throwing a more specific exception for any exceptions 
        /// that are generated by the shipping clerk while communicating with the FIMS API.
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
                CarrierException carrierException = (CarrierException)exception;

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

        /// <summary>
        /// Does not void a shipment as FIMS does not support voids.
        /// </summary>
        public void Void(ShipmentEntity shipmentEntity)
        {
            return;
        }

        /// <summary>
        /// Does not register an account as FIMS does not support registering an account .
        /// </summary>
        public void RegisterAccount(EntityBase2 account)
        {
            return;
        }

        /// <summary>
        /// Does not get rates as FIMS does not support getting rates.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            return new RateGroup(Enumerable.Empty<RateResult>());
        }

        /// <summary>
        /// Gets tracking information for the FIMS shipment
        /// </summary>
        public TrackingResult Track(ShipmentEntity shipmentEntity)
        {
            TrackingResult trackingResult = new TrackingResult();

            FedExSettings fedExSettings = new FedExSettings(settingsRepository);

            string link = string.Format(fedExSettings.FimsTrackEndpointUrlFormat, shipmentEntity.TrackingNumber);

            // By default, the anchor tag gets a blue background.  We want a transparent background and blue font color instead.
            string trackingLink = string.Format("Click <a href=\"{0}\" style='background-color: transparent; color:Blue;'>here</a> to view tracking information.", link);

            trackingResult.Summary = trackingLink;

            return trackingResult;
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public FedExEndOfDayCloseEntity CloseGround(FedExAccountEntity account)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public FedExEndOfDayCloseEntity CloseSmartPost(FedExAccountEntity account)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public DistanceAndLocationDetail[] PerformHoldAtLocationSearch(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}

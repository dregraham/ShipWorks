using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Label Service for the Endicia carrier
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.Endicia)]
    public class EndiciaLabelService : ILabelService
    {
        private readonly Express1EndiciaLabelService express1EndiciaLabelService;
        private readonly EndiciaRatingService endiciaRatingService;
        private readonly Express1EndiciaShipmentType express1EndiciaShipmentType;
        private readonly EndiciaShipmentType endiciaShipmentType;
        private readonly Func<ShipmentEntity, LabelRequestResponse, EndiciaDownloadedLabelData> createDownloadedLabelData;
        private readonly IShippingSettings shippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaLabelService(IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager,
            Express1EndiciaLabelService express1EndiciaLabelService,
            EndiciaRatingService endiciaRatingService,
            IShippingSettings shippingSettings,
            Func<ShipmentEntity, LabelRequestResponse, EndiciaDownloadedLabelData> createDownloadedLabelData)
        {
            this.express1EndiciaLabelService = express1EndiciaLabelService;
            this.endiciaRatingService = endiciaRatingService;

            express1EndiciaShipmentType = shipmentTypeManager[ShipmentTypeCode.Express1Endicia] as Express1EndiciaShipmentType;
            endiciaShipmentType = shipmentTypeManager[ShipmentTypeCode.Endicia] as EndiciaShipmentType;

            MethodConditions.EnsureArgumentIsNotNull(express1EndiciaShipmentType, nameof(express1EndiciaShipmentType));
            MethodConditions.EnsureArgumentIsNotNull(endiciaShipmentType, nameof(endiciaShipmentType));

            this.shippingSettings = shippingSettings;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates an Endicia label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            endiciaShipmentType.ValidateShipment(shipment);
            IShippingSettingsEntity settings = shippingSettings.FetchReadOnly();
            bool useExpress1 = ShouldUsedExpress1(shipment, settings);

            IEndiciaAccountEntity express1Account = EndiciaAccountManager.GetAccountReadOnly(settings.EndiciaAutomaticExpress1Account);

            EndiciaApiClient endiciaApiClient = new EndiciaApiClient(endiciaShipmentType.AccountRepository,
                endiciaShipmentType.LogEntryFactory, endiciaShipmentType.CertificateInspector);

            if (useExpress1)
            {
                if (express1Account == null)
                {
                    throw new ShippingException("The Express1 account to automatically use when processing with Endicia has not been selected.");
                }

                int originalShipmentType = shipment.ShipmentType;
                long? originalEndiciaAccountID = shipment.Postal.Endicia.OriginalEndiciaAccountID;
                long endiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;

                try
                {
                    // Check Endicia amount
                    RateResult endiciaRate = GetEndiciaRate(shipment, endiciaApiClient);

                    // Change the shipment to Express1
                    shipment.ShipmentType = (int) ShipmentTypeCode.Express1Endicia;
                    shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                    shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

                    // Check Express1 amount
                    RateResult express1Rate = GetExpress1Rate(shipment, express1Account);

                    useExpress1 = express1Rate?.AmountOrDefault <= endiciaRate?.AmountOrDefault;
                }
                catch (EndiciaApiException apiException)
                {
                    throw new ShippingException(apiException.Message, apiException);
                }
                finally
                {
                    // Reset back to the original values
                    shipment.ShipmentType = originalShipmentType;
                    shipment.Postal.Endicia.OriginalEndiciaAccountID = originalEndiciaAccountID;
                    shipment.Postal.Endicia.EndiciaAccountID = endiciaAccountID;
                }
            }

            // See if this shipment should really go through Express1
            if (useExpress1)
            {
                return CreateUsingExpress1(shipment, express1Account);
            }

            // This would be set if they have tried to process as Express1 but it failed... make sure its clear since we are not using it.
            shipment.Postal.Endicia.OriginalEndiciaAccountID = null;

            try
            {
                LabelRequestResponse response = endiciaApiClient.ProcessShipment(shipment, endiciaShipmentType);
                return createDownloadedLabelData(shipment, response);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Voids the Endicia label
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            try
            {
                EndiciaApiAccount.RequestRefund(shipment);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Create the label via Express1
        /// </summary>
        private IDownloadedLabelData CreateUsingExpress1(ShipmentEntity shipment, IEndiciaAccountEntity express1Account)
        {
            // Now we turn this into an Express1 shipment...
            shipment.ShipmentType = (int) ShipmentTypeCode.Express1Endicia;
            shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
            shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

            // Process via Express1
            express1EndiciaShipmentType.UpdateDynamicShipmentData(shipment);
            return express1EndiciaLabelService.Create(shipment);
        }

        private RateResult GetExpress1Rate(ShipmentEntity shipment, IEndiciaAccountEntity express1Account)
        {
            // Instantiate the Express1 shipment type, so the correct account repository is used when getting rates
            RateGroup express1Rates = endiciaRatingService.GetRates(shipment);
            RateResult express1Rate = express1Rates.Rates
                .Where(er => er.Selectable)
                .FirstOrDefault(RateServiceMatchesShipment(shipment));
            return express1Rate;
        }

        /// <summary>
        /// Get Endicia Rates
        /// </summary>
        private RateResult GetEndiciaRate(ShipmentEntity shipment, EndiciaApiClient endiciaApiClient)
        {
            List<RateResult> endiciaRates = endiciaApiClient.GetRatesFast(shipment, endiciaShipmentType);
            RateResult endiciaRate =
                endiciaRates
                    .Where(er => er.Selectable)
                    .FirstOrDefault(RateServiceMatchesShipment(shipment));
            return endiciaRate;
        }

        /// <summary>
        /// Does the service in the rate match the shipment's selected service
        /// </summary>
        private Func<RateResult, bool> RateServiceMatchesShipment(ShipmentEntity shipment)
        {
            return er =>
            {
                PostalRateSelection rateSelection = (PostalRateSelection) er.OriginalTag;

                return rateSelection.ServiceType == (PostalServiceType) shipment.Postal.Service &&
                       rateSelection.ConfirmationType == (PostalConfirmationType) shipment.Postal.Confirmation;
            };
        }

        /// <summary>
        /// Check to see if we should use Express1
        /// </summary>
        private bool ShouldUsedExpress1(ShipmentEntity shipment, IShippingSettingsEntity settings)
        {
            return Express1Utilities.IsPostageSavingService(shipment) &&
                !endiciaShipmentType.IsRateDiscountMessagingRestricted &&
                Express1Utilities.IsValidPackagingType((PostalServiceType) shipment.Postal.Service, (PostalPackagingType) shipment.Postal.PackagingType) &&
                settings.EndiciaAutomaticExpress1;
        }
    }
}
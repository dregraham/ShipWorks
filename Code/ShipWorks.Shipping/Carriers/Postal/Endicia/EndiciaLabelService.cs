using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Label Service for the Endicia carrier
    /// </summary>
    public class EndiciaLabelService : ILabelService
    {
        private readonly EndiciaShipmentType endiciaShipmentType;
        private readonly Express1EndiciaShipmentType express1EndiciaShipmentType;
        private readonly Express1EndiciaLabelService express1EndiciaLabelService;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaLabelService(EndiciaShipmentType endiciaShipmentType, Express1EndiciaShipmentType express1EndiciaShipmentType, Express1EndiciaLabelService express1EndiciaLabelService)
        {
            //TODO: stop using the ShipmentType when we pull rating into its own service
            this.endiciaShipmentType = endiciaShipmentType;
            this.express1EndiciaShipmentType = express1EndiciaShipmentType;
            this.express1EndiciaLabelService = express1EndiciaLabelService;
        }



        /// <summary>
        /// Creates an Endicia label
        /// </summary>
        public void Create(ShipmentEntity shipment)
        {
            endiciaShipmentType.ValidateShipment(shipment);
            bool useExpress1 = ShouldUsedExpress1(shipment);

            EndiciaAccountEntity express1Account = EndiciaAccountManager.GetAccount(ShippingSettings.Fetch().EndiciaAutomaticExpress1Account);

            EndiciaApiClient endiciaApiClient = new EndiciaApiClient(endiciaShipmentType.AccountRepository, endiciaShipmentType.LogEntryFactory, endiciaShipmentType.CertificateInspector);

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
                    List<RateResult> endiciaRates = endiciaApiClient.GetRatesFast(shipment, endiciaShipmentType);
                    RateResult endiciaRate = endiciaRates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                    ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                    && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

                    // Check Express1 amount
                    shipment.ShipmentType = (int)ShipmentTypeCode.Express1Endicia;
                    shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                    shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

                    // Instantiate the Express1 shipment type, so the correct account repository is used when getting rates
                    ShipmentType express1Type = ShipmentTypeManager.GetType(shipment);
                    RateGroup express1Rates = express1Type.GetRates(shipment);
                    RateResult express1Rate = express1Rates.Rates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                      ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                      && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

                    // Now set useExpress1 to true only if the express 1 rate is less than the endicia amount
                    if (endiciaRate != null && express1Rate != null)
                    {
                        useExpress1 = express1Rate.Amount <= endiciaRate.Amount;
                    }
                    else
                    {
                        // If we can't figure it out for sure, don't use it
                        useExpress1 = false;
                    }
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
                // Now we turn this into an Express1 shipment...
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Endicia;
                shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

                // Process via Express1
                express1EndiciaShipmentType.UpdateDynamicShipmentData(shipment);
                express1EndiciaLabelService.Create(shipment);
            }
            else
            {
                // This would be set if they have tried to process as Express1 but it failed... make sure its clear since we are not using it.
                shipment.Postal.Endicia.OriginalEndiciaAccountID = null;

                try
                {
                    endiciaApiClient.ProcessShipment(shipment, endiciaShipmentType);
                }
                catch (EndiciaException ex)
                {
                    throw new ShippingException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Check to see if we should use Express1
        /// </summary>
        private bool ShouldUsedExpress1(ShipmentEntity shipment)
        {
            return Express1Utilities.IsPostageSavingService(shipment) && !endiciaShipmentType.IsRateDiscountMessagingRestricted &&
                Express1Utilities.IsValidPackagingType((PostalServiceType)shipment.Postal.Service, (PostalPackagingType)shipment.Postal.PackagingType) &&
                ShippingSettings.Fetch().EndiciaAutomaticExpress1;
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
    }
}
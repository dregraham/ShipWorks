using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Rating service for the Endicia carrier
    /// </summary>
    public class EndiciaRatingService : PostalRatingService
    {
        private readonly Func<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;
        private readonly Func<ShipmentTypeCode, CarrierAccountRepositoryBase<EndiciaAccountEntity>> accountRepository;

        public EndiciaRatingService(Func<ShipmentTypeCode, ShipmentType> shipmentTypeFactory, Func<ShipmentTypeCode, CarrierAccountRepositoryBase<EndiciaAccountEntity>> accountRepository) 
            : base(shipmentTypeFactory, accountRepository)
        {
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                // Get rates 
                return GetRatesFromApi(shipment);
            }
            catch (EndiciaException ex)
            {
                // rethrow endicia exceptions as shipping excetions
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            List<RateResult> express1Rates = null;
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            
            EndiciaShipmentType endiciaShipmentType = shipmentTypeFactory((ShipmentTypeCode) shipment.ShipmentType) as EndiciaShipmentType;

            if (endiciaShipmentType == null)
            {
                throw new EndiciaException("Could not get endicia shipment type");
            }

                // See if this shipment should really go through Express1
                if (shipment.ShipmentType == (int)ShipmentTypeCode.Endicia
                && !shipmentTypeFactory((ShipmentTypeCode)shipment.ShipmentType).IsRateDiscountMessagingRestricted
                && Express1Utilities.IsValidPackagingType(null, (PostalPackagingType)shipment.Postal.PackagingType)
                && settings.EndiciaAutomaticExpress1)
            {
                // We don't have any Endicia accounts, so let the user know they need an account.
                if (!accountRepository((ShipmentTypeCode)shipment.ShipmentType).Accounts.Any())
                {
                    throw new EndiciaException($"An account is required to view {EnumHelper.GetDescription(ShipmentTypeCode.Endicia)} rates.");
                }

                var express1Account = EndiciaAccountManager.GetAccount(settings.EndiciaAutomaticExpress1Account);

                if (express1Account == null)
                {
                    throw new EndiciaException("The Express1 account to automatically use when processing with Endicia has not been selected.");
                }

                // We temporarily turn this into an Exprss1 shipment to get rated
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Endicia;
                shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;
                
                try
                {
                    // Currently this actually recurses into this same method
                    express1Rates = GetRates(shipment).Rates.ToList();
                }
                catch (ShippingException)
                {
                    // Eat the exception; we don't want to stop someone from using Endicia if Express1 can't get rates
                }
                finally
                {
                    shipment.ShipmentType = (int)ShipmentTypeCode.Endicia;
                    shipment.Postal.Endicia.EndiciaAccountID = shipment.Postal.Endicia.OriginalEndiciaAccountID.Value;
                    shipment.Postal.Endicia.OriginalEndiciaAccountID = null;
                }
            }

            EndiciaApiClient endiciaApiClient = new EndiciaApiClient(accountRepository((ShipmentTypeCode)shipment.ShipmentType), endiciaShipmentType.LogEntryFactory, endiciaShipmentType.CertificateInspector);

            List<RateResult> allEndiciaRates = (InterapptiveOnly.MagicKeysDown) ?
                                                endiciaApiClient.GetRatesSlow(shipment, endiciaShipmentType) :
                                                endiciaApiClient.GetRatesFast(shipment, endiciaShipmentType);

            // Filter out any excluded services, but always include the service that the shipment is configured with
            List<RateResult> endiciaRates = endiciaShipmentType.FilterRatesByExcludedServices(shipment, allEndiciaRates);


            // For endicia, we want to either promote Express1 or show the Express1 savings
            if (shipment.ShipmentType == (int)ShipmentTypeCode.Endicia)
            {
                if (endiciaShipmentType.ShouldRetrieveExpress1Rates && !endiciaShipmentType.IsRateDiscountMessagingRestricted)
                {
                    List<RateResult> finalRates = new List<RateResult>();

                    // Go through each Endicia rate
                    foreach (RateResult endiciaRate in endiciaRates)
                    {
                        PostalRateSelection endiciaRateDetail = (PostalRateSelection)endiciaRate.OriginalTag;

                        // If it's a rate they could (or have) saved on with Express1, we modify it
                        if (endiciaRate.Selectable &&
                            endiciaRateDetail != null &&
                            Express1Utilities.IsPostageSavingService(endiciaRateDetail.ServiceType))
                        {
                            // See if Express1 returned a rate for this servie
                            RateResult express1Rate = null;
                            if (express1Rates != null)
                            {
                                express1Rate = express1Rates.Where(e1r => e1r.Selectable).FirstOrDefault(e1r =>
                                                                                                         ((PostalRateSelection)e1r.OriginalTag).ServiceType == endiciaRateDetail.ServiceType && ((PostalRateSelection)e1r.OriginalTag).ConfirmationType == endiciaRateDetail.ConfirmationType);
                            }

                            // If Express1 returned a rate, check to make sure it is a lower amount
                            if (express1Rate != null && express1Rate.Amount <= endiciaRate.Amount)
                            {
                                // If the logo is currently set, make sure it's set to Endicia
                                if (express1Rate.ProviderLogo != null)
                                {
                                    express1Rate.ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.Endicia);
                                }

                                finalRates.Add(express1Rate);
                                //hasExpress1Savings = true;
                            }
                            else
                            {
                                finalRates.Add(endiciaRate);
                            }
                        }
                        else
                        {
                            finalRates.Add(endiciaRate);
                        }
                    }

                    // Filter out any excluded services, but always include the service that the shipment is configured with
                    List<RateResult> finalRatesFilteredByAvailableServices = endiciaShipmentType.FilterRatesByExcludedServices(shipment, finalRates.Select(e =>
                    {
                        e.ShipmentType = ShipmentTypeCode.Endicia;
                        return e;
                    }).ToList());

                    RateGroup finalGroup = new RateGroup(finalRatesFilteredByAvailableServices);

                    // As it pertains to Endicia, restricting discounted rate messaging means we're no longer obligated to promote
                    // Express1 with Endicia and can show promotion for USPS shipping. So when discount rate
                    // messaging is restricted on Endicia, we want to show the Usps promo
                    if (endiciaShipmentType.IsRateDiscountMessagingRestricted)
                    {
                        // Always show the USPS promotion when Express 1 is restricted - show the
                        // single account dialog if Endicia has Express1 accounts and is not using USPS
                        bool showSingleAccountDialog = EndiciaAccountManager.Express1Accounts.Any();
                        finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(endiciaShipmentType, shipment, showSingleAccountDialog));
                    }

                    return finalGroup;
                }
                // Express1 wasn't used, so we want to promote USPS

                // Filter out any excluded services, but always include the service that the shipment is configured with
                RateGroup finalEndiciaOnlyRates = new RateGroup(endiciaShipmentType.FilterRatesByExcludedServices(shipment, endiciaRates));

                if (endiciaShipmentType.IsRateDiscountMessagingRestricted)
                {
                    // Show the single account dialog if there are Express1 accounts and customer is not using USPS 
                    bool showSingleAccountDialog = EndiciaAccountManager.Express1Accounts.Any();
                    finalEndiciaOnlyRates.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(endiciaShipmentType, shipment, showSingleAccountDialog));
                }

                return finalEndiciaOnlyRates;
            }
            // Express1 rates - return rates filtered by what is available to the user
            RateGroup express1Group = endiciaShipmentType.BuildExpress1RateGroup(endiciaRates, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Endicia);
            if (endiciaShipmentType.IsRateDiscountMessagingRestricted)
            {
                // (Express1) rate discount messaging is restricted, so we're allowed to add the USPS 
                // poromo footnote to show single account marketing dialog
                express1Group.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(endiciaShipmentType, shipment, true));
            }

            return express1Group;
        }
    }
}
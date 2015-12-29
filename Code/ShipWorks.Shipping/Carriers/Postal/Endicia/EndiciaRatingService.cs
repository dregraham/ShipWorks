using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Logging;
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
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;
        private readonly ILogEntryFactory logEntryFactory;
        private readonly Func<string, ICertificateInspector> certificateInspectorFactory;

        public EndiciaRatingService(
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory, 
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory,
            IIndex<ShipmentTypeCode, ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository,
            ILogEntryFactory logEntryFactory,
            Func<string, ICertificateInspector> certificateInspectorFactory)
            : base(ratingServiceFactory, shipmentTypeFactory)
        {
            this.accountRepository = accountRepository;
            this.logEntryFactory = logEntryFactory;
            this.certificateInspectorFactory = certificateInspectorFactory;
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

            express1Rates = GetExpress1Rates(shipment, express1Rates, settings);

            EndiciaShipmentType endiciaShipmentType = shipmentTypeFactory[(ShipmentTypeCode)shipment.ShipmentType] as EndiciaShipmentType;

            if (endiciaShipmentType == null)
            {
                throw new EndiciaException("Could not get endicia shipment type");
            }

            EndiciaApiClient endiciaApiClient =
                new EndiciaApiClient(GetAccountRepository((ShipmentTypeCode)shipment.ShipmentType), logEntryFactory,
                    certificateInspectorFactory(string.Empty));

            List<RateResult> allEndiciaRates = (InterapptiveOnly.MagicKeysDown) ?
                endiciaApiClient.GetRatesSlow(shipment, endiciaShipmentType) :
                endiciaApiClient.GetRatesFast(shipment, endiciaShipmentType);

            // Filter out any excluded services, but always include the service that the shipment is configured with
            List<RateResult> endiciaRates = FilterRatesByExcludedServices(shipment, allEndiciaRates);

            // For endicia, we want to either promote Express1 or show the Express1 savings
            if (shipment.ShipmentType == (int)ShipmentTypeCode.Endicia)
            {
                return CompileEndiciaRates(shipment, endiciaShipmentType, endiciaRates, express1Rates);
            }

            // Express1 rates - return rates filtered by what is available to the user
            RateGroup express1Group = BuildExpress1RateGroup(endiciaRates, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Endicia);
            if (endiciaShipmentType.IsRateDiscountMessagingRestricted)
            {
                // (Express1) rate discount messaging is restricted, so we're allowed to add the USPS 
                // poromo footnote to show single account marketing dialog
                express1Group.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(endiciaShipmentType, shipment, true));
            }

            return express1Group;
        }

        /// <summary>
        /// Compiles the endica rates.
        /// </summary>
        private RateGroup CompileEndiciaRates(ShipmentEntity shipment, EndiciaShipmentType endiciaShipmentType, List<RateResult> endiciaRates, List<RateResult> express1Rates)
        {
            if (!endiciaShipmentType.IsRateDiscountMessagingRestricted)
            {
                List<RateResult> finalRates = CompileFinalRates(endiciaRates, express1Rates);

                // Filter out any excluded services, but always include the service that the shipment is configured with
                List<RateResult> finalRatesFilteredByAvailableServices = FilterRatesByExcludedServices(shipment, finalRates.Select(e =>
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
            RateGroup finalEndiciaOnlyRates = new RateGroup(FilterRatesByExcludedServices(shipment, endiciaRates));

            if (endiciaShipmentType.IsRateDiscountMessagingRestricted)
            {
                // Show the single account dialog if there are Express1 accounts and customer is not using USPS 
                bool showSingleAccountDialog = EndiciaAccountManager.Express1Accounts.Any();
                finalEndiciaOnlyRates.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(endiciaShipmentType, shipment, showSingleAccountDialog));
            }

            return finalEndiciaOnlyRates;
        }


        /// <summary>
        /// Go Through Endicia rates and add either the endicia rate or express1 rate to final rates.
        /// </summary>
        private static List<RateResult> CompileFinalRates(List<RateResult> endiciaRates, List<RateResult> express1Rates)
        {
            List<RateResult> finalRates = new List<RateResult>();

            // Go through each Endicia rate
            foreach (RateResult endiciaRate in endiciaRates)
            {
                PostalRateSelection endiciaRateDetail = (PostalRateSelection) endiciaRate.OriginalTag;

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
                            ((PostalRateSelection) e1r.OriginalTag).ServiceType == endiciaRateDetail.ServiceType &&
                            ((PostalRateSelection) e1r.OriginalTag).ConfirmationType ==
                            endiciaRateDetail.ConfirmationType);
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

            return finalRates;
        }

        /// <summary>
        /// Gets the express1 rates.
        /// </summary>
        private List<RateResult> GetExpress1Rates(ShipmentEntity shipment, List<RateResult> express1Rates, ShippingSettingsEntity settings)
        {
            // See if this shipment should really go through Express1
            if (UseExpressOne(shipment, settings))
            {
                // We don't have any Endicia accounts, so let the user know they need an account.
                if (!GetAccountRepository((ShipmentTypeCode)shipment.ShipmentType).Accounts.Any())
                {
                    throw new EndiciaException($"An account is required to view {EnumHelper.GetDescription(ShipmentTypeCode.Endicia)} rates.");
                }

                EndiciaAccountEntity express1Account = EndiciaAccountManager.GetAccount(settings.EndiciaAutomaticExpress1Account);

                if (express1Account == null)
                {
                    throw new EndiciaException("The Express1 account to automatically use when processing with Endicia has not been selected.");
                }

                express1Rates = GetExpress1Rates(shipment, express1Account, null);
            }

            return express1Rates;
        }

        /// <summary>
        /// Get Express1 rates for the given shipment
        /// </summary>
        private List<RateResult> GetExpress1Rates(ShipmentEntity shipment, EndiciaAccountEntity express1Account, List<RateResult> express1Rates)
        {
            // We temporarily turn this into an Exprss1 shipment to get rated
            shipment.ShipmentType = (int) ShipmentTypeCode.Express1Endicia;
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
                shipment.ShipmentType = (int) ShipmentTypeCode.Endicia;
                shipment.Postal.Endicia.EndiciaAccountID = shipment.Postal.Endicia.OriginalEndiciaAccountID.Value;
                shipment.Postal.Endicia.OriginalEndiciaAccountID = null;
            }
            return express1Rates;
        }

        /// <summary>
        /// Returns the an account repository for the given account
        /// </summary>
        private ICarrierAccountRepository<EndiciaAccountEntity> GetAccountRepository(ShipmentTypeCode shipmentTypeCode)
        {
            return accountRepository[shipmentTypeCode];
        }

        /// <summary>
        /// Returns true if we should use express 1
        /// </summary>
        private bool UseExpressOne(ShipmentEntity shipment, ShippingSettingsEntity settings)
        {
            return shipment.ShipmentType == (int)ShipmentTypeCode.Endicia
                   && !shipmentTypeFactory[ShipmentTypeCode.Endicia].IsRateDiscountMessagingRestricted
                   && Express1Utilities.IsValidPackagingType(null, (PostalPackagingType)shipment.Postal.PackagingType)
                   && settings.EndiciaAutomaticExpress1;
        }
    }
}
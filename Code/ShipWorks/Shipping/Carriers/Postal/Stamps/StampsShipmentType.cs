using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Properties;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.NotQualified;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Editions;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration.Promotion;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    public class StampsShipmentType : PostalShipmentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StampsShipmentType"/> class.
        /// </summary>
        public StampsShipmentType()
        {
            ShouldRetrieveExpress1Rates = true;

            // Use the "live" versions by default
            AccountRepository = new StampsAccountRepository();
            LogEntryFactory = new LogEntryFactory();            
        }

        /// <summary>
        /// Gets or sets the repository that should be used when retrieving account information.
        /// </summary>
        public ICarrierAccountRepository<UspsAccountEntity> AccountRepository { get; set; }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts
        {
            get
            {
                return AccountRepository.Accounts.Any();
            }
        }

        /// <summary>
        /// Gets or sets the log entry factory.
        /// </summary>
        public LogEntryFactory LogEntryFactory { get; set; }

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Stamps; }
        }

        /// <summary>
        /// Indicates if the shipment service type supports return shipments
        /// </summary>
        public override bool SupportsReturns
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the type of the reseller.
        /// </summary>
        public virtual StampsResellerType ResellerType
        {
            get { return StampsResellerType.None; }
        }

        /// <summary>
        /// Creates the web client to use to contact the underlying carrier API.
        /// </summary>
        /// <returns>An instance of IStampsWebClient. </returns>
        public virtual IStampsWebClient CreateWebClient()
        {
            // This needs to be created each time rather than just being an instance property, 
            // because of counter rates where the account repository is swapped out out prior 
            // to creating the web client.
            return new StampsWebClient(AccountRepository, LogEntryFactory, CertificateInspector, ResellerType);
        }

		/// <summary>
        /// Create the Form used to do the setup for the Stamps.com API
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            // Adding an account through this shipment type should always create an account
            // with CBP rates (primarily for customers migrating from Endicia that have 
            // contracted rates with the postal service).
		    IRegistrationPromotion promotion = new StampsCbpRegistrationPromotion();

            // Push customers to the USPS (Stamps.com Expedited) setup wizard
            return new UspsSetupWizard(promotion, true);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new UspsServiceControl(rateControl);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new UspsProfileControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new UspsSettingsControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Stamps.com supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates if the shipment type supports accounts as the origin
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get { return true; }
        }

        /// <summary>
        /// Should Express1 rates be checked when getting Endicia rates?
        /// </summary>
        public bool ShouldRetrieveExpress1Rates { get; set; }        

        /// <summary>
        /// Update the origin address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public override bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {
            if (shipment.Processed)
            {
                return true;
            }

            // The Stamps or Postal objects may not yet be set if we are in the middle of creating a new shipment
            if (originID == (int)ShipmentOriginSource.Account && shipment.Postal != null && shipment.Postal.Usps != null)
            {
                UspsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
                if (account == null)
                {
                    account = AccountRepository.Accounts.FirstOrDefault();
                }

                if (account != null)
                {
                    PersonAdapter.Copy(account, "", person);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return base.UpdatePersonAddress(shipment, person, originID);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            // Take this opportunity to try to update contract type of the account
            UspsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
            UpdateContractType(account);

            // Get counter rates if we don't have any Endicia accounts, letting the Postal shipment type take care of caching
            // since it should be using a different cache key
            return AccountRepository.Accounts.Any() ?
                GetCachedRates<StampsException>(shipment, GetRatesFromApi) :
                GetCounterRates(shipment);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected virtual RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            List<RateResult> express1Rates = null;
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            bool isExpress1Restricted = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps).IsShipmentTypeRestricted;

            // See if this shipment should really go through Express1
            if (shipment.ShipmentType == (int)ShipmentTypeCode.Stamps &&
               settings.UspsAutomaticExpress1 && !isExpress1Restricted &&
               Express1Utilities.IsValidPackagingType((PostalServiceType?)null, (PostalPackagingType)shipment.Postal.PackagingType))
            {
                var express1Account = StampsAccountManager.GetAccount(settings.UspsAutomaticExpress1Account);

                if (express1Account == null)
                {
                    throw new StampsException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
                }

                // We temporarily turn this into an Exprss1 shipment to get rated
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
                shipment.Postal.Usps.UspsAccountID = express1Account.UspsAccountID;

                try
                {
                    // Currently this actually recurses into this same method
                    express1Rates = (ShouldRetrieveExpress1Rates) ?
                        ShipmentTypeManager.GetType(shipment).GetRates(shipment).Rates.ToList() :
                        new List<RateResult>();
                }
                catch (ShippingException)
                {
                    // Eat the exception; we don't want to stop someone from using Stamps if Express1 can't get rates
                }
                finally
                {
                    shipment.ShipmentType = (int)ShipmentTypeCode.Stamps;
                    shipment.Postal.Usps.UspsAccountID = shipment.Postal.Usps.OriginalUspsAccountID.Value;
                    shipment.Postal.Usps.OriginalUspsAccountID = null;
                }
            }

            List<RateResult> stampsRates = CreateWebClient().GetRates(shipment);

            // For Stamps, we want to either promote Express1 or show the Express1 savings
            if (shipment.ShipmentType == (int)ShipmentTypeCode.Stamps)
            {
                if (ShouldRetrieveExpress1Rates && !IsRateDiscountMessagingRestricted)
                {
                    // Merge the discounted Express1 rates into the stamps.com rates
                    return MergeDiscountedRates(shipment, stampsRates, express1Rates, settings);
                }

                RateGroup rateGroup = new RateGroup(stampsRates);
                AddUspsRatePromotionFootnote(shipment, rateGroup);

                return rateGroup;
            }
            else
            {
                // Express1 rates - return rates filtered by what is available to the user
                RateGroup express1Group = BuildExpress1RateGroup(stampsRates, ShipmentTypeCode.Express1Stamps, ShipmentTypeCode.Express1Stamps);
                if (IsRateDiscountMessagingRestricted)
                {
                    // (Express1) rate discount messaging is restricted, so we're allowed to add the USPS (Stamps.com Expedited)
                    // promo footnote to show single account marketing dialog
                    express1Group.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, true));
                }

                return express1Group;
            }
        }

        /// <summary>
        /// Conditionally adds the usps rate promotion footnote based on the contract type of the account associated with the shipment
        /// and whether the shipping account conversion feature is restricted.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="rateGroup">The rate group.</param>
        protected void AddUspsRatePromotionFootnote(ShipmentEntity shipment, RateGroup rateGroup)
        {
            StampsAccountContractType contractType = (StampsAccountContractType) AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID).ContractType;

            // We may not want to show the conversion promotion for multi-user Stamps.com accounts due 
            // to a limitation on Stamps' side. (Tango will send these to ShipWorks via data contained
            // in ShipmentTypeFunctionality
            bool accountConversionRestricted = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.ShippingAccountConversion, ShipmentTypeCode).Level == EditionRestrictionLevel.Forbidden;
            if (contractType == StampsAccountContractType.Commercial && !accountConversionRestricted)
            {
                // Show the promotional footer for discounted rates 
                rateGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, false));
            }
        }

        /// <summary>
        /// Merges the discounted rates with the Stamps.com rates.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="stampsRates">The stamps rates.</param>
        /// <param name="discountedRates">The discounted rates.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A RateGroup containing the merged rate results.</returns>
        private RateGroup MergeDiscountedRates(ShipmentEntity shipment, List<RateResult> stampsRates, List<RateResult> discountedRates, ShippingSettingsEntity settings)
        {
            List<RateResult> finalRates = new List<RateResult>();

            // Go through each Stamps rate
            foreach (RateResult stampsRate in stampsRates)
            {
                PostalRateSelection stampsRateDetail = (PostalRateSelection)stampsRate.OriginalTag;
                stampsRate.ShipmentType = ShipmentTypeCode.Stamps;

                // If it's a rate they could (or have) saved on with Express1, we modify it
                if (stampsRate.Selectable &&
                    stampsRateDetail != null &&
                    Express1Utilities.IsPostageSavingService(stampsRateDetail.ServiceType))
                {
                    // See if Express1 returned a rate for this service
                    RateResult discountedRate = DetermineDiscountedRate(discountedRates, stampsRateDetail, stampsRate);

                    // If Express1 returned a rate, check to make sure it is a lower amount
                    if (discountedRate != null && discountedRate.Amount <= stampsRate.Amount)
                    {
                        finalRates.Add(discountedRate);
                    }
                    else
                    {
                        finalRates.Add(stampsRate);
                    }
                }
                else
                {
                    RateResult discountedRate = DetermineDiscountedRate(discountedRates, stampsRateDetail, stampsRate);
                    if (discountedRate != null)
                    {
                        stampsRate.ProviderLogo = discountedRate.ProviderLogo;
                    }

                    finalRates.Add(stampsRate);
                }
            }

            RateGroup finalGroup = new RateGroup(finalRates.Select(e => { e.ShipmentType = ShipmentTypeCode.Stamps; return e; }).ToList());

            // No longer show any Express1 related footnotes/promotions, but we always want to show the 
            // USPS (Stamps.com Expedited) promotion when Express 1 is restricted and the account has not
            // been converted from a commercial account
            if (StampsAccountManager.Express1Accounts.Any() && !settings.StampsUspsAutomaticExpedited)
            {
                // Show the single account dialog if the customer has Express1 accounts and hasn't converted to USPS (Stamps.com Expedited)
                finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, true));
            } 
            else if (AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID).ContractType == (int)StampsAccountContractType.Commercial)
            {
                // Show the promotional footer for discounted rates 
                finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, false));
            }

            return finalGroup;
        }

        /// <summary>
        /// Determines and returns a discounted RateResult if one exists.
        /// </summary>
        private static RateResult DetermineDiscountedRate(List<RateResult> discountedRates, PostalRateSelection stampsRateDetail, RateResult stampsRate)
        {
            RateResult discountedRate = null;
            if (stampsRateDetail != null && discountedRates != null && discountedRates.Any(express1Rate => express1Rate.Selectable == stampsRate.Selectable))
            {
                discountedRate = discountedRates.Where(express1Rate => express1Rate.Selectable == stampsRate.Selectable)
                                                .FirstOrDefault(express1Rate => ((PostalRateSelection)express1Rate.OriginalTag).ServiceType == stampsRateDetail.ServiceType &&
                                                                                ((PostalRateSelection)express1Rate.OriginalTag).ConfirmationType == stampsRateDetail.ConfirmationType);

                if (discountedRate != null)
                {
                    discountedRate.ShipmentType = stampsRate.ShipmentType;
                }
            }

            return discountedRate;
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        public override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new StampsShipmentProcessingSynchronizer(AccountRepository);
        }

        /// <summary>
        /// Allows the shipment type to run any pre-processing work that may need to be performed prior to
        /// actually processing the shipment. In most cases this is checking to see if an account exists
        /// and will call the counterRatesProcessing callback provided when trying to process a shipment
        /// without any accounts for this shipment type in ShipWorks, otherwise the shipment is unchanged.
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="counterRatesProcessing"></param>
        /// <param name="selectedRate"></param>
        /// <returns>
        /// The updates shipment (or shipments) that is ready to be processed. A null value may
        /// be returned to indicate that processing should be halted completely.
        /// </returns>
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            // We need to handle the case where the Stamps.com shipment type was selected to process the shipment and there
            // aren't any "native" Stamps.com accounts; we need to basically push the shipment over to USPS to create the 
            // account under USPS and process it there
            if (GetProcessingSynchronizer().HasAccounts && shipment.ShipmentType == (int) ShipmentTypeCode.Stamps)
            {
                // There is an existing "native" Stamps.com account, so do the pre-processing normally
                shipments = base.PreProcess(shipment, counterRatesProcessing, selectedRate);

                // Take this opportunity to try to update contract type of the account
                if (shipment.Postal != null && shipment.Postal.Usps != null)
                {
                    UspsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID);
                    UpdateContractType(account);
                }
            }
            else
            {
                // There aren't any Stamps.com accounts, so we need to run the pre-process of the UspsShipmentType in order
                // to create the account/process the shipment
                shipment.ShipmentType = (int) ShipmentTypeCode.Usps;

                UspsShipmentType uspsShipmentType = new UspsShipmentType();
                shipments = uspsShipmentType.PreProcess(shipment, counterRatesProcessing, selectedRate);
            }

            return shipments;
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            bool useExpress1 = Express1Utilities.IsPostageSavingService(shipment) && !IsRateDiscountMessagingRestricted &&
                Express1Utilities.IsValidPackagingType((PostalServiceType)shipment.Postal.Service, (PostalPackagingType)shipment.Postal.PackagingType) &&
                ShippingSettings.Fetch().UspsAutomaticExpress1;

            UspsAccountEntity express1Account = StampsAccountManager.GetAccount(ShippingSettings.Fetch().UspsAutomaticExpress1Account);

            if (useExpress1)
            {
                if (express1Account == null)
                {
                    throw new ShippingException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
                }

                int originalShipmentType = shipment.ShipmentType;
                long? originalStampsAccountID = shipment.Postal.Usps.OriginalUspsAccountID;
                long stampsAccountID = shipment.Postal.Usps.UspsAccountID;

                try
                {
                    // Check Stamps.com amount
                    List<RateResult> stampsRates = CreateWebClient().GetRates(shipment);
                    RateResult stampsRate = stampsRates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                  ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                  && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

                    // Check Express1 amount
                    shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                    shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
                    shipment.Postal.Usps.UspsAccountID = express1Account.UspsAccountID;

                    List<RateResult> express1Rates = new Express1StampsWebClient().GetRates(shipment);
                    RateResult express1Rate = express1Rates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                      ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                      && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

                    // Now set useExpress1 to true only if the express 1 rate is less than the Stamps amount
                    if (stampsRate != null && express1Rate != null)
                    {
                        useExpress1 = express1Rate.Amount <= stampsRate.Amount;
                    }
                    else
                    {
                        // If we can't figure it out for sure, don't use it
                        useExpress1 = false;
                    }
                }
                catch (StampsException stampsException)
                {
                    throw new ShippingException(stampsException.Message, stampsException);
                }
                finally
                {
                    // Reset back to the original values
                    shipment.ShipmentType = originalShipmentType;
                    shipment.Postal.Usps.OriginalUspsAccountID = originalStampsAccountID;
                    shipment.Postal.Usps.UspsAccountID = stampsAccountID;
                }
            }

            // See if this shipment should really go through Express1
            if (useExpress1)
            {
                // Now we turn this into an Express1 shipment...
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                shipment.Postal.Usps.OriginalUspsAccountID = shipment.Postal.Usps.UspsAccountID;
                shipment.Postal.Usps.UspsAccountID = express1Account.UspsAccountID;

                Express1StampsShipmentType shipmentType = (Express1StampsShipmentType)ShipmentTypeManager.GetType(shipment);

                // Process via Express1
                shipmentType.UpdateDynamicShipmentData(shipment);
                shipmentType.ProcessShipment(shipment);
            }
            else
            {
                // This would be set if they have tried to process as Express1 but it failed... make sure its clear since we are not using it.
                shipment.Postal.Usps.OriginalUspsAccountID = null;

                try
                {
                    CreateWebClient().ProcessShipment(shipment);
                }
                catch (StampsException ex)
                {
                    throw new ShippingException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Update the dyamic data of the shipment
        /// </summary>
        /// <param name="shipment"></param>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            if (shipment.Postal != null && shipment.Postal.Usps != null)
            {
                shipment.RequestedLabelFormat = shipment.Postal.Usps.RequestedLabelFormat;                
            }
        }

        /// <summary>
        /// Validate the shipment before processing or rating
        /// </summary>
        protected static void ValidateShipment(ShipmentEntity shipment)
        {
            if (shipment.TotalWeight == 0)
            {
                throw new ShippingException("The shipment weight cannot be zero.");
            }

            if (shipment.Postal.Service == (int)PostalServiceType.ExpressMail && shipment.Postal.Confirmation != (int)PostalConfirmationType.None)
            {
                throw new ShippingException("A confirmation option cannot be used with Express mail.");
            }
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            try
            {
                CreateWebClient().VoidShipment(shipment);
            }
            catch (StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get the USPS shipment details
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            UspsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);

            ShipmentCommonDetail commonDetail = base.GetShipmentCommonDetail(shipment);
            commonDetail.OriginAccount = (account == null) ? "" : account.Username;

            if (shipment.ShipmentType == (int)ShipmentTypeCode.Express1Stamps && shipment.Postal.Usps.OriginalUspsAccountID != null)
            {
                commonDetail.OriginalShipmentType = ShipmentTypeCode.Stamps;
            }

            return commonDetail;
        }

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            base.LoadShipmentData(shipment, refreshIfPresent);

            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment.Postal, "Usps", typeof(UspsShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Configure the properties of a newly created shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Usps != null)
            {
                // Use the empty guids for now - they'll get set properly during processing
                shipment.Postal.Usps.IntegratorTransactionID = Guid.Empty;
                shipment.Postal.Usps.UspsTransactionID = Guid.Empty;
                shipment.Postal.Usps.RequestedLabelFormat = (int)ThermalLanguage.None;
                shipment.Postal.Usps.RateShop = false;
            }

            // We need to call the base after setting up the Stamps.com specific information because LLBLgen was
            // sometimes not including the above values when we first save the shipment deep in the customs loader
            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            base.LoadProfileData(profile, refreshIfPresent);

            ShipmentTypeDataService.LoadProfileData(profile.Postal, "Usps", typeof(UspsProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            UspsProfileEntity usps = profile.Postal.Usps;

            usps.UspsAccountID = AccountRepository.Accounts.Any() ? AccountRepository.Accounts.First().UspsAccountID : 0;
            usps.RequireFullAddressValidation = true;
            usps.HidePostage = true;
            usps.Memo = string.Empty;
            profile.Postal.Usps.RateShop = false;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Usps != null)
            {
                UspsShipmentEntity uspsShipment = shipment.Postal.Usps;
                UspsProfileEntity uspsProfile = profile.Postal.Usps;

                ShippingProfileUtility.ApplyProfileValue(uspsProfile.UspsAccountID, uspsShipment, UspsShipmentFields.UspsAccountID);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.RequireFullAddressValidation, uspsShipment, UspsShipmentFields.RequireFullAddressValidation);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.HidePostage, uspsShipment, UspsShipmentFields.HidePostage);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.Memo, uspsShipment, UspsShipmentFields.Memo);
            }
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            var labels = new Lazy<List<TemplateLabelData>>(() => LoadLabelData(shipment));

            // Add the labels content
            container.AddElement("Labels",
                new LabelsOutline(container.Context, shipment, labels, ImageFormat.Png),
                ElementOutline.If(() => shipment().Processed));

        }

        /// <summary>
        /// Determines if delivery\signature confirmation is available for the given service
        /// </summary>
        public override List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            // If we don't know the packaging or country, it doesn't matter
            if (!string.IsNullOrWhiteSpace(countryCode) && packaging != null)
            {
                if (PostalUtility.IsFreeInternationalDeliveryConfirmation(countryCode, service, packaging.Value))
                {
                    return new List<PostalConfirmationType> { PostalConfirmationType.Delivery };
                }
            }

            if (service == PostalServiceType.PriorityMail && packaging == PostalPackagingType.Envelope)
            {
                return new List<PostalConfirmationType> { PostalConfirmationType.None };
            }

            return base.GetAvailableConfirmationTypes(countryCode, service, packaging);
        }

        /// <summary>
        /// Load all the label data for the given shipmentID
        /// </summary>
        private static List<TemplateLabelData> LoadLabelData(Func<ShipmentEntity> shipment)
        {
            List<TemplateLabelData> labelData = new List<TemplateLabelData>();

            // Get the resource list for our shipment
            List<DataResourceReference> resources = DataResourceManager.GetConsumerResourceReferences(shipment().ShipmentID);

            // Could be none for upgraded 2x shipments
            if (resources.Count > 0)
            {
                // Add our standard label output
                DataResourceReference labelResource = resources.Single(i => i.Label == "LabelPrimary");
                labelData.Add(new TemplateLabelData(null, "Label", TemplateLabelCategory.Primary, labelResource));

                // Add all label parts
                var labelResources = resources.Where(r => r.Label.StartsWith("LabelPart"));
                foreach (DataResourceReference otherLabel in labelResources)
                {
                    labelData.Add(new TemplateLabelData(null, otherLabel.Label, TemplateLabelCategory.Supplemental, otherLabel));
                }
            }

            return labelData;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the Stamps.com shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a StampsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            if (AccountRepository.Accounts.Any())
            {
                // We have an account, so use the normal broker
                return new StampsBestRateBroker(this, AccountRepository);
            }
            else
            {
                // No accounts, so use the counter rates broker to allow the user to
                // sign up for the account
                return new StampsCounterRatesBroker(new StampsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance));
            }
        }

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public override void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {
            if (shipment.Postal != null && shipment.Postal.Usps != null)
            {
                shipment.Postal.Usps.ScanFormBatchID = null;
            }
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            List<IEntityField2> fields = new List<IEntityField2>(base.GetRatingFields(shipment));

            fields.AddRange
            (
                new List<IEntityField2>()
                {
                    shipment.Postal.Usps.Fields[UspsShipmentFields.UspsAccountID.FieldIndex],
                    shipment.Postal.Usps.Fields[UspsShipmentFields.OriginalUspsAccountID.FieldIndex],
                }
            );

            return fields;
        }

        /// <summary>
        /// Uses the Stamps.com API to update the contract type of the account if it is unkown.
        /// </summary>
        /// <param name="account">The account.</param>
        public virtual void UpdateContractType(UspsAccountEntity account)
        {
            if (account != null)
            {
                // We want to update the contract if it's not in the cache (or dropped out) or if the contract type is unknown; the cache is used
                // so we don't have to perform this everytime, but does allow ShipWorks to handle cases where the contract type may have been
                // updated outside of ShipWorks.
                if (!StampsContractTypeCache.Contains(account.UspsAccountID) || StampsContractTypeCache.GetContractType(account.UspsAccountID) == StampsAccountContractType.Unknown)
                {
                    try
                    {
                        // Grab contract type from the Stamps API 
                        IStampsWebClient webClient = CreateWebClient();
                        StampsAccountContractType contractType = webClient.GetContractType(account);

                        bool hasContractChanged = account.ContractType != (int) contractType;
                        account.ContractType = (int) contractType;

                        // Save the contract to the DB and update the cache
                        AccountRepository.Save(account);
                        StampsContractTypeCache.Set(account.UspsAccountID, (StampsAccountContractType)account.ContractType);

                        if (hasContractChanged)
                        {
                            // Any cached rates are probably invalid now
                            RateCache.Instance.Clear();

                            // Only notify Tango of changes so it has the latest information (and cuts down on traffic)
                            ITangoWebClient tangoWebClient = new TangoWebClientFactory().CreateWebClient();
                            tangoWebClient.LogStampsAccount(account);
                        }
                    }
                    catch (Exception exception)
                    {
                        // Log the error
                        LogManager.GetLogger(GetType()).Error(string.Format("ShipWorks encountered an error when getting contract type for account {0}.", account.Username), exception);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.Postal != null && shipment.Postal.Usps != null)
            {
                shipment.Postal.Usps.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            // We're going to be temporarily swapping these out to get counter rates, so 
            // make a note of the original values
            ICarrierAccountRepository<UspsAccountEntity> originalAccountRepository = AccountRepository;
            ICertificateInspector originalCertificateInspector = CertificateInspector;

            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);

                AccountRepository = new StampsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance);
                CertificateInspector = new CertificateInspector(TangoCounterRatesCredentialStore.Instance.StampsCertificateVerificationData);

                // Fetch the rates now that we're setup to use counter rates
                return GetCachedRates<StampsException>(shipment, GetRates);

            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(this));

                return errorRates;
            }
            finally
            {
                // Set everything back to normal
                AccountRepository = originalAccountRepository;
                CertificateInspector = originalCertificateInspector;
            }
        }
    }
}

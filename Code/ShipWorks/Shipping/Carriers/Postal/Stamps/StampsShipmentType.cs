﻿using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Properties;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
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
        public ICarrierAccountRepository<StampsAccountEntity> AccountRepository { get; set; }

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
        /// Create the Form used to do the setup for the Stamps.com API
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new StampsSetupWizard(new StampsExpeditedRegistrationPromotion(), true);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new StampsServiceControl(rateControl);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new StampsProfileControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl(ShipmentTypeCode);
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
            if (originID == (int)ShipmentOriginSource.Account && shipment.Postal != null && shipment.Postal.Stamps != null)
            {
                StampsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID);
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
            StampsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID);
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
               settings.StampsAutomaticExpress1 && !isExpress1Restricted &&
               Express1Utilities.IsValidPackagingType((PostalServiceType?)null, (PostalPackagingType)shipment.Postal.PackagingType))
            {
                var express1Account = StampsAccountManager.GetAccount(settings.StampsAutomaticExpress1Account);

                if (express1Account == null)
                {
                    throw new StampsException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
                }

                // We temporarily turn this into an Exprss1 shipment to get rated
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                shipment.Postal.Stamps.OriginalStampsAccountID = shipment.Postal.Stamps.StampsAccountID;
                shipment.Postal.Stamps.StampsAccountID = express1Account.StampsAccountID;

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
                    shipment.Postal.Stamps.StampsAccountID = shipment.Postal.Stamps.OriginalStampsAccountID.Value;
                    shipment.Postal.Stamps.OriginalStampsAccountID = null;
                }
            }

            List<RateResult> stampsRates = new StampsApiSession(AccountRepository, LogEntryFactory, CertificateInspector).GetRates(shipment);

            // For Stamps, we want to either promote Express1 or show the Express1 savings
            if (shipment.ShipmentType == (int)ShipmentTypeCode.Stamps)
            {
                if (ShouldRetrieveExpress1Rates)
                {
                    // Merge the discounted Express1 rates into the stamps.com rates
                    return MergeDiscountedRates(shipment, stampsRates, express1Rates, settings);
                }

                RateGroup rateGroup = new RateGroup(stampsRates);
                StampsAccountContractType contractType  = (StampsAccountContractType) AccountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID).ContractType;

                if (contractType == StampsAccountContractType.Commercial)
                {
                    // Show the promotional footer for discounted rates 
                    rateGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, false));
                }
                // TODO: Enable this if/when we have rates coming back for commercial plus to compare with
                //else if (contractType == StampsAccountContractType.CommercialPlus)
                //{
                //    rateGroup.AddFootnoteFactory(new UspsRateDiscountedFootnoteFactory(this, stampsRates, stampsRates));
                //}

                return rateGroup;
            }
            else
            {
                // Express1 rates - return rates filtered by what is available to the user
                return BuildExpress1RateGroup(stampsRates, ShipmentTypeCode.Express1Stamps, ShipmentTypeCode.Express1Stamps);
            }
        }

        private RateGroup MergeDiscountedRates(ShipmentEntity shipment, List<RateResult> stampsRates, List<RateResult> discountedRates, ShippingSettingsEntity settings)
        {
            List<RateResult> finalRates = new List<RateResult>();
            bool isExpress1Restricted = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Stamps).IsShipmentTypeRestricted;
            bool hasDiscountFootnote = false;

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
                    RateResult discountedRate = null;
                    if (discountedRates != null && discountedRates.Any(e1r => e1r.Selectable))
                    {
                        discountedRate = discountedRates.Where(e1r => e1r.Selectable).FirstOrDefault(e1r =>
                                        ((PostalRateSelection) e1r.OriginalTag).ServiceType == stampsRateDetail.ServiceType && ((PostalRateSelection) e1r.OriginalTag).ConfirmationType == stampsRateDetail.ConfirmationType);

                        discountedRate.ShipmentType = stampsRate.ShipmentType;
                    }

                    // If Express1 returned a rate, check to make sure it is a lower amount
                    if (discountedRate != null && discountedRate.Amount <= stampsRate.Amount)
                    {
                        finalRates.Add(discountedRate);
                    }
                    else
                    {
                        finalRates.Add(stampsRate);

                        // Set the express rate to null so that it doesn't add the icon later
                        discountedRate = null;
                    }

                    //RateResult rate = finalRates[finalRates.Count - 1];

                    // Remove all checks/stars that bring attemtion to Express1
                    // TODO: Will need to convert this to use Stamps.com expedited rates
                    //if (!isExpress1Restricted)
                    //{
                    //    // Don't show indicators if Express1 is restricted
                    //    // If user wanted Express 1 rates
                    //    if (settings.StampsAutomaticExpress1)
                    //    {
                    //        // If they actually got the rate, show the check
                    //        if (discountedRate != null)
                    //        {
                    //            rate.AmountFootnote = Resources.check2;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        // Stamps rates only.  If it's not a valid Express1 packaging type, don't promote a savings
                    //        if (!isExpress1Restricted && Express1Utilities.IsValidPackagingType(((PostalRateSelection) rate.OriginalTag).ServiceType, (PostalPackagingType) shipment.Postal.PackagingType))
                    //        {
                    //            rate.AmountFootnote = Resources.star_green;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    finalRates.Add(stampsRate);
                }
            }

            RateGroup finalGroup = new RateGroup(finalRates.Select(e => { e.ShipmentType = ShipmentTypeCode.Stamps; return e; }).ToList());

            // No longer show any Express1 related footnotes/promotions, but we always want to show the 
            // USPS (Stamps.com Expedited) promotion when Express 1 is restricted and the account has not
            // been converted from a commercial account
            if (AccountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID).ContractType == (int)StampsAccountContractType.Commercial)
            {
                // Show the promotional footer for discounted rates 
                finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, false));
                hasDiscountFootnote = true;
            }

            if (!hasDiscountFootnote)
            {
                bool showFootnote = StampsAccountManager.Express1Accounts.Any() && !settings.StampsUspsAutomaticExpedited;
                if (showFootnote)
                {
                    // Show the single account dialog if the customer has Express1 accounts and hasn't converted to USPS (Stamps.com Expedited)
                    finalGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(this, shipment, true));
                }
            }

            return finalGroup;
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
            List<ShipmentEntity> shipments = base.PreProcess(shipment, counterRatesProcessing, selectedRate);

            // Take this opportunity to try to update contract type of the account
            StampsAccountEntity account = AccountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID);
            UpdateContractType(account);

            return shipments;
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            bool useExpress1 = Express1Utilities.IsPostageSavingService(shipment) &&
                Express1Utilities.IsValidPackagingType((PostalServiceType)shipment.Postal.Service, (PostalPackagingType)shipment.Postal.PackagingType) &&
                ShippingSettings.Fetch().StampsAutomaticExpress1;

            StampsAccountEntity express1Account = StampsAccountManager.GetAccount(ShippingSettings.Fetch().StampsAutomaticExpress1Account);

            if (useExpress1)
            {
                if (express1Account == null)
                {
                    throw new ShippingException("The Express1 account to automatically use when processing with Stamps.com has not been selected.");
                }

                int originalShipmentType = shipment.ShipmentType;
                long? originalStampsAccountID = shipment.Postal.Stamps.OriginalStampsAccountID;
                long stampsAccountID = shipment.Postal.Stamps.StampsAccountID;

                try
                {
                    // Check Stamps.com amount
                    List<RateResult> stampsRates = new StampsApiSession().GetRates(shipment);
                    RateResult stampsRate = stampsRates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                  ((PostalRateSelection)er.OriginalTag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                  && ((PostalRateSelection)er.OriginalTag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

                    // Check Express1 amount
                    shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                    shipment.Postal.Stamps.OriginalStampsAccountID = shipment.Postal.Stamps.StampsAccountID;
                    shipment.Postal.Stamps.StampsAccountID = express1Account.StampsAccountID;

                    List<RateResult> express1Rates = new StampsApiSession().GetRates(shipment);
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
                    shipment.Postal.Stamps.OriginalStampsAccountID = originalStampsAccountID;
                    shipment.Postal.Stamps.StampsAccountID = stampsAccountID;
                }
            }

            // See if this shipment should really go through Express1
            if (useExpress1)
            {
                // Now we turn this into an Express1 shipment...
                shipment.ShipmentType = (int)ShipmentTypeCode.Express1Stamps;
                shipment.Postal.Stamps.OriginalStampsAccountID = shipment.Postal.Stamps.StampsAccountID;
                shipment.Postal.Stamps.StampsAccountID = express1Account.StampsAccountID;

                Express1StampsShipmentType shipmentType = (Express1StampsShipmentType)ShipmentTypeManager.GetType(shipment);

                // Process via Express1
                shipmentType.UpdateDynamicShipmentData(shipment);
                shipmentType.ProcessShipment(shipment);
            }
            else
            {
                // This would be set if they have tried to process as Express1 but it failed... make sure its clear since we are not using it.
                shipment.Postal.Stamps.OriginalStampsAccountID = null;

                try
                {
                    new StampsApiSession().ProcessShipment(shipment);
                }
                catch (StampsException ex)
                {
                    throw new ShippingException(ex.Message, ex);
                }
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
                new StampsApiSession().VoidShipment(shipment);
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
            StampsAccountEntity account = StampsAccountManager.GetAccount(shipment.Postal.Stamps.StampsAccountID);

            ShipmentCommonDetail commonDetail = base.GetShipmentCommonDetail(shipment);
            commonDetail.OriginAccount = (account == null) ? "" : account.Username;

            if (shipment.ShipmentType == (int)ShipmentTypeCode.Express1Stamps && shipment.Postal.Stamps.OriginalStampsAccountID != null)
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

            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment.Postal, "Stamps", typeof(StampsShipmentEntity), refreshIfPresent);
        }
        
        /// <summary>
        /// Configure the properties of a newly created shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
 	        base.ConfigureNewShipment(shipment);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Stamps != null)
            {
                // Use the empty guids for now - they'll get set properly during processing
                shipment.Postal.Stamps.IntegratorTransactionID = Guid.Empty;
                shipment.Postal.Stamps.StampsTransactionID = Guid.Empty;
            }
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            base.LoadProfileData(profile, refreshIfPresent);

            ShipmentTypeDataService.LoadProfileData(profile.Postal, "Stamps", typeof(StampsProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            StampsProfileEntity stamps = profile.Postal.Stamps;

            stamps.StampsAccountID = AccountRepository.Accounts.Any() ? AccountRepository.Accounts.First().StampsAccountID : 0;
            stamps.RequireFullAddressValidation = true;
            stamps.HidePostage = false;
            stamps.Memo = string.Empty;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the Stamps one exists
            if (shipment.Postal.Stamps != null)
            {
                StampsShipmentEntity stampsShipment = shipment.Postal.Stamps;
                StampsProfileEntity stampsProfile = profile.Postal.Stamps;

                ShippingProfileUtility.ApplyProfileValue(stampsProfile.StampsAccountID, stampsShipment, StampsShipmentFields.StampsAccountID);
                ShippingProfileUtility.ApplyProfileValue(stampsProfile.RequireFullAddressValidation, stampsShipment, StampsShipmentFields.RequireFullAddressValidation);
                ShippingProfileUtility.ApplyProfileValue(stampsProfile.HidePostage, stampsShipment, StampsShipmentFields.HidePostage);
                ShippingProfileUtility.ApplyProfileValue(stampsProfile.Memo, stampsShipment, StampsShipmentFields.Memo);
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
            IBestRateShippingBroker counterBroker = base.GetShippingBroker(shipment);
            return counterBroker is NullShippingBroker ? new StampsBestRateBroker(this, AccountRepository) : counterBroker;
        }

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public override void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {
            if (shipment.Postal != null && shipment.Postal.Stamps != null)
            {
                shipment.Postal.Stamps.ScanFormBatchID = null;
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
                    shipment.Postal.Stamps.Fields[StampsShipmentFields.StampsAccountID.FieldIndex],
                    shipment.Postal.Stamps.Fields[StampsShipmentFields.OriginalStampsAccountID.FieldIndex],
                }
            );

            return fields;
        }

        /// <summary>
        /// Uses the Stamps.com API to update the contract type of the account if it is unkown.
        /// </summary>
        /// <param name="account">The account.</param>
        public virtual void UpdateContractType(StampsAccountEntity account)
        {
            // Only update the contract type if it's unknown 
            if (account != null && account.ContractType == (int)StampsAccountContractType.Unknown)
            {
                try
                {
                    // Grab contract type from the Stamps API 
                    StampsApiSession apiSession = new StampsApiSession(AccountRepository, new LogEntryFactory(), CertificateInspector);
                    account.ContractType = (int)apiSession.GetContractType(account);

                    // Save the contract to the DB and push it to Tango
                    AccountRepository.Save(account);

                    ITangoWebClient tangoWebClient = new TangoWebClientFactory().CreateWebClient();
                    tangoWebClient.LogStampsAccount(account);
                }
                catch (Exception exception)
                {
                    // Log the error
                    LogManager.GetLogger(GetType()).Error(string.Format("ShipWorks encountered an error when getting contract type for account {0}.", account.Username), exception);
                }
            }
        }
    }
}

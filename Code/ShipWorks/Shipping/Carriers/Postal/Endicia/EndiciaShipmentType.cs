using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Properties;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// ShipmentType for Endicia Label Server shipments
    /// </summary>
    public class EndiciaShipmentType : PostalShipmentType
    {
        private ICarrierAccountRepository<EndiciaAccountEntity> accountRepository;

        /// <summary>
        /// Endicia ShipmentType code
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Endicia; }
        }

        /// <summary>
        /// Reller of Endicia services.  
        /// </summary>
        public virtual EndiciaReseller EndiciaReseller
        {
            get { return EndiciaReseller.None; }
        }

        /// <summary>
        /// Should Express1 rates be checked when getting Endicia rates?
        /// </summary>
        public bool ShouldRetrieveExpress1Rates { get; set; }

        /// <summary>
        /// Gets or sets the repository that should be used for retrieving accounts
        /// </summary>
        public ICarrierAccountRepository<EndiciaAccountEntity> AccountRepository
        {
            get
            {
                // Default the settings repository to the "live" EndiciaAccountRepository if
                // it hasn't been set already
                return accountRepository ?? (accountRepository = new EndiciaAccountRepository());
            }
            set
            {
                accountRepository = value;
            }
        }

        /// <summary>
        /// Create an EndiciaShipmentType object
        /// </summary>
        public EndiciaShipmentType()
        {
            ShouldRetrieveExpress1Rates = true;
        }

        /// <summary>
        /// Get the service description for the shipment
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            string carrier;
            PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

            if (ShipmentTypeManager.IsEndiciaConsolidator(service))
            {
                return "Consolidator";
            }
            else
            {
                // The shipment is an Endicia shipment, check to see if it's DHL
                if (ShipmentTypeManager.IsEndiciaDhl(service))
                {
                    // The DHL carrier for Endicia is:
                    carrier = "DHL Global Mail";
                }
                else
                {
                    // Use the default carrier for other Endicia types
                    carrier = "USPS";
                }

                return string.Format("{0} {1}", carrier, EnumHelper.GetDescription((PostalServiceType) shipment.Postal.Service));
            }
        }

        /// <summary>
        /// Create the Form used to do the setup for Endicia label server
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new EndiciaSetupWizard();
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public override ServiceControlBase CreateServiceControl(RateControl rateControl)
        {
            return new EndiciaServiceControl(rateControl);
        }

        /// <summary>
        /// Create the UserControl used to handle Stamps.com profiles
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new EndiciaProfileControl(EndiciaReseller);
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new EndiciaSettingsControl(EndiciaReseller);
        }

        /// <summary>
        /// Gets the configured accounts for this Endicia reseller
        /// </summary>
        public virtual List<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.EndiciaAccounts;
            }
        }

        /// <summary>
        /// Endicia supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Endicia accounts can be used as origin addresses
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get { return true; }
        }

        /// <summary>
        /// Endicia supports returns
        /// </summary>
        public override bool SupportsReturns
        {
            get { return true; }
        }

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            base.LoadShipmentData(shipment, refreshIfPresent);

            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment.Postal, "Endicia", typeof(EndiciaShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Configure the properties of a newly created shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            base.ConfigureNewShipment(shipment);

            // We can be called during the creation of the base Postal shipment, before the Endicia one exists
            if (shipment.Postal.Endicia != null)
            {
                // Right now its all in the Profile
            }
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            base.LoadProfileData(profile, refreshIfPresent);

            ShipmentTypeDataService.LoadProfileData(profile.Postal, "Endicia", typeof(EndiciaProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            EndiciaProfileEntity endicia = profile.Postal.Endicia;

            endicia.EndiciaAccountID = Accounts.Count > 0 ? Accounts[0].EndiciaAccountID : 0;
            endicia.StealthPostage = true;
            endicia.NoPostage = false;
            endicia.ReferenceID = "{//Order/Number}";
            endicia.RubberStamp1 = "";
            endicia.RubberStamp2 = "";
            endicia.RubberStamp3 = "";
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the endicia one exists
            if (shipment.Postal.Endicia != null)
            {
                EndiciaShipmentEntity endiciaShipment = shipment.Postal.Endicia;
                EndiciaProfileEntity endiciaProfile = profile.Postal.Endicia;

                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.EndiciaAccountID, endiciaShipment, EndiciaShipmentFields.EndiciaAccountID);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.StealthPostage, endiciaShipment, EndiciaShipmentFields.StealthPostage);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.NoPostage, endiciaShipment, EndiciaShipmentFields.NoPostage);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.ReferenceID, endiciaShipment, EndiciaShipmentFields.ReferenceID);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.RubberStamp1, endiciaShipment, EndiciaShipmentFields.RubberStamp1);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.RubberStamp2, endiciaShipment, EndiciaShipmentFields.RubberStamp2);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.RubberStamp3, endiciaShipment, EndiciaShipmentFields.RubberStamp3);
            }
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

            return base.GetAvailableConfirmationTypes(countryCode, service, packaging);
        }

        /// <summary>
        /// Update shipment data for the shipment to ensure its synced with current settings
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            // Set the provider type based on endicia settings
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia)
            {
                shipment.InsuranceProvider = (int) (EndiciaUtility.IsEndiciaInsuranceActive ? InsuranceProvider.Carrier : InsuranceProvider.ShipWorks);
            }
            else
            {
                // If they had this shipment type as Endicia, set it to Endicia insurance, then switched to Express1... we need to be sure its always ShipWorks in the ex1 case
                shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
            }
        }

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

            // The Endicia object may not yet be set if we are in the middle of creating a new shipment
            if (originID == (int)ShipmentOriginSource.Account && shipment.Postal.Endicia != null)
            {
                EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(shipment.Postal.Endicia.EndiciaAccountID);
                if (account == null)
                {
                    account = Accounts.FirstOrDefault();
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
            string rateHash = GetRatingHash(shipment);

            if (RateCache.Instance.Contains(rateHash))
            {
                return RateCache.Instance.GetValue(rateHash);
            }

            List<RateResult> express1Rates = null;
            ShippingSettingsEntity settings = null;

            // See if this shipment should really go through Express1
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Endicia 
                && Express1Utilities.IsValidPackagingType((PostalServiceType?) null, (PostalPackagingType) shipment.Postal.PackagingType)
                && (settings = ShippingSettings.Fetch()).EndiciaAutomaticExpress1)
            {
                var express1Account = EndiciaAccountManager.GetAccount(settings.EndiciaAutomaticExpress1Account);

                if (express1Account == null)
                {
                    throw new ShippingException("The Express1 account to automatically use when processing with Endicia has not been selected.");
                }
                
                // We temporarily turn this into an Exprss1 shipment to get rated
                shipment.ShipmentType = (int) ShipmentTypeCode.Express1Endicia;
                shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

                try
                {
                    // Currently this actually recurses into this same method
                    express1Rates = (ShouldRetrieveExpress1Rates) ? 
                        ShipmentTypeManager.GetType(shipment).GetRates(shipment).Rates.ToList() : 
                        new List<RateResult>();
                }
                finally
                {
                    shipment.ShipmentType = (int)ShipmentTypeCode.Endicia;
                    shipment.Postal.Endicia.EndiciaAccountID = shipment.Postal.Endicia.OriginalEndiciaAccountID.Value;
                    shipment.Postal.Endicia.OriginalEndiciaAccountID = null;
                }
            }

            try
            {
                EndiciaApiClient endiciaApiClient = new EndiciaApiClient(AccountRepository);

                List<RateResult> endiciaRates = (InterapptiveOnly.MagicKeysDown) ?
                    endiciaApiClient.GetRatesSlow(shipment, this) : 
                    endiciaApiClient.GetRatesFast(shipment, this);

                // For endicia, we want to either promote Express1 or show the Express1 savings
                if (shipment.ShipmentType == (int)ShipmentTypeCode.Endicia)
                {
                    if (ShouldRetrieveExpress1Rates)
                    {
                        List<RateResult> finalRates = new List<RateResult>();

                        bool hasExpress1Savings = false;

                        if (settings == null) settings = ShippingSettings.Fetch();

                        // Go through each Endicia rate
                        foreach (RateResult endiciaRate in endiciaRates)
                        {
                            PostalRateSelection endiciaRateDetail = (PostalRateSelection)endiciaRate.Tag;

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
                                        ((PostalRateSelection)e1r.Tag).ServiceType == endiciaRateDetail.ServiceType && ((PostalRateSelection)e1r.Tag).ConfirmationType == endiciaRateDetail.ConfirmationType);
                                }

                                // If Express1 returned a rate, check to make sure it is a lower amount
                                if (express1Rate != null && express1Rate.Amount <= endiciaRate.Amount)
                                {
                                    finalRates.Add(express1Rate);
                                    hasExpress1Savings = true;
                                }
                                else
                                {
                                    finalRates.Add(endiciaRate);

                                    // Set the express rate to null so that it doesn't add the icon later
                                    express1Rate = null;
                                }

                                RateResult rate = finalRates[finalRates.Count - 1];

                                // If user wanted Express 1 rates
                                if (settings.EndiciaAutomaticExpress1)
                                {
                                    // If they actually got the rate, show the check
                                    if (express1Rate != null)
                                    {
                                        rate.AmountFootnote = Resources.check2;
                                    }
                                }
                                else
                                {
                                    // Endicia rates only.  If it's not a valid Express1 packaging type, don't promote a savings
                                    if (Express1Utilities.IsValidPackagingType(((PostalRateSelection)rate.Tag).ServiceType, (PostalPackagingType)shipment.Postal.PackagingType))
                                    {
                                        rate.AmountFootnote = Resources.star_green;
                                    }
                                }
                            }
                            else
                            {
                                finalRates.Add(endiciaRate);
                            }
                        }

                        RateGroup finalGroup = new RateGroup(finalRates.Select(e => { e.ShipmentType = ShipmentTypeCode.Endicia; return e; }).ToList());

                        if (settings.EndiciaAutomaticExpress1)
                        {
                            if (hasExpress1Savings)
                            {
                                finalGroup.AddFootnoteFactory(new Express1DiscountedRateFootnoteFactory(this, endiciaRates, express1Rates));
                            }
                            else
                            {
                                finalGroup.AddFootnoteFactory(new Express1NotQualifiedRateFootnoteFactory(this));
                            }
                        }
                        else
                        {
                            if (Express1Utilities.IsValidPackagingType(null, (PostalPackagingType)shipment.Postal.PackagingType))
                            {
                                IExpress1SettingsFacade express1Settings = new EndiciaExpress1SettingsFacade(ShippingSettings.Fetch());
                                finalGroup.AddFootnoteFactory(new Express1PromotionRateFootnoteFactory(this, express1Settings));
                            }
                        }

                        RateCache.Instance.Save(rateHash, finalGroup);
                        return finalGroup;
                    }
                    else
                    {
                        RateGroup rateGroup = BuildExpress1RateGroup(endiciaRates, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Endicia);
                        RateCache.Instance.Save(rateHash, rateGroup);

                        return rateGroup;
                    }

                }
                else
                {
                    // Express1 rates - return rates filtered by what is available to the user
                    RateGroup rateGroup = BuildExpress1RateGroup(endiciaRates, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Endicia);
                    RateCache.Instance.Save(rateHash, rateGroup);

                    return rateGroup;
                }
            }
            catch (EndiciaException ex)
            {
                // This is a bad configuration on some level, so cache an empty rate group
                // before throwing throwing the exceptions
                ShippingException shippingException = new ShippingException(ex.Message, ex);
                CacheInvalidRateGroup(shipment, shippingException);

                throw shippingException;
            }
        }

        /// <summary>
        /// Process the label server shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            bool useExpress1 = Express1Utilities.IsPostageSavingService(shipment) &&
                Express1Utilities.IsValidPackagingType((PostalServiceType)shipment.Postal.Service, (PostalPackagingType)shipment.Postal.PackagingType) &&
                ShippingSettings.Fetch().EndiciaAutomaticExpress1;

            EndiciaAccountEntity express1Account = EndiciaAccountManager.GetAccount(ShippingSettings.Fetch().EndiciaAutomaticExpress1Account);

            EndiciaApiClient endiciaApiClient = new EndiciaApiClient(AccountRepository);

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
                    List<RateResult> endiciaRates = endiciaApiClient.GetRatesFast(shipment, this);
                    RateResult endiciaRate = endiciaRates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                    ((PostalRateSelection)er.Tag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                    && ((PostalRateSelection)er.Tag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

                    // Check Express1 amount
                    shipment.ShipmentType = (int)ShipmentTypeCode.Express1Endicia;
                    shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                    shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

                    List<RateResult> express1Rates = endiciaApiClient.GetRatesFast(shipment, this);
                    RateResult express1Rate = express1Rates.Where(er => er.Selectable).FirstOrDefault(er =>
                                                                                                      ((PostalRateSelection)er.Tag).ServiceType == (PostalServiceType)shipment.Postal.Service
                                                                                                      && ((PostalRateSelection)er.Tag).ConfirmationType == (PostalConfirmationType)shipment.Postal.Confirmation);

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
                shipment.ShipmentType = (int) ShipmentTypeCode.Express1Endicia;
                shipment.Postal.Endicia.OriginalEndiciaAccountID = shipment.Postal.Endicia.EndiciaAccountID;
                shipment.Postal.Endicia.EndiciaAccountID = express1Account.EndiciaAccountID;

                Express1EndiciaShipmentType shipmentType = (Express1EndiciaShipmentType) ShipmentTypeManager.GetType(shipment);

                // Process via Express1
                shipmentType.UpdateDynamicShipmentData(shipment);
                shipmentType.ProcessShipment(shipment);
            }
            else
            {
                // This would be set if they have tried to process as Express1 but it failed... make sure its clear since we are not using it.
                shipment.Postal.Endicia.OriginalEndiciaAccountID = null;

                try
                {
                    endiciaApiClient.ProcessShipment(shipment, this);
                }
                catch (EndiciaException ex)
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
        }

        /// <summary>
        /// Void the given endicia shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
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
        /// Get the USPS shipment details
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(shipment.Postal.Endicia.EndiciaAccountID);

            ShipmentCommonDetail commonDetail = base.GetShipmentCommonDetail(shipment);

            commonDetail.OriginAccount = (account == null) ? "" : account.AccountNumber;

            if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Endicia && shipment.Postal.Endicia.OriginalEndiciaAccountID != null)
            {
                commonDetail.OriginalShipmentType = ShipmentTypeCode.Endicia;
            }

            return commonDetail;
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

            // Legacy stuff
            ElementOutline outline = container.AddElement("USPS", ElementOutline.If(() => shipment().Processed));
            outline.AddAttributeLegacy2x();
            outline.AddElement("CustomsNumber", () => shipment().TrackingNumber, ElementOutline.If(() => !PostalUtility.IsDomesticCountry(shipment().ShipCountryCode)));
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
                foreach (DataResourceReference documentResource in labelResources)
                {
                    labelData.Add(new TemplateLabelData(null, documentResource.Label, TemplateLabelCategory.Supplemental, documentResource));
                }

                // Supporting documents
                var customsResources = resources.Where(r => r.Label.StartsWith("Customs"));
                foreach (DataResourceReference customsResource in customsResources)
                {
                    labelData.Add(new TemplateLabelData(null, customsResource.Label, TemplateLabelCategory.Supplemental, customsResource));
                }
            }

            return labelData;
        }

        /// <summary>
        /// Get the endicia MailClass code for the given service
        /// </summary>
        public virtual string GetMailClassCode(PostalServiceType serviceType, PostalPackagingType packagingType)
        {
            switch (serviceType)
            {
                case PostalServiceType.ExpressMail: return "PriorityExpress";
                case PostalServiceType.FirstClass: return "First";
                case PostalServiceType.LibraryMail: return "LibraryMail";
                case PostalServiceType.MediaMail: return "MediaMail";
                case PostalServiceType.StandardPost: return "StandardPost";
                case PostalServiceType.ParcelSelect: return "ParcelSelect";
                case PostalServiceType.PriorityMail: return "Priority";
                case PostalServiceType.CriticalMail: return "CriticalMail";

                case PostalServiceType.InternationalExpress: return "PriorityMailExpressInternational";
                case PostalServiceType.InternationalPriority: return "PriorityMailInternational";

                case PostalServiceType.InternationalFirst:
                    {
                        return PostalUtility.IsEnvelopeOrFlat(packagingType) ? "FirstClassMailInternational" : "FirstClassPackageInternationalService";
                    }
            }

            if (ShipmentTypeManager.IsEndiciaDhl(serviceType) || ShipmentTypeManager.IsEndiciaConsolidator(serviceType))
            {
                return EnumHelper.GetApiValue(serviceType);
            }

            throw new EndiciaException(string.Format("{0} is not supported when shipping with Endicia.", PostalUtility.GetPostalServiceTypeDescription(serviceType)));
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for Endicia based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an EndiciaBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            IBestRateShippingBroker counterBroker = base.GetShippingBroker(shipment);
            return counterBroker is NullShippingBroker ? new EndiciaBestRateBroker() : counterBroker;
        }

        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override IEnumerable<IEntityField2> GetRatingFields(ShipmentEntity shipment)
        {
            List<IEntityField2> fields = new List<IEntityField2>(base.GetRatingFields(shipment));

            fields.AddRange
            (
                new List<IEntityField2>()
                {
                    shipment.Postal.Endicia.Fields[EndiciaShipmentFields.EndiciaAccountID.FieldIndex],
                    shipment.Postal.Endicia.Fields[EndiciaShipmentFields.OriginalEndiciaAccountID.FieldIndex],
                }
            );

            return fields;
        }
    }
}

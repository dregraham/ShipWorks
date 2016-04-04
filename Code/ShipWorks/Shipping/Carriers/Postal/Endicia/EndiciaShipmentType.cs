using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Endicia;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
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
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Endicia;

        /// <summary>
        /// Reseller of Endicia services.
        /// </summary>
        public virtual EndiciaReseller EndiciaReseller => EndiciaReseller.None;

        /// <summary>
        /// Should Express1 rates be checked when getting Endicia rates?
        /// </summary>
        public bool ShouldRetrieveExpress1Rates { get; set; }

        /// <summary>
        /// Gets or sets the log entry factory.
        /// </summary>
        public LogEntryFactory LogEntryFactory { get; set; }

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
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts => Accounts.Any();

        /// <summary>
        /// Create an EndiciaShipmentType object
        /// </summary>
        public EndiciaShipmentType()
        {
            ShouldRetrieveExpress1Rates = true;
            LogEntryFactory = new LogEntryFactory();
        }

        /// <summary>
        /// Get the service description for the shipment
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            string carrier;
            PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

            if (ShipmentTypeManager.IsConsolidator(service))
            {
                return "Consolidator";
            }
            // The shipment is an Endicia shipment, check to see if it's DHL
            carrier = ShipmentTypeManager.IsEndiciaDhl(service) ? "DHL Global Mail" : "USPS";

            return $"{carrier} {EnumHelper.GetDescription((PostalServiceType) shipment.Postal.Service)}";
        }

        /// <summary>
        /// Create the Form used to do the setup for Endicia label server
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            return new EndiciaSetupWizard();
        }

        /// <summary>
        /// Create the UserControl used to handle Endicia shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new EndiciaServiceControl(rateControl);
        }

        /// <summary>
        /// Create the UserControl used to handle Endicia profiles
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl()
        {
            return new EndiciaProfileControl(EndiciaReseller);
        }

        /// <summary>
        /// Create the settings control for Endicia
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl()
        {
            EndiciaSettingsControl settingsControl = new EndiciaSettingsControl(EndiciaReseller);
            settingsControl.Initialize(ShipmentTypeCode);

            return settingsControl;
        }

        /// <summary>
        /// Gets the configured accounts for this Endicia reseller
        /// </summary>
        public virtual List<EndiciaAccountEntity> Accounts => AccountRepository.Accounts.ToList();

        /// <summary>
        /// Endicia supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => false;

        /// <summary>
        /// Endicia accounts can be used as origin addresses
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Endicia supports returns
        /// </summary>
        public override bool SupportsReturns => true;

        /// <summary>
        /// Ensures that the USPS specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            base.LoadShipmentData(shipment, refreshIfPresent);

            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment.Postal, "Endicia", typeof(EndiciaShipmentEntity), refreshIfPresent);
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
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            EndiciaProfileEntity endicia = profile.Postal.Endicia;

            endicia.EndiciaAccountID = Accounts.Count > 0 ? Accounts[0].EndiciaAccountID : 0;
            endicia.StealthPostage = true;
            endicia.ReferenceID = "{//Order/Number}";
            endicia.ScanBasedReturn = false;
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
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.ReferenceID, endiciaShipment, EndiciaShipmentFields.ReferenceID);
                ShippingProfileUtility.ApplyProfileValue(endiciaProfile.ScanBasedReturn, endiciaShipment, EndiciaShipmentFields.ScanBasedReturn);
            }
        }

        /// <summary>
        /// Determines if delivery\signature confirmation is available for the given service
        /// </summary>
        public override List<PostalConfirmationType> GetAvailableConfirmationTypes(string countryCode, PostalServiceType service, PostalPackagingType? packaging)
        {
            List<PostalConfirmationType> availablePostalConfirmationTypes = new List<PostalConfirmationType>();

            // If we don't know the packaging or country, it doesn't matter
            if (!string.IsNullOrWhiteSpace(countryCode) && packaging != null)
            {
                if (IsFreeInternationalDeliveryConfirmation(countryCode, service, packaging.Value))
                {
                    availablePostalConfirmationTypes.Add(PostalConfirmationType.Delivery);
                    return availablePostalConfirmationTypes;
                }
            }

            availablePostalConfirmationTypes = base.GetAvailableConfirmationTypes(countryCode, service, packaging);

            return availablePostalConfirmationTypes;
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

            if (shipment.Postal != null && shipment.Postal.Endicia != null)
            {
                shipment.RequestedLabelFormat = shipment.Postal.Endicia.RequestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the origin address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public override bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {

            // A null reference error was being thrown.  Discovered by Crash Reports.
            // Let's figure out what is null....
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Processed)
            {
                return true;
            }

            // The Endicia or Postal object may not yet be set if we are in the middle of creating a new shipment
            if (originID == (int)ShipmentOriginSource.Account && shipment.Postal != null && shipment.Postal.Endicia != null)
            {
                EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(shipment.Postal.Endicia.EndiciaAccountID);
                if (account == null)
                {
                    if (Accounts == null)
                    {
                        throw new NullReferenceException("Account cannot be null.");
                    }

                    account = Accounts.FirstOrDefault();
                }

                if (account != null)
                {
                    PersonAdapter.Copy(account, "", person);
                    return true;
                }
                return false;
            }

            return base.UpdatePersonAddress(shipment, person, originID);
        }

        /// <summary>
        /// Checks to see if the shipment allows scan based payment returns
        /// </summary>
        public static bool IsScanBasedReturnsAllowed(ShipmentEntity shipment)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.EndiciaScanBasedReturns, null);

                return shipment.ReturnShipment &&
                       shipment.Postal.Endicia.ScanBasedReturn &&
                       shipment.ShipmentType == (int) ShipmentTypeCode.Endicia &&
                       restrictionLevel == EditionRestrictionLevel.None;
            }
        }

        /// <summary>
        /// Validate that scan based payment returns is allowed
        /// </summary>
        public void ValidateScanBasedReturns(ShipmentEntity shipment)
        {
            if (IsScanBasedReturnsAllowed(shipment))
            {
                if (!IsDomestic(shipment))
                {
                    throw new ShippingException("Endicia scan based payment returns are only available for domestic shipments.");
                }

                PostalServiceType postalServiceType = (PostalServiceType)shipment.Postal.Service;
                PostalConfirmationType postalConfirmationType = (PostalConfirmationType)shipment.Postal.Confirmation;

                switch (postalServiceType)
                {
                    case PostalServiceType.ParcelSelect:
                        if (postalConfirmationType != PostalConfirmationType.None)
                        {
                            throw new ShippingException("Endicia scan based payment returns are only available for Parcel Select with No Confirmation shipments.");
                        }
                        break;
                    case PostalServiceType.FirstClass:
                        if (postalConfirmationType == PostalConfirmationType.None)
                        {
                            throw new ShippingException("Endicia scan based payment returns are not available for First Class with no Confirmation shipments.");
                        }
                        break;
                    case PostalServiceType.PriorityMail:
                        if (postalConfirmationType == PostalConfirmationType.None)
                        {
                            throw new ShippingException("Endicia scan based payment returns are not available for Priority Mail with no Confirmation shipments.");
                        }
                        break;
                    case PostalServiceType.ExpressMail:
                        // nothing to check
                        break;
                    default:
                        string errorMessage =
                            string.Format("Endicia scan based payment returns are only available for {0}, {1}, {2}, and {3} services.",
                                EnumHelper.GetDescription(PostalServiceType.ParcelSelect),
                                EnumHelper.GetDescription(PostalServiceType.FirstClass),
                                EnumHelper.GetDescription(PostalServiceType.PriorityMail),
                                EnumHelper.GetDescription(PostalServiceType.ExpressMail));

                        throw new ShippingException(errorMessage);
                }

                if (shipment.Insurance)
                {
                    throw new ShippingException("Endicia scan based payment returns are not available for insured shipments.");
                }
            }
        }
        
        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new EndiciaShipmentProcessingSynchronizer();
        }

        /// <summary>
        /// Validate the shipment before processing or rating
        /// </summary>
        public void ValidateShipment(ShipmentEntity shipment)
        {
            if (shipment.TotalWeight == 0)
            {
                throw new ShippingException("The shipment weight cannot be zero.");
            }

            // Validate that scan based payment returns is allowed.
            // This method throws if not allowed.
            ValidateScanBasedReturns(shipment);
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
            outline.AddElement("CustomsNumber", () => shipment().TrackingNumber, ElementOutline.If(() => !shipment().ShipPerson.IsDomesticCountry()));
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

            if (ShipmentTypeManager.IsEndiciaDhl(serviceType) || ShipmentTypeManager.IsConsolidator(serviceType))
            {
                return EnumHelper.GetApiValue(serviceType);
            }

            throw new EndiciaException($"{PostalUtility.GetPostalServiceTypeDescription(serviceType)} is not supported when shipping with Endicia.");
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for Endicia based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an EndiciaBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Returns the Endicia Returns Control
        /// </summary>
        public override ReturnsControlBase CreateReturnsControl()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.EndiciaScanBasedReturns, null);

            // If scan based returns is not allowed, show the the default returns control
                if (restrictionLevel != EditionRestrictionLevel.None)
                {
                    return base.CreateReturnsControl();
                }

                return new EndiciaReturnsControl();
            }
        }

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public override void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {
            if (shipment.Postal != null && shipment.Postal.Endicia != null)
            {
                shipment.Postal.Endicia.TransactionID = null;
                shipment.Postal.Endicia.RefundFormID = null;
                shipment.Postal.Endicia.ScanFormBatchID = null;
            }
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.Postal != null && shipment.Postal.Endicia != null)
            {
                shipment.Postal.Endicia.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.PostalShipmentEntityUsingShipmentID);
            bucket.Relations.Add(PostalShipmentEntity.Relations.EndiciaShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new EndiciaShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }
    }
}

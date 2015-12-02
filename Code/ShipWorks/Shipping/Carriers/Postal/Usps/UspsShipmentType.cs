using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A shipment type for the USPS shipment type in ShipWorks. 
    /// </summary>
    public class UspsShipmentType : PostalShipmentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsShipmentType"/> class.
        /// </summary>
        public UspsShipmentType()
        {
            ShouldRetrieveExpress1Rates = true;

            // Use the "live" versions by default
            AccountRepository = new UspsAccountRepository();
            LogEntryFactory = new LogEntryFactory();
        }

        /// <summary>
        /// Gets or sets the repository that should be used when retrieving account information.
        /// </summary>
        public ICarrierAccountRepository<UspsAccountEntity> AccountRepository { get; set; }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts => AccountRepository.Accounts.Any();

        /// <summary>
        /// Gets or sets the log entry factory.
        /// </summary>
        public LogEntryFactory LogEntryFactory { get; set; }


        /// <summary>
        /// Indicates if the shipment service type supports return shipments
        /// </summary>
        public override bool SupportsReturns => true;

        /// <summary>
        /// Gets the type of the reseller.
        /// </summary>
        public virtual UspsResellerType ResellerType => UspsResellerType.None;

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Usps;

        /// <summary>
        /// USPS supports getting postal service rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => true;

        /// <summary>
        /// Indicates if the shipment type supports accounts as the origin
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Should Express1 rates be checked when getting Endicia rates?
        /// </summary>
        public bool ShouldRetrieveExpress1Rates { get; set; }        

        /// <summary>
        /// Creates the web client to use to contact the underlying carrier API.
        /// </summary>
        /// <returns>An instance of IUspsWebClient. </returns>
        public virtual IUspsWebClient CreateWebClient()
        {
            // This needs to be created each time rather than just being an instance property, 
            // because of counter rates where the account repository is swapped out out prior 
            // to creating the web client.
            return new UspsWebClient(AccountRepository, LogEntryFactory, CertificateInspector, ResellerType);
        }

        /// <summary>
        /// Create the settings control for USPS
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl()
        {
            UspsSettingsControl control = new UspsSettingsControl();
            control.Initialize(ShipmentTypeCode);

            return control;
        }

        /// <summary>
        /// Creates the USPS service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new UspsServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Create the Form used to do the setup for the USPS API
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            EnsureAccountsHaveCurrentContractData();

            IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            return new UspsSetupWizard(promotion, true);
        }

        /// <summary>
        /// Create the UserControl used to handle USPS profiles
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl()
        {
            return new UspsProfileControl();
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

            // The USPS or Postal objects may not yet be set if we are in the middle of creating a new shipment
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
        /// Ensure that all USPS accounts have up to date contract information
        /// </summary>
        private void EnsureAccountsHaveCurrentContractData()
        {
            Task[] tasks = AccountRepository.Accounts
                .Select(account => Task.Factory.StartNew(() => UpdateContractType(account)))
                .ToArray();

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Get the Express1 account that should be used for auto routing, or null if we should not auto route
        /// </summary>
        public static UspsAccountEntity GetExpress1AutoRouteAccount(PostalPackagingType packagingType)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            bool isExpress1Restricted = ShipmentTypeManager.GetType(ShipmentTypeCode.Express1Usps).IsShipmentTypeRestricted;
            bool shouldUseExpress1 = settings.UspsAutomaticExpress1 && !isExpress1Restricted &&
                                     Express1Utilities.IsValidPackagingType(null, packagingType);

            return shouldUseExpress1 ? UspsAccountManager.GetAccount(settings.UspsAutomaticExpress1Account) : null;
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            return new UspsShipmentProcessingSynchronizer(AccountRepository);
        }
        
        /// <summary>
        /// Should we rate shop before processing
        /// </summary>
        public bool ShouldRateShop(ShipmentEntity shipment)
        {
            return shipment.Postal.Usps.RateShop && 
                AccountRepository.Accounts.Count() > 1;
        }

        /// <summary>
        /// Get the Express1 account that should be used for auto routing.
        /// Returns null if auto routing should not be used.
        /// </summary>
        public bool ShouldTestExpress1Rates(ShipmentEntity shipment)
        {
            return Express1Utilities.IsPostageSavingService((PostalServiceType)shipment.Postal.Service) &&
                GetExpress1AutoRouteAccount((PostalPackagingType)shipment.Postal.PackagingType) != null;
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

            if (shipment.Postal.Service == (int)PostalServiceType.ExpressMail && 
                shipment.Postal.Confirmation != (int)PostalConfirmationType.None &&
                shipment.Postal.Confirmation != (int)PostalConfirmationType.AdultSignatureRestricted &&
                shipment.Postal.Confirmation != (int)PostalConfirmationType.AdultSignatureRequired)
            {
                throw new ShippingException("A confirmation option cannot be used with Express mail.");
            }
        }

        /// <summary>
        /// Get the USPS shipment details
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            UspsAccountEntity account = UspsAccountManager.GetAccount(shipment.Postal.Usps.UspsAccountID);

            ShipmentCommonDetail commonDetail = base.GetShipmentCommonDetail(shipment);
            commonDetail.OriginAccount = (account == null) ? "" : account.Username;

            if (shipment.ShipmentType == (int)ShipmentTypeCode.Express1Usps && shipment.Postal.Usps.OriginalUspsAccountID != null)
            {
                commonDetail.OriginalShipmentType = ShipmentTypeCode.Usps;
            }

            return commonDetail;
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

            shipment.InsuranceProvider = (int)(UspsUtility.IsStampsInsuranceActive ? InsuranceProvider.Carrier : InsuranceProvider.ShipWorks);
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
            // We can be called during the creation of the base Postal shipment, before the USPS one exists
            if (shipment.Postal.Usps != null)
            {
                // Use the empty guids for now - they'll get set properly during processing
                shipment.Postal.Usps.IntegratorTransactionID = Guid.Empty;
                shipment.Postal.Usps.UspsTransactionID = Guid.Empty;
                shipment.Postal.Usps.RequestedLabelFormat = (int)ThermalLanguage.None;
                shipment.Postal.Usps.RateShop = false;
            }

            // We need to call the base after setting up the USPS specific information because LLBLgen was
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
        /// Gets an instance to the best rate shipping broker for the USPS         /// shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a UspsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            if (AccountRepository.Accounts.Any())
            {
                // We have an account, so use the normal broker
                return new UspsBestRateBroker(this, AccountRepository);
            }
            
            // No accounts, so use the counter rates broker to allow the user to
            // sign up for the account. We can use the UspsCounterRateAccountRepository 
            // here because the underlying accounts being used are the same.
            return new UspsCounterRatesBroker(new UspsCounterRateAccountRepository(TangoCredentialStore.Instance));
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

            profile.Postal.Usps.RateShop = true;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            // We can be called during the creation of the base Postal shipment, before the USPS one exists
            if (shipment.Postal.Usps != null)
            {
                UspsShipmentEntity uspsShipment = shipment.Postal.Usps;
                UspsProfileEntity uspsProfile = profile.Postal.Usps;

                ShippingProfileUtility.ApplyProfileValue(uspsProfile.UspsAccountID, uspsShipment, UspsShipmentFields.UspsAccountID);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.RequireFullAddressValidation, uspsShipment, UspsShipmentFields.RequireFullAddressValidation);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.HidePostage, uspsShipment, UspsShipmentFields.HidePostage);
                ShippingProfileUtility.ApplyProfileValue(uspsProfile.RateShop, uspsShipment, UspsShipmentFields.RateShop);
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
                if (IsFreeInternationalDeliveryConfirmation(countryCode, service, packaging.Value))
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
        /// Update the shipment to use the specified account
        /// </summary>
        public void UseAccountForShipment(UspsAccountEntity account, ShipmentEntity shipment)
        {
            shipment.Postal.Usps.UspsAccountID = account.UspsAccountID;

            if (shipment.OriginOriginID == (int)ShipmentOriginSource.Account)
            {
                PersonAdapter.Copy(account, string.Empty, shipment, "Origin");
            }
        }

        /// <summary>
        /// Uses the USPS API to update the contract type of the account if it is unkown.
        /// </summary>
        /// <param name="account">The account.</param>
        public virtual void UpdateContractType(UspsAccountEntity account)
        {
            if (account != null)
            {
                // We want to update the contract if it's not in the cache (or dropped out) or if the contract type is unknown; the cache is used
                // so we don't have to perform this everytime, but does allow ShipWorks to handle cases where the contract type may have been
                // updated outside of ShipWorks.
                if (!UspsContractTypeCache.Contains(account.UspsAccountID) || UspsContractTypeCache.GetContractType(account.UspsAccountID) == UspsAccountContractType.Unknown)
                {
                    try
                    {
                        // Grab contract type from the USPS API 
                        IUspsWebClient webClient = CreateWebClient();
                        UspsAccountContractType contractType = webClient.GetContractType(account);

                        bool hasContractChanged = account.ContractType != (int)contractType;
                        account.ContractType = (int)contractType;

                        // Save the contract to the DB and update the cache
                        AccountRepository.Save(account);
                        UspsContractTypeCache.Set(account.UspsAccountID, (UspsAccountContractType)account.ContractType);

                        if (hasContractChanged)
                        {
                            // Any cached rates are probably invalid now
                            RateCache.Instance.Clear();

                            // Only notify Tango of changes so it has the latest information (and cuts down on traffic)
                            ITangoWebClient tangoWebClient = new TangoWebClientFactory().CreateWebClient();
                            tangoWebClient.LogUspsAccount(account);
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
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.PostalShipmentEntityUsingShipmentID);
            bucket.Relations.Add(PostalShipmentEntity.Relations.UspsShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new UspsShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Returns a list of countries eligible for free international delivery confirmation.
        /// </summary>
        /// <returns></returns>
        protected override List<string> CountriesEligibleForFreeInternationalDeliveryConfirmation()
        {
            return base.CountriesEligibleForFreeInternationalDeliveryConfirmation().Union(new[] { "MX", "PL" }).ToList();
        }
    }
}

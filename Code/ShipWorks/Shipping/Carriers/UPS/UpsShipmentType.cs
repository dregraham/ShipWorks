﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Tracking;
using ShipWorks.UI;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// ShipmentType implementation for UPS
    /// </summary>
    public abstract class UpsShipmentType : ShipmentType
    {

        /// <summary>
        /// Constructor
        /// </summary>
        protected UpsShipmentType()
        {
            // Use the "live" versions of the repository by default
            AccountRepository = new UpsAccountRepository();
            SettingsRepository = new UpsSettingsRepository();
        }

        /// <summary>
        /// UPS supports getting rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// UPS accounts have an address that can be used as the shipment origin
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Gets or sets the account repository that the shipment type should use
        /// to obtain Ups related account information. This provides
        /// the ability to use different UPS account info depending on how the shipment
        /// type is going to be used. For example, to obtain counter rates with a
        /// generic UPS account intended to be used with ShipWorks, all that would
        /// have to be done is to assign this property with a repository that contains
        /// the appropriate account information for getting counter rates.
        /// </summary>
        public ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> AccountRepository { get; set; }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts => AccountRepository.AccountsReadOnly.Any();

        /// <summary>
        /// Gets or sets the settings repository that the shipment type should use
        /// to obtain Ups related settings information. This provides
        /// the ability to use different UPS settings depending on how the shipment
        /// type is going to be used. For example, to obtain counter rates with a
        /// generic UPS account intended to be used with ShipWorks, all that would
        /// have to be done is to assign this property with a repository that contains
        /// the appropriate account information for getting counter rates.
        /// </summary>
        public ICarrierSettingsRepository SettingsRepository { get; set; }

        /// <summary>
        /// UPS always uses the residential indicator
        /// </summary>
        public override bool IsResidentialStatusRequired(IShipmentEntity shipment) => true;

        /// <summary>
        /// Create the user control used to edit UPS shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new UpsServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Create the UPS specific customs control
        /// </summary>
        public override CustomsControlBase CreateCustomsControl()
        {
            return new UpsCustomsControl();
        }

        /// <summary>
        /// Create the UPS specific table rows
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "Ups", typeof(UpsShipmentEntity), refreshIfPresent);

            // This will exist now
            UpsShipmentEntity ups = shipment.Ups;

            // If refreshing, start over on the packages
            if (refreshIfPresent)
            {
                ups.Packages.Clear();
            }

            // If there are no packages load them now
            if (ups.Packages.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(ups.Packages, new RelationPredicateBucket(UpsPackageFields.ShipmentID == shipment.ShipmentID));
                    ups.Packages.Sort((int) UpsPackageFieldIndex.UpsPackageID, ListSortDirection.Ascending);
                }

                // We reloaded the packages, so reset the tracker
                ups.Packages.RemovedEntitiesTracker = new UpsPackageCollection();
            }

            // There has to be at least one package.  Really the only way there would not already be a package is if this is a new shipment,
            // and the default profile set included no package stuff.
            if (ups.Packages.Count == 0)
            {
                // This was changed to an exception instead of creating the package when the creation was moved to ConfigureNewShipment
                throw new NotFoundException("Primary package not found.");
            }
        }

        /// <summary>
        /// Configure data for the newly created shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Order, nameof(shipment.Order));

            if (shipment.Ups == null)
            {
                shipment.Ups = new UpsShipmentEntity(shipment.ShipmentID);
            }

            shipment.Ups.CodEnabled = false;
            shipment.Ups.CodAmount = shipment.Order.OrderTotal;
            shipment.Ups.CodPaymentType = (int) UpsCodPaymentType.Cash;

            shipment.Ups.CustomsDocumentsOnly = false;
            shipment.Ups.CustomsDescription = string.Empty;
            shipment.Ups.CustomsRecipientTIN = string.Empty;
            shipment.Ups.CustomsRecipientTINType = (int) UpsCustomsRecipientTINType.IOSS;
            shipment.Ups.CustomsRecipientType = (int) UpsCustomsRecipientType.Business;

            shipment.Ups.CommercialPaperlessInvoice = false;
            shipment.Ups.CommercialInvoiceTermsOfSale = (int) UpsTermsOfSale.NotSpecified;
            shipment.Ups.CommercialInvoicePurpose = (int) UpsExportReason.Sale;
            shipment.Ups.CommercialInvoiceComments = "";
            shipment.Ups.CommercialInvoiceFreight = 0;
            shipment.Ups.CommercialInvoiceInsurance = 0;
            shipment.Ups.CommercialInvoiceOther = 0;

            shipment.Ups.WorldShipStatus = (int) WorldShipStatusType.None;

            shipment.Ups.NegotiatedRate = false;
            shipment.Ups.PublishedCharges = 0;

            shipment.Ups.UspsTrackingNumber = "";

            shipment.Ups.Endorsement = 0;
            shipment.Ups.Subclassification = 0;

            shipment.Ups.PaperlessAdditionalDocumentation = false;
            shipment.Ups.ShipperRelease = false;
            shipment.Ups.CarbonNeutral = false;

            shipment.Ups.UspsPackageID = string.Empty;
            shipment.Ups.CostCenter = string.Empty;
            shipment.Ups.IrregularIndicator = (int) UpsIrregularIndicatorType.NotApplicable;
            shipment.Ups.Cn22Number = string.Empty;

            shipment.Ups.ShipmentChargeAccount = string.Empty;
            shipment.Ups.ShipmentChargeCountryCode = string.Empty;
            shipment.Ups.ShipmentChargePostalCode = string.Empty;
            shipment.Ups.ShipmentChargeType = (int) UpsShipmentChargeType.BillReceiver;

            UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
            shipment.Ups.Packages.Add(package);
            shipment.Ups.Packages.RemovedEntitiesTracker = new UpsPackageCollection();

            // Weight of the first package equals the total shipment content weight
            package.Weight = shipment.ContentWeight;

            shipment.Ups.RequestedLabelFormat = (int) ThermalLanguage.None;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Configures the shipment for ShipSense. This is useful for carriers that support
        /// multiple package shipments, allowing the shipment type a chance to add new packages
        /// to coincide with the ShipSense knowledge base entry.
        /// </summary>
        /// <param name="knowledgebaseEntry">The knowledge base entry.</param>
        /// <param name="shipment">The shipment.</param>
        protected override void SyncNewShipmentWithShipSense(ShipSense.KnowledgebaseEntry knowledgebaseEntry, ShipmentEntity shipment)
        {
            if (shipment.Ups.Packages.RemovedEntitiesTracker == null)
            {
                shipment.Ups.Packages.RemovedEntitiesTracker = new UpsPackageCollection();
            }

            base.SyncNewShipmentWithShipSense(knowledgebaseEntry, shipment);

            while (shipment.Ups.Packages.Count < knowledgebaseEntry.Packages.Count())
            {
                UpsPackageEntity package = UpsUtility.CreateDefaultPackage();
                shipment.Ups.Packages.Add(package);
            }

            while (shipment.Ups.Packages.Count > knowledgebaseEntry.Packages.Count())
            {
                // Remove the last package until the packages counts match
                shipment.Ups.Packages.RemoveAt(shipment.Ups.Packages.Count - 1);
            }
        }

        /// <summary>
        /// Gets the service types that have been available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalServiceType values for a UspsShipmentType)
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            IEnumerable<int> allServiceTypes = Enum.GetValues(typeof(UpsServiceType)).Cast<int>();
            return allServiceTypes.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Gets the package types that have been available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to UpsPackagingType values for a UpsShipmentType)
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            IEnumerable<int> allPackageTypes = Enum.GetValues(typeof(UpsPackagingType)).Cast<int>();
            return allPackageTypes.Except(GetExcludedPackageTypes(repository));
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public override Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments, IExcludedPackageTypeRepository excludedServiceTypeRepository)
        {
            // Get valid packaging types
            List<int> validPackageTypes = UpsUtility.GetValidPackagingTypes(ShipmentTypeCode).Select(x => (int) x).ToList();
            IEnumerable<int> excludedPackageTypes = ShipmentTypeManager.GetType(ShipmentTypeCode).GetExcludedPackageTypes();

            // If there's an existing shipment with a package type that has been excluded, we need to re-add it here
            if (shipments != null && shipments.Any())
            {
                IEnumerable<int> neededPackageTypes = shipments.SelectMany(s => s.Ups.Packages.Select(p => p.PackagingType)).Distinct().ToList();
                excludedPackageTypes = excludedPackageTypes.Except(neededPackageTypes);
                validPackageTypes.AddRange(neededPackageTypes);
            }

            return validPackageTypes.Except(excludedPackageTypes)
                .ToDictionary(t => t, t => EnumHelper.GetDescription((UpsPackagingType) t));
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = UpsAccountManager.Accounts.Count > 0 ? UpsAccountManager.Accounts[0].UpsAccountID : (long) 0;

            profile.Ups.UpsAccountID = shipperID;
            profile.OriginID = (int) ShipmentOriginSource.Account;
            profile.Ups.ResidentialDetermination = (int) ResidentialDeterminationType.FromAddressValidation;

            profile.Ups.DeliveryConfirmation = (int) UpsDeliveryConfirmationType.None;
            profile.Ups.ReferenceNumber = "Order {//Order/Number}";
            profile.Ups.ReferenceNumber2 = "";

            profile.Ups.Service = (int) UpsServiceType.UpsGround;
            profile.Ups.SaturdayDelivery = false;

            profile.Ups.PayorType = (int) UpsPayorType.Sender;
            profile.Ups.PayorAccount = "";
            profile.Ups.PayorPostalCode = "";
            profile.Ups.PayorCountryCode = GetDefaultCountry();

            profile.Ups.EmailNotifySender = (int) UpsEmailNotificationType.None;
            profile.Ups.EmailNotifyRecipient = (int) UpsEmailNotificationType.None;
            profile.Ups.EmailNotifyOther = (int) UpsEmailNotificationType.None;
            profile.Ups.EmailNotifyOtherAddress = "";
            profile.Ups.EmailNotifyFrom = "";
            profile.Ups.EmailNotifySubject = (int) UpsEmailNotificationSubject.TrackingNumber;
            profile.Ups.EmailNotifyMessage = "";

            profile.Ups.ReturnService = (int) UpsReturnServiceType.ElectronicReturnLabel;
            profile.Ups.ReturnContents = "";
            profile.Ups.ReturnUndeliverableEmail = UpsAccountManager.Accounts.Count > 0 ? UpsAccountManager.Accounts[0].Email : "";

            profile.Ups.Endorsement = 0;
            profile.Ups.Subclassification = 0;

            profile.Ups.ShipperRelease = false;
            profile.Ups.CommercialPaperlessInvoice = false;
            profile.Ups.PaperlessAdditionalDocumentation = false;
            profile.Ups.CarbonNeutral = false;

            profile.Ups.UspsPackageID = string.Empty;
            profile.Ups.CostCenter = string.Empty;
            profile.Ups.IrregularIndicator = (int) UpsIrregularIndicatorType.NotApplicable;
            profile.Ups.Cn22Number = string.Empty;

            profile.Ups.ShipmentChargeAccount = string.Empty;
            profile.Ups.ShipmentChargeCountryCode = GetDefaultCountry();
            profile.Ups.ShipmentChargePostalCode = string.Empty;
            profile.Ups.ShipmentChargeType = (int) UpsShipmentChargeType.BillReceiver;

            profile.Ups.CustomsDescription = "Goods";
            profile.Ups.CustomsRecipientTIN = string.Empty;
            profile.Ups.CustomsRecipientTINType = (int) UpsCustomsRecipientTINType.IOSS;
            profile.Ups.CustomsRecipientType = (int) UpsCustomsRecipientType.Business;
        }

        /// <summary>
        /// If there is an account, set the country code to the first if account. If no account found, us "US"
        /// </summary>
        private static string GetDefaultCountry() =>
                    UpsAccountManager.Accounts.FirstOrDefault()?.CountryCode ?? "US";

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            RectifyCarrierSpecificData(shipment);
        }

        /// <summary>
        /// Rectifies carrier specific data on the shipment
        /// </summary>
        /// <remarks>
        /// This allows the ShipmentType to fix any issues on the shipment
        /// for example if the service is not valid for the ship to country
        /// or if the packaging type is not valid for the service type
        /// </remarks>
        public override void RectifyCarrierSpecificData(ShipmentEntity shipment)
        {
            base.RectifyCarrierSpecificData(shipment);

            UpsServiceType serviceType = (UpsServiceType) shipment.Ups.Service;

            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            var upsServiceManagerFactory = new UpsServiceManagerFactory(overriddenShipment);
            IUpsServiceManager carrierServiceManager = upsServiceManagerFactory.Create(overriddenShipment);
            List<UpsServiceType> serviceTypes = carrierServiceManager.GetServices(overriddenShipment).Select(s => s.UpsServiceType).ToList();

            // Default to a valid service type if the current one is invalid
            if (!serviceTypes.Contains(serviceType))
            {
                shipment.Ups.Service = (int) serviceTypes.First();
            }

            RedistributeContentWeight(shipment);

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // Consider the shipment insured of any package is insured
            shipment.Insurance = shipment.Ups.Packages.Any(p => p.Insurance);

            // Right now ShipWorks Insurance (due to Tango limitation) doesn't support multi-package - so in that case just auto-revert to carrier insurance
            // We're setting this once to avoid marking the entity as dirty
            shipment.InsuranceProvider = shipment.Ups.Packages.Count > 1 ?
                (int) InsuranceProvider.Carrier :
                settings.UpsInsuranceProvider;

            shipment.RequestedLabelFormat = shipment.Ups.RequestedLabelFormat;

            // Check the UPS wide PennyOne settings and get them updated
            foreach (var package in shipment.Ups.Packages)
            {
                package.InsurancePennyOne = settings.UpsInsurancePennyOne;

                // For WorldShip... if using a MailInnovations service class, we force Penny One since UPS does not provide the first $100 for that
                if (shipment.ShipmentType == (int) ShipmentTypeCode.UpsWorldShip)
                {
                    if (UpsUtility.IsUpsMiOrSurePostService((UpsServiceType) shipment.Ups.Service))
                    {
                        package.InsurancePennyOne = true;
                    }
                }

                // The only time we send the full insuredvalue as declared value is if insurance is enabled, and they are using carrier insurance.
                if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
                {
                    package.DeclaredValue = package.InsuranceValue;
                }
                else if (UpsUtility.IsUpsSurePostService((UpsServiceType) shipment.Ups.Service))
                {
                    // If SurePost, don't send any declared value.
                    package.DeclaredValue = 0;
                }
                else
                {
                    // Otherwise, regardless of if they are insuring or not, penny one or not, etc., we just send up to the first $100 since it's free anyway
                    package.DeclaredValue = Math.Min(100, package.InsuranceValue);
                }
            }
        }

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.  Returns true if weight had to be redistributed.
        /// </summary>
        public static bool RedistributeContentWeight(ShipmentEntity shipment)
        {
            // Determine what our content weight should be
            double contentWeight = shipment.Ups.Packages.Sum(p => p.Weight);

            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (contentWeight != shipment.ContentWeight)
            {
                foreach (UpsPackageEntity package in shipment.Ups.Packages)
                {
                    package.Weight = shipment.ContentWeight / shipment.Ups.Packages.Count;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get weights from packages
        /// </summary>
        protected override IEnumerable<(double weight, bool addDimsWeight, double dimsWeight)> GetPackageWeights(IShipmentEntity shipment) =>
            shipment.Ups?.Packages?.Select(x => (x.Weight, x.DimsAddWeight, x.DimsWeight));

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment) =>
            GetServiceDescriptionInternal((UpsServiceType) shipment.Ups.Service);

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(string serviceCode) =>
            Functional.ParseInt(serviceCode)
                .Match(x => GetServiceDescriptionInternal((UpsServiceType) x), _ => "Unknown");

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        private string GetServiceDescriptionInternal(UpsServiceType service) =>
            EnumHelper.GetDescription(service);

        /// <summary>
        /// Get the ups shipment detail
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            UpsShipmentEntity ups = shipment.Ups;
            UpsAccountEntity account = UpsAccountManager.GetAccount(ups.UpsAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.AccountNumber;
            commonDetail.ServiceType = ups.Service;

            commonDetail.PackagingType = ups.Packages[0].PackagingType;
            commonDetail.PackageLength = ups.Packages[0].DimsLength;
            commonDetail.PackageWidth = ups.Packages[0].DimsWidth;
            commonDetail.PackageHeight = ups.Packages[0].DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Get the total packages contained by the shipment
        /// </summary>
        public override int GetParcelCount(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return shipment.Ups.Packages.Count;
        }

        /// <summary>
        /// Get the parcel data for the shipment
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (parcelIndex >= 0 && parcelIndex < shipment.Ups.Packages.Count)
            {
                var package = shipment.Ups.Packages[parcelIndex];

                return new ShipmentParcel(shipment, package.UpsPackageID, package.TrackingNumber,
                    new InsuranceChoice(shipment, package, package, package),
                    new DimensionsAdapter(package))
                {
                    TotalWeight = package.Weight + (package.DimsAddWeight ? package.DimsWeight : 0)
                };
            }

            throw new ArgumentException(string.Format("'{0}' is out of range for the shipment.", parcelIndex), "parcelIndex");
        }

        /// <summary>
        /// Get the tracking numbers for the shipment
        /// </summary>
        public override List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return new List<string>();
            }

            UpsShipmentEntity ups = shipment.Ups;

            if (ups.Packages.Count == 1)
            {
                return base.GetTrackingNumbers(shipment);
            }
            else
            {
                List<string> trackingList = new List<string>();

                for (int i = 0; i < ups.Packages.Count; i++)
                {
                    string trackingNumber = ups.Packages[i].TrackingNumber;
                    trackingNumber = string.IsNullOrWhiteSpace(trackingNumber) ? ups.Packages[i].UspsTrackingNumber : trackingNumber;

                    trackingList.Add(string.Format("Package {0}: {1}", i + 1, trackingNumber));
                }

                return trackingList;
            }
        }

        /// <summary>
        /// Provide UPS tracking results for the given shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                if (!InterapptiveOnly.Registry.GetValue("UpsTrackingAgreement", false))
                {
                    using (UpsTrackingAgreementDlg dlg = new UpsTrackingAgreementDlg())
                    {
                        if (dlg.ShowDialog(DisplayHelper.GetActiveForm()) != DialogResult.OK)
                        {
                            throw new ShippingException("You must agree to the UPS Developer Kit Tracking Agreement.");
                        }
                    }

                    InterapptiveOnly.Registry.SetValue("UpsTrackingAgreement", true);
                }

                using (var scope = IoC.BeginLifetimeScope())
                {
                    var trackClient = scope.Resolve<IUpsTrackClient>();
                    return Task.Run(() =>
                    {
                        return trackClient.TrackShipment(shipment);
                    }).Result;
                }
            }
            catch (AggregateException ex)
            {
                throw new ShippingException(ex.GetBaseException());
            }
        }

        /// <summary>
        /// Get UPS tracking URL
        /// </summary>
        protected override string GetCarrierTrackingUrlInternal(ShipmentEntity shipment) =>
            $"https://www.ups.com/track?loc=en_US&tracknum={shipment.TrackingNumber}";
        

        /// <summary>
        /// Determines whether [is mail innovations enabled] for OLT.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMailInnovationsEnabled()
        {
            return ShippingSettings.FetchReadOnly().UpsMailInnovationsEnabled;
        }

        /// <summary>
        /// Determines if a shipment will be domestic or international
        /// </summary>
        public override bool IsDomestic(IShipmentEntity shipmentEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentEntity, nameof(shipmentEntity));

            string originCountryCode = shipmentEntity.AdjustedOriginCountryCode();
            string destinationCountryCode = shipmentEntity.AdjustedShipCountryCode();

            return string.Equals(originCountryCode, destinationCountryCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the UPS shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an UpsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            IEnumerable<long> excludedAccounts = bestRateExcludedAccountRepository.GetAll();
            IEnumerable<UpsAccountEntity> nonExcludedUpsAccounts = UpsAccountManager.Accounts.Where(a => !excludedAccounts.Contains(a.AccountId));
            
            if (nonExcludedUpsAccounts.Any())
            {
                return new UpsBestRateBroker(this);
            }

            return new NullShippingBroker();
        }

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public override void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {
            if (shipment.Ups != null && shipment.Ups.Packages != null)
            {
                shipment.Ups.UspsTrackingNumber = String.Empty;
                shipment.Ups.Cn22Number = String.Empty;
                shipment.Ups.ShipEngineLabelID = null;

                foreach (UpsPackageEntity package in shipment.Ups.Packages)
                {
                    package.TrackingNumber = String.Empty;
                    package.UspsTrackingNumber = String.Empty;
                    EntityUtility.MarkAsNew(package);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether multiple packages are supported by this shipment type.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.Ups == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            if (!shipment.Ups.Packages.Any())
            {
                throw new UpsException("There must be at least one package to create the UPS package adapter.");
            }

            // Return an adapter per package
            List<IPackageAdapter> adapters = new List<IPackageAdapter>();
            for (int index = 0; index < shipment.Ups.Packages.Count; index++)
            {
                UpsPackageEntity packageEntity = shipment.Ups.Packages[index];
                adapters.Add(new UpsPackageAdapter(shipment, packageEntity, index + 1));
            }

            return adapters;
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address.
        /// </summary>
        protected override bool IsCustomsRequiredByShipment(IShipmentEntity shipment)
        {
            if (shipment.AdjustedOriginCountryCode() == "PR" &&
                shipment.AdjustedShipCountryCode() == "US")
            {
                return false;
            }

            return base.IsCustomsRequiredByShipment(shipment);
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.Ups != null)
            {
                shipment.Ups.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }
    }
}

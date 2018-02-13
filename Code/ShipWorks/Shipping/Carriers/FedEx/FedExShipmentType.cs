﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.FedEx;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// FedEx implementation of ShipmentType
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(ICustomsRequired))]
    public class FedExShipmentType : ShipmentType, ICustomsRequired
    {
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly IExcludedPackageTypeRepository excludedPackageTypeRepository;
        private ICarrierSettingsRepository settingsRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentType"/> class.
        /// </summary>
        public FedExShipmentType() : this(new ExcludedServiceTypeRepository(), new ExcludedPackageTypeRepository(),
            new ShippingSettingsWrapper(Messenger.Current), new DateTimeProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentType"/> class.
        /// </summary>
        /// <param name="excludedServiceTypeRepository">The excluded service type repository.</param>
        /// <param name="excludedPackageTypeRepository"></param>
        public FedExShipmentType(IExcludedServiceTypeRepository excludedServiceTypeRepository,
            IExcludedPackageTypeRepository excludedPackageTypeRepository, IShippingSettings shippingSettings,
            IDateTimeProvider dateTimeProvider)
        {
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            this.excludedPackageTypeRepository = excludedPackageTypeRepository;
            this.shippingSettings = shippingSettings;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.FedEx;

        /// <summary>
        /// FedEx accounts have an address that can be used as the shipment origin
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// FedEx supports rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Returns are supported for Online Tools
        /// </summary>
        public override bool SupportsReturns => true;

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public override bool SupportsCounterRates => true;

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public override bool HasAccounts => SettingsRepository.GetAccounts().Any();

        /// <summary>
        /// Gets a value indicating whether the shipment type [supports multiple packages].
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsMultiplePackages => true;

        /// <summary>
        /// Gets the service types that have been available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalServiceType values for this shipment type)
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            List<int> allServiceTypes = Enum.GetValues(typeof(FedExServiceType)).Cast<int>().ToList();
            return allServiceTypes.Except(GetExcludedServiceTypes(repository));
        }

        /// <summary>
        /// Uses the ExcludedServiceTypeRepository implementation to get the service types that have
        /// are available for this shipment type (i.e have not been excluded). The integer values are
        /// intended to correspond to the appropriate enumeration values of the specific shipment type
        /// (i.e. the integer values would correspond to PostalServiceType values for a UspsShipmentType).
        /// </summary>
        public override IEnumerable<int> GetAvailableServiceTypes()
        {
            return GetAvailableServiceTypes(excludedServiceTypeRepository);
        }

        /// <summary>
        /// Gets the Package types that have been available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalPackageType values for this shipment type)
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            List<int> allPackageTypes = Enum.GetValues(typeof(FedExPackagingType)).Cast<int>().ToList();
            return allPackageTypes.Except(GetExcludedPackageTypes(repository));
        }

        /// <summary>
        /// Uses the ExcludedPackageTypeRepository implementation to get the Package types that have
        /// are available for this shipment type (i.e have not been excluded). The integer values are
        /// intended to correspond to the appropriate enumeration values of the specific shipment type
        /// (i.e. the integer values would correspond to PostalPackageType values for a UspsShipmentType).
        /// </summary>
        public override IEnumerable<int> GetAvailablePackageTypes()
        {
            return GetAvailablePackageTypes(excludedPackageTypeRepository);
        }

        /// <summary>
        /// Gets or sets the settings repository that the shipment type should use
        /// to obtain FedEx related settings and account information. This provides
        /// the ability to use different FedEx settings depending on how the shipment
        /// type is going to be used. For example, to obtain counter rates with a
        /// generic FedEx account intended to be used with ShipWorks, all that would
        /// have to be done is to assign this property with a repository that contains
        /// the appropriate account information for getting counter rates.
        /// </summary>
        public ICarrierSettingsRepository SettingsRepository
        {
            get
            {
                // Default the settings repository to the "live" FedExSettingsRepository if
                // it hasn't been set already
                return settingsRepository ?? (settingsRepository = new FedExSettingsRepository());
            }
            set
            {
                settingsRepository = value;
            }
        }

        /// <summary>
        /// Create the UserControl used to handle FedEx shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new FedExServiceControl(rateControl);
        }

        /// <summary>
        /// Create the UserControl used to edit the overall fedex settings
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl()
        {
            FedExSettingsControl control = new FedExSettingsControl();
            control.Initialize(ShipmentTypeCode);
            return control;
        }

        /// <summary>
        /// Custom customs settings for FedEx
        /// </summary>
        public override CustomsControlBase CreateCustomsControl()
        {
            return new FedExCustomsControl();
        }

        /// <summary>
        /// Create the UserControl used to edit FedEx profiles.
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl()
        {
            return new FedExProfileControl();
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.FedEx == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            if (!shipment.FedEx.Packages.Any())
            {
                throw new FedExException("There must be at least one package to create the FedEx package adapter.");
            }

            // Return an adapter per package
            List<IPackageAdapter> adapters = new List<IPackageAdapter>();
            for (int index = 0; index < shipment.FedEx.Packages.Count; index++)
            {
                FedExPackageEntity packageEntity = shipment.FedEx.Packages[index];
                adapters.Add(new FedExPackageAdapter(shipment, packageEntity, index + 1));
            }

            return adapters;
        }

        /// <summary>
        /// Ensures that the FedEx specific data for the shipment is loaded.  If the data already exists, nothing is done.  It is not refreshed.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "FedEx", typeof(FedExShipmentEntity), refreshIfPresent);

            // This will exist now
            FedExShipmentEntity fedex = shipment.FedEx;

            // If refreshing, start over on the packages
            if (refreshIfPresent)
            {
                fedex.Packages.Clear();
            }

            // If there are no packages load them now
            if (fedex.Packages.Count == 0)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(fedex.Packages, new RelationPredicateBucket(FedExPackageFields.ShipmentID == shipment.ShipmentID));
                    fedex.Packages.Sort((int) FedExPackageFieldIndex.FedExPackageID, ListSortDirection.Ascending);
                }

                // We reloaded the packages, so reset the tracker
                fedex.Packages.RemovedEntitiesTracker = new FedExPackageCollection();
            }

            // There has to be at least one package.  Really the only way there would not already be a package is if this is a new shipment,
            // and the default profile set included no package stuff.
            if (fedex.Packages.Count == 0)
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
            if (shipment.FedEx == null)
            {
                shipment.FedEx = new FedExShipmentEntity(shipment.ShipmentID);
            }

            shipment.FedEx.MasterFormID = "";

            shipment.FedEx.HomeDeliveryType = (int) FedExHomeDeliveryType.None;
            shipment.FedEx.HomeDeliveryInstructions = "";
            shipment.FedEx.HomeDeliveryDate = shipment.ShipDate;
            shipment.FedEx.HomeDeliveryPhone = shipment.ShipPhone;

            shipment.FedEx.FreightInsidePickup = false;
            shipment.FedEx.FreightInsideDelivery = false;
            shipment.FedEx.FreightBookingNumber = "";
            shipment.FedEx.FreightLoadAndCount = 0;

            shipment.FedEx.CodEnabled = false;
            shipment.FedEx.CodAmount = shipment.Order.OrderTotal;
            shipment.FedEx.CodPaymentType = (int) FedExCodPaymentType.Any;
            shipment.FedEx.CodAddFreight = true;
            shipment.FedEx.CodOriginID = (int) ShipmentOriginSource.Account;
            shipment.FedEx.CodTrackingFormID = "";
            shipment.FedEx.CodTrackingNumber = "";
            shipment.FedEx.CodTIN = string.Empty;
            shipment.FedEx.CodChargeBasis = (int) FedExCodAddTransportationChargeBasisType.NetCharge;
            shipment.FedEx.CodCountryCode = string.Empty;
            shipment.FedEx.CodAccountNumber = string.Empty;

            shipment.FedEx.BrokerEnabled = false;
            shipment.FedEx.BrokerAccount = "";
            shipment.FedEx.BrokerFirstName = "";
            shipment.FedEx.BrokerLastName = "";
            shipment.FedEx.BrokerCompany = "";
            shipment.FedEx.BrokerCity = "";
            shipment.FedEx.BrokerStreet1 = "";
            shipment.FedEx.BrokerStreet2 = "";
            shipment.FedEx.BrokerStreet3 = "";
            shipment.FedEx.BrokerStateProvCode = "";
            shipment.FedEx.BrokerPostalCode = "";
            shipment.FedEx.BrokerCountryCode = "US";
            shipment.FedEx.BrokerPhone = "";
            shipment.FedEx.BrokerPhoneExtension = string.Empty;
            shipment.FedEx.BrokerEmail = "";

            shipment.FedEx.PayorTransportName = string.Empty;
            shipment.FedEx.PayorDutiesCountryCode = string.Empty;
            shipment.FedEx.PayorDutiesName = string.Empty;

            shipment.FedEx.CustomsAdmissibilityPackaging = (int) FedExPhysicalPackagingType.Box;
            shipment.FedEx.CustomsDocumentsOnly = false;
            shipment.FedEx.CustomsDocumentsDescription = "General Documents";
            shipment.FedEx.CustomsRecipientTIN = "";
            shipment.FedEx.CustomsAESEEI = string.Empty;
            shipment.FedEx.CustomsRecipientIdentificationType = (int) FedExCustomsRecipientIdentificationType.None;
            shipment.FedEx.CustomsRecipientIdentificationValue = string.Empty;
            shipment.FedEx.CustomsOptionsType = (int) FedExCustomsOptionType.None;
            shipment.FedEx.CustomsOptionsDesription = string.Empty;
            shipment.FedEx.CustomsExportFilingOption = (int) FedExCustomsExportFilingOption.NotRequired;
            shipment.FedEx.CustomsNaftaEnabled = false;
            shipment.FedEx.CustomsNaftaDeterminationCode = (int) FedExNaftaDeterminationCode.ProducerOfCommodity;
            shipment.FedEx.CustomsNaftaNetCostMethod = (int) FedExNaftaNetCostMethod.NotCalculated;
            shipment.FedEx.CustomsNaftaPreferenceType = (int) FedExNaftaPreferenceCriteria.A;
            shipment.FedEx.CustomsNaftaProducerId = string.Empty;

            shipment.FedEx.CommercialInvoice = false;
            shipment.FedEx.CommercialInvoiceFileElectronically = false;
            shipment.FedEx.CommercialInvoiceTermsOfSale = (int) FedExTermsOfSale.FOB_or_FCA;
            shipment.FedEx.CommercialInvoicePurpose = (int) FedExCommercialInvoicePurpose.Sold;
            shipment.FedEx.CommercialInvoiceComments = "";
            shipment.FedEx.CommercialInvoiceFreight = 0;
            shipment.FedEx.CommercialInvoiceInsurance = 0;
            shipment.FedEx.CommercialInvoiceOther = 0;
            shipment.FedEx.CommercialInvoiceReference = string.Empty;

            shipment.FedEx.ImporterOfRecord = false;
            shipment.FedEx.ImporterTIN = "";
            shipment.FedEx.ImporterAccount = "";
            shipment.FedEx.ImporterFirstName = "";
            shipment.FedEx.ImporterLastName = "";
            shipment.FedEx.ImporterCompany = "";
            shipment.FedEx.ImporterStreet1 = "";
            shipment.FedEx.ImporterStreet2 = "";
            shipment.FedEx.ImporterStreet3 = "";
            shipment.FedEx.ImporterCity = "";
            shipment.FedEx.ImporterStateProvCode = "";
            shipment.FedEx.ImporterPostalCode = "";
            shipment.FedEx.ImporterCountryCode = "US";
            shipment.FedEx.ImporterPhone = "";

            // If we couldn't apply the COD origin address here (then the FedEx account must have been deleted), fall back to blank
            if (!UpdatePersonAddress(shipment, new PersonAdapter(shipment.FedEx, "Cod"), shipment.FedEx.CodOriginID))
            {
                PersonAdapter.ApplyDefaults(shipment.FedEx, "Cod");
            }
            shipment.FedEx.IntlExportDetailType = 0;
            shipment.FedEx.IntlExportDetailForeignTradeZoneCode = string.Empty;
            shipment.FedEx.IntlExportDetailEntryNumber = string.Empty;
            shipment.FedEx.IntlExportDetailLicenseOrPermitNumber = string.Empty;

            shipment.FedEx.FedExHoldAtLocationEnabled = false;
            shipment.FedEx.HoldCity = null;
            shipment.FedEx.HoldCompanyName = null;
            shipment.FedEx.HoldContactId = null;
            shipment.FedEx.HoldCountryCode = null;
            shipment.FedEx.HoldEmailAddress = null;
            shipment.FedEx.HoldFaxNumber = null;
            shipment.FedEx.HoldLocationId = null;
            shipment.FedEx.HoldLocationType = null;
            shipment.FedEx.HoldPagerNumber = null;
            shipment.FedEx.HoldPersonName = null;
            shipment.FedEx.HoldPhoneExtension = null;
            shipment.FedEx.HoldPhoneNumber = null;
            shipment.FedEx.HoldPostalCode = null;
            shipment.FedEx.HoldResidential = null;
            shipment.FedEx.HoldStateOrProvinceCode = null;
            shipment.FedEx.HoldStreet1 = null;
            shipment.FedEx.HoldStreet2 = null;
            shipment.FedEx.HoldStreet3 = null;
            shipment.FedEx.HoldTitle = null;
            shipment.FedEx.HoldUrbanizationCode = null;

            shipment.FedEx.ReturnType = 0;
            shipment.FedEx.RmaNumber = string.Empty;
            shipment.FedEx.RmaReason = string.Empty;
            shipment.FedEx.ReturnSaturdayPickup = false;

            shipment.FedEx.TrafficInArmsLicenseNumber = string.Empty;

            // Purely for certification purposes at this point
            shipment.FedEx.WeightUnitType = (int) WeightUnitOfMeasure.Pounds;
            shipment.FedEx.LinearUnitType = (int) FedExLinearUnitOfMeasure.IN;

            shipment.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;

            shipment.FedEx.SmartPostUspsApplicationId = string.Empty;

            shipment.FedEx.ThirdPartyConsignee = false;

            shipment.FedEx.FreightClass = FedExFreightClassType.None;
            shipment.FedEx.FreightCollectTerms = FedExFreightCollectTermsType.None;
            shipment.FedEx.FreightRole = FedExFreightShipmentRoleType.None;
            shipment.FedEx.FreightSpecialServices = (int) FedExFreightSpecialServicesType.None;
            shipment.FedEx.FreightTotalHandlinUnits = 0;
            shipment.FedEx.FreightGuaranteeType = FedExFreightGuaranteeType.None;
            shipment.FedEx.FreightGuaranteeDate = dateTimeProvider.Now;

            FedExPackageEntity package = FedExUtility.CreateDefaultPackage();
            shipment.FedEx.Packages.Add(package);
            shipment.FedEx.Packages.RemovedEntitiesTracker = new FedExPackageCollection();

            // Weight of the first package equals the total shipment content weight
            package.Weight = shipment.ContentWeight;

            shipment.FedEx.RequestedLabelFormat = (int) ThermalLanguage.None;

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
            if (shipment.FedEx.Packages.RemovedEntitiesTracker == null)
            {
                shipment.FedEx.Packages.RemovedEntitiesTracker = new FedExPackageCollection();
            }

            base.SyncNewShipmentWithShipSense(knowledgebaseEntry, shipment);

            while (shipment.FedEx.Packages.Count < knowledgebaseEntry.Packages.Count())
            {
                FedExPackageEntity package = FedExUtility.CreateDefaultPackage();
                shipment.FedEx.Packages.Add(package);
            }

            while (shipment.FedEx.Packages.Count > knowledgebaseEntry.Packages.Count())
            {
                // Remove the last package until the packages counts match
                shipment.FedEx.Packages.RemoveAt(shipment.FedEx.Packages.Count - 1);
            }
        }

        /// <summary>
        /// Ensure the carrier specific profile data is created and loaded for the given profile
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            base.LoadProfileData(profile, refreshIfPresent);

            bool existed = profile.FedEx != null;

            // Load the profile data
            ShipmentTypeDataService.LoadProfileData(profile, "FedEx", typeof(FedExProfileEntity), refreshIfPresent);

            FedExProfileEntity fedex = profile.FedEx;

            // If this is the first time loading it, or we are supposed to refresh, do it now
            if (!existed || refreshIfPresent)
            {
                fedex.Packages.Clear();

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(fedex.Packages, new RelationPredicateBucket(FedExProfilePackageFields.ShippingProfileID == profile.ShippingProfileID));
                    fedex.Packages.Sort((int) FedExProfilePackageFieldIndex.FedExProfilePackageID, ListSortDirection.Ascending);
                }

                foreach (FedExProfilePackageEntity fedexPackageProfile in fedex.Packages)
                {
                    fedexPackageProfile.PackageProfile = profile.PackageProfile.First(p => fedexPackageProfile.PackageProfileID == p.PackageProfileID);
                }
            }
        }

        /// <summary>
        /// Save FedEx specific profile data
        /// </summary>
        public override bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
        {
            bool changes = base.SaveProfileData(profile, adapter);

            // First delete out anything that needs deleted
            foreach (FedExProfilePackageEntity package in profile.FedEx.Packages.ToList())
            {
                // If its new but deleted, just get rid of it
                if (package.Fields.State == EntityState.Deleted)
                {
                    if (package.IsNew)
                    {
                        profile.FedEx.Packages.Remove(package);
                    }

                    // If its deleted, delete it
                    else
                    {
                        package.Fields.State = EntityState.Fetched;
                        profile.FedEx.Packages.Remove(package);

                        adapter.DeleteEntity(package);

                        changes = true;
                    }
                }

                if (package.IsNew)
                {
                    profile.PackageProfile.Add(package.PackageProfile);
                    adapter.SaveAndRefetch(package.FedExProfile);
                }
            }

            return changes;
        }

        /// <summary>
        /// Get the default profile for the shipment type
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            base.ConfigurePrimaryProfile(profile);

            long shipperID = FedExAccountManager.Accounts.Count > 0 ? FedExAccountManager.Accounts[0].FedExAccountID : 0;

            profile.FedEx.FedExAccountID = shipperID;
            profile.OriginID = (int) ShipmentOriginSource.Account;

            profile.FedEx.ResidentialDetermination = (int) ResidentialDeterminationType.FedExAddressLookup;

            profile.FedEx.Service = (int) FedExServiceType.FedExGround;
            profile.FedEx.Signature = (int) FedExSignatureType.ServiceDefault;
            profile.FedEx.PackagingType = (int) FedExPackagingType.Custom;
            profile.FedEx.DropoffType = (int) FedExDropoffType.RegularPickup;
            profile.FedEx.NonStandardContainer = false;
            profile.FedEx.ReferenceFIMS = "Order {//Order/Number}";
            profile.FedEx.ReferenceCustomer = "Order {//Order/Number}";
            profile.FedEx.ReferenceInvoice = "";
            profile.FedEx.ReferencePO = "";
            profile.FedEx.ReferenceShipmentIntegrity = string.Empty;
            profile.FedEx.OriginResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;

            profile.FedEx.PayorTransportType = (int) FedExPayorType.Sender;
            profile.FedEx.PayorTransportAccount = "";
            profile.FedEx.PayorDutiesType = (int) FedExPayorType.Recipient;
            profile.FedEx.PayorDutiesAccount = "";

            profile.FedEx.SaturdayDelivery = false;

            profile.FedEx.EmailNotifySender = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyRecipient = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyBroker = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyOther = (int) FedExEmailNotificationType.None;
            profile.FedEx.EmailNotifyOtherAddress = "";
            profile.FedEx.EmailNotifyMessage = "";

            profile.FedEx.SmartPostIndicia = (int) FedExSmartPostIndicia.ParcelSelect;
            profile.FedEx.SmartPostEndorsement = (int) FedExSmartPostEndorsement.AddressCorrection;
            profile.FedEx.SmartPostConfirmation = true;
            profile.FedEx.SmartPostCustomerManifest = "";
            profile.FedEx.SmartPostHubID = "0";

            profile.FedEx.ReturnSaturdayPickup = false;
            profile.FedEx.RmaNumber = string.Empty;
            profile.FedEx.RmaReason = string.Empty;
            profile.FedEx.ReturnType = (int) FedExReturnType.PrintReturnLabel;
            profile.FedEx.ReturnSaturdayPickup = false;
            profile.FedEx.ReturnsClearance = false;
            profile.FedEx.ThirdPartyConsignee = false;
        }

        /// <summary>
        /// Apply the given shipping profile to the shipment
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void ApplyProfile(ShipmentEntity shipment, IShippingProfileEntity profile)
        {
            FedExShipmentEntity fedex = shipment.FedEx;
            IFedExProfileEntity source = profile.FedEx;

            bool changedPackageWeights = false;
            int profilePackageCount = profile.FedEx.Packages.Count();

            // Apply all package profiles
            for (int i = 0; i < profilePackageCount; i++)
            {
                // Get the profile to apply
                IFedExProfilePackageEntity fedexPackageProfile = profile.FedEx.Packages.ElementAt(i);                
                FedExPackageEntity package;

                // Get the existing, or create a new package
                if (fedex.Packages.Count > i)
                {
                    package = fedex.Packages[i];
                }
                else
                {
                    package = FedExUtility.CreateDefaultPackage();
                    fedex.Packages.Add(package);
                }

                IPackageProfileEntity packageProfile = fedexPackageProfile.PackageProfile;

                ShippingProfileUtility.ApplyProfileValue(packageProfile.Weight, package, FedExPackageFields.Weight);
                changedPackageWeights |= (packageProfile.Weight != null);

                ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsProfileID, package, FedExPackageFields.DimsProfileID);
                if (packageProfile.DimsProfileID != null)
                {
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsLength, package, FedExPackageFields.DimsLength);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWidth, package, FedExPackageFields.DimsWidth);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsHeight, package, FedExPackageFields.DimsHeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsWeight, package, FedExPackageFields.DimsWeight);
                    ShippingProfileUtility.ApplyProfileValue(packageProfile.DimsAddWeight, package, FedExPackageFields.DimsAddWeight);
                }

                if (fedexPackageProfile.DryIceWeight > 0)
                {
                    ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DryIceWeight, package, FedExPackageFields.DryIceWeight);
                }

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.ContainsAlcohol, package, FedExPackageFields.ContainsAlcohol);

                if (fedexPackageProfile.PriorityAlert.HasValue && fedexPackageProfile.PriorityAlert.Value)
                {
                    ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PriorityAlertDetailContent, package, FedExPackageFields.PriorityAlertDetailContent);
                    ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PriorityAlertEnhancementType, package, FedExPackageFields.PriorityAlertEnhancementType);
                }

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsEnabled, package, FedExPackageFields.DangerousGoodsEnabled);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsType, package, FedExPackageFields.DangerousGoodsType);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsAccessibilityType, package, FedExPackageFields.DangerousGoodsAccessibilityType);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsCargoAircraftOnly, package, FedExPackageFields.DangerousGoodsCargoAircraftOnly);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsEmergencyContactPhone, package, FedExPackageFields.DangerousGoodsEmergencyContactPhone);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsOfferor, package, FedExPackageFields.DangerousGoodsOfferor);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.DangerousGoodsPackagingCount, package, FedExPackageFields.DangerousGoodsPackagingCount);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.ContainerType, package, FedExPackageFields.ContainerType);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.NumberOfContainers, package, FedExPackageFields.NumberOfContainers);

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.SignatoryContactName, package, FedExPackageFields.SignatoryContactName);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.SignatoryTitle, package, FedExPackageFields.SignatoryTitle);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.SignatoryPlace, package, FedExPackageFields.SignatoryPlace);

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialNumber, package, FedExPackageFields.HazardousMaterialNumber);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialClass, package, FedExPackageFields.HazardousMaterialClass);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialProperName, package, FedExPackageFields.HazardousMaterialProperName);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialPackingGroup, package, FedExPackageFields.HazardousMaterialPackingGroup);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialQuantityValue, package, FedExPackageFields.HazardousMaterialQuantityValue);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.HazardousMaterialQuanityUnits, package, FedExPackageFields.HazardousMaterialQuanityUnits);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PackingDetailsCargoAircraftOnly, package, FedExPackageFields.PackingDetailsCargoAircraftOnly);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.PackingDetailsPackingInstructions, package, FedExPackageFields.PackingDetailsPackingInstructions);

                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.BatteryMaterial, package, FedExPackageFields.BatteryMaterial);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.BatteryPacking, package, FedExPackageFields.BatteryPacking);
                ShippingProfileUtility.ApplyProfileValue(fedexPackageProfile.BatteryRegulatorySubtype, package, FedExPackageFields.BatteryRegulatorySubtype);
            }

            // Remove any packages that are too many for the profile
            if (profilePackageCount > 0)
            {
                // Go through each package that needs removed
                foreach (FedExPackageEntity package in fedex.Packages.Skip(profilePackageCount).ToList())
                {
                    if (package.Weight != 0)
                    {
                        changedPackageWeights = true;
                    }

                    // Remove it from the list
                    fedex.Packages.Remove(package);

                    // If its saved in the database, we have to delete it
                    if (!package.IsNew)
                    {
                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.DeleteEntity(package);
                        }
                    }
                }
            }

            base.ApplyProfile(shipment, profile);

            long? accountID = (source.FedExAccountID == 0 && FedExAccountManager.Accounts.Count > 0) ?
                                  FedExAccountManager.Accounts[0].FedExAccountID :
                                  source.FedExAccountID;

            ShippingProfileUtility.ApplyProfileValue(source.ResidentialDetermination, shipment, ShipmentFields.ResidentialDetermination);

            ShippingProfileUtility.ApplyProfileValue(accountID, fedex, FedExShipmentFields.FedExAccountID);
            ShippingProfileUtility.ApplyProfileValue(source.Service, fedex, FedExShipmentFields.Service);
            ShippingProfileUtility.ApplyProfileValue(source.PackagingType, fedex, FedExShipmentFields.PackagingType);
            ShippingProfileUtility.ApplyProfileValue(source.DropoffType, fedex, FedExShipmentFields.DropoffType);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnsClearance, fedex, FedExShipmentFields.ReturnsClearance);
            ShippingProfileUtility.ApplyProfileValue(source.NonStandardContainer, fedex, FedExShipmentFields.NonStandardContainer);
            ShippingProfileUtility.ApplyProfileValue(source.OriginResidentialDetermination, fedex, FedExShipmentFields.OriginResidentialDetermination);

            ShippingProfileUtility.ApplyProfileValue(source.Signature, fedex, FedExShipmentFields.Signature);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceFIMS, fedex, FedExShipmentFields.ReferenceFIMS);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceCustomer, fedex, FedExShipmentFields.ReferenceCustomer);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceInvoice, fedex, FedExShipmentFields.ReferenceInvoice);
            ShippingProfileUtility.ApplyProfileValue(source.ReferencePO, fedex, FedExShipmentFields.ReferencePO);
            ShippingProfileUtility.ApplyProfileValue(source.ReferenceShipmentIntegrity, fedex, FedExShipmentFields.ReferenceShipmentIntegrity);

            ShippingProfileUtility.ApplyProfileValue(source.PayorTransportType, fedex, FedExShipmentFields.PayorTransportType);
            ShippingProfileUtility.ApplyProfileValue(source.PayorTransportAccount, fedex, FedExShipmentFields.PayorTransportAccount);
            ShippingProfileUtility.ApplyProfileValue(source.PayorDutiesType, fedex, FedExShipmentFields.PayorDutiesType);
            ShippingProfileUtility.ApplyProfileValue(source.PayorDutiesAccount, fedex, FedExShipmentFields.PayorDutiesAccount);

            ShippingProfileUtility.ApplyProfileValue(source.SaturdayDelivery, fedex, FedExShipmentFields.SaturdayDelivery);

            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifySender, fedex, FedExShipmentFields.EmailNotifySender);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyRecipient, fedex, FedExShipmentFields.EmailNotifyRecipient);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyBroker, fedex, FedExShipmentFields.EmailNotifyBroker);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyOther, fedex, FedExShipmentFields.EmailNotifyOther);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyOtherAddress, fedex, FedExShipmentFields.EmailNotifyOtherAddress);
            ShippingProfileUtility.ApplyProfileValue(source.EmailNotifyMessage, fedex, FedExShipmentFields.EmailNotifyMessage);

            ShippingProfileUtility.ApplyProfileValue(source.SmartPostIndicia, fedex, FedExShipmentFields.SmartPostIndicia);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostEndorsement, fedex, FedExShipmentFields.SmartPostEndorsement);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostConfirmation, fedex, FedExShipmentFields.SmartPostConfirmation);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostCustomerManifest, fedex, FedExShipmentFields.SmartPostCustomerManifest);
            ShippingProfileUtility.ApplyProfileValue(source.SmartPostHubID, fedex, FedExShipmentFields.SmartPostHubID);

            ShippingProfileUtility.ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnType, fedex, FedExShipmentFields.ReturnType);
            ShippingProfileUtility.ApplyProfileValue(source.RmaNumber, fedex, FedExShipmentFields.RmaNumber);
            ShippingProfileUtility.ApplyProfileValue(source.RmaReason, fedex, FedExShipmentFields.RmaReason);
            ShippingProfileUtility.ApplyProfileValue(source.ReturnSaturdayPickup, fedex, FedExShipmentFields.ReturnSaturdayPickup);
            ShippingProfileUtility.ApplyProfileValue(source.ThirdPartyConsignee, fedex, FedExShipmentFields.ThirdPartyConsignee);

            if (changedPackageWeights)
            {
                UpdateTotalWeight(shipment);
            }

            UpdateDynamicShipmentData(shipment);
        }
        
        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            if (shipment.FedEx.HomeDeliveryDate < dateTimeProvider.Now.Date)
            {
                shipment.FedEx.HomeDeliveryDate = dateTimeProvider.Now.Date.AddHours(12);
            }

            if (shipment.FedEx.FreightGuaranteeDate < dateTimeProvider.Now.Date)
            {
                shipment.FedEx.FreightGuaranteeDate = dateTimeProvider.Now.Date.AddHours(12);
            }

            // Ensure the cod address is up-to-date
            if (!UpdatePersonAddress(shipment, new PersonAdapter(shipment.FedEx, "Cod"), shipment.FedEx.CodOriginID))
            {
                shipment.FedEx.CodOriginID = (long) ShipmentOriginSource.Other;
            }

            FedExServiceType serviceType = (FedExServiceType) shipment.FedEx.Service;

            // Need to check with the store  to see if anything about the shipment was overridden in case
            // it may have effected the shipping services available (i.e. the eBay GSP program)
            ShipmentEntity overriddenShipment = ShippingManager.GetOverriddenStoreShipment(shipment);

            // If its international ensure an international service
            if (!FedExUtility.GetValidServiceTypes(overriddenShipment).Contains(serviceType))
            {
                shipment.FedEx.Service = (int) FedExUtility.GetValidServiceTypes(overriddenShipment).First();
            }

            RedistributeContentWeight(shipment);

            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();

            // Consider the shipment insured of any package is insured
            shipment.Insurance = shipment.FedEx.Packages.Any(p => p.Insurance);

            // Right now ShipWorks Insurance (due to Tango limitation) doesn't support multi-package - so in that case just auto-revert to carrier insurance
            // We're setting this once to avoid marking the entity as dirty
            shipment.InsuranceProvider = IsDeclaredValueRequired(shipment) ?
                (int) InsuranceProvider.Carrier :
                settings.FedExInsuranceProvider;

            shipment.RequestedLabelFormat = shipment.FedEx.RequestedLabelFormat;

            // If it's international we have to make sure we wouldn't send more total declared value than the customs value - use the overridden shipment
            // to compare the country code in case it's been overridden such as an eBay GSP order
            decimal? maxPackageDeclaredValue = overriddenShipment.AdjustedShipCountryCode() != "US" ?
                shipment.CustomsValue / shipment.FedEx.Packages.Count :
                (decimal?) null;

            // Check the FedEx wide PennyOne settings and get them updated
            foreach (var package in shipment.FedEx.Packages)
            {
                package.InsurancePennyOne = settings.FedExInsurancePennyOne;

                // For SmartPost, we force Penny One since FedEx does not provide the first $100 for that
                // For LTL Freight, we force Penny One to match liability calculations from DHL Express and Asendia
                if (shipment.FedEx.Service == (int) FedExServiceType.SmartPost ||
                    FedExUtility.IsFreightLtlService(shipment.FedEx.Service))
                {
                    package.InsurancePennyOne = true;
                }

                // The only time we send the full insured value as declared value is if insurance is enabled, and they are using carrier insurance.
                if (shipment.Insurance && shipment.InsuranceProvider == (int) InsuranceProvider.Carrier)
                {
                    package.DeclaredValue = package.InsuranceValue;
                }
                else if (shipment.FedEx.Service == (int) FedExServiceType.SmartPost)
                {
                    // SmartPost shouldn't be sending any insurance value
                    package.DeclaredValue = 0;
                }
                else
                {
                    // Otherwise, regardless of if they are insuring or not, penny one or not, etc., we just send up to the first $100 since it's free anyway
                    package.DeclaredValue = Math.Min(100, package.InsuranceValue);

                    // We may have to lower it some more for international shipments so we aren't higher than the CustomsValue
                    if (maxPackageDeclaredValue != null)
                    {
                        package.DeclaredValue = Math.Min(package.DeclaredValue, (decimal) maxPackageDeclaredValue);
                    }
                }
            }
        }

        /// <summary>
        /// Does the shipment require FedEx declared value
        /// </summary>
        private static bool IsDeclaredValueRequired(IShipmentEntity shipment) =>
            shipment.FedEx.Packages.IsCountGreaterThan(1) || FedExUtility.IsFreightAnyService(shipment.FedEx.Service);

        /// <summary>
        /// Redistribute the ContentWeight from the shipment to each package in the shipment.  This only does something
        /// if the ContentWeight is different from the total Content.  Returns true if weight had to be redistributed.
        /// </summary>
        public static bool RedistributeContentWeight(ShipmentEntity shipment)
        {
            // Determine what our content weight should be
            double contentWeight = shipment.FedEx.Packages.Sum(p => p.Weight);

            // If the content weight changed outside of us, redistribute what the new weight among the packages
            if (contentWeight != shipment.ContentWeight)
            {
                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    package.Weight = shipment.ContentWeight / shipment.FedEx.Packages.Count;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Update the total weight of the shipment based on the individual package weights
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            double contentWeight = 0;
            double totalWeight = 0;

            foreach (FedExPackageEntity package in shipment.FedEx.Packages)
            {
                contentWeight += package.Weight;
                totalWeight += package.Weight;

                if (package.DimsAddWeight)
                {
                    totalWeight += package.DimsWeight;
                }
            }

            shipment.ContentWeight = contentWeight;
            shipment.TotalWeight = totalWeight;
        }

        /// <summary>
        /// Indicates if the residential status indicator is required for the given shipment
        /// </summary>
        public override bool IsResidentialStatusRequired(IShipmentEntity shipment) =>
            IsDomestic(shipment);

        /// <summary>
        /// Get the carrier specific description of the shipping service used
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return string.Format("{0}", EnumHelper.GetDescription((FedExServiceType) shipment.FedEx.Service));
        }

        /// <summary>
        /// Get the FedEx account number used for the shipment
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            FedExShipmentEntity fedex = shipment.FedEx;
            FedExAccountEntity account = FedExAccountManager.GetAccount(fedex.FedExAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.AccountNumber;
            commonDetail.ServiceType = fedex.Service;

            commonDetail.PackagingType = fedex.PackagingType;
            commonDetail.PackageLength = fedex.Packages[0].DimsLength;
            commonDetail.PackageWidth = fedex.Packages[0].DimsWidth;
            commonDetail.PackageHeight = fedex.Packages[0].DimsHeight;

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

            return shipment.FedEx.Packages.Count;
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

            if (parcelIndex >= 0 && parcelIndex < shipment.FedEx.Packages.Count)
            {
                var package = shipment.FedEx.Packages[parcelIndex];

                return new ShipmentParcel(shipment, package.FedExPackageID, package.TrackingNumber,
                    new InsuranceChoice(shipment, package, package, package),
                    new DimensionsAdapter(package))
                {
                    TotalWeight = package.Weight + package.DimsWeight
                };
            }

            throw new ArgumentException(string.Format("'{0}' is out of range for the shipment.", parcelIndex), "parcelIndex");
        }

        /// <summary>
        /// Get all the tracking numbers for the shipment
        /// </summary>
        public override List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return new List<string>();
            }

            FedExShipmentEntity fedex = shipment.FedEx;

            if (fedex.Packages.Count == 1)
            {
                return new List<string> { shipment.TrackingNumber };
            }
            else
            {
                List<string> trackingList = new List<string>();

                for (int i = 0; i < fedex.Packages.Count; i++)
                {
                    trackingList.Add(string.Format("Package {0}: {1}", i + 1, fedex.Packages[i].TrackingNumber));
                }

                return trackingList;
            }
        }

        /// <summary>
        // Check with the SettingsRepository here rather than FedExAccountManager, so getting
        // We need to swap out the SettingsRepository and certificate inspector
        /// Provide FedEx tracking results for the given shipment
        /// </summary>
        public override TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            try
            {
                using (var lifetimeScope = IoC.BeginLifetimeScope())
                {
                    return lifetimeScope.Resolve<IFedExShippingClerkFactory>().Create(shipment).Track(shipment);
                }
            }
            catch (FedExException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the UserControl that is used to edit return shipments
        /// </summary>
        /// <returns></returns>
        public override ReturnsControlBase CreateReturnsControl()
        {
            return new FedExReturnsControl();
        }

        /// <summary>
        /// Generate the carrier specific template xml
        /// </summary>
        public override void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {
            FedExTemplateElementGenerator.Generate(container, shipment, loaded);
        }

        /// <summary>
        /// Determines if a shipment will be domestic or international
        /// </summary>
        public override bool IsDomestic(IShipmentEntity shipmentEntity)
        {
            if (shipmentEntity == null)
            {
                throw new ArgumentNullException("shipmentEntity");
            }

            string originCountryCode = shipmentEntity.AdjustedOriginCountryCode();
            string destinationCountryCode = shipmentEntity.AdjustedShipCountryCode();

            // If the origin is in Puerto Rico it is an international shipment
            // confirmed on FedEx.com that the behavior is the same
            if (originCountryCode.Equals("PR", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return string.Equals(originCountryCode, destinationCountryCode, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the FedEx shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a FedExBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            if (FedExAccountManager.Accounts.Any())
            {
                return new FedExBestRateBroker();
            }

            // We want to be able to show counter rates to users that don't have
            return new NullShippingBroker();
        }

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public override void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {
            if (shipment.FedEx != null)
            {
                shipment.FedEx.SmartPostUspsApplicationId = string.Empty;
            }
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.FedEx != null)
            {
                shipment.FedEx.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.FedExShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new FedExShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address.
        /// </summary>
        protected override bool IsCustomsRequiredByShipment(IShipmentEntity shipment)
        {
            if (FedExUtility.IsSmartPostEnabled(shipment)
                && shipment.ShipPerson.IsUSInternationalTerritory()
                && ((FedExServiceType) shipment.FedEx.Service) == FedExServiceType.SmartPost)
            {
                return false;
            }

            return base.IsCustomsRequiredByShipment(shipment);
        }

        /// <summary>
        /// Implement the ICustomsRequired interface explicitly
        /// </summary>
        bool ICustomsRequired.IsCustomsRequired(IShipmentEntity shipment) =>
            IsCustomsRequired(shipment);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Filters;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Templates.Processing.TemplateXml.ElementOutlines;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Base class that all shipment types\carriers must derive from
    /// </summary>
    public abstract class ShipmentType
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentType));

        /// <summary>
        /// HTTPS certificate inspector to use.
        /// </summary>
        private ICertificateInspector certificateInspector;

        private static object syncLock = new object();

        protected ShipmentType()
        {
            // Use the trusting inspector until told otherwise trusting so that calls will continue to work as expected.
            // Calls that require specific inspection should override the CertificateInspector property.
            certificateInspector = new TrustingCertificateInspector();

            ShouldApplyShipSense = true;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public abstract ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// The user-displayable name of the shipment type
        /// </summary>
        public virtual string ShipmentTypeName
        {
            get
            {
                return EnumHelper.GetDescription(ShipmentTypeCode);
            }
        }

        /// <summary>
        /// Overridden to provide the name of the shipment type
        /// </summary>
        public override string ToString()
        {
            return ShipmentTypeName;
        }

        /// <summary>
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public virtual bool SupportsGetRates
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates if the shipment service type supports return shipments
        /// </summary>
        public virtual bool SupportsReturns
        {
            get { return false; }
        }

        /// <summary>
        /// Supports using an origin address from a shipping account
        /// </summary>
        public virtual bool SupportsAccountAsOrigin
        {
            get { return false; }
        }

        /// <summary>
        /// Supports getting counter rates.
        /// </summary>
        public virtual bool SupportsCounterRates
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the shipment type [supports multiple packages].
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool SupportsMultiplePackages
        {
            get { return false; }
        }

        /// <summary>
        /// Indicates if the ShipmentType needs the ResidentialResult field determined for the given shipment.
        /// </summary>
        public virtual bool IsResidentialStatusRequired(IShipmentEntity shipment) => false;

        /// <summary>
        /// Gets or sets the certificate inspector that should be used when wanting to add additional security
        /// around API calls to shipping partners. This is defaulted to the trusting inspector so that calls
        /// will continue to work as expected. Calls that require specific inspection should assign this property
        /// accordingly.
        /// </summary>
        public virtual ICertificateInspector CertificateInspector
        {
            get
            {
                return certificateInspector;
            }
            set
            {
                certificateInspector = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether account registration allowed for this shipment type.
        /// </summary>
        public virtual bool IsAccountRegistrationAllowed
        {
            get
            {
                return GetRestrictionLevel(EditionFeature.ShipmentTypeRegistration) == EditionRestrictionLevel.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has been restricted.
        /// </summary>
        public virtual bool IsShipmentTypeRestricted
        {
            get
            {
                return GetRestrictionLevel(EditionFeature.ShipmentType) != EditionRestrictionLevel.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has rate discount messaging restricted. This will mean different things to different shipment types.
        /// </summary>
        public bool IsRateDiscountMessagingRestricted
        {
            get
            {
                return GetRestrictionLevel(EditionFeature.RateDiscountMessaging) != EditionRestrictionLevel.None;
            }
        }

        /// <summary>
        /// Gets the restriction level of the given feature for this shipment type.
        /// </summary>
        private EditionRestrictionLevel GetRestrictionLevel(EditionFeature feature)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(feature, ShipmentTypeCode);

                return restrictionLevel;
            }
        }

        /// <summary>
        /// Checks whether this shipment type is allowed for the given shipment
        /// </summary>
        public virtual bool IsAllowedFor(ShipmentEntity shipment)
        {
            IAmazonOrder order;
            if (shipment.Order == null)
            {
                order = DataProvider.GetEntity(shipment.OrderID) as IAmazonOrder;
            }
            else
            {
                order = shipment.Order as IAmazonOrder;
            }

            // If the order is Amazon Prime return false
            return !order?.IsPrime ?? true;
        }

        /// <summary>
        /// Gets a value indicating whether this shipment type has accounts
        /// </summary>
        public virtual bool HasAccounts
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [should apply ship sense].
        /// </summary>
        public bool ShouldApplyShipSense { get; set; }

        /// <summary>
        /// Creates the UserControl that is used to edit service options for the shipment type
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public ServiceControlBase CreateServiceControl(RateControl rateControl, ILifetimeScope lifetimeScope)
        {
            ServiceControlBase serviceControlBase = null;
            int retries = 0;

            // Sometimes the krypton tools will crash when trying to get font heights.  This code will try to create
            // the specific shipment type service control, and if an ArgumentException is encountered and it has GetHeight
            // in it's stack or source, will attempt to create the service control a number of times, sleeping between tries.
            while (retries < 5)
            {
                try
                {
                    serviceControlBase = lifetimeScope.IsRegisteredWithKey<ServiceControlBase>(ShipmentTypeCode) ?
                        lifetimeScope.ResolveKeyed<ServiceControlBase>(ShipmentTypeCode, TypedParameter.From(rateControl)) :
                        InternalCreateServiceControl(rateControl);

                    break;
                }
                catch (ArgumentException ex)
                {
                    if (ex.StackTrace.ToUpperInvariant().Contains("GetHeight(".ToUpperInvariant()) ||
                        ex.Source.ToUpperInvariant().Contains("GetHeight(".ToUpperInvariant()))
                    {
                        retries++;

                        if (retries == 5)
                        {
                            log.Error(string.Format("The font GetHeight ArgumentException occurred in {0}.  This was the final try, so re-throwing.", ShipmentTypeName), ex);
                            throw;
                        }

                        log.Error(string.Format("The font GetHeight ArgumentException occurred in {0} on try {1}", ShipmentTypeName, retries), ex);

                        Thread.Sleep(1000);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return serviceControlBase;
        }

        /// <summary>
        /// Creates the UserControl that is used to edit service options for the shipment type
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected virtual ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            throw new NotImplementedException("Either override InternalCreateServiceControl or register one with the IoC container");
        }

        /// <summary>
        /// Creates the UserControl that is used to edit customs options
        /// </summary>
        public virtual CustomsControlBase CreateCustomsControl()
        {
            return new CustomsControlBase();
        }

        /// <summary>
        /// Creates the UserControl that is used to edit customs options
        /// </summary>
        public virtual CustomsControlBase CreateCustomsControl(ILifetimeScope lifetimeScope)
        {
            return lifetimeScope.IsRegisteredWithKey<CustomsControlBase>(ShipmentTypeCode) ?
                lifetimeScope.ResolveKeyed<CustomsControlBase>(ShipmentTypeCode) :
                CreateCustomsControl();
        }

        /// <summary>
        /// Creates the UserControl that is used to edit return shipments
        /// </summary>
        public virtual ReturnsControlBase CreateReturnsControl()
        {
            return new ReturnsControlBase();
        }
        
        /// <summary>
        /// Creates the UserControl that is used to edit the defaults\settings for the service
        /// </summary>
        protected virtual SettingsControlBase CreateSettingsControl()
        {
            return null;
        }

        /// <summary>
        /// Creates the UserControl that is used to edit the defaults\settings for the service
        /// </summary>
        public virtual SettingsControlBase CreateSettingsControl(ILifetimeScope lifetimeScope)
        {
            return lifetimeScope.IsRegisteredWithKey<SettingsControlBase>(ShipmentTypeCode) ?
                lifetimeScope.ResolveKeyed<SettingsControlBase>(ShipmentTypeCode) :
                CreateSettingsControl();
        }

        /// <summary>
        /// Create the UserControl that is used to edit a profile for the service
        /// </summary>
        protected virtual ShippingProfileControlBase CreateProfileControl()
        {
            return null;
        }

        /// <summary>
        /// Create the UserControl that is used to edit a profile for the service
        /// </summary>
        public virtual ShippingProfileControlBase CreateProfileControl(ILifetimeScope lifetimeScope)
        {
            return lifetimeScope.IsRegisteredWithKey<ShippingProfileControlBase>(ShipmentTypeCode) ?
                lifetimeScope.ResolveKeyed<ShippingProfileControlBase>(ShipmentTypeCode) :
                CreateProfileControl();
        }

        /// <summary>
        /// Uses the ExcludedServiceTypeRepository implementation to get the service types that have
        /// been excluded for this shipment type. The integer values are intended to correspond to
        /// the appropriate enumeration values of the specific shipment type (i.e. the integer values
        /// would correspond to PostalServiceType values for a UspsShipmentType).
        /// </summary>
        public IEnumerable<int> GetExcludedServiceTypes()
        {
            return GetExcludedServiceTypes(new ExcludedServiceTypeRepository());
        }

        /// <summary>
        /// Gets the service types that have been excluded for this shipment type. The integer
        /// values are intended to correspond to the appropriate enumeration values of the specific
        /// shipment type (i.e. the integer values would correspond to PostalServiceType values
        /// for a UspsShipmentType).
        /// </summary>
        /// <param name="repository">The repository from which the service types are fetched.</param>
        public virtual IEnumerable<int> GetExcludedServiceTypes(IExcludedServiceTypeRepository repository)
        {
            List<ExcludedServiceTypeEntity> excludedServiceTypes = repository.GetExcludedServiceTypes(this);
            return excludedServiceTypes.Select(s => s.ServiceType);
        }

        /// <summary>
        /// Uses the ExcludedServiceTypeRepository implementation to get the service types that have
        /// are available for this shipment type (i.e have not been excluded). The integer values are
        /// intended to correspond to the appropriate enumeration values of the specific shipment type
        /// (i.e. the integer values would correspond to PostalServiceType values for a UspsShipmentType).
        /// </summary>
        public virtual IEnumerable<int> GetAvailableServiceTypes()
        {
            return GetAvailableServiceTypes(new ExcludedServiceTypeRepository());
        }

        /// <summary>
        /// Gets the service types that are available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalServiceType values for a UspsShipmentType).
        /// </summary>
        /// <param name="repository">The repository from which the service types are fetched.</param>
        public virtual IEnumerable<int> GetAvailableServiceTypes(IExcludedServiceTypeRepository repository)
        {
            return Enumerable.Empty<int>();
        }

        /// <summary>
        /// Uses the ExcludedPackageTypeRepository implementation to get the Package types that have
        /// been excluded for this shipment type. The integer values are intended to correspond to
        /// the appropriate enumeration values of the specific shipment type (i.e. the integer values
        /// would correspond to PostalPackageType values for a UspsShipmentType).
        /// </summary>
        public IEnumerable<int> GetExcludedPackageTypes()
        {
            return GetExcludedPackageTypes(new ExcludedPackageTypeRepository());
        }

        /// <summary>
        /// Gets the Package types that have been excluded for this shipment type. The integer
        /// values are intended to correspond to the appropriate enumeration values of the specific
        /// shipment type (i.e. the integer values would correspond to PostalPackageType values
        /// for a UspsShipmentType).
        /// </summary>
        /// <param name="repository">The repository from which the Package types are fetched.</param>
        public virtual IEnumerable<int> GetExcludedPackageTypes(IExcludedPackageTypeRepository repository)
        {
            List<ExcludedPackageTypeEntity> excludedPackageTypes = repository.GetExcludedPackageTypes(this);
            return excludedPackageTypes.Select(s => s.PackageType);
        }

        /// <summary>
        /// Uses the ExcludedPackageTypeRepository implementation to get the Package types that have
        /// are available for this shipment type (i.e have not been excluded). The integer values are
        /// intended to correspond to the appropriate enumeration values of the specific shipment type
        /// (i.e. the integer values would correspond to PostalPackageType values for a UspsShipmentType).
        /// </summary>
        public virtual IEnumerable<int> GetAvailablePackageTypes()
        {
            return GetAvailablePackageTypes(new ExcludedPackageTypeRepository());
        }

        /// <summary>
        /// Gets the Package types that are available for this shipment type (i.e have not
        /// been excluded). The integer values are intended to correspond to the appropriate
        /// enumeration values of the specific shipment type (i.e. the integer values would
        /// correspond to PostalPackageType values for a UspsShipmentType).
        /// </summary>
        /// <param name="repository">The repository from which the Package types are fetched.</param>
        public virtual IEnumerable<int> GetAvailablePackageTypes(IExcludedPackageTypeRepository repository)
        {
            return Enumerable.Empty<int>();
        }

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public virtual Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments, IExcludedPackageTypeRepository excludedServiceTypeRepository)
            => new Dictionary<int, string>();

        /// <summary>
        /// Gets the AvailablePackageTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildPackageTypeDictionary(List<ShipmentEntity> shipments)
            => BuildPackageTypeDictionary(shipments, new ExcludedPackageTypeRepository());

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public abstract IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment);

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        public void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            LoadShipmentDataInternal(shipment, refreshIfPresent);

            if (!shipment.Processed)
            {
                UpdateAccountIDIfNecessary(shipment);
            }
        }

        /// <summary>
        /// Update the account ID on the shipment if necessary
        /// </summary>
        private void UpdateAccountIDIfNecessary(ShipmentEntity shipment)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ICarrierAccountRetriever accountRetriever = lifetimeScope.ResolveKeyed<ICarrierAccountRetriever>(ShipmentTypeCode);

                if (accountRetriever.GetAccountReadOnly(shipment) == null)
                {
                    ICarrierAccount carrierAccount = accountRetriever.AccountsReadOnly.FirstOrDefault();
                    carrierAccount?.ApplyTo(shipment);
                }
            }
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected abstract void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent);

        /// <summary>
        /// Apply the configured defaults and profile rule settings to the given shipment
        /// </summary>
        public virtual void ConfigureNewShipment(ShipmentEntity shipment)
        {
            shipment.ActualLabelFormat = null;
            shipment.ShipSenseStatus = (int) ShipSenseStatus.NotApplied;
            shipment.BilledType = 0;
            shipment.BilledWeight = 0;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileManager shippingProfileManager = lifetimeScope.Resolve<IShippingProfileManager>();
                ICustomsManager customsManager = lifetimeScope.Resolve<ICustomsManager>();
                ICarrierAccountRetriever accountRetriever = lifetimeScope.ResolveKeyed<ICarrierAccountRetriever>(ShipmentTypeCode);
                IFilterHelper filterHelper = lifetimeScope.Resolve<IFilterHelper>();

                // First apply the base profile
                ApplyProfile(shipment, shippingProfileManager.GetOrCreatePrimaryProfileReadOnly(this));

                // ApplyShipSense will call CustomsManager.LoadCustomsItems which will save the shipment to the database,
                // but we want to defer that as long as possible, so call GenerateCustomsItems here so that when
                // LoadCustomsItems is called, saving will be skipped.
                if (shipment.CustomsGenerated)
                {
                    shipment.CustomsGenerated = !IsCustomsRequired(shipment);
                }
                customsManager.GenerateCustomsItems(shipment);

                // Now apply ShipSense
                ApplyShipSense(shipment);

                // Go through each additional profile and apply it as well
                foreach (ShippingDefaultsRuleEntity rule in ShippingDefaultsRuleManager.GetRules(ShipmentTypeCode))
                {
                    IShippingProfileEntity profile = shippingProfileManager.GetProfileReadOnly(rule.ShippingProfileID);
                    if (profile != null && filterHelper.IsObjectInFilterContent(shipment.OrderID, rule))
                    {
                        ApplyProfile(shipment, profile);
                    }
                }

                // This was brought in from LoadShipmentData.  Since we are no longer using that method for creating a new shipment,
                // we still needed to do this logic.
                if (accountRetriever.GetAccountReadOnly(shipment) == null)
                {
                    ICarrierAccount carrierAccount = accountRetriever.AccountsReadOnly.FirstOrDefault();
                    carrierAccount?.ApplyTo(shipment);
                }
            }
        }

        /// <summary>
        /// Attempts to apply ShipSense values to the given shipment.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ApplyShipSense(ShipmentEntity shipment)
        {
            if (!ShouldApplyShipSense)
            {
                return;
            }

            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            if (!settings.ShipSenseEnabled)
            {
                return;
            }

            BestRateEventTypes eventTypes = (BestRateEventTypes) shipment.BestRateEvents;
            BestRateEventTypes latestEvent = eventTypes.GetLatestBestRateEvent();

            if (ShipmentTypeCode != ShipmentTypeCode.BestRate && (latestEvent == BestRateEventTypes.RateSelected || latestEvent == BestRateEventTypes.RateAutoSelectedAndProcessed))
            {
                return;
            }

            // Populate the order items so we can compute the hash
            OrderUtility.PopulateOrderDetails(shipment);

            // Get our knowledge base entry for this shipment
            Knowledgebase knowledgebase = new Knowledgebase();

            KnowledgebaseEntry knowledgebaseEntry = knowledgebase.GetEntry(shipment.Order);
            knowledgebaseEntry.ConsolidateMultiplePackagesIntoSinglePackage = !SupportsMultiplePackages;

            if (!knowledgebaseEntry.IsNew)
            {
                // We have a valid knowledge base entry for this order, so we need to check to
                // see if we can apply ShipSense
                bool applyShipSense = true;
                if (knowledgebaseEntry.Packages.Count() > 1)
                {
                    // Don't want to apply ShipSense when the entry is configured for multiple
                    // packages and the shipment type does not support multiple packages
                    applyShipSense = SupportsMultiplePackages;
                }

                if (applyShipSense)
                {
                    // Do any shipment type specific to get the shipment in sync with the knowledge base
                    // entry (e.g. setting up the shipment to have the same number of packages as the
                    // KB entry for carriers that support multiple package shipments)
                    SyncNewShipmentWithShipSense(knowledgebaseEntry, shipment);
                    List<IPackageAdapter> packageAdapters = GetPackageAdapters(shipment).ToList();

                    if (IsCustomsRequired(shipment))
                    {
                        // Make sure the customs items are loaded before applying the knowledge base entry
                        // data to the shipment/packages and customs info otherwise the customs data of
                        // the "before" data will be empty in the first change set
                        Debug.Assert(shipment.CustomsItems.Any() || shipment.Order.OrderItems.None(), "Customs have not been loaded.  Be sure to load customs prior to calling this method.");

                        knowledgebaseEntry.ApplyTo(packageAdapters, shipment.CustomsItems);

                        if (shipment.CustomsItems.Any())
                        {
                            shipment.CustomsGenerated = true;

                            if (shipment.CustomsItems.RemovedEntitiesTracker == null)
                            {
                                // Set the removed tracker for tracking deletions in the UI until saved
                                shipment.CustomsItems.RemovedEntitiesTracker = new ShipmentCustomsItemCollection();
                            }

                            // Consider them loaded.  This is an in-memory field
                            shipment.CustomsItemsLoaded = true;

                            decimal customsValue = shipment.CustomsItems.Sum(ci => (decimal) ci.Quantity * ci.UnitValue);
                            shipment.CustomsValue = customsValue;
                        }
                    }
                    else
                    {
                        // We don't need to do anything with customs and only need to apply the package adapters
                        knowledgebaseEntry.ApplyTo(packageAdapters);
                    }

                    shipment.ContentWeight = packageAdapters.Sum(a => a.Weight);
                    UpdateTotalWeight(shipment);

                    // Update the status of the shipment and record the changes that were applied to the shipment's packages
                    shipment.ShipSenseStatus = (int) ShipSenseStatus.Applied;
                    XElement changeSets = XElement.Parse(shipment.ShipSenseChangeSets);

                    KnowledgebaseEntryChangeSetXmlWriter changeSetWriter = new KnowledgebaseEntryChangeSetXmlWriter(knowledgebaseEntry);
                    changeSetWriter.WriteTo(changeSets);

                    shipment.ShipSenseChangeSets = changeSets.ToString();
                }
            }
            else
            {
                // Note that ShipSense was not applied for the case where the shipment type
                // was changed after the shipment type was already created (i.e. going from
                // a multi-package carrier to a single package carrier when entry is configured
                // for multiple packages)
                shipment.ShipSenseStatus = (int) ShipSenseStatus.NotApplied;
            }
        }

        /// <summary>
        /// Configures the shipment for ShipSense. This is useful for carriers that support
        /// multiple package shipments, allowing the shipment type a chance to add new packages
        /// to coincide with the ShipSense knowledge base entry.
        /// </summary>
        /// <param name="knowledgebaseEntry">The knowledge base entry.</param>
        /// <param name="shipment">The shipment.</param>
        protected virtual void SyncNewShipmentWithShipSense(KnowledgebaseEntry knowledgebaseEntry, ShipmentEntity shipment)
        {
            // Nothing to do here
        }

        /// <summary>
        /// Ensures that the carrier specific data for the given profile exists and is loaded
        /// </summary>
        public virtual void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {

        }

        /// <summary>
        /// Save carrier specific profile data to the database.  Return true if anything was dirty and saved, or was deleted.
        /// </summary>
        public virtual bool SaveProfileData(ShippingProfileEntity profile, SqlAdapter adapter)
        {
            return false;
        }

        /// <summary>
        /// Allows bases classes to apply the default settings to the given profile
        /// </summary>
        public virtual void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            profile.OriginID = (int) ShipmentOriginSource.Store;

            profile.Insurance = false;
            profile.InsuranceInitialValueSource = (int) InsuranceInitialValueSource.ItemSubtotal;
            profile.InsuranceInitialValueAmount = 0;

            profile.ReturnShipment = false;

            profile.RequestedLabelFormat = (int) ThermalLanguage.None;
        }

        /// <summary>
        /// Ensures the ShipDate on an unprocessed shipment is Up-To-Date
        /// </summary>
        private void UpdateShipmentShipDate(ShipmentEntity shipment)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                var manipulator = lifetimeScope.IsRegisteredWithKey<IShipmentDateManipulator>(ShipmentTypeCode) ?
                    lifetimeScope.ResolveKeyed<IShipmentDateManipulator>(ShipmentTypeCode) :
                    lifetimeScope.Resolve<DefaultShipmentDateManipulator>();

                manipulator.Manipulate(shipment);
            }
        }

        /// <summary>
        /// Update any data that could have changed dynamically or externally
        /// </summary>
        public virtual void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            if (shipment.Processed)
            {
                InvalidOperationException ex = new InvalidOperationException("Cannot update dynamic data on a processed shipment.");
                ex.Data["UpdateDynamicData"] = true;

                throw ex;
            }

            // ensure the ship date is up-to-date
            UpdateShipmentShipDate(shipment);

            // Ensure the from address is up-to-date
            if (!UpdateOriginAddress(shipment, shipment.OriginOriginID))
            {
                shipment.OriginOriginID = (long) ShipmentOriginSource.Other;
            }
        }

        /// <summary>
        /// Update the origin address of the shipment based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public bool UpdateOriginAddress(ShipmentEntity shipment, long originID)
        {
            return UpdatePersonAddress(shipment, new PersonAdapter(shipment, "Origin"), originID);
        }

        /// <summary>
        /// Update the person address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingOriginManager shippingOriginManager = lifetimeScope.Resolve<IShippingOriginManager>();

                PersonAdapter originAddress = shippingOriginManager.GetOriginAddress(originID, shipment);

                if (originAddress != null)
                {
                    originAddress.CopyTo(person);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Update the total weight of the shipment based on its ContentWeight and any packaging weight.  The default
        /// implementation is just to set the TotalWeight equal to the ContentWeight.
        /// </summary>
        public virtual void UpdateTotalWeight(ShipmentEntity shipment)
        {
            if (shipment.Processed)
            {
                throw new InvalidOperationException("Cannot update weight on a processed shipment.");
            }

            shipment.TotalWeight = shipment.ContentWeight;
        }

        /// <summary>
        /// Get the available origins for the ShipmentType.  This is used to display the origin address UI.
        /// </summary>
        public virtual List<KeyValuePair<string, long>> GetOrigins()
        {
            List<KeyValuePair<string, long>> origins = new List<KeyValuePair<string, long>>();

            if (SupportsAccountAsOrigin)
            {
                origins.Add(new KeyValuePair<string, long>("Account Address", (long) ShipmentOriginSource.Account));
            }

            // Add all the shippers
            foreach (ShippingOriginEntity origin in ShippingOriginManager.Origins)
            {
                origins.Add(new KeyValuePair<string, long>(origin.Description, origin.ShippingOriginID));
            }

            // Add store and manual always
            origins.Add(new KeyValuePair<string, long>("Store Address", (long) ShipmentOriginSource.Store));
            origins.Add(new KeyValuePair<string, long>("Other Address", (long) ShipmentOriginSource.Other));

            return origins;
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public abstract string GetServiceDescription(ShipmentEntity shipment);

        /// <summary>
        /// Get the carrier specific description of the shipping service used, overridden by shipment types to provide a
        /// compatible description for special one off service types like USPS GlobalPost
        /// </summary>
        public virtual string GetOveriddenServiceDescription(ShipmentEntity shipment)
        {
            return GetServiceDescription(shipment);
        }

        /// <summary>
        /// Can be overridden by derived classes to provide common carrier details of shipments
        /// </summary>
        public virtual ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            return new ShipmentCommonDetail();
        }

        /// <summary>
        /// Get the total number of parcels contained in the shipment.  For shipments without "packages", the shipment itself counts as 1.
        /// </summary>
        public virtual int GetParcelCount(ShipmentEntity shipment)
        {
            return 1;
        }

        /// <summary>
        /// Get detailed information about the parcel in a generic way that can be used across shipment types
        /// </summary>
        public abstract ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex);

        /// <summary>
        /// Get the tracking numbers for the shipment.  This can include extra text, such as "Package 1: (track#)"
        /// </summary>
        public virtual List<string> GetTrackingNumbers(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return new List<string>();
            }

            return new List<string> { shipment.TrackingNumber };
        }

        /// <summary>
        /// Apply the specified shipment profile to the given shipment.
        /// </summary>
        public virtual void ApplyProfile(ShipmentEntity shipment, IShippingProfileEntity profile)
        {
            ShippingProfileUtility.ApplyProfileValue(profile.OriginID, shipment, ShipmentFields.OriginOriginID);
            ShippingProfileUtility.ApplyProfileValue(profile.ReturnShipment, shipment, ShipmentFields.ReturnShipment);

            ShippingProfileUtility.ApplyProfileValue(profile.RequestedLabelFormat, shipment, ShipmentFields.RequestedLabelFormat);
            SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment);

            // Special case for insurance
            for (int i = 0; i < GetParcelCount(shipment); i++)
            {
                IInsuranceChoice insuranceChoice = GetParcelDetail(shipment, i).Insurance;

                if (profile.Insurance != null)
                {
                    insuranceChoice.Insured = profile.Insurance.Value;
                }

                if (profile.InsuranceInitialValueSource != null)
                {
                    // Don't apply the value to the subsequent parcels - that would probably end up over-ensuring the whole shipment.
                    if (i == 0)
                    {
                        InsuranceInitialValueSource source = (InsuranceInitialValueSource) profile.InsuranceInitialValueSource;
                        insuranceChoice.InsuranceValue = InsuranceUtility.GetInsuranceValue(shipment, source, profile.InsuranceInitialValueAmount);
                    }
                }
            }
        }

        /// <summary>
        /// Must be overridden by derived types to provide tracking details for the given shipment.
        /// </summary>
        public virtual TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            throw new ShippingException(string.Format("Tracking is not supported for {0}.", ShipmentTypeName));
        }

        /// <summary>
        /// Create the XML elements that are the input to the XSL engine for the given shipment
        /// </summary>
        public virtual void GenerateTemplateElements(ElementOutline container, Func<ShipmentEntity> shipment, Func<ShipmentEntity> loaded)
        {

        }

        /// <summary>
        /// Returns currency to be used for a country
        /// </summary>
        /// <param name="countryCode"></param>
        public static CurrencyType GetCurrencyForCountryCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                return CurrencyType.USD;
            }

            switch (countryCode.ToUpperInvariant())
            {
                case "CA":
                    return CurrencyType.CAD;
                case "US":
                    return CurrencyType.USD;
                default:
                    return CurrencyType.USD;
            }
        }

        /// <summary>
        /// Returns a url to the carrier's website for the specified shipment
        /// </summary>
        public virtual string GetCarrierTrackingUrl(ShipmentEntity shipment)
        {
            return string.Empty;
        }

        /// <summary>
        /// Determines if a shipment will be domestic or international
        /// </summary>
        /// <param name="shipmentEntity"></param>
        public virtual bool IsDomestic(IShipmentEntity shipmentEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentEntity, nameof(shipmentEntity));

            return shipmentEntity.AdjustedOriginCountryCode().ToUpperInvariant() == shipmentEntity.AdjustedShipCountryCode().ToUpperInvariant();
        }

        /// <summary>
        /// Gets whether the specified settings tab should be hidden in the UI
        /// </summary>
        public virtual bool IsSettingsTabHidden(ShipmentTypeSettingsControl.Page tab)
        {
            return false;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for a provider based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of an IBestRateShippingBroker.</returns>
        public abstract IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment);

        /// <summary>
        /// Clear any data that should not be part of a shipment after it has been copied.
        /// </summary>
        public virtual void ClearDataForCopiedShipment(ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        /// <remarks>
        /// This override is only needed for BestRate since it loads data into the shipment.
        /// No other shipment type needs this to be a concrete type.
        /// </remarks>
        public virtual bool IsCustomsRequired(ShipmentEntity shipment) =>
            IsCustomsRequired(shipment as IShipmentEntity);

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        protected bool IsCustomsRequired(IShipmentEntity shipment)
        {
            // Some carts have an international shipping program in place that allow
            // sellers to ship international orders to a domestic facility meaning
            // customs is not required despite the international shipping address, so
            // let the store take a look at the shipment as well to determine if customs
            // are required in addition to the just looking at the shipping address.
            bool requiresCustoms = IsCustomsRequiredByShipment(shipment);

            if (requiresCustoms)
            {
                // This shipment requires customs based on the shipping address
                // but allow the store to have the final say
                long storeID = shipment.Order?.StoreID ?? DataProvider.GetOrderHeader(shipment.OrderID).StoreID;
                StoreType storeType = StoreTypeManager.GetType(StoreManager.GetStore(storeID));

                // Pass a true value indicating customs is required based on the shipping address
                requiresCustoms = storeType.IsCustomsRequired(shipment, true);
            }

            return requiresCustoms;
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address.
        /// </summary>
        [NDependIgnoreComplexMethodAttribute]
        protected virtual bool IsCustomsRequiredByShipment(IShipmentEntity shipment)
        {
            bool requiresCustoms = !IsDomestic(shipment);

            if (shipment.AdjustedShipCountryCode() == "US")
            {
                if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode))
                {
                    requiresCustoms = true;
                }

                // Show customs tab from any international territory (except Puerto Rico) to US
                if (shipment.OriginPerson.IsUSInternationalTerritory() &&
                    !shipment.OriginPerson.IsUSInternationalTerritory("PR") &&
                    !shipment.ShipPerson.IsUSInternationalTerritory())
                {
                    requiresCustoms = true;
                }

                // Foreign US territories requiring customs forms - http://pe.usps.com/text/dmm300/608.htm
                if (shipment.ShipPostalCode.StartsWith("96910") ||
                    shipment.ShipPostalCode.StartsWith("96912") ||
                    shipment.ShipPostalCode.StartsWith("96913") ||
                    shipment.ShipPostalCode.StartsWith("96915") ||
                    shipment.ShipPostalCode.StartsWith("96916") ||
                    shipment.ShipPostalCode.StartsWith("96917") ||
                    shipment.ShipPostalCode.StartsWith("96919") ||
                    shipment.ShipPostalCode.StartsWith("96921") ||
                    shipment.ShipPostalCode.StartsWith("96923") ||
                    shipment.ShipPostalCode.StartsWith("96928") ||
                    shipment.ShipPostalCode.StartsWith("96929") ||
                    shipment.ShipPostalCode.StartsWith("96931") ||
                    shipment.ShipPostalCode.StartsWith("96932") ||
                    shipment.ShipPostalCode.StartsWith("96939") ||
                    shipment.ShipPostalCode.StartsWith("96940") ||
                    shipment.ShipPostalCode.StartsWith("96941") ||
                    shipment.ShipPostalCode.StartsWith("96942") ||
                    shipment.ShipPostalCode.StartsWith("96943") ||
                    shipment.ShipPostalCode.StartsWith("96944") ||
                    shipment.ShipPostalCode.StartsWith("96950") ||
                    shipment.ShipPostalCode.StartsWith("96951") ||
                    shipment.ShipPostalCode.StartsWith("96952") ||
                    shipment.ShipPostalCode.StartsWith("96960") ||
                    shipment.ShipPostalCode.StartsWith("96970") ||
                    shipment.ShipPostalCode.StartsWith("96799"))
                {
                    requiresCustoms = true;
                }
            }

            return requiresCustoms;
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public virtual void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Gets whether the shipment is going from the US to PR or from PR to US
        /// </summary>
        /// <param name="shipment">Shipment that should be checked</param>
        /// <returns></returns>
        /// <remarks>This check handles the situation where a PR address has US as the country but PR as the state</remarks>
        public static bool IsShipmentBetweenUnitedStatesAndPuertoRico(IShipmentEntity shipment)
        {
            return (shipment.OriginCountryCode.Equals("US", StringComparison.OrdinalIgnoreCase) &&
                    !IsPuertoRicoAddress(shipment.OriginPerson) &&
                    IsPuertoRicoAddress(shipment.ShipPerson)) ||
                   (IsPuertoRicoAddress(shipment.OriginPerson) &&
                    shipment.ShipCountryCode.Equals("US", StringComparison.OrdinalIgnoreCase) &&
                    !IsPuertoRicoAddress(shipment.ShipPerson));
        }

        /// <summary>
        /// Gets whether the entity's specified address is in Puerto Rico
        /// </summary>
        /// <param name="entity">Entity whose address should be checked</param>
        /// <param name="fieldPrefix">Prefix of the address that should be checked</param>
        /// <returns>True if the address is a Puerto Rico address or false if not</returns>
        /// <remarks>This check handles situations where the address has US as the country but PR as the state</remarks>
        public static bool IsPuertoRicoAddress(EntityBase2 entity, string fieldPrefix) =>
            IsPuertoRicoAddress(new PersonAdapter(entity, fieldPrefix));

        /// <summary>
        /// Gets whether the entity's specified address is in Puerto Rico
        /// </summary>
        /// <param name="entity">Entity whose address should be checked</param>
        /// <param name="fieldPrefix">Prefix of the address that should be checked</param>
        /// <returns>True if the address is a Puerto Rico address or false if not</returns>
        /// <remarks>This check handles situations where the address has US as the country but PR as the state</remarks>
        public static bool IsPuertoRicoAddress(PersonAdapter address)
        {
            return address.CountryCode.Equals("PR", StringComparison.OrdinalIgnoreCase) ||
                (address.CountryCode.Equals("US", StringComparison.OrdinalIgnoreCase) && address.StateProvCode.Equals("PR", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public virtual void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            // Default will have nothing to update
        }        
    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Xml.Linq;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
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
		private static readonly ILog log = LogManager.GetLogger(typeof(ShipmentType));

		/// <summary>
		/// HTTPS certificate inspector to use.
		/// </summary>
		private ICertificateInspector certificateInspector;

		private static object syncLock = new object();
		private readonly IDataProvider dataProvider;

		/// <summary>
		/// Constructor
		/// </summary>
		protected ShipmentType(IDataProvider dataProvider)
		{
			this.dataProvider = dataProvider;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected ShipmentType() : this(new DataProviderWrapper(new SqlAdapterFactory()))
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
		public virtual string ShipmentTypeName => EnumHelper.GetDescription(ShipmentTypeCode);

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
		public virtual bool SupportsGetRates => false;

		/// <summary>
		/// Indicates if the shipment service type supports return shipments
		/// </summary>
		public virtual bool SupportsReturns => false;

		/// <summary>
		/// Supports using an origin address from a shipping account
		/// </summary>
		public virtual bool SupportsAccountAsOrigin => false;

		/// <summary>
		/// Supports getting counter rates.
		/// </summary>
		public virtual bool SupportsCounterRates => false;

		/// <summary>
		/// Gets a value indicating whether the shipment type [supports multiple packages].
		/// </summary>
		/// <value>
		/// <c>true</c> if [supports multiple packages]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool SupportsMultiplePackages => false;

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
			get => certificateInspector;
			set => certificateInspector = value;
		}

		/// <summary>
		/// Gets a value indicating whether account registration allowed for this shipment type.
		/// </summary>
		public virtual bool IsAccountRegistrationAllowed => GetRestrictionLevel(EditionFeature.ShipmentTypeRegistration) == EditionRestrictionLevel.None;

		/// <summary>
		/// Gets a value indicating whether this shipment type has been restricted.
		/// </summary>
		public virtual bool IsShipmentTypeRestricted => GetRestrictionLevel(EditionFeature.ShipmentType) != EditionRestrictionLevel.None;

		/// <summary>
		/// Gets a value indicating whether this shipment type has rate discount messaging restricted. This will mean different things to different shipment types.
		/// </summary>
		public bool IsRateDiscountMessagingRestricted => GetRestrictionLevel(EditionFeature.RateDiscountMessaging) != EditionRestrictionLevel.None;

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
			return true;
			//As of October 2023 Prime orders can be sent using any carrier, not only Amazon Buy Shipping
			//IAmazonOrder order;
			//if (shipment.Order == null)
			//{
			//    order = dataProvider.GetEntity(shipment.OrderID) as IAmazonOrder;
			//}
			//else
			//{
			//    order = shipment.Order as IAmazonOrder;
			//}

			//// If the order is Amazon Prime return false
			//return !order?.IsPrime ?? true;
		}

		/// <summary>
		/// Gets a value indicating whether this shipment type has accounts
		/// </summary>
		public virtual bool HasAccounts => false;

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
		protected virtual SettingsControlBase CreateSettingsControlInternal(ILifetimeScope scope)
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
				CreateSettingsControlInternal(lifetimeScope);
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
		/// Set the shipments dimensions
		/// </summary>
		private void SetPackageDimensions(ShipmentEntity shipment, ILifetimeScope lifetimeScope)
		{
			ICarrierShipmentAdapterFactory shipmentAdapterFactory = lifetimeScope.Resolve<ICarrierShipmentAdapterFactory>();
			IPackageAdapter package = shipmentAdapterFactory.Get(shipment).GetPackageAdapters().Single();

			package.DimsLength = 0;
			package.DimsWidth = 0;
			package.DimsHeight = 0;

			if (shipment.Order.OrderItems.Count() == 1)
			{
				OrderItemEntity item = shipment.Order.OrderItems.Single();

				if (item.Quantity.IsEquivalentTo(1))
				{
					package.DimsLength = (double) item.Length;
					package.DimsWidth = (double) item.Width;
					package.DimsHeight = (double) item.Height;
				}
			}
		}

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
				IShippingProfileService shippingProfileService = lifetimeScope.Resolve<IShippingProfileService>();

				SetPackageDimensions(shipment, lifetimeScope);

				// LoadCustomsItems no longer automatically saves the shipment, so we can call it here
				customsManager.LoadCustomsItems(shipment, false, x => true);

				// First apply the base profile
				ShippingProfileEntity primaryProfile = shippingProfileManager.GetOrCreatePrimaryProfile(this);
				if (primaryProfile.IsNew)
				{
					shippingProfileManager.SaveProfile(primaryProfile);
				}

				var shippingProfile = shippingProfileService.Get(primaryProfile);

				// Save the original ReturnShipment value 
				bool originalReturnShipment = shipment.ReturnShipment;

				// Apply the Shipping profile
				shippingProfile.Apply(shipment);

				// Reset ReturnShipment to the original value if IncludeReturn hasn't been set
				if (!shipment.IncludeReturn)
				{
					shipment.ReturnShipment = originalReturnShipment;
				}

				// Now apply ShipSense
				ApplyShipSense(shipment, lifetimeScope);

				ApplyShippingRules(shipment, shippingProfile, shippingProfileManager, shippingProfileService);

				UpdateTotalWeight(shipment);

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
		/// Attempts to apply shipping rules to the given shipment.
		/// </summary>
		private void ApplyShippingRules(ShipmentEntity shipment, IShippingProfile shippingProfile,
			IShippingProfileManager shippingProfileManager, IShippingProfileService shippingProfileService)
		{
			using (ITrackedEvent telementryEvent = new TrackedEvent("ShipmentType"))
			{
				TelemetricResult<Unit> telemetricResult = new TelemetricResult<Unit>("ApplyShippingRules");

				telemetricResult.RunTimedEvent("Time(ms)", () =>
				{
					ApplyShippingRulesInternal(shipment, shippingProfile, shippingProfileManager, shippingProfileService);
				});

				telemetricResult.WriteTo(telementryEvent);
			}
		}

		/// <summary>
		/// Attempts to apply shipping rules to the given shipment.
		/// </summary>
		private void ApplyShippingRulesInternal(ShipmentEntity shipment, IShippingProfile shippingProfile,
			IShippingProfileManager shippingProfileManager, IShippingProfileService shippingProfileService)
		{
			// Go through each additional profile and apply it as well
			foreach (ShippingDefaultsRuleEntity rule in ShippingDefaultsRuleManager.GetRules(ShipmentTypeCode))
			{
				if (shippingProfile.ShippingProfileEntity.ShippingProfileID != rule.ShippingProfileID &&
					FilterHelper.IsObjectInFilterContent(shipment.OrderID, rule.FilterNodeID))
				{
					IShippingProfileEntity profile = shippingProfileManager.GetProfileReadOnly(rule.ShippingProfileID);
					if (profile != null)
					{
						shippingProfileService.Get(profile).Apply(shipment);
					}
				}
			}
		}

		/// <summary>
		/// Attempts to apply ShipSense values to the given shipment.
		/// </summary>
		[NDependIgnoreLongMethod]
		private void ApplyShipSense(ShipmentEntity shipment, ILifetimeScope lifetimeScope)
		{
			if (!ShouldApplyShipSense)
			{
				return;
			}

			IShippingSettings shippingSettings = lifetimeScope.Resolve<IShippingSettings>();
			bool shipSenseEnabled = shippingSettings.FetchReadOnly().ShipSenseEnabled;
			if (!shipSenseEnabled)
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
			IKnowledgebase knowledgebase = lifetimeScope.Resolve<IKnowledgebase>();
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
		/// Allows bases classes to apply the default settings to the given profile
		/// </summary>
		public virtual void ConfigurePrimaryProfile(ShippingProfileEntity profile)
		{
			profile.OriginID = (int) ShipmentOriginSource.Store;

			profile.Insurance = false;
			profile.InsuranceInitialValueSource = (int) InsuranceInitialValueSource.ItemSubtotal;
			profile.InsuranceInitialValueAmount = 0;

			profile.ReturnShipment = false;
			profile.IncludeReturn = false;
			profile.ApplyReturnProfile = false;
			profile.ReturnProfileID = -1;

			profile.RequestedLabelFormat = (int) ThermalLanguage.None;

			//Single package carriers only have one package profile, initialize it now
			if (!SupportsMultiplePackages)
			{
				// LoadPackageProfile sets up the profile before ConfigurePrimaryProfile is called and creates
				// an in memory PackageProfile with null fields. Let's clear it out and create a new one with initial values.
				profile.Packages.Clear();
				profile.Packages.Add(new PackageProfileEntity()
				{
					Weight = 0,
					DimsProfileID = 0,
					DimsLength = 0,
					DimsWidth = 0,
					DimsHeight = 0,
					DimsWeight = 0,
					DimsAddWeight = true
				});
			}
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
		/// Update the total weight of the shipment based on its ContentWeight and any packaging weight.
		/// </summary>
		public virtual void UpdateTotalWeight(ShipmentEntity shipment)
		{
			if (shipment.Processed)
			{
				throw new InvalidOperationException("Cannot update weight on a processed shipment.");
			}

			if (SupportsMultiplePackages)
			{
				UpdateMultiplePackageWeight(shipment);
			}
			else
			{
				UpdateSinglePackageWeight(shipment);
			}
		}

		/// <summary>
		/// Update weight for multi-package shipments
		/// </summary>
		/// <remarks>
		/// We don't want to update the weight unless it's actually different because we now use the notify property changed
		/// event on the entities and it seems to fire regardless of whether the value has actually changed.  This could also
		/// be caused by the fact that weight is a double, and 1.5 does not equal 1.500000001
		/// </remarks>
		private void UpdateMultiplePackageWeight(ShipmentEntity shipment)
		{
			MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

			double contentWeight = 0;
			double totalWeight = 0;

			var packageDimensions = GetPackageWeights(shipment) ?? Enumerable.Empty<(double, bool, double)>();
			foreach ((double weight, bool addDimsWeight, double dimsWeight) in packageDimensions)
			{
				contentWeight += weight;
				totalWeight += weight;

				if (addDimsWeight)
				{
					totalWeight += dimsWeight;
				}
			}

			if (!contentWeight.IsEquivalentTo(shipment.ContentWeight))
			{
				shipment.ContentWeight = contentWeight;
			}

			if (!totalWeight.IsEquivalentTo(shipment.TotalWeight))
			{
				shipment.TotalWeight = totalWeight;
			}
		}

		/// <summary>
		/// Update weight for a single-package shipments
		/// </summary>
		/// <remarks>
		/// We don't want to update the weight unless it's actually different because we now use the notify property changed
		/// event on the entities and it seems to fire regardless of whether the value has actually changed.  This could also
		/// be caused by the fact that weight is a double, and 1.5 does not equal 1.500000001
		/// </remarks>
		private void UpdateSinglePackageWeight(ShipmentEntity shipment)
		{
			MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

			var newWeight = shipment.ContentWeight + GetDimsWeight(shipment);
			if (!newWeight.IsEquivalentTo(shipment.TotalWeight))
			{
				shipment.TotalWeight = newWeight;
			}
		}

		/// <summary>
		/// Get weights from packages
		/// </summary>
		protected virtual IEnumerable<(double weight, bool addDimsWeight, double dimsWeight)> GetPackageWeights(IShipmentEntity shipment) =>
			Enumerable.Empty<(double, bool, double)>();

		/// <summary>
		/// Get the dims weight from a shipment, if any
		/// </summary>
		protected virtual double GetDimsWeight(IShipmentEntity shipment) => 0;

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
		/// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
		/// when this method is called.
		/// </summary>
		public abstract string GetServiceDescription(string serviceCode);

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
		[SuppressMessage("ShipWorks", "SW0002:Identifier should not be obfuscated",
		Justification = "Identifier is not being used for data binding")]
		public string GetCarrierTrackingUrl(ShipmentEntity shipment)
		{
			if (shipment == null)
			{
				throw new ArgumentNullException(nameof(shipment));
			}

			if (!shipment.Processed || string.IsNullOrEmpty(shipment.TrackingNumber))
			{
				return string.Empty;
			}

			return GetCarrierTrackingUrlInternal(shipment);
		}

		/// <summary>
		/// Common tracking method
		/// </summary>
		protected virtual string GetCarrierTrackingUrlInternal(ShipmentEntity shipment) => string.Empty;

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
		public abstract IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository);

		/// <summary>
		/// Clear any data that should not be part of a shipment after it has been copied.
		/// </summary>
		public virtual void ClearDataForCopiedShipment(ShipmentEntity shipment)
		{

		}

		/// <summary>
		/// Rectifies carrier specific data on the shipment
		/// </summary>
		/// <remarks>
		/// This allows the ShipmentType to fix any issues on the shipment
		/// for example if the service is not valid for the ship to country
		/// or if the packaging type is not valid for the service type
		/// </remarks>
		public virtual void RectifyCarrierSpecificData(ShipmentEntity shipment)
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

		/// <summary>
		/// Sets a shipment and its packages to have no insurance
		/// </summary>
		public void UnsetInsurance(ShipmentEntity shipment)
		{
			shipment.Insurance = false;

			var packages = GetPackageAdapters(shipment);

			packages.ForEach(x => x.InsuranceChoice.Insured = false);
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac.Features.Indexed;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Best rate implementation of ShipmentType
    /// </summary>
    public class 
        BestRateShipmentType : ShipmentType
    {
        private readonly ILog log;
        private readonly IBestRateShippingBrokerFactory brokerFactory;
        private readonly IIndex<ShipmentTypeCode, SettingsControlBase> settingsControlFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShipmentType" /> class. This version of
        /// the constructor is primarily for testing purposes.
        /// </summary>
        public BestRateShipmentType(IBestRateShippingBrokerFactory brokerFactory, Func<Type, ILog> createLogger,
                                    IIndex<ShipmentTypeCode, SettingsControlBase> settingsControlFactory)
        {
            this.brokerFactory = brokerFactory;
            log = createLogger(typeof(BestRateShipmentType));
            this.settingsControlFactory = settingsControlFactory;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.BestRate;

        /// <summary>
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public override bool SupportsGetRates => true;

        /// <summary>
        /// Indicates that this shipment type supports shipping from an account address
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Create the UserControl used to handle best rate shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new BestRateServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.BestRate == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new BestRatePackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Allows bases classes to apply the default settings to the given profile
        /// </summary>
        public override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            log.Warn("ConfigurePrimaryProfile called for BestRateShipmentType.");
            Debug.Assert(false, "ConfigurePrimaryProfile maybe shouldn't be called for BestRateShipmentType.");
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "BestRate", typeof(BestRateShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(string serviceCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the insurance data that describes what type of insurance is being used and on what parcels.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">shipment</exception>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment.BestRate, shipment.BestRate, null),
                new DimensionsAdapter(shipment.BestRate));
        }

        /// <summary>
        /// Gets whether the specified settings tab should be hidden in the UI
        /// </summary>
        public override bool IsSettingsTabHidden(ShipmentTypeSettingsControl.Page tab)
        {
            return tab == ShipmentTypeSettingsControl.Page.Actions || tab == ShipmentTypeSettingsControl.Page.Printing;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for a provider based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a NullShippingBroker since this is not applicable to the best rate shipment type.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Creates the UserControl that is used to edit the defaults\settings for the service
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl() => settingsControlFactory[ShipmentTypeCode.BestRate];

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        public override bool IsCustomsRequired(ShipmentEntity shipment)
        {
            // Make sure the best rate shipment data is loaded (in the event that we're
            // coming from somewhere other than the shipping screen)
            LoadShipmentData(shipment, false);

            IEnumerable<IBestRateShippingBroker> brokers = brokerFactory.CreateBrokers(shipment);
            return brokers.Any(b => b.IsCustomsRequired(shipment));
        }

        /// <summary>
        /// Applies the selected rate to the specified shipment
        /// </summary>
        /// <param name="shipment">Shipment that will have the rate applied</param>
        /// <param name="bestRate">Rate that should be applied to the shipment</param>
        public static void ApplySelectedShipmentRate(ShipmentEntity shipment, RateResult bestRate)
        {
            AddBestRateEvent(shipment, BestRateEventTypes.RateSelected);
            BestRateEventTypes originalEventTypes = (BestRateEventTypes) shipment.BestRateEvents;

            BestRateResultTag bestRateResultTag = ((BestRateResultTag) bestRate.Tag);

            bestRateResultTag.RateSelectionDelegate(shipment);

            // Reset the event types after the selected shipment has been applied to
            // avoid losing them during the transition to the targeted shipment type
            shipment.BestRateEvents = (byte) originalEventTypes;
        }

        /// <summary>
        /// Apply the configured defaults and profile rule settings to the given shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.BestRate == null)
            {
                shipment.BestRate = new BestRateShipmentEntity(shipment.ShipmentID);
            }

            shipment.BestRate.InsuranceValue = 0;
            shipment.BestRate.Insurance = false;

            shipment.BestRate.RequestedLabelFormat = (int) ThermalLanguage.None;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Configure a new shipment for Best Rate rating.  
        /// </summary>
        public static void ConfigureNewShipmentForRating(ShipmentType shipmentType, ShipmentEntity shipment)
        {
            ShipmentCustomsItemEntity[] originalCustomsItemEntities = new ShipmentCustomsItemEntity[shipment.CustomsItems.Count];
            shipment.CustomsItems.CopyTo(originalCustomsItemEntities, 0);

            shipmentType.ConfigureNewShipment(shipment);

            shipment.CustomsItems.Clear();
            shipment.CustomsItems.AddRange(originalCustomsItemEntities);
            shipment.CustomsItems.RemovedEntitiesTracker?.Clear();
        }

        /// <summary>
        /// Update any data that could have changed dynamically or externally
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            InsuranceProvider shipmentInsuranceProvider = GetShipmentInsuranceProvider(shipment);

            shipment.InsuranceProvider = (int) shipmentInsuranceProvider;
            shipment.Insurance = shipment.BestRate.Insurance;

            shipment.RequestedLabelFormat = shipment.BestRate.RequestedLabelFormat;
        }

        /// <summary>
        /// Gets the shipment insurance provider based on carriers selected.
        /// </summary>
        /// <returns></returns>
        public InsuranceProvider GetShipmentInsuranceProvider(ShipmentEntity shipment)
        {
            IShippingSettingsEntity settings = ShippingSettings.FetchReadOnly();
            IEnumerable<IBestRateShippingBroker> brokersWithAccounts = brokerFactory.CreateBrokers(shipment).Where(b => b.HasAccounts).ToList();

            // Default shipmentInsuranceProvider is ShipWorks
            InsuranceProvider shipmentInsuranceProvider;

            if (brokersWithAccounts.Count() == 1)
            {
                // If 1 carrier, use that carrier's insurance provider
                shipmentInsuranceProvider = brokersWithAccounts.First().GetInsuranceProvider(settings);
            }
            else if (brokersWithAccounts.Any())
            {
                // If more than 1 carrier, if any of the carrier's are not ShipWorks insurance, set to invalid.
                if (brokersWithAccounts.Any(b => b.GetInsuranceProvider(settings) != InsuranceProvider.ShipWorks))
                {
                    shipmentInsuranceProvider = InsuranceProvider.Invalid;
                }
                else
                {
                    shipmentInsuranceProvider = InsuranceProvider.ShipWorks;
                }
            }
            else
            {
                // No brokersWithAccounts
                shipmentInsuranceProvider = InsuranceProvider.Invalid;
            }

            return shipmentInsuranceProvider;
        }

        /// <summary>
        /// Adds the best rate event.
        /// </summary>
        public static void AddBestRateEvent(ShipmentEntity shipment, BestRateEventTypes eventType)
        {
            if ((shipment.BestRateEvents & (byte) BestRateEventTypes.RateAutoSelectedAndProcessed) != (byte) BestRateEventTypes.RateAutoSelectedAndProcessed)
            {
                // User already processed it, don't give credit for getting rates which happens during process...
                shipment.BestRateEvents |= (byte) eventType;
            }
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.BestRate != null)
            {
                shipment.BestRate.RequestedLabelFormat = (int) requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.BestRateShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new BestRateShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Get the dims weight from a shipment, if any
        /// </summary>
        protected override double GetDimsWeight(IShipmentEntity shipment) =>
            shipment.BestRate?.DimsAddWeight == true ? shipment.BestRate.DimsWeight : 0;
    }
}

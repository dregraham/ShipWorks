﻿using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Services;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.ShipEngine;
using Interapptive.Shared.Enums;
using ShipWorks.Shipping.Insurance;
using System.Linq;
using ShipWorks.Data.Model.EntityInterfaces;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Asendia implementation of shipment type
    /// </summary>
    [Component(RegistrationType.Self)]
    [KeyedComponent(typeof(ShipmentType), ShipmentTypeCode.Asendia, SingleInstance = true)]
    public class AsendiaShipmentType : ShipmentType
    {
        private readonly ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsendiaShipmentType(ICarrierAccountRepository<AsendiaAccountEntity, IAsendiaAccountEntity> accountRepository, IShipEngineWebClient shipEngineWebClient, IShipEngineTrackingResultFactory trackingResultFactory, IShippingManager shippingManager)
        {
            this.accountRepository = accountRepository;
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.Asendia;

        /// <summary>
        /// Whether multiple packages are supported by this shipment type.
        /// </summary>
        public override bool SupportsMultiplePackages => false;

        /// <summary>
        /// Supports using an origin address from a shipping account
        /// </summary>
        public override bool SupportsAccountAsOrigin => true;

        /// <summary>
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public override bool SupportsGetRates => false;

        /// <summary>
        /// Create and Initialize a new shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            if (shipment.Asendia == null)
            {
                shipment.Asendia = new AsendiaShipmentEntity(shipment.ShipmentID);
            }

            AsendiaShipmentEntity asendiaShipment = shipment.Asendia;
            asendiaShipment.Service = AsendiaServiceType.AsendiaPriorityTracked;
            asendiaShipment.RequestedLabelFormat = (int) ThermalLanguage.None;
            asendiaShipment.Contents = (int) ShipEngineContentsType.Merchandise;
            asendiaShipment.NonDelivery = (int) ShipEngineNonDeliveryType.ReturnToSender;
            asendiaShipment.NonMachinable = false;
            asendiaShipment.AsendiaAccountID = 0;
            asendiaShipment.ShipEngineLabelID = string.Empty;
            asendiaShipment.DimsProfileID = 0;
            asendiaShipment.DimsLength = 0;
            asendiaShipment.DimsWidth = 0;
            asendiaShipment.DimsHeight = 0;
            asendiaShipment.DimsWeight = 0;
            asendiaShipment.DimsAddWeight = true;
            asendiaShipment.Insurance = false;
            asendiaShipment.InsuranceValue = 0;
            asendiaShipment.TrackingNumber = string.Empty;

            base.ConfigureNewShipment(shipment);
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.Asendia == null)
            {
                shippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new AsendiaPackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Get the parcel data that describes details about a particular parcel
        /// </summary>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment, shipment.Asendia, shipment.Asendia),
                new DimensionsAdapter(shipment.Asendia))
            {
                TotalWeight = shipment.TotalWeight
            };
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            return EnumHelper.GetDescription(shipment.Asendia.Service);
        }

        /// <summary>
        /// Gets the best rate shipping broker for Asendia
        /// </summary>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the dynamic shipment data that could have changed "outside" the known editor
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);
            
            shipment.Insurance = shipment.Asendia.Insurance;
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;

            shipment.RequestedLabelFormat = shipment.Asendia.RequestedLabelFormat;
        }

        /// <summary>
        /// Update the total weight of the shipment
        /// </summary>
        public override void UpdateTotalWeight(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            shipment.TotalWeight = shipment.ContentWeight;

            if (shipment.Asendia.DimsAddWeight)
            {
                shipment.TotalWeight += shipment.Asendia.DimsWeight;
            }
        }

        /// <summary>
        /// Get the shipment common detail for tango
        /// </summary>
        public override ShipmentCommonDetail GetShipmentCommonDetail(ShipmentEntity shipment)
        {
            ShipmentCommonDetail commonDetail = new ShipmentCommonDetail();

            AsendiaShipmentEntity asendiaShipmentEntity = shipment.Asendia;
            AsendiaAccountEntity account = accountRepository.GetAccount(asendiaShipmentEntity.AsendiaAccountID);

            commonDetail.OriginAccount = (account == null) ? "" : account.Description;
            commonDetail.ServiceType = (int) asendiaShipmentEntity.Service;

            // Asendia doesn't have a packaging type concept, so default to 0
            commonDetail.PackagingType = 0;
            commonDetail.PackageLength = asendiaShipmentEntity.DimsLength;
            commonDetail.PackageWidth = asendiaShipmentEntity.DimsWidth;
            commonDetail.PackageHeight = asendiaShipmentEntity.DimsHeight;

            return commonDetail;
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.AsendiaShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new AsendiaShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        /// <remarks>
        /// Asendia only supports international shipments, so customs is always required
        /// </remarks>
        protected override bool IsCustomsRequiredByShipment(ShipmentEntity shipment) => true;

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the Asendia data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        protected override void LoadShipmentDataInternal(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(
                            this, shipment, shipment, "Asendia", typeof(AsendiaShipmentEntity), refreshIfPresent);
        }
    }
}

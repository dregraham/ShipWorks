﻿using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// LabelService for Ups World Ship
    /// </summary>
    public class WorldShipLabelService : UpsLabelService
    {
        private readonly Func<UpsLabelResponse, WorldShipDownloadedLabelData> createDownloadedLabelData;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorldShipLabelService(Func<UpsLabelResponse, WorldShipDownloadedLabelData> createDownloadedLabelData, 
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> upsAccountRepository, 
            IUpsLabelClientFactory upsLabelClientFactory, IIndex<ShipmentTypeCode, ICarrierSettingsRepository> settingsRepository)
            : base(upsLabelClientFactory, settingsRepository)
        {
            this.createDownloadedLabelData = createDownloadedLabelData;
            this.upsAccountRepository = upsAccountRepository;
        }

        /// <summary>
        /// Creates the label
        /// </summary>
        public override Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            try
            {
                if (!string.IsNullOrEmpty(upsAccountRepository.GetAccountReadOnly(shipment).ShipEngineCarrierId))
                {
                    throw new ShippingException("This account cannot be used to process shipments using WorldShip.");
                }

                base.Create(shipment);

                TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);
                IDownloadedLabelData downloadedLabelData = createDownloadedLabelData(new UpsLabelResponse { Shipment = shipment });
                telemetricResult.SetValue(downloadedLabelData);
                
                return Task.FromResult(telemetricResult);
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
            catch (TemplateTokenException templateTokenException)
            {
                throw new ShippingException("ShipWorks encountered an error attempting to process the shipment.", templateTokenException);
            }
        }

        /// <summary>
        /// Voids the label
        /// </summary>
        public override void Void(ShipmentEntity shipment)
        {
            // If it's been exported but not yet processed in WorldShip, then don't actually void the underlying shipment, but
            // we do have to remove the entry from the table
            if (shipment.Ups.WorldShipStatus == (int) WorldShipStatusType.Exported)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.DeleteEntity(new WorldShipShipmentEntity(shipment.ShipmentID));
                }
            }
            else
            {
                UpsServiceType serviceType = (UpsServiceType) shipment.Ups.Service;

                // If we are WolrdShip AND the WorldShipStatus is already set to Voided
                // then this void request MUST have come from WorldShipImportMonitor, so just return
                if (shipment.Ups.WorldShipStatus == (int) WorldShipStatusType.Voided)
                {
                    return;
                }

                if (UpsUtility.IsUpsMiOrSurePostService(serviceType))
                {
                    // If we got here, the request must have come from the user clicking void in ShipWorks,
                    // which is not supported...they need to start the void in WorldShip
                    throw new ShippingException("UPS Mail Innovations or UPS SurePost shipments must be voided using WorldShip.");
                }

                base.Void(shipment);
            }
        }
    }
}
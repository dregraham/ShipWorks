﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Label Service
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.OnTrac)]
    public class OnTracLabelService : ILabelService
    {
        private readonly Func<ShipmentEntity, Schemas.ShipmentResponse.Shipment, OnTracDownloadedLabelData> createDownloadedLabelData;
        private readonly ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> onTracAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracLabelService(ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> onTracAccountRepository,
            Func<ShipmentEntity, Schemas.ShipmentResponse.Shipment, OnTracDownloadedLabelData> createDownloadedLabelData)
        {
            this.onTracAccountRepository = onTracAccountRepository;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Processes the OnTrac shipment
        /// </summary>
        /// <param name="shipment"></param>
        public Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            try
            {
                MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

                IOnTracAccountEntity account = GetAccountForShipment(shipment);
                OnTracShipmentRequest onTracShipmentRequest = new OnTracShipmentRequest(account);

                // None is only an option if an invalid country is selected.
                if (shipment.OnTrac.Service == (int) OnTracServiceType.None)
                {
                    throw new OnTracException("OnTrac does not provide service outside of the United States.", true);
                }

                shipment.ActualLabelFormat = shipment.RequestedLabelFormat != (int) ThermalLanguage.None ?
                    shipment.RequestedLabelFormat :
                    (int?) null;

                // Transform shipment to OnTrac DTO
                Schemas.ShipmentRequest.OnTracShipmentRequest shipmentRequest = OnTracDtoAdapter.CreateShipmentRequest(
                    shipment,
                    account.AccountNumber);

                TelemetricResult<IDownloadedLabelData> telemetricResult =
                    new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");
                
                // Get new shipment from OnTrac and save the shipment info
                Schemas.ShipmentResponse.Shipment shipmentResponse = null;
                telemetricResult.RunTimedEvent(TelemetricEventType.GetLabel, () => shipmentResponse = onTracShipmentRequest.ProcessShipment(shipmentRequest));
                telemetricResult.SetValue(createDownloadedLabelData(shipment, shipmentResponse));

                return Task.FromResult(telemetricResult);
            }
            catch (OnTracException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Voids the OnTrac shipment
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
            // We don't void with OnTrac
        }

        /// <summary>
        /// Get the OnTrac account to be used for the given shipment
        /// </summary>
        private IOnTracAccountEntity GetAccountForShipment(ShipmentEntity shipment)
        {
            IOnTracAccountEntity account = onTracAccountRepository.GetAccountReadOnly(shipment.OnTrac.OnTracAccountID);

            if (account == null)
            {
                throw new OnTracException("No OnTrac account is selected for the shipment.");
            }

            return account;
        }
    }
}
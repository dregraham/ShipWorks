using System;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Label Service
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.OnTrac)]
    public class OnTracLabelService : ILabelService
    {
        private readonly Func<ShipmentEntity, ShipmentResponse, OnTracDownloadedLabelData> createDownloadedLabelData;
        private readonly ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> onTracAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracLabelService(ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> onTracAccountRepository,
            Func<ShipmentEntity, ShipmentResponse, OnTracDownloadedLabelData> createDownloadedLabelData)
        {
            this.onTracAccountRepository = onTracAccountRepository;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Processes the OnTrac shipment
        /// </summary>
        /// <param name="shipment"></param>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
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
                ShipmentRequestList shipmentRequestList = OnTracDtoAdapter.CreateShipmentRequestList(
                    shipment,
                    account.AccountNumber);

                // Get new shipment from OnTrac and save the shipment info
                ShipmentResponse shipmentResponse = onTracShipmentRequest.ProcessShipment(shipmentRequestList);

                return createDownloadedLabelData(shipment, shipmentResponse);
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
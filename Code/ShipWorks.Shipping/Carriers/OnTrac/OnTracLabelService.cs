using System;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Shipment;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Shipment;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// OnTrac Label Service
    /// </summary>
    public class OnTracLabelService : ILabelService
    {
        private readonly ICarrierAccountRepository<OnTracAccountEntity> onTracAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracLabelService(ICarrierAccountRepository<OnTracAccountEntity> onTracAccountRepository)
        {
            this.onTracAccountRepository = onTracAccountRepository;
        }

        /// <summary>
        /// Processes the OnTrac shipment
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            try
            {
                if (shipment == null)
                {
                    throw new ArgumentNullException("shipment");
                }

                OnTracAccountEntity account = GetAccountForShipment(shipment);

                OnTracShipmentRequest onTracShipmentRequest = new OnTracShipmentRequest(account);
                DatabaseOnTracShipmentRepository onTracShipmentRepository = new DatabaseOnTracShipmentRepository();

                // None is only an option if an invalid country is selected.
                if (((OnTracServiceType)shipment.OnTrac.Service) == OnTracServiceType.None)
                {
                    throw new OnTracException("OnTrac does not provide service outside of the United States.", true);
                }

                if (shipment.RequestedLabelFormat != (int)ThermalLanguage.None)
                {
                    shipment.ActualLabelFormat = shipment.RequestedLabelFormat;
                }
                else
                {
                    shipment.ActualLabelFormat = null;
                }

                // Transform shipment to OnTrac DTO
                ShipmentRequestList shipmentRequestList = OnTracDtoAdapter.CreateShipmentRequestList(
                    shipment,
                    account.AccountNumber);

                // Get new shipment from OnTrac and save the shipment info
                ShipmentResponse shipmentResponse = onTracShipmentRequest.ProcessShipment(shipmentRequestList);

                onTracShipmentRepository.SaveShipmentFromOnTrac(shipmentResponse, shipment);
            }
            catch (OnTracException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        public void Void(ShipmentEntity shipment)
        {

        }

        /// <summary>
        /// Get the OnTrac account to be used for the given shipment
        /// </summary>
        private OnTracAccountEntity GetAccountForShipment(ShipmentEntity shipment)
        {
            OnTracAccountEntity account = onTracAccountRepository.GetAccount(shipment.OnTrac.OnTracAccountID);

            if (account == null)
            {
                throw new OnTracException("No OnTrac account is selected for the shipment.");
            }

            return account;
        }
    }
}
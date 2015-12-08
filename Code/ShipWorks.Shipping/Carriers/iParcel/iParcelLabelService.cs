using System;
using System.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Label service for the iParcel carrier
    /// </summary>
    public class iParcelLabelService : ILabelService
    {
        private readonly ICarrierAccountRepository<IParcelAccountEntity> accountRepository;
        private readonly IiParcelRepository repository;
        private readonly IiParcelServiceGateway serviceGateway;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelLabelService(ICarrierAccountRepository<IParcelAccountEntity> accountRepository, IiParcelRepository repository, IiParcelServiceGateway serviceGateway, IOrderManager orderManager)
        {
            this.accountRepository = accountRepository;
            this.repository = repository;
            this.serviceGateway = serviceGateway;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Process the shipment for iParcel
        /// </summary>
        public void Create(ShipmentEntity shipment)
        {
            ProcessShipmentAndReturnResponse(shipment);
        }
        
        /// <summary>
        /// Process the shipment this is used for integration tests
        /// </summary>
        public DataSet ProcessShipmentAndReturnResponse(ShipmentEntity shipment)
        {
            try
            {
                MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

                shipment.ActualLabelFormat = shipment.RequestedLabelFormat != (int) ThermalLanguage.None ? 
                    shipment.RequestedLabelFormat
                    : (int?) null;

                IParcelAccountEntity iParcelAccount = accountRepository.GetAccount(shipment.IParcel.IParcelAccountID);
                iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);

                // i-parcel requires that we upload item information, so fetch the order and order items
                orderManager.PopulateOrderDetails(shipment);

                DataSet response = serviceGateway.SubmitShipment(credentials, shipment);

                repository.SaveLabel(shipment, response);
                repository.SaveTrackingInfoToEntity(shipment, response);

                return response;
            }
            catch (iParcelException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Voids the shipment for iParcel
        /// Currently does nothing
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
           
        }
    }
}
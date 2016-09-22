using System;
using System.Data;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Label service for the iParcel carrier
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.iParcel)]
    public class iParcelLabelService : ILabelService
    {
        private readonly ICarrierAccountRepository<IParcelAccountEntity, IIParcelAccountEntity> accountRepository;
        private readonly IiParcelServiceGateway serviceGateway;
        private readonly IOrderManager orderManager;
        private readonly Func<ShipmentEntity, DataSet, iParcelDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelLabelService(ICarrierAccountRepository<IParcelAccountEntity, IIParcelAccountEntity> accountRepository,
            IiParcelServiceGateway serviceGateway,
            IOrderManager orderManager,
            Func<ShipmentEntity, DataSet, iParcelDownloadedLabelData> createDownloadedLabelData)
        {
            this.accountRepository = accountRepository;
            this.serviceGateway = serviceGateway;
            this.orderManager = orderManager;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Process the shipment for iParcel
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            try
            {
                MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

                shipment.ActualLabelFormat = shipment.RequestedLabelFormat != (int) ThermalLanguage.None ?
                    shipment.RequestedLabelFormat :
                    (int?) null;

                IIParcelAccountEntity iParcelAccount = accountRepository.GetAccountReadOnly(shipment.IParcel.IParcelAccountID);
                iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);

                // i-parcel requires that we upload item information, so fetch the order and order items
                orderManager.PopulateOrderDetails(shipment);

                DataSet dataSet = serviceGateway.SubmitShipment(credentials, shipment);
                return createDownloadedLabelData(shipment, dataSet);
            }
            catch (iParcelException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Voids the shipment for iParcel
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
            // Currently does nothing
        }
    }
}
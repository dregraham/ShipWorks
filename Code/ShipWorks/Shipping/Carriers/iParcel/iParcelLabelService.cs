using System;
using System.Data;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelLabelService : ILabelService
    {
        private readonly IiParcelRepository repository;
        private readonly IiParcelServiceGateway serviceGateway;

        /// <summary>
        /// Constructor
        /// </summary>
        public iParcelLabelService(IiParcelRepository repository, IiParcelServiceGateway serviceGateway)
        {
            this.repository = repository;
            this.serviceGateway = serviceGateway;
        }

        /// <summary>
        /// Process the shipment for iParcel
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

                if (shipment.RequestedLabelFormat != (int)ThermalLanguage.None)
                {
                    shipment.ActualLabelFormat = shipment.RequestedLabelFormat;
                }
                else
                {
                    shipment.ActualLabelFormat = null;
                }

                IParcelAccountEntity iParcelAccount = repository.GetiParcelAccount(shipment);
                iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);

                // i-parcel requires that we upload item information, so fetch the order and order items
                repository.PopulateOrderDetails(shipment);

                DataSet response = serviceGateway.SubmitShipment(credentials, shipment);

                repository.SaveLabel(shipment, response);
                repository.SaveTrackingInfoToEntity(shipment, response);
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
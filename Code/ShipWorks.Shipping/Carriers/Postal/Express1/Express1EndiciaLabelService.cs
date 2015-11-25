using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Label Service for Express1 Endicia
    /// </summary>
    public class Express1EndiciaLabelService : ILabelService
    {
        private readonly EndiciaShipmentType endiciaShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaLabelService(EndiciaShipmentType endiciaShipmentType)
        {
            this.endiciaShipmentType = endiciaShipmentType;
        }

        /// <summary>
        /// Creates the Endicia Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            endiciaShipmentType.ValidateShipment(shipment);

            try
            {
                (new EndiciaApiClient(endiciaShipmentType.AccountRepository, endiciaShipmentType.LogEntryFactory,
                    endiciaShipmentType.CertificateInspector)).ProcessShipment(shipment, endiciaShipmentType);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Voids the Endicia Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
            try
            {
                Express1EndiciaCustomerServiceClient.RequestRefund(shipment);
            }
            catch (EndiciaException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}
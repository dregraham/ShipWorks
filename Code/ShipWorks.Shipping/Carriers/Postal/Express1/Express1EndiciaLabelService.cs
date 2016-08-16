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
        private readonly Express1EndiciaShipmentType express1EndiciaShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaLabelService(Express1EndiciaShipmentType express1EndiciaShipmentType)
        {
            this.express1EndiciaShipmentType = express1EndiciaShipmentType;
        }

        /// <summary>
        /// Creates the Endicia Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public void Create(ShipmentEntity shipment)
        {
            express1EndiciaShipmentType.ValidateShipment(shipment);

            try
            {
                (new EndiciaApiClient(express1EndiciaShipmentType.AccountRepository, express1EndiciaShipmentType.LogEntryFactory,
                    express1EndiciaShipmentType.CertificateInspector)).ProcessShipment(shipment, express1EndiciaShipmentType);
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
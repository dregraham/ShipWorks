using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Label Service for Express1 Endicia
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.Endicia)]
    [Component(RegistrationType.Self)]
    public class Express1EndiciaLabelService : ILabelService
    {
        private readonly Func<ShipmentEntity, LabelRequestResponse, EndiciaDownloadedLabelData> createDownloadedLabelData;
        private readonly Express1EndiciaShipmentType express1EndiciaShipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaLabelService(Express1EndiciaShipmentType express1EndiciaShipmentType,
            Func<ShipmentEntity, LabelRequestResponse, EndiciaDownloadedLabelData> createDownloadedLabelData)
        {
            this.express1EndiciaShipmentType = express1EndiciaShipmentType;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates the Endicia Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            express1EndiciaShipmentType.ValidateShipment(shipment);

            try
            {
                EndiciaApiClient client = new EndiciaApiClient(express1EndiciaShipmentType.AccountRepository,
                    express1EndiciaShipmentType.LogEntryFactory,
                    express1EndiciaShipmentType.CertificateInspector);
                LabelRequestResponse response = client.ProcessShipment(shipment, express1EndiciaShipmentType);
                return createDownloadedLabelData(shipment, response);
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
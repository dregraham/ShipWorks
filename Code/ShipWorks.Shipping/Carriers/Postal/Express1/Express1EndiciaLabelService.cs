using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Label Service for Express1 Endicia
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.Express1Endicia)]
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
        public Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            express1EndiciaShipmentType.ValidateShipment(shipment);

            try
            {
                EndiciaApiClient client = new EndiciaApiClient(express1EndiciaShipmentType.AccountRepository,
                    express1EndiciaShipmentType.LogEntryFactory,
                    express1EndiciaShipmentType.CertificateInspector);
                
                TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");
                LabelRequestResponse response = null;
                telemetricResult.RunTimedEvent("GetLabel", () => response = client.ProcessShipment(shipment, express1EndiciaShipmentType));
                telemetricResult.SetValue(createDownloadedLabelData(shipment, response));
                
                return Task.FromResult(telemetricResult);
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
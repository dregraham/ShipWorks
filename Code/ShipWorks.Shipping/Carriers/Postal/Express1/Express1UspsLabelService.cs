using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1 Usps Label Service
    /// </summary>
    public class Express1UspsLabelService : ILabelService
    {
        private readonly Express1UspsShipmentType express1UspsShipmentType;
        private readonly Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsLabelService(Express1UspsShipmentType express1UspsShipmentType,
            Func<UspsLabelResponse, UspsDownloadedLabelData> createDownloadedLabelData)
        {
            this.express1UspsShipmentType = express1UspsShipmentType;
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Creates the Usps(stamps.com) Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            express1UspsShipmentType.ValidateShipment(shipment);

            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");
            try
            {
                // Express1 for USPS requires that postage be hidden per their negotiated
                // service agreement
                shipment.Postal.Usps.HidePostage = true;
                
                telemetricResult.StartTimedEvent("GetLabel");
                UspsLabelResponse uspsLabelResponse = await new Express1UspsWebClient().ProcessShipment(shipment).ConfigureAwait(false);
                telemetricResult.StopTimedEvent("GetLabel");
                telemetricResult.SetValue(createDownloadedLabelData(uspsLabelResponse));
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }

            return telemetricResult;
        }

        /// <summary>
        /// Uses the Usps label service to void the Usps Express1 label
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
            try
            {
                express1UspsShipmentType.CreateWebClient().VoidShipment(shipment);
            }
            catch (UspsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}
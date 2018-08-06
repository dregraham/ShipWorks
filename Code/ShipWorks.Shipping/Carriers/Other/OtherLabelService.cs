using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Label Service for the Other carrier
    /// </summary>
    public class OtherLabelService : ILabelService
    {
        /// <summary>
        /// Creates an Other label
        /// </summary>
        /// <param name="shipment"></param>
        public Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            if (string.IsNullOrWhiteSpace(shipment.Other.Carrier))
            {
                throw new ShippingException("No carrier is specified.");
            }

            if (string.IsNullOrWhiteSpace(shipment.Other.Service))
            {
                throw new ShippingException("No service is specified.");
            }

            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");
            telemetricResult.SetValue(new NullDownloadedLabelData());
            
            return Task.FromResult(telemetricResult);
        }

        /// <summary>
        /// Voids the Other label
        /// </summary>
        /// <param name="shipment"></param>
        public void Void(ShipmentEntity shipment)
        {
            // Other does not support voiding
        }
    }
}
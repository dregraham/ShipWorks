using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Platform
{
    /// <summary>
    /// Client used to get Amazon Buy Shipping labels
    /// </summary>
    public interface IAmazonSfpLabelClient
    {
        /// <summary>
        /// Create a label from the given shipment
        /// </summary>
        Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment);

        /// <summary>
        /// Void the given shipment
        /// </summary>
        void Void(ShipmentEntity entity);
    }
}

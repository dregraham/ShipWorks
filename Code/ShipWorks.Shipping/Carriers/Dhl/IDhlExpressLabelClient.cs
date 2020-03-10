using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Client used to get DHL Express labels
    /// </summary>
    public interface IDhlExpressLabelClient
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

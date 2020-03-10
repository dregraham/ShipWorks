using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

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
        Task<TelemetricResult<IDownloadedLabelData>> CreateLabel(ShipmentEntity shipment);
    }
}

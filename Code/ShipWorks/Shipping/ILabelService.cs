using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Create Amazon labels
    /// </summary>
    public interface ILabelService
    {
        /// <summary>
        /// Create a label
        /// </summary>
        Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment);

        /// <summary>
        /// Voids the shipment
        /// </summary>
        void Void(ShipmentEntity shipment);
    }
}

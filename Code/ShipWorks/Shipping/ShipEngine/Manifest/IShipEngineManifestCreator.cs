using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.Manifest
{
    /// <summary>
    /// Interface for creating ShipEngine manifests
    /// </summary>
    public interface IShipEngineManifestCreator
    {
        /// <summary>
        /// Create a ShipEngine Manifest from today's shipments
        /// </summary>
        Task<GenericResult<CreateManifestResponse>> CreateManifest(ShipmentTypeCode shipmentTypeCode);
    }
}
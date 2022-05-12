using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce.Manifest
{
    /// <summary>
    /// Interface for creating DHL eCommerce manifests
    /// </summary>
    public interface IDhlEcommerceManifestCreator
    {
        /// <summary>
        /// Create a DHL eCommerce Manifest from today's shipments
        /// </summary>
        Task<GenericResult<CreateManifestResponse>> CreateManifest();
    }
}
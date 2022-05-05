using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Creates a DHL eCommerce Manifest
    /// </summary>
    public interface IDhlEcommerceManifestCreator
    {
        /// <summary>
        /// Create a DHL eCommerce Manifest from today's shipments
        /// </summary>
        Task<Result> CreateManifest();
    }
}
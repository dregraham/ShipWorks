using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Asendia
{
    /// <summary>
    /// Creates an Asendia Manifest
    /// </summary>
    public interface IAsendiaManifestCreator
    {
        /// <summary>
        /// Create an Asendia Manifest from today's shipments
        /// </summary>
        Task<Result> CreateManifest();
    }
}

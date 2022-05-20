using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom;
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
        Task<List<GenericResult<CreateManifestResponse>>> CreateManifest(ICarrierAccount carrierAccount, IProgressReporter progress);
    }
}
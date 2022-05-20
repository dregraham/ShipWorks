using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.ShipEngine.Manifest
{
    /// <summary>
    /// Interface for getting and saving ShipEngine manifests
    /// </summary>
    public interface IShipEngineManifestRepo
    {
        /// <summary>
        /// Save a ShipEngine Manifest to ShipWorks
        /// </summary>
        Task<Result> SaveManifest(CreateManifestResponse createManifestResponse, ICarrierAccount account);

        /// <summary>
        /// Get ShipEngineManifestEntities for a carrier account
        /// </summary>
        Task<List<ShipEngineManifestEntity>> GetManifests(ICarrierAccount account, int maxToReturn);
    }
}
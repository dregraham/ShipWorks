using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
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
        Task<Result> SaveManifest(CreateManifestResponse createManifestResponse);

        /// <summary>
        /// Get ShipEngineManifestEntities for a shipment type
        /// </summary>
        Task<List<ShipEngineManifestEntity>> GetManifests(ShipmentTypeCode shipmentTypeCode);
    }
}
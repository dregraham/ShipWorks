using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    public interface ICarrierShipmentAdapterOptionsProvider
    {
        /// <summary>
        /// Get an enumerable of package types for the adapter
        /// </summary>
        IEnumerable<KeyValuePair<int, string>> GetPackageTypes(ICarrierShipmentAdapter carrierAdapter);

        /// <summary>
        /// Get a dictionary of available providers for the adapter
        /// </summary>
        Dictionary<ShipmentTypeCode, string> GetProviders(ICarrierShipmentAdapter carrierAdapter, ShipmentTypeCode includeProvider);

        /// <summary>
        /// Get the service types available for the adapter
        /// </summary>
        IEnumerable<KeyValuePair<int, string>> GetServiceTypes(ICarrierShipmentAdapter carrierAdapter);

        /// <summary>
        /// Get the available profiles for the package adapter
        /// </summary>
        IEnumerable<DimensionsProfileEntity> GetDimensionsProfiles(IPackageAdapter package);
    }
}
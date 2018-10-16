using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    public interface ICarrierShipmentAdapterOptionsProvider
    {
        IEnumerable<KeyValuePair<int, string>> GetPackageTypes(ICarrierShipmentAdapter carrierAdapter);
        Dictionary<ShipmentTypeCode, string> GetProviders(ICarrierShipmentAdapter carrierAdapter);
        IEnumerable<KeyValuePair<int, string>> GetServiceTypes(ICarrierShipmentAdapter carrierAdapter);
        IEnumerable<DimensionsProfileEntity> GetDimensionsProfiles(IPackageAdapter package);
    }
}
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;

namespace ShipWorks.OrderLookup.Controls.ShipmentDetails
{
    /// <summary>
    /// Resource for getting options available for a carrier adapter
    /// </summary>
    [Component]
    public class CarrierShipmentAdapterOptionsProvider : ICarrierShipmentAdapterOptionsProvider
    {
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;
        private readonly ShipmentTypeProvider shipmentTypeProvider;
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;
        private readonly IDimensionsManager dimsManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierShipmentAdapterOptionsProvider(
            IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory,
            ShipmentTypeProvider shipmentTypeProvider,
            IShipmentServicesBuilderFactory shipmentServicesBuilderFactory,
            IDimensionsManager dimsManager)
        {
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory;
            this.shipmentTypeProvider = shipmentTypeProvider;
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory;
            this.dimsManager = dimsManager;
        }

        /// <summary>
        /// Get an enumerable of package types for the adapter
        /// </summary>
        public IDictionary<int, string> GetPackageTypes(ICarrierShipmentAdapter carrierAdapter)
        {
            return shipmentPackageTypesBuilderFactory.Get(carrierAdapter.ShipmentTypeCode)
                    .BuildPackageTypeDictionary(new[] { carrierAdapter.Shipment });
        }

        /// <summary>
        /// Get a dictionary of available providers for the adapter
        /// </summary>
        public Dictionary<ShipmentTypeCode, string> GetProviders(ICarrierShipmentAdapter carrierAdapter, ShipmentTypeCode includeProvider)
        {
            return shipmentTypeProvider
                    .GetAvailableShipmentTypes(carrierAdapter)
                    .Union(new[] { carrierAdapter.ShipmentTypeCode, includeProvider })
                    .ToDictionary(s => s, s => EnumHelper.GetDescription(s));
        }

        /// <summary>
        /// Get the service types available for the adapter
        /// </summary>
        public IDictionary<int, string> GetServiceTypes(ICarrierShipmentAdapter carrierAdapter)
        {
            Dictionary<int, string> updatedServices = new Dictionary<int, string>();

            try
            {
                updatedServices = shipmentServicesBuilderFactory.Get(carrierAdapter.ShipmentTypeCode)
                    .BuildServiceTypeDictionary(new[] { carrierAdapter.Shipment });
            }
            catch (InvalidRateGroupShippingException)
            {
                updatedServices.Add(carrierAdapter.ServiceType, "Error getting service types.");
            }

            return updatedServices;
        }

        /// <summary>
        /// Get the available profiles for the package adapter
        /// </summary>
        public IEnumerable<DimensionsProfileEntity> GetDimensionsProfiles(IPackageAdapter package)
        {
            return dimsManager.Profiles(package);
        }
    }
}

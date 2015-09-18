using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.UI;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExShipmentServicesBuilder : IShipmentServicesBuilder
    {
        private readonly FedExShipmentType fedExShipmentType;
        private readonly IShippingManager shippingManager;
        private readonly IFedExUtility fedExUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentServicesBuilder"/> class.
        /// </summary>
        public FedExShipmentServicesBuilder(FedExShipmentType fedExShipmentType, IShippingManager shippingManager, IFedExUtility fedExUtility)
        {
            this.fedExShipmentType = fedExShipmentType;
            this.shippingManager = shippingManager;
            this.fedExUtility = fedExUtility;
        }

        /// <summary>
        /// Gets the AvailableServiceTypes for this shipment type and shipment along with their descriptions.
        /// </summary>
        public Dictionary<int, string> BuildServiceTypeDictionary(IEnumerable<ShipmentEntity> shipments)
        {
            List<ShipmentEntity> shipmentList = shipments?.ToList() ?? new List<ShipmentEntity>();

            // The service types need to to be loaded based on the overridden shipment data to account
            // for various shipping programs/rules offered by stores (i.e. eBay GSP)
            List<ShipmentEntity> overriddenShipments = shipmentList.Select(shippingManager.GetOverriddenStoreShipment).ToList();

            bool allDomestic = overriddenShipments.All(fedExShipmentType.IsDomestic);
            bool allInternational = overriddenShipments.None(fedExShipmentType.IsDomestic);
            bool allCanada = overriddenShipments.All(shipment => shipment.AdjustedShipCountryCode() == "CA");

            // If they are all of the same service class, we can load the service classes
            if (!allDomestic && !allInternational && !allCanada)
            {
                return new Dictionary<int, string>();
            }

            List<FedExServiceType> availableServices = fedExShipmentType
                .GetAvailableServiceTypes()
                .Cast<FedExServiceType>().ToList();

            // Get a list of all valid service types for the shipments
            List<FedExServiceType> validServiceTypes = fedExUtility.GetValidServiceTypes(overriddenShipments);

            // load shipment types that are valid and enabled (avaialbeServices)
            List<FedExServiceType> fedExServiceTypes = validServiceTypes.Intersect(availableServices).ToList();

            if (shipmentList.Any())
            {
                // Always include the service type that the shipment is currently configured in the 
                // event the shipment was configured prior to a service being excluded
                // Always include the service that the shipments are currently configured with
                // Only if the service type is a validServiceType
                IEnumerable<FedExServiceType> loadedServices = shipmentList.Select(s => (FedExServiceType)s.FedEx.Service).Intersect(validServiceTypes).Distinct();
                fedExServiceTypes = fedExServiceTypes.Union(loadedServices).ToList();
            }

            return fedExServiceTypes.ToDictionary(s => (int)s, s => EnumHelper.GetDescription(s));
        }
    }
}
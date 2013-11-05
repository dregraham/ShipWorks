using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// An implementation of the IBestRateShippingBrokerFactory interface that creates brokers for the
    /// shipping providers that have been activated and configured in ShipWorks.
    /// </summary>
    public class BestRateShippingBrokerFactory : IBestRateShippingBrokerFactory
    {
        /// <summary>
        /// Creates all of the best rate shipping brokers available in the system for the shipping
        /// providers that are activated and configured.
        /// </summary>
        /// <returns>The shipping broker for all activated and configured shipment types that have not 
        /// been excluded.</returns>
        public IEnumerable<IBestRateShippingBroker> CreateBrokers()
        {
            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>();
            ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
            List <ShipmentType> shipmentTypes = ShipmentTypeManager.ShipmentTypes;

            // If both UPS OnlineTools AND WorldShip are selected, remove WorldShip so we don't get double rates returned
            if (shipmentTypes.Any(st => st.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools && IsShipmentTypeActiveConfiguredAndNotExcluded(shippingSettings, st.ShipmentTypeCode)) &&
                shipmentTypes.Any(st => st.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip && IsShipmentTypeActiveConfiguredAndNotExcluded(shippingSettings, st.ShipmentTypeCode)))
            {
                shipmentTypes.Remove(shipmentTypes.First(st => st.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip));
            }

            // Add a broker for every shipment type that has been activated, configured, and hasn't been excluded
            foreach (ShipmentType shipmentType in shipmentTypes)
            {
                if (IsShipmentTypeActiveConfiguredAndNotExcluded(shippingSettings, shipmentType.ShipmentTypeCode))
                {
                    // This shipment type is activated, configured, and hasn't been excluded, so add it to our list of brokers
                    brokers.Add(shipmentType.GetShippingBroker());
                }
            }

            return brokers;
        }

        /// <summary>
        /// Determines if a Shipment Type Code is active, configured and not excluded
        /// </summary>
        private static bool IsShipmentTypeActiveConfiguredAndNotExcluded(ShippingSettingsEntity shippingSettings, ShipmentTypeCode shipmentTypeCode)
        {
            int shipmentTypeCodeValue = (int) shipmentTypeCode;

            return shippingSettings.ActivatedTypes.Contains(shipmentTypeCodeValue) &&
                   shippingSettings.ConfiguredTypes.Contains(shipmentTypeCodeValue) && 
                   !shippingSettings.ExcludedTypes.Contains(shipmentTypeCodeValue);
        }
    }
}

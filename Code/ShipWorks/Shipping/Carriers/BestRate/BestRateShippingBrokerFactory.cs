using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
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
        /// <param name="shipment">The shipment.</param>
        /// <returns>The shipping broker for all activated and configured shipment types that have not
        /// been excluded from being used to find the best rate.</returns>
        public IEnumerable<IBestRateShippingBroker> CreateBrokers(ShipmentEntity shipment)
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
                    IBestRateShippingBroker broker = shipmentType.GetShippingBroker(shipment);
                    if (broker.HasAccounts)
                    {
                        // We only want to return brokers that have accounts setup
                        brokers.Add(shipmentType.GetShippingBroker(shipment));
                    }
                }
            }

            // We need to configure each of the brokers now that we have our final
            // list that should be used
            BestRateBrokerSettings brokerSettings = new BestRateBrokerSettings(shippingSettings, brokers, EditionManager.ActiveRestrictions);
            foreach (IBestRateShippingBroker broker in brokers)
            {
                broker.Configure(brokerSettings);
            }

            return brokers;
        }

        /// <summary>
        /// Determines if a Shipment Type Code is active, configured, not a globally excluded shipment type, and not a shipment type
        /// that has been excluded from being used with the best rate shipment type.
        /// </summary>
        private static bool IsShipmentTypeActiveConfiguredAndNotExcluded(ShippingSettingsEntity shippingSettings, ShipmentTypeCode shipmentTypeCode)
        {
            int shipmentTypeCodeValue = (int) shipmentTypeCode;

            return shippingSettings.ActivatedTypes.Contains(shipmentTypeCodeValue) &&
                   shippingSettings.ConfiguredTypes.Contains(shipmentTypeCodeValue) && 
                   !shippingSettings.ExcludedTypes.Contains(shipmentTypeCodeValue) &&
                   !shippingSettings.BestRateExcludedTypes.Contains((int)shipmentTypeCode);
        }
    }
}

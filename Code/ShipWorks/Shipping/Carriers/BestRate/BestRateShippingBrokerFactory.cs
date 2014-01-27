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
        /// <param name="createCounterRateBrokers">Should counter rate brokers be created</param>
        /// <returns>The shipping broker for all activated and configured shipment types that have not
        /// been excluded from being used to find the best rate.</returns>
        public IEnumerable<IBestRateShippingBroker> CreateBrokers(ShipmentEntity shipment, bool createCounterRateBrokers)
        {
            ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
            List <ShipmentType> shipmentTypes = ShipmentTypeManager.ShipmentTypes;

            // If both UPS OnlineTools AND WorldShip are selected, remove WorldShip so we don't get double rates returned
            if (shipmentTypes.Any(st => IsUsableType(ShipmentTypeCode.UpsOnLineTools, shippingSettings, st)) &&
                shipmentTypes.Any(st => IsUsableType(ShipmentTypeCode.UpsWorldShip, shippingSettings, st)))
            {
                shipmentTypes.Remove(shipmentTypes.First(st => st.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip));
            }

            List<IBestRateShippingBroker> brokers = shipmentTypes.Where(st => !IsShipmentTypeExcluded(shippingSettings, st.ShipmentTypeCode))
                .Select(st => st.GetShippingBroker(shipment))
                .Where(broker => broker.HasAccounts && (createCounterRateBrokers || !broker.IsCounterRate))
                .ToList();
            
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
        /// Gets whether the shipment type is of the desired type and is usable
        /// </summary>
        /// <param name="desiredType">Shipment type that we're testing for</param>
        /// <param name="shippingSettings">Settings that will be used to test whether the type is usable</param>
        /// <param name="shipmentType"></param>
        /// <returns></returns>
        private static bool IsUsableType(ShipmentTypeCode desiredType, ShippingSettingsEntity shippingSettings, ShipmentType shipmentType)
        {
            return shipmentType.ShipmentTypeCode == desiredType &&
                    IsShipmentTypeActiveAndConfigured(shippingSettings, shipmentType.ShipmentTypeCode) &&
                    !IsShipmentTypeExcluded(shippingSettings, shipmentType.ShipmentTypeCode);
        }

        /// <summary>
        /// Determines if a Shipment Type Code is active and configured
        /// </summary>
        private static bool IsShipmentTypeActiveAndConfigured(ShippingSettingsEntity shippingSettings, ShipmentTypeCode shipmentTypeCode)
        {
            int shipmentTypeCodeValue = (int) shipmentTypeCode;

            return shippingSettings.ActivatedTypes.Contains(shipmentTypeCodeValue) &&
                   shippingSettings.ConfiguredTypes.Contains(shipmentTypeCodeValue);
        }

        /// <summary>
        /// Determines if a Shipment Type Code is not a globally excluded shipment type and not a shipment type
        /// that has been excluded from being used with the best rate shipment type.
        /// </summary>
        private static bool IsShipmentTypeExcluded(ShippingSettingsEntity shippingSettings, ShipmentTypeCode shipmentTypeCode)
        {
            int shipmentTypeCodeValue = (int)shipmentTypeCode;

            // Always include web tools, so we get USPS counter rates as needed
            return (shippingSettings.ExcludedTypes.Contains(shipmentTypeCodeValue) ||
                   shippingSettings.BestRateExcludedTypes.Contains(shipmentTypeCodeValue) && shipmentTypeCode != ShipmentTypeCode.PostalWebTools);
        }
    }
}

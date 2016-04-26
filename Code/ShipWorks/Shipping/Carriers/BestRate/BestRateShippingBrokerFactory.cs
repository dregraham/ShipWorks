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
        private readonly IEnumerable<IShippingBrokerFilter> filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShippingBrokerFactory"/> class.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public BestRateShippingBrokerFactory(IEnumerable<IShippingBrokerFilter> filters)
        {
            this.filters = filters;
        }

        /// <summary>
        /// Creates all of the best rate shipping brokers available in the system for the shipping
        /// providers that are activated and configured. This will not return any counter rate brokers.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public IEnumerable<IBestRateShippingBroker> CreateBrokers(ShipmentEntity shipment)
        {
            ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
            List <ShipmentType> shipmentTypes = ShipmentTypeManager.ShipmentTypes;

            List<IBestRateShippingBroker> brokers = new List<IBestRateShippingBroker>();
            foreach (ShipmentType shipmentType in shipmentTypes)
            {
                if (!IsShipmentTypeExcluded(shippingSettings, shipmentType.ShipmentTypeCode))
                {
                    // Disregard the counter rate brokers
                    IBestRateShippingBroker broker = shipmentType.GetShippingBroker(shipment);
                    if (broker.HasAccounts && !broker.IsCounterRate)
                    {
                        brokers.Add(broker);
                    }
                }
            }

            foreach (IShippingBrokerFilter filter in filters)
            {
                brokers = filter.Filter(brokers).ToList();
            }

            // We need to configure each of the brokers now that we have our final
            // list that should be used
            BestRateBrokerSettings brokerSettings = new BestRateBrokerSettings(shippingSettings, EditionManager.ActiveRestrictions);
            foreach (IBestRateShippingBroker broker in brokers)
            {
                broker.Configure(brokerSettings);
            }

            return brokers;
        }

        /// <summary>
        /// Determines if a Shipment Type Code is not a globally excluded shipment type and not a shipment type
        /// that has been excluded from being used with the best rate shipment type.
        /// </summary>
        private static bool IsShipmentTypeExcluded(ShippingSettingsEntity shippingSettings, ShipmentTypeCode shipmentTypeCode)
        {
            int shipmentTypeCodeValue = (int)shipmentTypeCode;

            // Always include web tools, so we get USPS counter rates as needed
            return (shippingSettings.BestRateExcludedTypes.Contains(shipmentTypeCodeValue) && shipmentTypeCode != ShipmentTypeCode.PostalWebTools);
        }
    }
}

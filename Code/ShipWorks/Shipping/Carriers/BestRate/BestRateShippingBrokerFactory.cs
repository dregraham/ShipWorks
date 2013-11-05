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
            
            // Add a broker for every shipment type that has been activated, configured, and hasn't been excluded
            foreach (ShipmentType shipmentType in ShipmentTypeManager.ShipmentTypes)
            {
                int shipmentTypeCodeValue = (int)shipmentType.ShipmentTypeCode;

                if (shippingSettings.ActivatedTypes.Contains(shipmentTypeCodeValue) 
                    && shippingSettings.ConfiguredTypes.Contains(shipmentTypeCodeValue) 
                    && !shippingSettings.ExcludedTypes.Contains(shipmentTypeCodeValue))
                {
                    // This shipment type is activated, configured, and hasn't been excluded, so add it to our list of brokers
                    brokers.Add(shipmentType.GetShippingBroker());
                }
            }

            return brokers;
        }
    }
}

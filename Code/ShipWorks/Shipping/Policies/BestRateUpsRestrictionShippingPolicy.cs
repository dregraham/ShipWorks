using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Policies
{
    /// <summary>
    /// If Tango says we can't use UPS for BestRate, filter out UPS
    /// </summary>
    public class BestRateUpsRestrictionShippingPolicy : IShippingPolicy
    {
        private bool isRestricted = false;

        /// <summary>
        /// Uses the configuration data provided to configure the shipping policy.
        /// </summary>
        /// <param name="configuration">The configuration data.</param>
        /// <exception cref="System.ArgumentException">configuration</exception>
        public void Configure(string configuration)
        {
            bool parsedConfiguration;

            if (bool.TryParse(configuration, out parsedConfiguration))
            {
                if (parsedConfiguration)
                {
                    isRestricted = true;
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Unknown configuration value '{0}.' Expected 'true' or 'false.'", configuration), "configuration");
            }
        }

        /// <summary>
        /// If rategroup, we can act on it. return true
        /// </summary>
        public bool IsApplicable(object target)
        {
            return isRestricted && (target is List<IBestRateShippingBroker> || target is List<ShipmentTypeCode>);
        }

        /// <summary>
        /// Filter out 
        /// </summary>
        public void Apply(object target)
        {
            if (!isRestricted)
            {
                return;
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (!IsApplicable(target))
            {
                throw new ArgumentException("target not of type List<IBestRateShippingBroker> or List<ShipmentTypeCode>", "target");                
            }

            List<IBestRateShippingBroker> brokers = target as List<IBestRateShippingBroker>;
            List<ShipmentTypeCode> shipmentTypesToExclude = target as List<ShipmentTypeCode>;

            if (brokers != null)
            {
                RemoveUpsRateGroups(brokers);
            }

            if (shipmentTypesToExclude != null)
            {
                shipmentTypesToExclude.Add(ShipmentTypeCode.UpsOnLineTools);
                shipmentTypesToExclude.Add(ShipmentTypeCode.UpsWorldShip);
            }

        }

        /// <summary>
        /// Removes the ups rate groups.
        /// </summary>
        private static void RemoveUpsRateGroups(List<IBestRateShippingBroker> brokers)
        {
            int originalNumberOfRateGroups = brokers.Count;

            for (int i = originalNumberOfRateGroups - 1; i >= 0; i--)
            {
                IBestRateShippingBroker broker = brokers[i];

                if (IsUpsBroker(broker))
                {
                    brokers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Determines whether broker is a UPS broker type.
        /// </summary>
        private static bool IsUpsBroker(IBestRateShippingBroker broker)
        {
            Type type = broker.GetType();

            return type == typeof(WorldShipBestRateBroker) ||
                   type == typeof(UpsBestRateBroker) ||
                   type == typeof(UpsCounterRatesBroker);
        }
    }
}
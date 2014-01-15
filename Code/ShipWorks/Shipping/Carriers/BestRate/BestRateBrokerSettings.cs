using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Calculates Settings needed by BestRate Brokers.
    /// </summary>
    public class BestRateBrokerSettings : IBestRateBrokerSettings
    {
        private readonly ShippingSettingsEntity settings;
        private readonly List<IBestRateShippingBroker> brokers;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateBrokerSettings"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="brokers">The brokers.</param>
        public BestRateBrokerSettings(ShippingSettingsEntity settings, List<IBestRateShippingBroker> brokers)
        {
            this.settings = settings;
            this.brokers = brokers;
        }

        /// <summary>
        /// Checks the express1 rates.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        public bool CheckExpress1Rates(ShipmentType shipmentType)
        {
            // assumption: ShipmentType is enabled. If it were not, this wouldn't be called.

            // Check if Express1 for this shipment type is disabled in Best Rates. If so, return false
            if (IsExpress1ForShipmentTypeDisabled(shipmentType))
            {
                return false;
            }

            // If Express1 account exists, return false
            if (IsExpress1AccountUsed())
            {
                return false;
            }

            // At this point, Express1 is enabled in BestRates and the user doesn't have an express1 account setup. return true
            return true;
        }

        /// <summary>
        /// Determines whether express1 account is used in Best Rates.
        /// </summary>
        /// <returns></returns>
        private bool IsExpress1AccountUsed()
        {
            return brokers.Any(b => b is Express1StampsBestRateBroker || b is Express1EndiciaBestRateBroker);
        }

        /// <summary>
        /// Determines whether express1 for shipment type is disabled in best rates or in the general settings.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <exception cref="System.ArgumentException">shipmentType should be either Endicia or Stamps</exception>
        private bool IsExpress1ForShipmentTypeDisabled(ShipmentType shipmentType)
        {
            ShipmentTypeCode express1ShipmentType;

            switch (shipmentType.ShipmentTypeCode)
            {
                case ShipmentTypeCode.Endicia:
                    express1ShipmentType = ShipmentTypeCode.Express1Endicia;
                    break;
                case ShipmentTypeCode.Stamps:
                    express1ShipmentType = ShipmentTypeCode.Express1Stamps;
                    break;
                default:
                    throw new ArgumentException("shipmentType should be either Endicia or Stamps", "shipmentType");
            }

            if (settings.BestRateExcludedTypes.Contains((int)express1ShipmentType))
            {
                // Express1 is disabled at the best rates level.
                return true;
            }
            if (!settings.ActivatedTypes.Contains((int)express1ShipmentType))
            {
                // Express1 is disabled at the general settings level.
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether mail innovations is available for the specified shipment type.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <exception cref="System.ArgumentException">shipmentType should be UPS type</exception>
        public bool IsMailInnovationsAvailable(ShipmentType shipmentType)
        {
            switch (shipmentType.ShipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                    return settings.UpsMailInnovationsEnabled;
                case ShipmentTypeCode.UpsWorldShip:
                    return settings.WorldShipMailInnovationsEnabled;
                default:
                    throw new ArgumentException("shipmentType should be UPS type", "shipmentType");
            }
        }

        public bool CanUseSurePost()
        {
            return UpsUtility.CanUseSurePost();
        }
    }
}
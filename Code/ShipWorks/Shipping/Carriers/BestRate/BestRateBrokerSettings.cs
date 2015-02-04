using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Calculates Settings needed by BestRate Brokers.
    /// </summary>
    public class BestRateBrokerSettings : IBestRateBrokerSettings
    {
        private readonly ShippingSettingsEntity settings;
        private readonly EditionRestrictionSet activeRestrictions;
        private IEnumerable<ShipmentTypeCode> enabledShipmentTypeCodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateBrokerSettings" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="activeRestrictions">The active restrictions.</param>
        public BestRateBrokerSettings(ShippingSettingsEntity settings, EditionRestrictionSet activeRestrictions)
        {
            this.settings = settings;
            this.activeRestrictions = activeRestrictions;
        }

        /// <summary>
        /// Get shipment types that are currently enabled
        /// </summary>
        /// <remarks>This property exists to allow the class to be tested, as tests can set their own list of enabled shipment types.</remarks>
        public IEnumerable<ShipmentTypeCode> EnabledShipmentTypeCodes
        {
            get
            {
                return enabledShipmentTypeCodes ??
                       ShipmentTypeManager.EnabledShipmentTypes.Select(x => x.ShipmentTypeCode);
            }
            set
            {
                enabledShipmentTypeCodes = value;
            }
        }

        /// <summary>
        /// Checks the express1 rates.
        /// </summary>
        public bool CheckExpress1Rates(ShipmentType shipmentType)
        {
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

        /// <summary>
        /// Determines whether a customer [can use sure post].
        /// </summary>
        /// <returns></returns>
        public bool CanUseSurePost()
        {
            return UpsUtility.CanUseSurePost();
        }

        /// <summary>
        /// Determines if endicia DHL is enabled.
        /// </summary>
        public bool IsEndiciaDHLEnabled()
        {
            return (activeRestrictions.CheckRestriction(EditionFeature.EndiciaDhl).Level == EditionRestrictionLevel.None);
        }

        /// <summary>
        /// Determines if Consolidator enabled.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsEndiciaConsolidatorEnabled()
        {
            return (activeRestrictions.CheckRestriction(EditionFeature.EndiciaConsolidator).Level == EditionRestrictionLevel.None);
        }
    }
}
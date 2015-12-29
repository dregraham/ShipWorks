using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages the global shipping settings instance
    /// </summary>
    /// <remarks>
    /// Wraps the static ShippingSettings so that static dependencies can be broken
    /// </remarks>
    public class ShippingSettingsWrapper : IShippingSettings
    {
        private readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeLookup;
        private readonly IFilterHelper filterHelper;
        private readonly IShippingProviderRuleManager shippingProviderRuleManager;

        public ShippingSettingsWrapper(IFilterHelper filterHelper,
            IShippingProviderRuleManager shippingProviderRuleManager,
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeLookup)
        {
            this.filterHelper = filterHelper;
            this.shippingProviderRuleManager = shippingProviderRuleManager;
            this.shipmentTypeLookup = shipmentTypeLookup;
        }

        /// <summary>
        /// The list of shipment types that have been fully configured for use within ShipWorks
        /// </summary>
        public IEnumerable<ShipmentTypeCode> GetConfiguredTypes()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            return settings.ConfiguredTypes.Cast<ShipmentTypeCode>();
        }

        /// <summary>
        /// Marks the given ShipmentTypeCode as completely configured
        /// </summary>
        public void MarkAsConfigured(ShipmentTypeCode shipmentTypeCode) =>
            ShippingSettings.MarkAsConfigured(shipmentTypeCode);

        /// <summary>
        /// Fetch the current shipping settings
        /// </summary>
        public ShippingSettingsEntity Fetch() => ShippingSettings.Fetch();

        /// <summary>
        /// Determine what the initial shipment type for the given order should be, given the shipping settings rules
        /// </summary>
        public ShipmentType InitialShipmentType(ShipmentEntity shipment)
        {
            ShipmentTypeCode initialShipmentType = (ShipmentTypeCode) Fetch().DefaultType;

            // Go through each rule and see if we can find one that is applicable
            foreach (ShippingProviderRuleEntity rule in shippingProviderRuleManager.GetRules())
            {
                long? filterContentID = filterHelper.GetFilterNodeContentID(rule.FilterNodeID);
                if (filterContentID != null)
                {
                    if (filterHelper.IsObjectInFilterContent(shipment.OrderID, filterContentID.Value))
                    {
                        initialShipmentType = (ShipmentTypeCode) rule.ShipmentType;
                    }
                }
            }

            ShipmentType shipmentType = shipmentTypeLookup[initialShipmentType];
            return shipmentType.IsAllowedFor(shipment) ? shipmentType : shipmentTypeLookup[ShipmentTypeCode.None];
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wrapper for the static shipment type manager
    /// </summary>
    public class ShipmentTypeManagerWrapper : IShipmentTypeManager
    {
        private readonly IEnumerable<ShipmentTypeCode> allShipmentTypeCodes;
        private readonly IEnumerable<ShipmentTypeCode> noAccountShipmentTypes;
        private readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeLookup;
        private readonly IFilterHelper filterHelper;
        private readonly IShippingProviderRuleManager shippingProviderRuleManager;
        private readonly IShippingSettings shippingSettings;

        public ShipmentTypeManagerWrapper(IFilterHelper filterHelper,
            IShippingProviderRuleManager shippingProviderRuleManager,
            IShippingSettings shippingSettings,
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeLookup) : this()
        {
            this.filterHelper = filterHelper;
            this.shippingProviderRuleManager = shippingProviderRuleManager;
            this.shippingSettings = shippingSettings;
            this.shipmentTypeLookup = shipmentTypeLookup;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeManagerWrapper()
        {
            allShipmentTypeCodes = Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>();

            noAccountShipmentTypes = new List<ShipmentTypeCode>()
                {
                    ShipmentTypeCode.None,
                    ShipmentTypeCode.Other,
                    ShipmentTypeCode.PostalWebTools,
                    ShipmentTypeCode.BestRate
                };
        }

        /// <summary>
        /// Returns all shipment types in ShipWorks
        /// </summary>
        public List<ShipmentType> ShipmentTypes => ShipmentTypeManager.ShipmentTypes;

        /// <summary>
        /// Get a list of enabled shipment types
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ShipmentTypeCodes => ShipmentTypeManager.ShipmentTypeCodes;

        /// <summary>
        /// Get a list of enabled shipment types
        /// </summary>
        public IEnumerable<ShipmentTypeCode> EnabledShipmentTypeCodes => ShipmentTypeManager.EnabledShipmentTypeCodes;

        /// <summary>
        /// Get the sort value for a given shipment type code
        /// </summary>
        public int GetSortValue(ShipmentTypeCode shipmentTypeCode) => ShipmentTypeManager.GetSortValue(shipmentTypeCode);

        /// <summary>
        /// Returns a list of ShipmentTypeCodes that support accounts.
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ShipmentTypesSupportingAccounts =>
            allShipmentTypeCodes.Except(noAccountShipmentTypes);

        /// <summary>
        /// Configured ShipmentTypes
        /// </summary>
        public IEnumerable<ShipmentTypeCode> ConfiguredShipmentTypes =>
            ShipmentTypeCodes.Where(c => c == ShipmentTypeCode.BestRate || shippingSettings.IsConfigured(c));

        /// <summary>
        /// Determine what the initial shipment type for the given order should be, given the shipping settings rules
        /// </summary>
        public ShipmentType InitialShipmentType(ShipmentEntity shipment)
        {
            ShipmentTypeCode initialShipmentType = GetLastApplicableRule(shipment)?.ShipmentTypeCode ??
                shippingSettings.Fetch().DefaultShipmentTypeCode;

            // Amazon prime orders are currently only allowed to be shipped via Amazon Shipment Type
            IAmazonOrder order = shipment.Order as IAmazonOrder;
            if (order != null && order.IsPrime)
            {
                initialShipmentType = ShipmentTypeCode.Amazon;
            }

            ShipmentType shipmentType = shipmentTypeLookup[initialShipmentType];
            return shipmentType.IsAllowedFor(shipment) ? shipmentType : shipmentTypeLookup[ShipmentTypeCode.None];
        }

        /// <summary>
        /// Get the provider for the specified shipment
        /// </summary>
        public ShipmentType Get(IShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            return Get(shipment.ShipmentTypeCode);
        }

        /// <summary>
        /// Get the shipment type based on its code
        /// </summary>
        public ShipmentType Get(ShipmentTypeCode shipmentTypeCode) => shipmentTypeLookup[shipmentTypeCode];

        /// <summary>
        /// Get the last rule that is applicable for the given shipment
        /// </summary>
        private ShippingProviderRuleEntity GetLastApplicableRule(ShipmentEntity shipment) =>
            shippingProviderRuleManager.GetRules()
                .FirstOrDefault(x => filterHelper.IsObjectInFilterContent(shipment.OrderID, x));
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Wrapper for the static shipment type manager
    /// </summary>
    public class ShipmentTypeManagerWrapper : IShipmentTypeManager
    {
        private readonly IEnumerable<ShipmentTypeCode> allShipmentTypeCodes;
        private readonly IEnumerable<ShipmentTypeCode> noAccountShipmentTypes;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeManagerWrapper()
        {
            allShipmentTypeCodes = EnumHelper.GetEnumList<ShipmentTypeCode>().Select(x => x.Value);

            noAccountShipmentTypes = new List<ShipmentTypeCode>()
                {
                    ShipmentTypeCode.None,
                    ShipmentTypeCode.Other,
                    ShipmentTypeCode.PostalWebTools,
                    ShipmentTypeCode.BestRate
                };
        }

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
        public IEnumerable<ShipmentTypeCode> ShipmentTypesSupportingAccounts
        {
            get
            {
                return allShipmentTypeCodes.Except(noAccountShipmentTypes);
            }
        }
    }
}

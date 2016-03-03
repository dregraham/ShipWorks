using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.UI.ShippingPanel.ValueConverters
{
    /// <summary>
    /// Convert multiple sources of ShipmentTypeCodes to a single list
    /// </summary>
    public class ShipmentTypeListConverter : IMultiValueConverter
    {
        private readonly Func<ShipmentTypeCode, string> getDescription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeListConverter() : this(x => EnumHelper.GetDescription(x))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeListConverter(Func<ShipmentTypeCode, string> getDescription)
        {
            this.getDescription = getDescription;
        }

        /// <summary>
        /// Convert to a list shipment type codes
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return Enumerable.Empty<ShipmentTypeListItem>();
            }

            IEnumerable<ShipmentTypeCode> codesFromLists = values.OfType<IEnumerable<ShipmentTypeCode>>().SelectMany(x => x);

            IEnumerable<ShipmentTypeCode> results = values.OfType<ShipmentTypeCode>()
                .Concat(codesFromLists)
                .Distinct()
                .OrderBy(x => ShipmentTypeManager.GetSortValue(x));

            if (values.Length > 1 && values[1] is ShipmentTypeCode)
            {
                // We don't want Amazon in the list, so filter it out, unless the initial shipment type code was amazon
                ShipmentTypeCode? intialShipmentTypeCode = (ShipmentTypeCode) values[1];
                results = results.Where(x => IsIncludedShipmentType(x) ||
                    IsShipmentTypeInUseByShipment(x, intialShipmentTypeCode));
            }

            return results.Select(x => new ShipmentTypeListItem(x, getDescription(x))).ToList() ?? Enumerable.Empty<ShipmentTypeListItem>();
        }

        /// <summary>
        /// We don't support converting back
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Is the shipment type currently in use by the shipment
        /// </summary>
        private bool IsShipmentTypeInUseByShipment(ShipmentTypeCode shipmentTypeCode, ShipmentTypeCode? intialShipmentTypeCode)
        {
            return intialShipmentTypeCode.HasValue &&
                intialShipmentTypeCode == shipmentTypeCode;
        }

        /// <summary>
        /// Should the specified shipment type be excluded
        /// </summary>
        private bool IsIncludedShipmentType(ShipmentTypeCode shipmentTypeCode)
        {
            return shipmentTypeCode != ShipmentTypeCode.Amazon &&
                shipmentTypeCode != ShipmentTypeCode.BestRate;
        }
    }
}

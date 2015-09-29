using Interapptive.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.Views.ShippingPanel.ValueConverters
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

            return values.OfType<ShipmentTypeCode>()
                .Concat(codesFromLists)
                .Distinct()
                .OrderBy(x => ShipmentTypeManager.GetSortValue(x))
                .Select(x => new ShipmentTypeListItem(x, getDescription(x))).ToList() ?? 
                Enumerable.Empty<ShipmentTypeListItem>();
        }

        /// <summary>
        /// We don't support converting back
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

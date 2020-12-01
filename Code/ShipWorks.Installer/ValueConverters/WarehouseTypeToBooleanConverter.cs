using System;
using System.Globalization;
using System.Windows.Data;
using ShipWorks.Installer.Api.DTO;

namespace ShipWorks.Installer.ValueConverters
{
    /// <summary>
    /// Convert WarehouseType to Boolean
    /// </summary>
    public class WarehouseTypeToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Convert from WarehouseType to Boolean
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            var warehouse = (Warehouse) value;

            return warehouse.Details.SQLConfig == null;
        }

        /// <summary>
        /// Convert from Boolean to WarehouseType
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

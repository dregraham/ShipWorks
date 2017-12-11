using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// True if not nulls
    /// </summary>
    public class AddressKeyValueToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// If its null return false otherwise true
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KeyValuePair<string, ValidatedAddressEntity>)
            {
                KeyValuePair<string, ValidatedAddressEntity> data = (KeyValuePair<string, ValidatedAddressEntity>)value;

                if (data.Value == null)
                {
                    return false;
                }

                return true;
            }
            return false;
        }
        /// <summary>
        /// Do not convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
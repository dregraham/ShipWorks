using Interapptive.Shared.Business.Geography;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI
{
    /// <summary>
    /// Convert a country input to a list of applicable states or provinces
    /// </summary>
    public class StateProvinceConverter : IValueConverter
    {
        /// <summary>
        /// Convert from country to state
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string country = value as string;

            if (country == "US")
            {
                return Geography.States;
            }

            if (country == "CA")
            {
                return Geography.Provinces;
            }

            return new List<string>();
        }

        /// <summary>
        /// Don't support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Cannot convert back");
        }
    }
}

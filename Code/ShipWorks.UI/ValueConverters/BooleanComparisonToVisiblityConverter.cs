using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Compare a collection of booleans and return visible or collapsed if true or false, respectively
    /// </summary>
    public class BooleanComparisonToVisiblityConverter : BooleanComparisonConverter
    {
        /// <summary>
        /// Convert a collection of booleans to Visbile or Collapsed
        /// </summary>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if((bool) base.Convert(values, targetType, parameter, culture))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
    }
}

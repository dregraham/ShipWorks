using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    /// <summary>
    /// Combines order, address and item maps into one collection of maps to be bound to using multibinding.
    /// </summary>
    public class OdbcFieldMapMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts the given OdbcFieldMapDisplays into a collection of OdbcFieldMapDisplays to be bound to.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            OdbcFieldMapDisplay order = values[0] as OdbcFieldMapDisplay;
            OdbcFieldMapDisplay address = values[1] as OdbcFieldMapDisplay;
            ObservableCollection<OdbcFieldMapDisplay> items = values[2] as ObservableCollection<OdbcFieldMapDisplay>;

            ObservableCollection<OdbcFieldMapDisplay> maps = new ObservableCollection<OdbcFieldMapDisplay> { order, address };

            items?.ToList().ForEach(maps.Add);

            return maps;
        }

        /// <summary>
        /// Not needed for this converter
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ShipWorks.Stores.UI.Platforms.Odbc
{
    public class OdbcFieldMapMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            OdbcFieldMapDisplay order = values[0] as OdbcFieldMapDisplay;
            OdbcFieldMapDisplay address = values[1] as OdbcFieldMapDisplay;
            ObservableCollection<OdbcFieldMapDisplay> items = values[2] as ObservableCollection<OdbcFieldMapDisplay>;

            ObservableCollection<OdbcFieldMapDisplay> maps = new ObservableCollection<OdbcFieldMapDisplay> {order, address};

            items?.ToList().ForEach(maps.Add);

            return maps;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
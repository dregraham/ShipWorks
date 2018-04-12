using System;
using System.Globalization;
using System.Windows.Data;
using ShipWorks.Data.Administration;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// Convert a schema version to a text status
    /// </summary>
    public class SchemaVersionStatusConverter : IValueConverter
    {
        /// <summary>
        /// Perform the conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Version schemaVersion)
            {
                var requiredSchemaVersion = SqlSchemaUpdater.GetRequiredSchemaVersion();

                if (schemaVersion > SqlSchemaUpdater.GetRequiredSchemaVersion())
                {
                    return "Newer";
                }
                else if (schemaVersion < SqlSchemaUpdater.GetRequiredSchemaVersion())
                {
                    return "Out of Date";
                }
                else
                {
                    return "Ready";
                }
            }

            return "(Unknown)";
        }

        /// <summary>
        /// We don't support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

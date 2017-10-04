using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Core.UI.ValueConverters
{
    /// <summary>
    /// Convert a shipment type code to a string
    /// </summary>
    public class ShipmentDateCutoffConverter : IValueConverter
    {
        // Use a func<func> so that we can handle designer, production, and test execution more easily, since
        // each environment has different methods of getting dependencies into the object
        private readonly Func<Type, Func<IShippingSettingsEntity, object>, object> withShippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDateCutoffConverter()
        {
            withShippingSettings = (targetType, action) =>
            {
                if (DesignModeDetector.IsDesignerHosted())
                {
                    return targetType == typeof(string) ?
                        "Cutoff time is 3:00 PM" :
                        (object) Visibility.Visible;
                }

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    return action(lifetimeScope.Resolve<IShippingSettings>().FetchReadOnly());
                }
            };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDateCutoffConverter(IShippingSettings shippingSettings)
        {
            withShippingSettings = (targetType, action) => action(shippingSettings.FetchReadOnly());
        }

        /// <summary>
        /// Convert
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string) && targetType != typeof(Visibility))
            {
                throw new ArgumentException("Target type must be string or Visibility");
            }

            switch (value as ShipmentTypeCode?)
            {
                case ShipmentTypeCode.Usps:
                    return withShippingSettings(targetType, x =>
                        GetConvertedValue(targetType, x.UspsShippingDateCutoffEnabled, x.UspsShippingDateCutoffTime));
                default:
                    return GetConvertedValue(targetType, false, TimeSpan.MinValue);
            }
        }

        /// <summary>
        /// Get the converted value
        /// </summary>
        private object GetConvertedValue(Type targetType, bool enabled, TimeSpan cutoffTime)
        {
            if (targetType == typeof(string))
            {
                return enabled ?
                    $"Labels created after {FormatTime(cutoffTime)} today will have a ship date of the next valid shipping day." :
                    string.Empty;
            }

            if (targetType == typeof(Visibility))
            {
                return enabled ? Visibility.Visible : Visibility.Hidden;
            }

            throw new InvalidOperationException("Target type must be string or Visibility");
        }

        /// <summary>
        /// Format the time
        /// </summary>
        private string FormatTime(TimeSpan time)
        {
            return time.Hours > 12 ?
                time.Subtract(TimeSpan.FromHours(12)).ToString(@"h\:mm") + " PM" :
                time.ToString(@"h\:mm") + " AM";
        }

        /// <summary>
        /// Convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Cannot convert back");
        }
    }
}

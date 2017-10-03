using System;
using System.Globalization;
using System.Windows.Data;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ShippingPanel.ValueConverters
{
    /// <summary>
    /// Convert a shipment type code to a string
    /// </summary>
    public class ShipmentDateCutoffConverter : IValueConverter
    {
        //private readonly IShippingSettings shippingSettings;
        private readonly Func<Func<IShippingSettingsEntity, string>, string> withShippingSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDateCutoffConverter()
        {
            withShippingSettings = action =>
            {
                if (DesignModeDetector.IsDesignerHosted())
                {
                    return "Cutoff time is 3:00 PM";
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
            withShippingSettings = action => action(shippingSettings.FetchReadOnly());
        }

        /// <summary>
        /// Convert
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
            {
                throw new ArgumentException("Target type must be string");
            }

            if (value == null)
            {
                return string.Empty;
            }

            if (!(value is ShipmentTypeCode))
            {
                throw new ArgumentException("Value must be ShipmentType");
            }

            switch ((ShipmentTypeCode) value)
            {
                case ShipmentTypeCode.Usps:
                    return withShippingSettings(x =>
                    {
                        return x.UspsShippingDateCutoffEnabled ?
                            $"Shipment cutoff time is {FormatTime(x.UspsShippingDateCutoffTime)}" :
                            string.Empty;
                    });
                default:
                    return string.Empty;
            }

            return string.Empty;
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

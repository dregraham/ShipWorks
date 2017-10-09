using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Settings;
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
        readonly Func<ShipmentTypeCode, ShipmentDateCutoff> getShipmentDateCutoff;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentDateCutoffConverter()
        {
            getShipmentDateCutoff = x =>
            {
                if (DesignModeDetector.IsDesignerHosted())
                {
                    return new ShipmentDateCutoff(true, TimeSpan.FromHours(17));
                }

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    return lifetimeScope.Resolve<IShippingSettings>().FetchReadOnly().GetShipmentDateCutoff(x);
                }
            };
        }

        /// <summary><shippingsettingsentity
        /// Constructor
        /// </summary>
        public ShipmentDateCutoffConverter(IShippingSettings shippingSettings)
        {
            getShipmentDateCutoff = shippingSettings.FetchReadOnly().GetShipmentDateCutoff;
        }

        /// <summary>
        /// Convert
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ShipmentTypeCode? shipmentType = value as ShipmentTypeCode?;

            var shipmentDateCutoff = shipmentType.HasValue ?
                getShipmentDateCutoff(shipmentType.Value) :
                ShipmentDateCutoff.Default;

            if (targetType == typeof(string))
            {
                return shipmentDateCutoff.Enabled ?
                    $"Shipments processed after {FormatTime(shipmentDateCutoff.CutoffTime)} today will have a ship date of the next valid shipping day." +
                    $"{Environment.NewLine}To update this setting, go to Manage > Shipping Settings > {EnumHelper.GetDescription(shipmentType.GetValueOrDefault())} > Settings." :
                    string.Empty;
            }

            if (targetType == typeof(Visibility))
            {
                return shipmentDateCutoff.Enabled ? Visibility.Visible : Visibility.Hidden;
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

using System;
using System.Globalization;
using System.Windows.Data;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Returns string.Empty or value based on whether a bound property is equal to the converter parameter
    /// </summary>
    public class StringEmptyWhenEqualToParameterConverter : IValueConverter
    {
        private bool isDesignMode;

        /// <summary>
        /// Constructor
        /// </summary>
        public StringEmptyWhenEqualToParameterConverter() : this(DesignModeDetector.IsDesignerHosted())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StringEmptyWhenEqualToParameterConverter(bool isDesignMode)
        {
            this.isDesignMode = isDesignMode;
        }

        /// <summary>
        /// Convert to an empty string if the value is the parameter
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = Equals(value.ToString().Trim(), parameter.ToString());

            return b ? string.Empty : value.ToString().Trim();
        }

        /// <summary>
        /// Convert to the parameter if the value is an empty string
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (value?.ToString().Trim() == string.Empty) ? parameter : value;
    }
}

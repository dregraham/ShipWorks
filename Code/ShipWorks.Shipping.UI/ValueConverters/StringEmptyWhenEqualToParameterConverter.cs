using ShipWorks.UI.Controls.Design;
using System;
using System.Globalization;
using System.Windows;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Returns string.Empty or value based on whether a bound property is equal to the converter parameter
    /// </summary>
    public class StringEmptyWhenEqualToParameterConverter : ValueEqualToParameterConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StringEmptyWhenEqualToParameterConverter() : this(false, DesignModeDetector.IsDesignerHosted())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StringEmptyWhenEqualToParameterConverter(bool invert, bool isDesignMode) : base(invert, isDesignMode)
        {

        }
        
        /// <summary>
        /// Return Visible if the bound value is equal to the converter parameter, else collapsed
        /// </summary>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool) base.Convert(value.ToString().Trim(), typeof (bool), parameter.ToString(), culture);

            return b ? string.Empty : value.ToString().Trim();
        }

        /// <summary>
        /// Converting back does not make sense here
        /// </summary>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

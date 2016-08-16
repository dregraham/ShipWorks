using ShipWorks.UI.Controls.Design;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Show or hide UI elements based on whether a bound property is equal to the converter parameter
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class VisibleWhenEqualToParameterConverter : ValueEqualToParameterConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VisibleWhenEqualToParameterConverter() : this(false, DesignModeDetector.IsDesignerHosted())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public VisibleWhenEqualToParameterConverter(bool invert, bool isDesignMode) : base(invert, isDesignMode)
        {

        }

        /// <summary>
        /// Return Visible if the bound value is equal to the converter parameter, else collapsed
        /// </summary>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
            {
                throw new InvalidOperationException("Destination type of value converter must be Visibility");
            }

            bool b = (bool)base.Convert(value, typeof(bool), parameter, culture);
            Visibility viz = b ? Visibility.Visible : Visibility.Collapsed;

          return viz;
        }
    }
}

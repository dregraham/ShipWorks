using ShipWorks.UI.Controls.Design;
using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts to a boolean when the bound value is equal to the converter parameter
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class ValueEqualToParameterConverter : IValueConverter
    {
        readonly bool inDesignMode;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEqualToParameterConverter() : this(false, DesignModeDetector.IsDesignerHosted())
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueEqualToParameterConverter(bool invert, bool inDesignMode)
        {
            Invert = invert;
            this.inDesignMode = inDesignMode;
        }

        /// <summary>
        /// Should we show or hide when the bound value equals the parameter
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Return true if the bound value is equal to the converter parameter, else false
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
            {
                throw new InvalidOperationException("Destination type of value converter must be a bool");
            }

            return inDesignMode || (Equals(value, parameter) ^ Invert);
        }

        /// <summary>
        /// Converting back does not make sense here
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Cannot convert back");
        }
    }
}

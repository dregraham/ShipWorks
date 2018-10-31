using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Visibility based on multiple visibilities
    /// </summary>
    public class VisibilityMultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VisibilityMultiConverter() : this(BooleanOperator.And)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public VisibilityMultiConverter(BooleanOperator booleanOperator)
        {
            BooleanOperator = booleanOperator;
        }

        /// <summary>
        /// Operator that should be used
        /// </summary>
        public BooleanOperator BooleanOperator { get; set; }

        /// <summary>
        /// Convert the value
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var visibilityValues = values.OfType<Visibility>();
            var shouldShow = BooleanOperator == BooleanOperator.And ?
                visibilityValues.All(x => x == Visibility.Visible) :
                visibilityValues.Any(x => x == Visibility.Visible);

            return shouldShow ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert back to source
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("We don't support converting back");
        }
    }
}

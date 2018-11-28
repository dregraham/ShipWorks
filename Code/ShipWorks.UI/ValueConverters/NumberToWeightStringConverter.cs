using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert double to formatted weight text
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class NumberToWeightStringConverter : IValueConverter
    {
        /// <summary>
        /// Convert from Double to weight String (lbs and oz)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            GetConverter().FormatWeight(System.Convert.ToDouble(value));

        /// <summary>
        /// Convert from weight String (lbs and oz) to Double
        /// </summary>
        [SuppressMessage("SonarQube", "S2486:Exceptions should not be ignored",
            Justification = "We treat a format exception as just invalid data, so the exception should be eaten")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            GetConverter().ParseWeight(value as string);

        /// <summary>
        /// Get the converter that should be used
        /// </summary>
        private IWeightConverter GetConverter() =>
            DesignModeDetector.IsDesignerHosted() ?
            new WeightConverter(null) :
            IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IWeightConverter>>().Value;
    }
}

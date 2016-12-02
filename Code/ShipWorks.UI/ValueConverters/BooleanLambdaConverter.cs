using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Converts the boolean results of the lambda with the given input to another type.
    /// </summary>
    [Obfuscation(Exclude=true)]
    public abstract class BooleanLambdaConverter<TInput, TOutput> : IValueConverter
    {
        private readonly Func<TInput, bool> evaluatorFunc;
        private readonly bool inDesignMode;
        private readonly TOutput trueValue;
        private readonly TOutput falseValue;

        protected BooleanLambdaConverter(Func<TInput, bool> evaluatorFunc, TOutput trueValue, TOutput falseValue, bool inDesignMode)
        {
            this.evaluatorFunc = evaluatorFunc;
            this.inDesignMode = inDesignMode;
            this.trueValue = trueValue;
            this.falseValue = falseValue;
        }

        /// <summary>
        /// Converts a value to TOutput
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (inDesignMode || (value is TInput &&  evaluatorFunc((TInput) value))) ? trueValue : falseValue;
        }

        /// <summary>
        /// Convert a value back to TInput
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

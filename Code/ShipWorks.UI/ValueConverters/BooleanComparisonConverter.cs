﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Operator to use for boolean comparisons
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BooleanOperator
    {
        [Description("And")]
        And,

        [Description("Or")]
        Or
    }

    /// <summary>
    /// Compare a collection of booleans
    /// </summary>
    public class BooleanComparisonConverter : IMultiValueConverter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanComparisonConverter() : this(BooleanOperator.And)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanComparisonConverter(BooleanOperator booleanOperator)
        {
            BooleanOperator = booleanOperator;
        }

        /// <summary>
        /// Operator that should be used
        /// </summary>
        public BooleanOperator BooleanOperator { get; set; }

        /// <summary>
        /// Convert a collection of booleans to a single boolean
        /// </summary>
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<bool> booleanValues = values.OfType<bool>();
            return BooleanOperator == BooleanOperator.And ?
                booleanValues.All(x => x) :
                booleanValues.Any(x => x);
        }

        /// <summary>
        /// Don't support converting back
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

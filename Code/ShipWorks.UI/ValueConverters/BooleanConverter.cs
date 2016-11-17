﻿using ShipWorks.UI.Controls.Design;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean into another type
    /// </summary>
    [Obfuscation(Exclude = true)]
    public abstract class BooleanConverter<T> : IValueConverter
    {
        readonly bool inDesignMode;

        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanConverter(T trueValue, T falseValue) : this(trueValue, falseValue, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanConverter(T trueValue, T falseValue, bool inDesignMode)
        {
            True = trueValue;
            False = falseValue;
            Invert = false;
            this.inDesignMode = inDesignMode;
        }

        /// <summary>
        /// Value to use for true
        /// </summary>
        public T True { get; set; }

        /// <summary>
        /// Value to use for false
        /// </summary>
        public T False { get; set; }

        /// <summary>
        /// Return the opposite value based on criteria.  
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Convert a boolean into the requested values
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is bool && ((bool) value)) || inDesignMode) ^ Invert ? True : False;
        }

        /// <summary>
        /// Convert a value back to boolean
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is T && EqualityComparer<T>.Default.Equals((T) value, True)) ^ Invert;
        }
    }
}

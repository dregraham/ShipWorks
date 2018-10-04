﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// True if not null
    /// </summary>
    public class NullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// If its null, return false, else true
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value != null;

        /// <summary>
        /// Do not convert back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
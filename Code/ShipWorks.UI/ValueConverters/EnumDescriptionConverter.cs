﻿using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Stores;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert an enum into it's description
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class EnumDescriptionConverter : IValueConverter
    {
        /// <summary>
        /// Convert an enum into it's description
        /// </summary>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignModeDetector.IsDesignerHosted())
            {
                return "Designer enum description.";
            }

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return null;
            }

            return EnumHelper.GetDescription(value as Enum);
        }

        /// <summary>
        /// Convert a value back to enum
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

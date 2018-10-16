using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.UI.ValueConverters
{
    public class UpsEmailNotificationTypeConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Dependency property for Flag.
        /// </summary>
        public static readonly DependencyProperty FlagToCheckProperty = 
            DependencyProperty.Register("FlagToCheck", typeof(UpsEmailNotificationType), typeof(UpsEmailNotificationTypeConverter));
        /// <summary>
        /// Dependency property for Flags.
        /// </summary>
        public static readonly DependencyProperty FlagValueProperty = 
            DependencyProperty.Register("FlagValue", typeof(int), typeof(UpsEmailNotificationTypeConverter), new PropertyMetadata());

        public UpsEmailNotificationType FlagToCheck
        {
            get => (UpsEmailNotificationType) GetValue(FlagToCheckProperty);
            set => SetValue(FlagToCheckProperty, value);
        }

        public int FlagValue
        {
            get => (int) GetValue(FlagValueProperty);
            set => SetValue(FlagValueProperty, value);
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((UpsEmailNotificationType)FlagValue).HasFlag(FlagToCheck);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UpsEmailNotificationType notifications = (UpsEmailNotificationType) FlagValue;
            
            if (value is bool boolValue)
            {
                if (boolValue)
                {
                    notifications |= FlagToCheck;
                }
                else
                {
                    notifications &= ~ FlagToCheck;
                }
            }

            return (int) notifications;
        }
    }
}
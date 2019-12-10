﻿using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// TabItem for the ScanToShipControl
    /// </summary>
    public class ScanToShipTabItem : TabItem
    {
        public static readonly DependencyProperty SuccessProperty = DependencyProperty.Register("Success", typeof(bool), typeof(ScanToShipTabItem), new PropertyMetadata(false));
        public static readonly DependencyProperty ErrorProperty = DependencyProperty.Register("Error", typeof(bool), typeof(ScanToShipTabItem), new PropertyMetadata(false));

        /// <summary>
        /// Whether or not this tabs operation was successful
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Success
        {
            get => (bool) GetValue(SuccessProperty);
            set => SetValue(SuccessProperty, value);
        }

        /// <summary>
        /// Whether or not there is an error on this tab
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Error
        {
            get => (bool) GetValue(ErrorProperty);
            set => SetValue(ErrorProperty, value);
        }
    }
}

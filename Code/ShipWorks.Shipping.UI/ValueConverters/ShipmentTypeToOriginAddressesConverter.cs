﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Convert a shipment type into a list of available origin addresses
    /// </summary>
    public class ShipmentTypeToOriginAddressesConverter : IValueConverter
    {
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor (should only be used by WPF)
        /// </summary>
        public ShipmentTypeToOriginAddressesConverter() : this(null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeToOriginAddressesConverter(IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager ?? GetShipmentTypeManager();
        }

        /// <summary>
        /// Convert the shipment type into a list of origins
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ShipmentTypeCode))
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            return shipmentTypeManager.Get((ShipmentTypeCode)value).GetOrigins();
        }

        /// <summary>
        /// Do not support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Does not support converting back");
        }

        /// <summary>
        /// Get a shipment type factory
        /// </summary>
        private IShipmentTypeManager GetShipmentTypeManager() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IShipmentTypeManager>>().Value;
    }
}

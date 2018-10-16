using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Services;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Convert from CarrierAdapter to Available ShipmentTypes
    /// </summary>
    public class CarrierAdapterToAvailableShipmentTypesConverter : IValueConverter
    {
        private readonly ShipmentTypeProvider shipmentTypeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierAdapterToAvailableShipmentTypesConverter() : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierAdapterToAvailableShipmentTypesConverter(ShipmentTypeProvider shipmentTypeProvider)
        {
            this.shipmentTypeProvider = shipmentTypeProvider ?? GetShipmentTypeProvider();
        }

        /// <summary>
        /// Get a Dictinary of ShipmentTypes from ICarrierShipmentAdapter
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICarrierShipmentAdapter)
            {
                return shipmentTypeProvider.GetAvailableShipmentTypes(value as ICarrierShipmentAdapter)
                    .ToDictionary(s => s, s => EnumHelper.GetDescription(s));
            }

            return new Dictionary<ShipmentTypeCode, string>();
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The ShipmentTypeProvider
        /// </summary>
        private ShipmentTypeProvider GetShipmentTypeProvider() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<ShipmentTypeProvider>>().Value;
    }
}

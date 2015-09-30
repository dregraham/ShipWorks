using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.Views.ShippingPanel.ValueConverters
{
    /// <summary>
    /// Convert a shipment type into a list of available accounts
    /// </summary>
    public class ShipmentTypeToAccountsConverter : IValueConverter
    {
        private IShippingAccountListProvider shippingAccountListProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentTypeToAccountsConverter"/> class.
        /// </summary>
        public ShipmentTypeToAccountsConverter() : this(null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentTypeToAccountsConverter"/> class.
        /// </summary>
        public ShipmentTypeToAccountsConverter(IShippingAccountListProvider shippingAccountListProvider)
        {
            this.shippingAccountListProvider = shippingAccountListProvider ?? GetShippingAccountListProvider();
        }

        /// <summary>
        /// Convert the shipment type into a list of ICarrierAccounts
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ShipmentTypeCode))
            {
                return Enumerable.Empty<ICarrierAccount>();
            }

            IEnumerable<ICarrierAccount> availableAccounts = shippingAccountListProvider.GetAvailableAccounts((ShipmentTypeCode) value);

            if (availableAccounts.None())
            {
                return new List<ICarrierAccount> { new NullCarrierAccount() };
            }

            return availableAccounts;
        }

        /// <summary>
        /// Not supported
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Get a shipment type factory
        /// </summary>
        private IShippingAccountListProvider GetShippingAccountListProvider() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IShippingAccountListProvider>>().Value;
    }
}
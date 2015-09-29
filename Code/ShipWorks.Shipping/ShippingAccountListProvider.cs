using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides an IObservable collection of accounts for the shipment type
    /// </summary>
    public class ShippingAccountListProvider : IShippingAccountListProvider
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRetriever<ICarrierAccount>> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingAccountListProvider(IIndex<ShipmentTypeCode, ICarrierAccountRetriever<ICarrierAccount>> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Gets the available accounts for the ShipmentTypeCode
        /// </summary>
        public IEnumerable<ICarrierAccount> GetAvailableAccounts(ShipmentTypeCode shipmentTypeCode)
        {
            return lookup[shipmentTypeCode].Accounts;
        }
    }
}

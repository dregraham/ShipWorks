using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Provides an IObservable collection of accounts for the shipment type
    /// </summary>
    public class ShippingAccountListProvider : IShippingAccountListProvider
    {
        private readonly IIndex<ShipmentTypeCode, ICarrierAccountRetriever> lookup;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingAccountListProvider(IIndex<ShipmentTypeCode, ICarrierAccountRetriever> lookup)
        {
            this.lookup = lookup;
        }

        /// <summary>
        /// Gets the available accounts for the ShipmentTypeCode
        /// </summary>
        public IEnumerable<ICarrierAccount> GetAvailableAccounts(ShipmentTypeCode shipmentTypeCode)
        {
            return lookup[shipmentTypeCode].AccountsReadOnly;
        }
    }
}

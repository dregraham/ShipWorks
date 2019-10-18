using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    public interface IShippingAccountListProvider
    {

        /// <summary>
        /// Gets the available accounts.
        /// </summary>
        IEnumerable<ICarrierAccount> GetAvailableAccounts(ShipmentTypeCode shipmentTypeCode);
    }
}

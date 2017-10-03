using Interapptive.Shared.ComponentRegistration;
using ShipEngine.ApiClient.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Factory for creating ShipEngine CarrierAccountsApi
    /// </summary>
    [Component]
    public class ShipEngineCarrierAccountsApiFactory : IShipEngineCarrierAccountsApiFactory
    {
        /// <summary>
        /// Create the CarrierAccountsApi
        /// </summary>
        public ICarrierAccountsApi CreateCarrierAccountsApi() => new CarrierAccountsApi();
    }
}

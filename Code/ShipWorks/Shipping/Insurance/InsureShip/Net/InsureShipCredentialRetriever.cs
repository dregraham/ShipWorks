using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Retrieve InsureShip credentials
    /// </summary>
    /// <remarks>This is where we'll retrieve credentials from either Tango or some other api</remarks>
    [Component]
    public class InsureShipCredentialRetriever : IInsureShipCredentialRetriever
    {
        /// <summary>
        /// Retrieve InsureShip credentials from a given store
        /// </summary>
        public IInsureShipCredentials Get(IStoreEntity store) =>
            new InsureShipCredentials(store);
    }
}
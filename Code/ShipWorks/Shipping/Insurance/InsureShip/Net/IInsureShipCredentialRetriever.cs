using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Retrieve InsureShip credentials
    /// </summary>
    public interface IInsureShipCredentialRetriever
    {
        /// <summary>
        /// Retrieve InsureShip credentials from a given store
        /// </summary>
        IInsureShipCredentials Get(IStoreEntity store);
    }
}
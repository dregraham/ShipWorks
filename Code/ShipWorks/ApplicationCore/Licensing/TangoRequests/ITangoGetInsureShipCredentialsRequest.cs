using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get InsureShip credentials from Tango
    /// </summary>
    public interface ITangoGetInsureShipCredentialsRequest
    {
        /// <summary>
        /// Populate the given store with InsureShip credentials
        /// </summary>
        void PopulateCredentials(StoreEntity store);
    }
}

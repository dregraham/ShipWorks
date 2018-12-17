using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Credentials for InsureShip
    /// </summary>
    public class InsureShipCredentials : IInsureShipCredentials
    {
        private IStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipCredentials(IStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Client ID
        /// </summary>
        public string ClientID => store.InsureShipClientID.ToString();

        /// <summary>
        /// Api Key
        /// </summary>
        public string ApiKey => store.InsureShipApiKey;
    }
}
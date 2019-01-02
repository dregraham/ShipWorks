using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Retrieve InsureShip credentials
    /// </summary>
    /// <remarks>This is where we'll retrieve credentials from either Tango or some other api</remarks>
    [Component]
    public class InsureShipCredentialRetriever : IInsureShipCredentialRetriever
    {
        private readonly IStoreManager storeManager;
        private readonly ITangoGetInsureShipCredentialsRequest getInsureShipCredentialsRequest;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsureShipCredentialRetriever(
            IStoreManager storeManager,
            ITangoGetInsureShipCredentialsRequest getInsureShipCredentialsRequest)
        {
            this.getInsureShipCredentialsRequest = getInsureShipCredentialsRequest;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Retrieve InsureShip credentials from a given store
        /// </summary>
        public IInsureShipCredentials Get(IStoreEntity store) =>
            BuildCredentialsFrom(store)
                .OrElse(() => LoadCredentials(store.StoreID));

        /// <summary>
        /// Get InsureShip credentials from Tango
        /// </summary>
        private IInsureShipCredentials LoadCredentials(long storeID)
        {
            var store = storeManager.GetStore(storeID);

            return BuildCredentialsFrom(store)
                .OrElse(() => GetCredentialsFromTango(store));
        }

        /// <summary>
        /// Get the InsureShip credentials from Tango
        /// </summary>
        private IInsureShipCredentials GetCredentialsFromTango(StoreEntity store)
        {
            getInsureShipCredentialsRequest.PopulateCredentials(store);
            storeManager.SaveStore(store);

            return new InsureShipCredentials(store);
        }

        /// <summary>
        /// Build credentials from the given store
        /// </summary>
        private Maybe<IInsureShipCredentials> BuildCredentialsFrom(IStoreEntity store) =>
            string.IsNullOrEmpty(store.InsureShipApiKey) || !store.InsureShipClientID.HasValue ?
                Maybe.Empty<IInsureShipCredentials>() :
                Maybe.Value<IInsureShipCredentials>(new InsureShipCredentials(store));
    }
}
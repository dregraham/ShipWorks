using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Provides credentials for making requests to the Sears api
    /// </summary>
    public interface ISearsCredentials
    {
        /// <summary>
        /// Adds credentials to the request based on the store
        /// </summary>
        void AddCredentials(ISearsStoreEntity store, IHttpVariableRequestSubmitter request);
    }
}
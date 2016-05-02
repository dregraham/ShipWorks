using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Linq;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Provides credentials for making requests to the Sears api
    /// </summary>
    public class SearsCredentials
    {
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsCredentials(IDateTimeProvider dateTimeProvider)
        {
            MethodConditions.EnsureArgumentIsNotNull(dateTimeProvider);

            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Adds credentials to the request based on the store
        /// </summary>
        public void AddCredentials(SearsStoreEntity store, HttpVariableRequestSubmitter request)
        {
            GetCredentialsHttpVariables(store).ToList().ForEach(v => request.Variables.Add(v));
        }

        /// <summary>
        /// Return the collection of HTTP variables for authenticating
        /// </summary>
        private static HttpVariableCollection GetCredentialsHttpVariables(SearsStoreEntity store)
        {
            HttpVariableCollection credentials = new HttpVariableCollection();
            credentials.Add("email", store.Email);
            credentials.Add("password", SecureText.Decrypt(store.Password, store.Email));

            return credentials;
        }
    }
}

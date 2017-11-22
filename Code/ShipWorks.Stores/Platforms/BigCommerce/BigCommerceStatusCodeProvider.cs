using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Provides online status codes for BigCommerce
    /// </summary>
    [Component]
    public class BigCommerceStatusCodeProvider : OnlineStatusCodeProvider<int>, IBigCommerceStatusCodeProvider
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceStatusCodeProvider));
        readonly IBigCommerceWebClientFactory webClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceStatusCodeProvider(BigCommerceStoreEntity store, IBigCommerceWebClientFactory webClientFactory)
            : base(store, BigCommerceStoreFields.StatusCodes)
        {
            this.webClientFactory = webClientFactory;
        }

        /// <summary>
        /// Retrieve new status codes from BigCommerce
        /// </summary>
        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            GenericResult<Dictionary<int, string>> codeMapResult = Task.Run(async () =>
            {
                try
                {
                    return GenericResult.FromSuccess(await GetCodeMapAsync(Store as BigCommerceStoreEntity).ConfigureAwait(false));
                }
                catch (BigCommerceException ex)
                {
                    log.ErrorFormat("Failed to fetch online status codes from BigCommerce: {0}", ex);
                    return GenericResult.FromError<Dictionary<int, string>>("Failed to fetch online status codes from BigCommerce.");
                }

            }).Result;

            if (codeMapResult.Success)
            {
                Dictionary<int, string> codeMap = codeMapResult.Value;

                // BigCommerce has "Deleted" status, but does not return it via the api.  So manually add it here.
                if (!codeMap.Keys.Contains(BigCommerceConstants.OnlineStatusDeletedCode))
                {
                    codeMap.Add(BigCommerceConstants.OnlineStatusDeletedCode, BigCommerceConstants.OnlineStatusDeletedName);
                }

                return codeMap;
            }

            return null;
        }

        /// <summary>
        /// Get the code map from the given store
        /// </summary>
        private async Task<Dictionary<int, string>> GetCodeMapAsync(IBigCommerceStoreEntity store)
        {
            IBigCommerceWebClient client = webClientFactory.Create(store);
            var codes = await client.FetchOrderStatuses().ConfigureAwait(false);
            return codes.ToDictionary(x => x.StatusID, x => x.StatusText);
        }
    }
}

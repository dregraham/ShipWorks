using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Provides online status codes for BigCommerce
    /// </summary>
    public class BigCommerceStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(BigCommerceStatusCodeProvider));

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceStatusCodeProvider(BigCommerceStoreEntity store)
            : base(store, BigCommerceStoreFields.StatusCodes)
        {

        }

        /// <summary>
        /// Retrieve new status codes from BigCommerce
        /// </summary>
        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            try
            {
                Dictionary<int, string> codeMap = new Dictionary<int, string>();
                BigCommerceStoreEntity store = (BigCommerceStoreEntity)Store;
                BigCommerceWebClient client = new BigCommerceWebClient(store.ApiUserName, store.ApiUrl, store.ApiToken);
                
                foreach (BigCommerceOrderStatus orderStatus in client.FetchOrderStatuses())
                {
                    codeMap.Add(orderStatus.StatusID, orderStatus.StatusText);
                }

                // BigCommerce has "Deleted" status, but does not return it via the api.  So manually add it here.
                if (!codeMap.Keys.Contains(BigCommerceConstants.OnlineStatusDeletedCode))
                {
                    codeMap.Add(BigCommerceConstants.OnlineStatusDeletedCode, BigCommerceConstants.OnlineStatusDeletedName);
                }

                return codeMap;
            }
            catch (BigCommerceException ex)
            {
                log.ErrorFormat("Failed to fetch online status codes from BigCommerce: {0}", ex);

                return null;
            }
        }
    }
}

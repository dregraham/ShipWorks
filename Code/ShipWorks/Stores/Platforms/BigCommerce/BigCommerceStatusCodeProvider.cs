using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;
using ShipWorks.Data.Model.EntityInterfaces;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Provides online status codes for BigCommerce
    /// </summary>
    [Component(RegistrationType.Self)]
    public class BigCommerceStatusCodeProvider : OnlineStatusCodeProvider<int>
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
            try
            {
                Dictionary<int, string> codeMap = GetCodeMap(Store as IBigCommerceStoreEntity);

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

        /// <summary>
        /// Get the code map from the given store
        /// </summary>
        private Dictionary<int, string> GetCodeMap(IBigCommerceStoreEntity store)
        {
            IBigCommerceWebClient client = webClientFactory.Create(store);
            return client.FetchOrderStatuses().ToDictionary(x => x.StatusID, x => x.StatusText);
        }
    }
}

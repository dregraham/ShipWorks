using System.Collections.Generic;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Provides online status codes for AmeriCommerce
    /// </summary>
    public class AmeriCommerceStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        // Logger 
        private static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceStatusCodeProvider));
        private readonly AmeriCommerceStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceStatusCodeProvider(AmeriCommerceStoreEntity store)
            : base(store, AmeriCommerceStoreFields.StatusCodes)
        {
            this.store = store;
        }

        /// <summary>
        /// Retrieve new status codes from AmeriCommerce
        /// </summary>
        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            try
            {
                Dictionary<int, string> codeMap = new Dictionary<int, string>();

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    // Create the client for connecting to the module
                    IAmeriCommerceWebClient webClient = lifetimeScope.Resolve<IAmeriCommerceWebClient>(TypedParameter.From(store));

                    foreach (OrderStatusTrans orderStatus in webClient.GetStatusCodes())
                    {
                        codeMap.Add(orderStatus.orderStatusID.GetValue(0), orderStatus.orderStatus);
                    }
                }

                return codeMap;
            }
            catch (AmeriCommerceException ex)
            {
                log.ErrorFormat("Failed to fetch online status codes from AmeriCommerce: {0}", ex);

                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Provides online status codes for AmeriCommerce
    /// </summary>
    public class ThreeDCartStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ThreeDCartStatusCodeProvider));

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartStatusCodeProvider(ThreeDCartStoreEntity store)
            : base(store, ThreeDCartStoreFields.StatusCodes)
        {

        }

        /// <summary>
        /// Retrieve new status codes from ThreeDCart
        /// </summary>
        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            try
            {
                Dictionary<int, string> codeMap = new Dictionary<int, string>();

                ThreeDCartWebClient client = new ThreeDCartWebClient((ThreeDCartStoreEntity)Store, null);
                foreach (ThreeDCartOrderStatus orderStatus in client.OrderStatuses)
                {
                    codeMap.Add(orderStatus.StatusID, orderStatus.StatusText);
                }

                return codeMap;
            }
            catch (ThreeDCartException ex)
            {
                log.ErrorFormat("Failed to fetch online status codes from 3dcart: {0}", ex);

                return null;
            }
        }
    }
}

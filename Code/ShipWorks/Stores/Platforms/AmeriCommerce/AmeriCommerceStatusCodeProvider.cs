using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Provides online status codes for AmeriCommerce
    /// </summary>
    public class AmeriCommerceStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceStatusCodeProvider));

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceStatusCodeProvider(AmeriCommerceStoreEntity store)
            : base(store, AmeriCommerceStoreFields.StatusCodes)
        {

        }

        /// <summary>
        /// Retrieve new status codes from AmeriCommerce
        /// </summary>
        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            try
            {
                Dictionary<int, string> codeMap = new Dictionary<int, string>();

                AmeriCommerceWebClient client = new AmeriCommerceWebClient((AmeriCommerceStoreEntity)Store);
                foreach (OrderStatusTrans orderStatus in client.GetStatusCodes())
                {
                    codeMap.Add(orderStatus.orderStatusID.GetValue(0), orderStatus.orderStatus);
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

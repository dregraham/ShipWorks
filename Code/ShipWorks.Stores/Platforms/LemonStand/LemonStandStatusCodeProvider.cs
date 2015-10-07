using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Newtonsoft.Json.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.BigCommerce;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    public class LemonStandStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        private readonly ILog log;

        public LemonStandStatusCodeProvider(LemonStandStoreEntity store)
            : this(store, LogManager.GetLogger(typeof(LemonStandStatusCodeProvider)))
        {

        }
        public LemonStandStatusCodeProvider(StoreEntity store, ILog log)
            : base(store, LemonStandStoreFields.StatusCodes)
        {
            this.log = log;
        }

        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            try
            {
                LemonStandStoreEntity store = (LemonStandStoreEntity)Store;
                LemonStandWebClient client = new LemonStandWebClient(store);

                List<JToken> statuses = client.GetOrderStatuses().SelectToken("data").Children().ToList();

                return statuses.ToDictionary(status => int.Parse(status.SelectToken("id").ToString()), status => status.SelectToken("name").ToString());
            }
            catch (LemonStandException ex)
            {
                log.ErrorFormat("Failed to fetch online status codes from LemonStand: {0}", ex);

                return null;
            }
        }
    }
}

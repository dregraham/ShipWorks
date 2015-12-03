using System.Collections.Generic;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz.Util;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.LemonStand.DTO;

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

                LemonStandOrderStatuses stats =
                    JsonConvert.DeserializeObject<LemonStandOrderStatuses>(client.GetOrderStatuses().ToString());

                Dictionary<int, string> dictionary = new Dictionary<int, string>();

                foreach (var status in stats.OrderStatus)
                {
                    if (status.Name.IsNullOrWhiteSpace() || status.ID == 0)
                    {
                        throw new LemonStandException();
                    }
                    
                    dictionary.Add(status.ID, status.Name);
                }

                return dictionary;
            }
            catch (LemonStandException ex)
            {
                log.ErrorFormat("Failed to fetch online status codes from LemonStand: {0}", ex);

                return null;
            }
        }
    }
}

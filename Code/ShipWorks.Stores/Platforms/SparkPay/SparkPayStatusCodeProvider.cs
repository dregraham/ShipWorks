using Autofac;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        SparkPayStoreEntity store;

        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(SparkPayStatusCodeProvider));

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayStatusCodeProvider(SparkPayStoreEntity store)
            : base(store, SparkPayStoreFields.StatusCodes)
        {
            this.store = store;
        }

        /// <summary>
        /// Returns a Dictionary of statues from SparkPay
        /// </summary>
        protected override Dictionary<int, string> GetCodeMapFromOnline()
        {
            try
            {
                Dictionary<int, string> codeMap = new Dictionary<int, string>();

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    SparkPayWebClient webClient = lifetimeScope.Resolve<SparkPayWebClient>();
                    log.Debug("Getting statuses from web client");
                    IEnumerable<OrderStatus> statuses = webClient.GetStatuses(store).Statuses;
                    log.Debug($"Got statuses");
                    foreach (OrderStatus orderStatus in statuses)
                    {
                        log.Debug($"Adding status {orderStatus.Name}");
                        codeMap.Add(orderStatus.Id, orderStatus.Name);
                    }
                }
                log.Debug($"Done with statuses");
                return codeMap;
            }
            catch (SparkPayException ex)
            {
                log.ErrorFormat("Failed to fetch online status codes from SparkPay: {0}", ex);
                return null;
            }
        }
    }
}

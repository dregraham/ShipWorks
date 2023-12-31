﻿using System.Collections.Generic;
using Autofac;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    public class SparkPayStatusCodeProvider : OnlineStatusCodeProvider<int>
    {
        private readonly SparkPayStoreEntity store;

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
                    var webClient = lifetimeScope.Resolve<ISparkPayWebClient>();
                    log.Debug("Getting statuses from web client");
                    OrderStatusResponse response = webClient.GetStatuses(store);
                    log.Debug("Getting statuses from response");
                    IEnumerable<OrderStatus> statuses = response.Statuses;
                    log.Debug($"Got statuses");
                    foreach (OrderStatus orderStatus in statuses)
                    {
                        log.Debug($"Adding status {orderStatus.Name}");
                        codeMap.Add(orderStatus.Id, orderStatus.Name);
                    }
                }
                log.Debug("Done with statuses");
                return codeMap;
            }
            catch (SparkPayException ex)
            {
                log.ErrorFormat($"Failed to fetch online status codes from SparkPay: {ex}");
                return null;
            }
        }
    }
}

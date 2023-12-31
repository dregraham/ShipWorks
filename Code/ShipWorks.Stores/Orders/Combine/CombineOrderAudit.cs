﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// Class for auditing combined orders
    /// </summary>
    [Component]
    public class CombineOrderAudit : ICombineOrderAudit
    {
        private readonly IAuditUtility auditUtility;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderAudit(IAuditUtility auditUtility, ISqlAdapterFactory sqlAdapterFactory, 
            IStoreTypeManager storeTypeManager, IConfigurationData configurationData)
        {
            this.storeTypeManager = storeTypeManager;
            this.auditUtility = auditUtility;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Audit the combined order.
        /// </summary>
        public async Task Audit(long survivingOrderID, IEnumerable<IOrderEntity> orders)
        {
            if (!configurationData.FetchReadOnly().AuditEnabled)
            {
                return;
            }

            var identifiers = orders.Select(GetOrderIdentifier).Combine(", ");
            string reason = $"Combined from orders : {identifiers}";
            reason = reason.Truncate(100);

            AuditReason auditReason = new AuditReason(AuditReasonType.CombineOrder, reason);

            await auditUtility.AuditAsync(survivingOrderID, AuditActionType.CombineOrder, auditReason, sqlAdapterFactory.Create())
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get an order identifier from an order
        /// </summary>
        private string GetOrderIdentifier(IOrderEntity order) =>
            order.CombineSplitStatus.IsCombined() || order.IsManual ?
                order.OrderNumberComplete :
                storeTypeManager.GetType(order.StoreID).GetAuditDescription(order);
    }
}

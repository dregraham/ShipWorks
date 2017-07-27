using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Class for auditing combined orders
    /// </summary>
    [Component]
    public class CombineOrderAudit : ICombineOrderAudit
    {
        private readonly IAuditUtility auditUtility;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderAudit(IAuditUtility auditUtility, IStoreTypeManager storeTypeManager, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.auditUtility = auditUtility;
            this.storeTypeManager = storeTypeManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Audit the combined order.
        /// </summary>
        public async Task Audit(long survivingOrderID, IEnumerable<IOrderEntity> orders)
        {
            StoreType storeType = storeTypeManager.GetType(orders.First().StoreID);

            string reason = $"Combined from orders : {string.Join(", ", orders.Select(o => ParseOrderIdentifier(storeType, o)))}";
            reason = reason.Truncate(100);

            AuditReason auditReason = new AuditReason(AuditReasonType.CombineOrder, reason);

            await auditUtility.AuditAsync(survivingOrderID, AuditActionType.CombineOrder, auditReason, sqlAdapterFactory.Create())
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Parse the order identifier ToString() to remove the extra info that isn't necessary for the audit
        /// </summary>
        public string ParseOrderIdentifier(StoreType storeType, IOrderEntity order)
        {
            string orderIdentifierText = storeType.CreateOrderIdentifier(order as OrderEntity).ToString();
            return orderIdentifierText?.Substring(orderIdentifierText.IndexOf(':') + 1) ?? string.Empty;
        }
    }
}

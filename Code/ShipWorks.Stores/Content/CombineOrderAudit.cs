using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
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
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderAudit(IAuditUtility auditUtility, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.auditUtility = auditUtility;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Audit the combined order.
        /// </summary>
        public async Task Audit(long survivingOrderID, IEnumerable<IOrderEntity> orders)
        {
            string reason = $"Combined from orders : {string.Join(", ", orders.Select(o => o.OrderNumberComplete))}";
            reason = reason.Truncate(100);

            AuditReason auditReason = new AuditReason(AuditReasonType.CombineOrder, reason);

            await auditUtility.AuditAsync(survivingOrderID, AuditActionType.CombineOrder, auditReason, sqlAdapterFactory.Create())
                .ConfigureAwait(false);
        }
    }
}

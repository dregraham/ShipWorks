using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Interface for auditing split orders
    /// </summary>
    [Component]
    public class OrderSplitAudit : IOrderSplitAudit
    {
        private readonly IAuditUtility auditUtility;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private const string SplitToOrderReasonFormat = "Split to order : {0}";
        private const string SplitFromOrderReasonFormat = "Split from order : {0}";
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitAudit(IAuditUtility auditUtility, IStoreTypeManager storeTypeManager, 
            ISqlAdapterFactory sqlAdapterFactory, IConfigurationData configurationData)
        {
            this.storeTypeManager = storeTypeManager;
            this.auditUtility = auditUtility;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Audit the original order and split order.
        /// </summary>
        public async Task Audit(IOrderEntity originalOrder, IOrderEntity splitOrder)
        {
            if (!configurationData.FetchReadOnly().AuditEnabled)
            {
                return;
            }

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                await AuditOrder(originalOrder, splitOrder, SplitToOrderReasonFormat, sqlAdapter).ConfigureAwait(false);
                await AuditOrder(splitOrder, originalOrder, SplitFromOrderReasonFormat, sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Audit a specific order
        /// </summary>
        private async Task AuditOrder(IOrderEntity fromOrder, IOrderEntity toOrder, string auditReasonFormat, ISqlAdapter sqlAdapter)
        {
            string reason = string.Format(auditReasonFormat, GetOrderIdentifier(toOrder));

            AuditReason auditReason = new AuditReason(AuditReasonType.SplitOrder, reason.Truncate(100));

            await auditUtility.AuditAsync(fromOrder.OrderID, AuditActionType.SplitOrder, auditReason, sqlAdapter)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get an order identifier from an order
        /// </summary>
        private string GetOrderIdentifier(IOrderEntity order) =>
            order.IsManual ? 
                order.OrderNumberComplete :
                storeTypeManager.GetType(order.StoreID).GetAuditDescription(order);
    }
}

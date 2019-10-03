using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Interface for auditing split orders
    /// </summary>
    [Component]
    public class OrderSplitAuditHub : IOrderSplitAuditHub
    {
        private readonly IAuditUtility auditUtility;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private const string SplitToOrderReasonFormat = "Split order : {0}";
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitAuditHub(IAuditUtility auditUtility, IStoreTypeManager storeTypeManager, 
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
        public async Task Audit(IOrderEntity originalOrder)
        {
            if (!configurationData.FetchReadOnly().AuditEnabled)
            {
                return;
            }

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                await AuditOrder(originalOrder, SplitToOrderReasonFormat, sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Audit a specific order
        /// </summary>
        private async Task AuditOrder(IOrderEntity fromOrder, string auditReasonFormat, ISqlAdapter sqlAdapter)
        {
            string reason = string.Format(auditReasonFormat, GetOrderIdentifier(fromOrder));

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

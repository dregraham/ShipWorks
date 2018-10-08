using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Users;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Locate the last processed shipment for order lookup
    /// </summary>
    [Component]
    public class OrderLookupPreviousShipmentLocator : IOrderLookupPreviousShipmentLocator
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IUserSession userSession;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupPreviousShipmentLocator(
            ISqlAdapterFactory sqlAdapterFactory,
            IDateTimeProvider dateTimeProvider,
            IUserSession userSession,
            IShippingManager shippingManager)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.userSession = userSession;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Get details of the last processed shipment
        /// </summary>
        public async Task<PreviousProcessedShipmentDetails> GetLatestShipmentDetails()
        {
            QueryFactory factory = new QueryFactory();
            EntityQuery<ProcessedShipmentEntity> queryStarter = factory.ProcessedShipment
                .Where(ProcessedShipmentFields.ProcessedDate >= dateTimeProvider.GetUtcNow().Date)
                .AndWhere(ProcessedShipmentFields.ProcessedUserID == userSession.User.UserID)
                .AndWhere(ProcessedShipmentFields.ProcessedWithUiMode == UIMode.OrderLookup)
                .OrderBy(ProcessedShipmentFields.ProcessedDate.Descending())
                .Limit(1);

            var shipmentQuery = queryStarter
                .Select(() => new PreviousProcessedShipmentDetails(
                    ProcessedShipmentFields.ShipmentID.ToValue<long>(),
                    ProcessedShipmentFields.Voided.ToValue<bool>()));

            return await UsingAsync(
                    sqlAdapterFactory.Create(),
                    x => x.FetchFirstAsync(shipmentQuery))
                .ConfigureAwait(false);
        }
    }
}

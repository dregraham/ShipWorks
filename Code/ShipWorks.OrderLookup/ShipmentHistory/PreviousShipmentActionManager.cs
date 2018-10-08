using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Users;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Class to delegate tasks on previous shipments (reprint, void, etc)
    /// </summary>
    [Component]
    public class PreviousShipmentActionManager : IPreviousShipmentActionManager, IDisposable
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMessenger messenger;
        private readonly IUserSession userSession;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public PreviousShipmentActionManager(IMessenger messenger, ISqlAdapterFactory sqlAdapterFactory, 
            IDateTimeProvider dateTimeProvider, IUserSession userSession, IShippingManager shippingManager)
        {
            this.messenger = messenger;
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.userSession = userSession;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Reprint the last shipment
        /// </summary>
        public async Task ReprintLastShipment()
        {
            long? shipmentID = await GetLatestShipmentIDToReprint().ConfigureAwait(true);

            if (!shipmentID.HasValue)
            {
                return;
            }

            ShipmentEntity shipment = new ShipmentEntity(shipmentID.Value);

            try
            {
                shippingManager.RefreshShipment(shipment);

                if (shipment != null && shipment.Processed && !shipment.Voided)
                {
                    messenger.Send(new ReprintLabelsMessage(this, new[] { shipment }), string.Empty);
                }
            }
            catch (ObjectDeletedException)
            {
                // Just continue
            }
        }

        /// <summary>
        /// Get the last shipment ID to reprint.
        /// </summary>
        private async Task<long?> GetLatestShipmentIDToReprint()
        {
            QueryFactory factory = new QueryFactory();
            EntityQuery<ProcessedShipmentEntity> queryStarter = factory.ProcessedShipment
                .Where(ProcessedShipmentFields.ProcessedDate >= dateTimeProvider.GetUtcNow().Date)
                .AndWhere(ProcessedShipmentFields.ProcessedUserID == userSession.User.UserID)
                .AndWhere(ProcessedShipmentFields.ProcessedWithUiMode == UIMode.OrderLookup)
                .AndWhere(ProcessedShipmentFields.Voided == false)
                .OrderBy(ProcessedShipmentFields.ProcessedDate.Descending())
                .Limit(1);

            DynamicQuery<long?> shipmentQuery = queryStarter
                .Select(() => ProcessedShipmentFields.ShipmentID.ToValue<long?>());

            return await UsingAsync(
                    sqlAdapterFactory.Create(),
                    x => x.FetchFirstAsync(shipmentQuery))
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}

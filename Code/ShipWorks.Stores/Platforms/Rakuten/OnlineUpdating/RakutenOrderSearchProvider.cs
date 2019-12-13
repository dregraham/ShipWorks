using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.Rakuten.OnlineUpdating
{
    /// <summary>
    /// Get order search identifiers for uploading shipment data
    /// </summary>
    [Component]
    public class RakutenOrderSearchProvider : CombineOrderSearchBaseProvider<RakutenUploadDetails>, IRakutenOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlAdapterFactory"></param>
        public RakutenOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override async Task<IEnumerable<RakutenUploadDetails>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.RakutenOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(RakutenOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => new RakutenUploadDetails
                {
                    OrderNumber = OrderSearchFields.OrderNumberComplete.ToValue<string>(),
                    PackageID = RakutenOrderSearchFields.RakutenPackageID.ToValue<string>()
                })
                .Distinct()
                .Where(RakutenOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the Rakuten online order identifier
        /// </summary>
        protected override RakutenUploadDetails GetOnlineOrderIdentifier(IOrderEntity order)
        {
            return new RakutenUploadDetails
            {
                OrderNumber = order.OrderNumberComplete,
                PackageID = ((IRakutenOrderEntity) order).RakutenPackageID
            };
        }
    }
}

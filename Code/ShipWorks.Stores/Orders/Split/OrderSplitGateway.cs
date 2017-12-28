using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Orders.Split.Errors;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Gateway used when splitting an order
    /// </summary>
    [Component]
    public class OrderSplitGateway : IOrderSplitGateway
    {
        private readonly IOrderManager orderManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitGateway(IOrderManager orderManager, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.orderManager = orderManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Load an order that will be split
        /// </summary>
        public async Task<OrderEntity> LoadOrder(long orderID)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await orderManager
                    .LoadOrderAsync(orderID, sqlAdapter, FullOrderPrefetchPath.Value)
                    .Bind(x => x == null ?
                        Task.FromException<OrderEntity>(Error.LoadSurvivingOrderFailed) :
                        Task.FromResult(x))
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Is the order allowed to be split
        /// </summary>
        public bool CanSplit(long orderID)
        {
            return orderManager.GetLatestActiveShipment(orderID) == null;
        }

        /// <summary>
        /// Get the next order ID that should be used for a split order
        /// </summary>
        public async Task<string> GetNextOrderNumber(long orderID, string existingOrderNumber)
        {
            QueryFactory factory = new QueryFactory();
            var from = factory.Order
                                .LeftJoin(factory.OrderSearch)
                                .On(OrderFields.OrderID == OrderSearchFields.OrderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                int index = 1;
                while (index <= 100)
                {
                    string nextOrderNumber = $"{existingOrderNumber}-{index}";

                    var q = factory.Create()
                        .Select(OrderFields.OrderID, OrderSearchFields.OriginalOrderID)
                        .From(from)
                        .Where(OrderFields.OrderID == orderID | OrderSearchFields.OrderID == orderID | OrderSearchFields.OriginalOrderID == orderID)
                        .AndWhere(OrderFields.OrderNumberComplete == nextOrderNumber);

                    if (await sqlAdapter.FetchScalarAsync<int>(factory.Create().Select(q.CountRow())).ConfigureAwait(false) == 0)
                    {
                        return $"-{index}";
                    }

                    index++;
                }
            }

            return "-S";
        }

        /// <summary>
        /// Create the pre-fetch path used to load an order
        /// </summary>
        private static readonly Lazy<IEnumerable<IPrefetchPathElement2>> FullOrderPrefetchPath = new Lazy<IEnumerable<IPrefetchPathElement2>>(() =>
        {
            List<IPrefetchPathElement2> prefetchPath = new List<IPrefetchPathElement2>();

            prefetchPath.Add(OrderEntity.PrefetchPathStore);
            prefetchPath.Add(OrderEntity.PrefetchPathNotes);
            prefetchPath.Add(OrderEntity.PrefetchPathOrderCharges);
            prefetchPath.Add(OrderEntity.PrefetchPathOrderPaymentDetails);

            AddOrderSearchPrefetchPaths(prefetchPath);

            IPrefetchPathElement2 itemsPath = OrderEntity.PrefetchPathOrderItems;
            itemsPath.SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);
            prefetchPath.Add(itemsPath);

            IPrefetchPathElement2 shipmentsPath = OrderEntity.PrefetchPathShipments;

            IPrefetchPathElement2 upsShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathUps);
            upsShipmentPath.SubPath.Add(UpsShipmentEntity.PrefetchPathPackages);

            IPrefetchPathElement2 postalShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathPostal);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathUsps);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathEndicia);

            IPrefetchPathElement2 iParcelShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathIParcel);
            iParcelShipmentPath.SubPath.Add(IParcelShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOnTrac);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathAmazon);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathBestRate);

            IPrefetchPathElement2 fedexShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathFedEx);
            fedexShipmentPath.SubPath.Add(FedExShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOther);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathInsurancePolicy);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathCustomsItems);
            prefetchPath.Add(OrderEntity.PrefetchPathShipmentCollectionViaValidatedAddress);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathValidatedAddress);

            prefetchPath.Add(shipmentsPath);

            return prefetchPath;
        });

        /// <summary>
        /// Add OrderSearch prefetch paths
        /// </summary>
        private static void AddOrderSearchPrefetchPaths(List<IPrefetchPathElement2> prefetchPath)
        {
            prefetchPath.Add(OrderEntity.PrefetchPathOrderSearch);
            prefetchPath.Add(AmazonOrderEntity.PrefetchPathAmazonOrderSearch);
            prefetchPath.Add(ChannelAdvisorOrderEntity.PrefetchPathChannelAdvisorOrderSearch);
            prefetchPath.Add(ClickCartProOrderEntity.PrefetchPathClickCartProOrderSearch);
            prefetchPath.Add(CommerceInterfaceOrderEntity.PrefetchPathCommerceInterfaceOrderSearch);
            prefetchPath.Add(EbayOrderEntity.PrefetchPathEbayOrderSearch);
            prefetchPath.Add(GrouponOrderEntity.PrefetchPathGrouponOrderSearch);
            prefetchPath.Add(JetOrderEntity.PrefetchPathJetOrderSearch);
            prefetchPath.Add(LemonStandOrderEntity.PrefetchPathLemonStandOrderSearch);
            prefetchPath.Add(MagentoOrderEntity.PrefetchPathMagentoOrderSearch);
            prefetchPath.Add(MarketplaceAdvisorOrderEntity.PrefetchPathMarketplaceAdvisorOrderSearch);
            prefetchPath.Add(NetworkSolutionsOrderEntity.PrefetchPathNetworkSolutionsOrderSearch);
            prefetchPath.Add(OrderMotionOrderEntity.PrefetchPathOrderMotionOrderSearch);
            prefetchPath.Add(PayPalOrderEntity.PrefetchPathPayPalOrderSearch);
            prefetchPath.Add(ProStoresOrderEntity.PrefetchPathProStoresOrderSearch);
            prefetchPath.Add(SearsOrderEntity.PrefetchPathSearsOrderSearch);
            prefetchPath.Add(ShopifyOrderEntity.PrefetchPathShopifyOrderSearch);
            prefetchPath.Add(ThreeDCartOrderEntity.PrefetchPathThreeDCartOrderSearch);
            prefetchPath.Add(WalmartOrderEntity.PrefetchPathWalmartOrderSearch);
            prefetchPath.Add(YahooOrderEntity.PrefetchPathYahooOrderSearch);
        }
    }
}

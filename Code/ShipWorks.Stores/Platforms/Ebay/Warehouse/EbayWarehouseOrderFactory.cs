using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Stores.Platforms.Ebay.Warehouse
{
    /// <summary>
    /// eBay warehouse order factory
    /// </summary>
    [KeyedComponent(typeof(IWarehouseOrderFactory), StoreTypeCode.Ebay)]
    public class EbayWarehouseOrderFactory : WarehouseOrderFactory
    {
        private const string ebayEntryKey = "ebay";
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayWarehouseOrderFactory(IOrderElementFactory orderElementFactory,
            Func<Type, ILog> logFactory) : base(orderElementFactory)
        {
            log = logFactory(typeof(EbayWarehouseOrderFactory));
        }

        /// <summary>
        /// Create an order entity with an eBay identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            var ebayWarehouseOrder = warehouseOrder.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseOrder>();
            var identifier = GetEbayOrderIdentifier(warehouseOrder, ebayWarehouseOrder);

            // get the order instance
            GenericResult<OrderEntity> result = await orderElementFactory
                .CreateOrder(identifier).ConfigureAwait(false);

            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", warehouseOrder.OrderNumber, result.Message);
            }

            return result;
        }

        /// <summary>
        /// Get an EbayOrderIdentifier for the warehouse order
        /// </summary>
        public static EbayOrderIdentifier GetEbayOrderIdentifier(WarehouseOrder warehouseOrder, EbayWarehouseOrder ebayWarehouseOrder)
        {
            string orderID = ebayWarehouseOrder.EbayOrderID ?? warehouseOrder.OrderNumber;
            var items = warehouseOrder.Items.Select(i => i.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseItem>());
            var item = items?.FirstOrDefault();
            var transactionId = item?.EbayTransactionID.ToString() ?? "";
            var itemId = item?.EbayItemID.ToString() ?? "";

           return EbayOrderIdentifier.GetIdentifier(orderID, transactionId, itemId);
        }

        /// <summary>
        /// Load eBay order details
        /// </summary>
        protected override void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            EbayOrderEntity ebayOrderEntity = (EbayOrderEntity) orderEntity;
            var ebayWarehouseOrder = warehouseOrder.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseOrder>();

            // Update the order number to match the warehouse order number
            ebayOrderEntity.ChangeOrderNumber(warehouseOrder.OrderNumber);

            ebayOrderEntity.EbayBuyerID = ebayWarehouseOrder.EbayBuyerID;
            ebayOrderEntity.SelectedShippingMethod = ebayWarehouseOrder.SelectedShippingMethod;
            ebayOrderEntity.SellingManagerRecord = ebayWarehouseOrder.SellingManagerRecord;
            ebayOrderEntity.GspEligible = ebayWarehouseOrder.GspEligible;
            ebayOrderEntity.GspFirstName = ebayWarehouseOrder.GspFirstName;
            ebayOrderEntity.GspLastName = ebayWarehouseOrder.GspLastName;
            ebayOrderEntity.GspStreet1 = ebayWarehouseOrder.GspStreet1;
            ebayOrderEntity.GspStreet2 = ebayWarehouseOrder.GspStreet2;
            ebayOrderEntity.GspCity = ebayWarehouseOrder.GspCity;
            ebayOrderEntity.GspStateProvince = ebayWarehouseOrder.GspStateProvince;
            ebayOrderEntity.GspPostalCode = ebayWarehouseOrder.GspPostalCode;
            ebayOrderEntity.GspCountryCode = ebayWarehouseOrder.GspCountryCode;
            ebayOrderEntity.GspReferenceID = ebayWarehouseOrder.GspReferenceID;
            ebayOrderEntity.GuaranteedDelivery = ebayWarehouseOrder.GuaranteedDelivery;

            if (string.IsNullOrWhiteSpace(ebayOrderEntity.ExtendedOrderID))
            {
                ebayOrderEntity.ExtendedOrderID = ebayWarehouseOrder.ExtendedOrderID;
            }

            // If all items are shipped set the local status to shipped
            if (ebayWarehouseOrder.MyEbayShipped || warehouseOrder.Items.All(item =>
            {
                var ebayItem = item.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseItem>();
                return ebayItem.MyEbayShipped;
            }))
            {
                ebayOrderEntity.LocalStatus = "Shipped";
            }
        }

        /// <summary>
        /// Load eBay item details
        /// </summary>
        protected override void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            EbayOrderItemEntity ebayItemEntity = (EbayOrderItemEntity) itemEntity;
            var ebayWarehouseItem = warehouseItem.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseItem>();

            ebayItemEntity.EbayItemID = ebayWarehouseItem.EbayItemID;
            ebayItemEntity.EbayTransactionID = ebayWarehouseItem.EbayTransactionID;
            ebayItemEntity.SellingManagerRecord = ebayWarehouseItem.SellingManagerRecord;
            ebayItemEntity.PaymentStatus = ebayWarehouseItem.PaymentStatus;
            ebayItemEntity.PaymentMethod = ebayWarehouseItem.PaymentMethod;
            ebayItemEntity.CompleteStatus = ebayWarehouseItem.CompleteStatus;
            ebayItemEntity.MyEbayPaid = ebayWarehouseItem.MyEbayPaid;
            ebayItemEntity.MyEbayShipped = ebayWarehouseItem.MyEbayShipped;

            if (string.IsNullOrWhiteSpace(ebayItemEntity.ExtendedOrderID))
            {
                ebayItemEntity.ExtendedOrderID = ebayWarehouseItem.ExtendedOrderID;
            }
        }

        /// <summary>
        /// Load any additional store-specific details
        /// </summary>
        protected override void LoadAdditionalDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            var ebayWarehouseOrder = warehouseOrder.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseOrder>();

            if (ebayWarehouseOrder.MyEbayShipped)
            {
                // Make sure items are fetched
                OrderUtility.PopulateOrderDetails(orderEntity);

                foreach (OrderItemEntity orderItem in orderEntity.OrderItems)
                {
                    var eBayOrderItem = (EbayOrderItemEntity) orderItem;
                    eBayOrderItem.MyEbayShipped = true;
                }
            }
        }
    }
}

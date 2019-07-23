using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Ebay.Warehouse
{
    /// <summary>
    /// Ebay warehouse order factory
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
        /// Create an order entity with an Ebay identifier
        /// </summary>
        protected override async Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            var ebayWarehouseOrder = warehouseOrder.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseOrder>();

            string orderID = ebayWarehouseOrder.EbayOrderID == null ? warehouseOrder.OrderNumber : ebayWarehouseOrder.EbayOrderID;

            var identifier = new EbayOrderIdentifier(orderID);

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
        /// Load Ebay order details
        /// </summary>
        protected override void LoadStoreOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
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

            // If all items are shipped set the local status to shipped
            if (warehouseOrder.Items.All(item =>
            {
                var ebayItem = item.AdditionalData[ebayEntryKey].ToObject<EbayWarehouseItem>();
                return ebayItem.MyEbayShipped;
            }))
            {
                ebayOrderEntity.LocalStatus = "Shipped";
            }
        }

        /// <summary>
        /// Load Ebay item details
        /// </summary>
        protected override void LoadStoreItemDetails(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
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
        }
    }
}

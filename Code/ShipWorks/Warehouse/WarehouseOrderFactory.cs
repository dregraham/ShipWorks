using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    /// <summary>
    /// Base order loader for loading ShipWorks Warehouse orders
    /// </summary>
    public abstract class WarehouseOrderFactory : IWarehouseOrderFactory
    {
        protected readonly IOrderElementFactory orderElementFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        protected WarehouseOrderFactory(IOrderElementFactory orderElementFactory)
        {
            this.orderElementFactory = orderElementFactory;
        }
        
        /// <summary>
        /// Load the order details from the warehouse order into the order entity
        /// </summary>
        public async Task<OrderEntity> CreateOrder(WarehouseOrder warehouseOrder)
        {
            GenericResult<OrderEntity> result = await CreateStoreOrderEntity(warehouseOrder).ConfigureAwait(false);
            if (result.Failure)
            {
                return null;
            }

            OrderEntity orderEntity = result.Value;
            
            // todo: orderid, storeid, warehousecustomerid
            // todo: figure out what should and shouldn't be downloaded when new
            orderEntity.ChangeOrderNumber(warehouseOrder.OrderNumber);
            orderEntity.OrderDate = warehouseOrder.OrderDate;
            orderEntity.OrderTotal = warehouseOrder.OrderTotal;
            orderEntity.OnlineLastModified = warehouseOrder.OnlineLastModified;
            orderEntity.OnlineCustomerID = warehouseOrder.OnlineCustomerID;
            orderEntity.OnlineStatus = warehouseOrder.OnlineStatus;
            orderEntity.OnlineStatusCode = warehouseOrder.OnlineStatusCode;
            orderEntity.RequestedShipping = warehouseOrder.RequestedShipping;
            orderEntity.ChannelOrderID = warehouseOrder.ChannelOrderID;
            orderEntity.ShipByDate = warehouseOrder.ShipByDate;
            
            LoadBillingAddress(orderEntity, warehouseOrder);

            LoadShippingAddress(orderEntity, warehouseOrder);

            LoadItems(orderEntity, warehouseOrder.Items);

            LoadCharges(orderEntity, warehouseOrder);

            LoadPaymentDetails(orderEntity, warehouseOrder);

            await LoadNotes(orderEntity, warehouseOrder).ConfigureAwait(false);

            LoadStoreOrderDetails(orderEntity, warehouseOrder);

            return orderEntity;
        }

        /// <summary>
        /// Create an order entity with the store specific identifier
        /// </summary>
        protected abstract Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(WarehouseOrder warehouseOrder);

        /// <summary>
        /// Load store specific order details
        /// </summary>
        protected abstract void LoadStoreOrderDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder);
        
        /// <summary>
        /// Load store specific item details
        /// </summary>
        protected abstract void LoadStoreItemDetails(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem);

        /// <summary>
        /// Load the billing address from the warehouse order into the order entity
        /// </summary>
        private static void LoadBillingAddress(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            // todo: parse names if needed
            orderEntity.BillUnparsedName = warehouseOrder.BillUnparsedName;
            orderEntity.BillFirstName = warehouseOrder.BillFirstName;
            orderEntity.BillMiddleName = warehouseOrder.BillMiddleName;
            orderEntity.BillLastName = warehouseOrder.BillLastName;
            orderEntity.BillCompany = warehouseOrder.BillCompany;
            orderEntity.BillStreet1 = warehouseOrder.BillStreet1;
            orderEntity.BillStreet2 = warehouseOrder.BillStreet2;
            orderEntity.BillStreet3 = warehouseOrder.BillStreet3;
            orderEntity.BillCity = warehouseOrder.BillCity;
            orderEntity.BillStateProvCode = Geography.GetStateProvCode(warehouseOrder.BillStateProvCode);
            orderEntity.BillPostalCode = warehouseOrder.BillPostalCode;
            orderEntity.BillCountryCode = Geography.GetCountryCode(warehouseOrder.BillCountryCode);
            orderEntity.BillPhone = warehouseOrder.BillPhone;
            orderEntity.BillFax = warehouseOrder.BillFax;
            orderEntity.BillEmail = warehouseOrder.BillEmail;
            orderEntity.BillWebsite = warehouseOrder.BillWebsite;
        }

        /// <summary>
        /// Load the shipping address from the warehouse order into the order entity
        /// </summary>
        private static void LoadShippingAddress(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            orderEntity.ShipUnparsedName = warehouseOrder.ShipUnparsedName;
            orderEntity.ShipFirstName = warehouseOrder.ShipFirstName;
            orderEntity.ShipMiddleName = warehouseOrder.ShipMiddleName;
            orderEntity.ShipLastName = warehouseOrder.ShipLastName;
            orderEntity.ShipCompany = warehouseOrder.ShipCompany;
            orderEntity.ShipStreet1 = warehouseOrder.ShipStreet1;
            orderEntity.ShipStreet2 = warehouseOrder.ShipStreet2;
            orderEntity.ShipStreet3 = warehouseOrder.ShipStreet3;
            orderEntity.ShipCity = warehouseOrder.ShipCity;
            orderEntity.ShipStateProvCode = Geography.GetStateProvCode(warehouseOrder.ShipStateProvCode);
            orderEntity.ShipPostalCode = warehouseOrder.ShipPostalCode;
            orderEntity.ShipCountryCode = Geography.GetCountryCode(warehouseOrder.ShipCountryCode);
            orderEntity.ShipPhone = warehouseOrder.ShipPhone;
            orderEntity.ShipFax = warehouseOrder.ShipFax;
            orderEntity.ShipEmail = warehouseOrder.ShipEmail;
            orderEntity.ShipWebsite = warehouseOrder.ShipWebsite;
        }
        
        /// <summary>
        /// Load items from the warehouse order into the order entity
        /// </summary>
        private void LoadItems(OrderEntity orderEntity, IEnumerable<WarehouseOrderItem> warehouseOrderItems)
        {
            foreach (WarehouseOrderItem warehouseOrderItem in warehouseOrderItems)
            {
                OrderItemEntity itemEntity = orderElementFactory.CreateItem(orderEntity);
                LoadItem(itemEntity, warehouseOrderItem);
            }
        }
        
        /// <summary>
        /// Load the item details from the warehouse item into the item entity
        /// </summary>
        private void LoadItem(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            itemEntity.Name = warehouseItem.Name;
            itemEntity.Code = warehouseItem.Code;
            itemEntity.SKU = warehouseItem.SKU;
            itemEntity.ISBN = warehouseItem.ISBN;
            itemEntity.UPC = warehouseItem.UPC;
            itemEntity.HarmonizedCode = warehouseItem.HarmonizedCode;
            itemEntity.Brand = warehouseItem.Brand;
            itemEntity.MPN = warehouseItem.MPN;
            itemEntity.Description = warehouseItem.Description;
            itemEntity.Location = warehouseItem.Location;
            itemEntity.Image = warehouseItem.Image;
            itemEntity.Thumbnail = warehouseItem.Thumbnail;
            itemEntity.UnitPrice = warehouseItem.UnitPrice;
            itemEntity.UnitCost = warehouseItem.UnitCost;
            itemEntity.Quantity = warehouseItem.Quantity;
            itemEntity.Weight = warehouseItem.Weight;
            itemEntity.Length = warehouseItem.Length;
            itemEntity.Width = warehouseItem.Width;
            itemEntity.Height = warehouseItem.Height;

            LoadItemAttributes(itemEntity, warehouseItem);
            
            LoadStoreItemDetails(itemEntity, warehouseItem);
        }
        
        /// <summary>
        /// Load item attributes from the warehouse item into the item entity
        /// </summary>
        private void LoadItemAttributes(OrderItemEntity itemEntity, WarehouseOrderItem warehouseOrderItem)
        {
            foreach (WarehouseOrderItemAttribute warehouseItemAttribute in warehouseOrderItem.ItemAttributes)
            {
                orderElementFactory.CreateItemAttribute(itemEntity, 
                                                        warehouseItemAttribute.Name,
                                                        warehouseItemAttribute.Description,
                                                        warehouseItemAttribute.UnitPrice,
                                                        false);
            }
        }

        /// <summary>
        /// Load charges from the warehouse order into the order entity
        /// </summary>
        private void LoadCharges(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderCharge warehouseOrderCharge in warehouseOrder.Charges)
            {
                orderElementFactory.CreateCharge(orderEntity,
                                                 warehouseOrderCharge.Type,
                                                 warehouseOrderCharge.Description,
                                                 warehouseOrderCharge.Amount);
            }
        }
        
        /// <summary>
        /// Load payment details from the warehouse order into the order entity
        /// </summary>
        private void LoadPaymentDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderPaymentDetail warehouseOrderCharge in warehouseOrder.PaymentDetails)
            {
                orderElementFactory.CreatePaymentDetail(orderEntity, warehouseOrderCharge.Label,
                                                        warehouseOrderCharge.Value);
            }
        }

        /// <summary>
        /// Load notes from the warehouse order into the order entity
        /// </summary>
        private async Task LoadNotes(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderNote warehouseOrderCharge in warehouseOrder.Notes)
            {
                await orderElementFactory.CreateNote(orderEntity, warehouseOrderCharge.Text, warehouseOrderCharge.Edited,
                                               (NoteVisibility) warehouseOrderCharge.Visibility).ConfigureAwait(false);
            }
        }
    }
}

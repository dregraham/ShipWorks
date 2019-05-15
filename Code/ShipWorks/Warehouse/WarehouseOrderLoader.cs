using System.Collections.Generic;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Warehouse
{
    public class WarehouseOrderLoader : IWarehouseOrderLoader
    {
        private readonly IOrderElementFactory orderElementFactory;

        public WarehouseOrderLoader(IOrderElementFactory orderElementFactory)
        {
            this.orderElementFactory = orderElementFactory;
        }
        
        public virtual void LoadOrder(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            // todo: orderid, storeid, warehousecustomerid
            orderEntity.OrderNumberComplete = warehouseOrder.OrderNumber;
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

            LoadNotes(orderEntity, warehouseOrder);
        }

        protected virtual void LoadItem(OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
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
        }

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

            // todo: parse state and country code
            orderEntity.BillStateProvCode = warehouseOrder.BillStateProvCode;
            orderEntity.BillPostalCode = warehouseOrder.BillPostalCode;
            orderEntity.BillCountryCode = warehouseOrder.BillCountryCode;
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
            orderEntity.ShipStateProvCode = warehouseOrder.ShipStateProvCode;
            orderEntity.ShipPostalCode = warehouseOrder.ShipPostalCode;
            orderEntity.ShipCountryCode = warehouseOrder.ShipCountryCode;
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
        private void LoadNotes(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderNote warehouseOrderCharge in warehouseOrder.Notes)
            {
                orderElementFactory.CreateNote(orderEntity, warehouseOrderCharge.Text, warehouseOrderCharge.Edited,
                                               (NoteVisibility) warehouseOrderCharge.Visibility);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
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
            orderEntity.OrderTotal = Math.Round(warehouseOrder.OrderTotal, 2);
            orderEntity.OnlineLastModified = warehouseOrder.OnlineLastModified;
            orderEntity.OnlineCustomerID = warehouseOrder.OnlineCustomerID;
            orderEntity.OnlineStatus = warehouseOrder.OnlineStatus;
            orderEntity.OnlineStatusCode = warehouseOrder.OnlineStatusCode;
            orderEntity.RequestedShipping = warehouseOrder.RequestedShipping;
            orderEntity.ChannelOrderID = warehouseOrder.ChannelOrderID;
            orderEntity.ShipByDate = warehouseOrder.ShipByDate;
            orderEntity.HubOrderID = Guid.Parse(warehouseOrder.HubOrderId);
            orderEntity.HubSequence = warehouseOrder.HubSequence;

            LoadAddress(orderEntity.BillPerson, warehouseOrder.BillAddress);
            LoadAddress(orderEntity.ShipPerson, warehouseOrder.ShipAddress);

            LoadItems(orderEntity, warehouseOrder.Items);

            LoadCharges(orderEntity, warehouseOrder);

            LoadPaymentDetails(orderEntity, warehouseOrder);

            await LoadNotes(orderEntity, warehouseOrder).ConfigureAwait(false);

            LoadStoreOrderDetails(orderEntity, warehouseOrder);

            return orderEntity;
        }

        private void LoadAddress(PersonAdapter localAddress, WarehouseOrderAddress warehouseOrderAddress)
        {
            // todo: parse names if needed
            localAddress.UnparsedName = warehouseOrderAddress.UnparsedName;
            localAddress.FirstName = warehouseOrderAddress.FirstName;
            localAddress.MiddleName = warehouseOrderAddress.MiddleName;
            localAddress.LastName = warehouseOrderAddress.LastName;
            localAddress.Company = warehouseOrderAddress.Company;
            localAddress.Street1 = warehouseOrderAddress.Street1;
            localAddress.Street2 = warehouseOrderAddress.Street2;
            localAddress.Street3 = warehouseOrderAddress.Street3;
            localAddress.City = warehouseOrderAddress.City;
            localAddress.StateProvCode = Geography.GetStateProvCode(warehouseOrderAddress.StateProvCode);
            localAddress.PostalCode = warehouseOrderAddress.PostalCode;
            localAddress.CountryCode = Geography.GetCountryCode(warehouseOrderAddress.CountryCode);
            localAddress.Phone = warehouseOrderAddress.Phone;
            localAddress.Fax = warehouseOrderAddress.Fax;
            localAddress.Email = warehouseOrderAddress.Email;
            localAddress.Website = warehouseOrderAddress.Website;
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
            itemEntity.UnitPrice = Math.Round(warehouseItem.UnitPrice, 2);
            itemEntity.UnitCost = Math.Round(warehouseItem.UnitCost, 2);
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

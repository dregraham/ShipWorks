using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders.DTO;

namespace ShipWorks.Warehouse.Orders
{
    /// <summary>
    /// Base order loader for loading ShipWorks Warehouse orders
    /// </summary>
    public abstract class WarehouseOrderFactory : IWarehouseOrderFactory
    {
        protected readonly IOrderElementFactory orderElementFactory;
        protected IStoreEntity storeEntity;

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
        public async Task<OrderEntity> CreateOrder(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            this.storeEntity = store;
            try
            {
                GenericResult<OrderEntity> result = await CreateStoreOrderEntity(store, storeType, warehouseOrder).ConfigureAwait(false);
                if (result.Failure)
                {
                    return null;
                }

                OrderEntity orderEntity = result.Value;
                LoadOrderData(warehouseOrder, orderEntity);

                LoadCustoms(warehouseOrder, orderEntity);

                LoadAddress(orderEntity.BillPerson, warehouseOrder.BillAddress);
                LoadAddress(orderEntity.ShipPerson, warehouseOrder.ShipAddress);

                if (orderEntity.IsNew)
                {
                    LoadItems(store, orderEntity, warehouseOrder.Items);
                    LoadCharges(orderEntity, warehouseOrder);
                    LoadPaymentDetails(orderEntity, warehouseOrder);
                }

                await LoadNotes(orderEntity, warehouseOrder).ConfigureAwait(false);

                LoadStoreOrderDetails(store, orderEntity, warehouseOrder);
                LoadAdditionalDetails(store, orderEntity, warehouseOrder);

                Debug.Assert(orderEntity.OrderNumber != 0,
                             "Ensure order number was set by the store specific order factory");

                return orderEntity;
            }
            catch (KeyNotFoundException ex)
            {
                throw new DownloadException("Could not load store specific data for order", ex);
            }
            catch (Exception ex)
            {
                throw new DownloadException("Could not load store specific data for order", ex.InnerException);
            }
        }

        /// <summary>
        /// Load any additional store-specific details
        /// </summary>
        protected virtual void LoadAdditionalDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {

        }

        /// <summary>
        /// Load order data
        /// </summary>
        protected virtual void LoadOrderData(WarehouseOrder warehouseOrder, OrderEntity orderEntity)
        {
            orderEntity.OrderDate = warehouseOrder.OrderDate;
            orderEntity.OrderTotal = Math.Round(warehouseOrder.OrderTotal, 2);
            orderEntity.OnlineLastModified = warehouseOrder.OnlineLastModified;
            orderEntity.OnlineCustomerID = warehouseOrder.OnlineCustomerId;
            orderEntity.OnlineStatus = warehouseOrder.OnlineStatus;
            orderEntity.OnlineStatusCode = warehouseOrder.OnlineStatusCode;
            orderEntity.RequestedShipping = warehouseOrder.RequestedShipping;
            orderEntity.ShipByDate = warehouseOrder.ShipByDate;
            orderEntity.DeliverByDate = warehouseOrder.DeliverByDate;
            orderEntity.HubOrderID = Guid.Parse(warehouseOrder.HubOrderId);
            orderEntity.HubSequence = warehouseOrder.HubSequence;
            SetChannelOrderId(orderEntity, warehouseOrder);
        }


        protected virtual void SetChannelOrderId(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            orderEntity.ChannelOrderID = warehouseOrder.ChannelOrderId;
        }

        /// <summary>
        /// Load customs info
        /// </summary>
        private static void LoadCustoms(WarehouseOrder warehouseOrder, OrderEntity orderEntity)
        {
            orderEntity.Custom1 = warehouseOrder.Custom1;
            orderEntity.Custom2 = warehouseOrder.Custom2;
            orderEntity.Custom3 = warehouseOrder.Custom3;
            orderEntity.Custom4 = warehouseOrder.Custom4;
            orderEntity.Custom5 = warehouseOrder.Custom5;
            orderEntity.Custom6 = warehouseOrder.Custom6;
            orderEntity.Custom7 = warehouseOrder.Custom7;
            orderEntity.Custom8 = warehouseOrder.Custom8;
            orderEntity.Custom9 = warehouseOrder.Custom9;
            orderEntity.Custom10 = warehouseOrder.Custom10;
        }

        /// <summary>
        /// Load Address
        /// </summary>
        private void LoadAddress(PersonAdapter localAddress, WarehouseOrderAddress warehouseOrderAddress)
        {
            PersonName parsedName = warehouseOrderAddress.UnparsedName.IsNullOrWhiteSpace() ?
                new PersonName(warehouseOrderAddress.FirstName, warehouseOrderAddress.MiddleName, warehouseOrderAddress.LastName) :
                PersonName.Parse(warehouseOrderAddress.UnparsedName);

            localAddress.UnparsedName = parsedName.UnparsedName;
            localAddress.FirstName = parsedName.First;
            localAddress.MiddleName = parsedName.Middle;
            localAddress.LastName = parsedName.Last;

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
        protected virtual Task<GenericResult<OrderEntity>> CreateStoreOrderEntity(IStoreEntity store, StoreType storeType, WarehouseOrder warehouseOrder)
        {
            return Task.FromResult(GenericResult.FromSuccess(new OrderEntity()));
        }

        /// <summary>
        /// Load store specific order details
        /// </summary>
        protected virtual void LoadStoreOrderDetails(IStoreEntity store, OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {

        }

        /// <summary>
        /// Load store specific item details
        /// </summary>
        protected virtual void LoadStoreItemDetails(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {

        }

        /// <summary>
        /// Load items from the warehouse order into the order entity
        /// </summary>
        private void LoadItems(IStoreEntity store, OrderEntity orderEntity, IEnumerable<WarehouseOrderItem> warehouseOrderItems)
        {
            foreach (WarehouseOrderItem warehouseOrderItem in warehouseOrderItems)
            {
                OrderItemEntity itemEntity = orderElementFactory.CreateItem(orderEntity);
                LoadItem(store, itemEntity, warehouseOrderItem);
            }
        }

        /// <summary>
        /// Load the item details from the warehouse item into the item entity
        /// </summary>
        private void LoadItem(IStoreEntity store, OrderItemEntity itemEntity, WarehouseOrderItem warehouseItem)
        {
            itemEntity.HubItemID = warehouseItem.ID;
            itemEntity.StoreOrderItemID = warehouseItem.StoreOrderItemID;
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

            LoadCustomItems(itemEntity, warehouseItem);
            LoadItemAttributes(itemEntity, warehouseItem);
            LoadStoreItemDetails(store, itemEntity, warehouseItem);
        }

        /// <summary>
        /// Load custom items info
        /// </summary>
        private void LoadCustomItems(OrderItemEntity item, WarehouseOrderItem warehouseItem)
        {
            item.Custom1 = warehouseItem.Custom1;
            item.Custom2 = warehouseItem.Custom2;
            item.Custom3 = warehouseItem.Custom3;
            item.Custom4 = warehouseItem.Custom4;
            item.Custom5 = warehouseItem.Custom5;
            item.Custom6 = warehouseItem.Custom6;
            item.Custom7 = warehouseItem.Custom7;
            item.Custom8 = warehouseItem.Custom8;
            item.Custom9 = warehouseItem.Custom9;
            item.Custom10 = warehouseItem.Custom10;
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
                var charge = orderElementFactory.CreateCharge(orderEntity,
                                                 warehouseOrderCharge.Type,
                                                 warehouseOrderCharge.Description,
                                                 warehouseOrderCharge.Amount);

                charge.HubChargeID = warehouseOrderCharge.ID;
            }
        }

        /// <summary>
        /// Load payment details from the warehouse order into the order entity
        /// </summary>
        protected void LoadPaymentDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
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
            foreach (WarehouseOrderNote warehouseOrderNote in warehouseOrder.Notes)
            {
                await orderElementFactory.CreateNote(orderEntity, warehouseOrderNote.Text, warehouseOrderNote.Edited,
                                               (NoteVisibility) warehouseOrderNote.Visibility, true).ConfigureAwait(false);
            }
        }
    }
}

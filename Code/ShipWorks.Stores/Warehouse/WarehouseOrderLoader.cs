using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Warehouse
{
    public class WarehouseOrderLoader : IWarehouseOrderLoader
    {
        private readonly Func<StoreTypeCode, IWarehouseOrderItemLoader> itemLoaderFactory;

        public WarehouseOrderLoader(Func<StoreTypeCode, IWarehouseOrderItemLoader> itemLoaderFactory)
        {
            this.itemLoaderFactory = itemLoaderFactory;
        }
        
        public void LoadOrder(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            // todo: orderid, storeid, warehousecustomerid
            orderEntity.OrderNumberComplete = warehouseOrder.OrderNumber;
            orderEntity.OrderDate = warehouseOrder.OrderDate;
            
            
            // todo: do the order total check, OrderUtility maybe?
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

            LoadItems(orderEntity, warehouseOrder);

            LoadCharges(orderEntity, warehouseOrder);

            LoadPaymentDetails(orderEntity, warehouseOrder);

            LoadNotes(orderEntity, warehouseOrder);
        }
        
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

        private void LoadItems(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            IWarehouseOrderItemLoader itemLoader = itemLoaderFactory(orderEntity.Store.StoreTypeCode);
            
            foreach (WarehouseOrderItem warehouseOrderItem in warehouseOrder.Items)
            {
                // todo: replace with instantiate order item
                OrderItemEntity itemEntity = new OrderItemEntity(orderEntity.OrderID);
                itemLoader.LoadItem(itemEntity, warehouseOrderItem);
            }
        }

        private void LoadCharges(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderCharge warehouseOrderCharge in warehouseOrder.Charges)
            {
                // todo: replace with instantiate order charge
                OrderChargeEntity charge = new OrderChargeEntity(orderEntity.OrderID);
                charge.Type = warehouseOrderCharge.Type;
                charge.Description = warehouseOrderCharge.Description;
                charge.Amount = warehouseOrderCharge.Amount;                
            }
        }
    
        private void LoadPaymentDetails(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderPaymentDetail warehouseOrderCharge in warehouseOrder.PaymentDetails)
            {
                // todo: replace with instantiate payment detail
                OrderPaymentDetailEntity paymentDetailEntity = new OrderPaymentDetailEntity(orderEntity.OrderID);
                paymentDetailEntity.Label = warehouseOrderCharge.Label;
                paymentDetailEntity.Value = warehouseOrderCharge.Value;
            }
        }

        private void LoadNotes(OrderEntity orderEntity, WarehouseOrder warehouseOrder)
        {
            foreach (WarehouseOrderNote warehouseOrderCharge in warehouseOrder.Notes)
            {
                // todo: replace with instantiate note
                NoteEntity note = new NoteEntity(orderEntity.OrderID);
                note.Edited = warehouseOrderCharge.Edited;
                note.Text = warehouseOrderCharge.Text;
                note.Visibility = warehouseOrderCharge.Visibility;
            }
        }
    }
}
using System;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Warehouse.DTO.Orders;

namespace ShipWorks.Stores.Platforms.Odbc.Warehouse
{
    /// <summary>
    /// OdbcWarehouseOrderDtoFactory
    /// </summary>
    public class OdbcWarehouseOrderDtoFactory
    {
        readonly Lazy<string> warehouseID;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcWarehouseOrderDtoFactory(IConfigurationData configurationData)
        {
            warehouseID = new Lazy<string>(() => configurationData.FetchReadOnly().WarehouseID);
        }
        
        /// <summary>
        /// Create a WarehouseOrder from a OrderEntity
        /// </summary>
        public WarehouseOrder Create(OrderEntity order)
        {
            if (order.Store == null)
            {
                throw new ShipWorksOdbcException("order.Store is null.");
            }

            var warehouseStoreID = order.Store.WarehouseStoreID;
            if (warehouseStoreID == null)
            {
                throw new ShipWorksOdbcException("Cannot create a warehouse order for a store that doesn't have a warehouse store id.");
            }
            
            return new WarehouseOrder
            {
                StoreID = warehouseStoreID.Value.ToString(),
                StoreType = (int) StoreTypeCode.Odbc,
                Warehouse = warehouseID.Value,
                OrderNumber = order.OrderNumberComplete,
                OrderDate = order.OrderDate,
                OrderTotal = order.OrderTotal,
                OnlineLastModified = order.OnlineLastModified,
                OnlineCustomerID = order.OnlineCustomerID.ToString(),
                OnlineStatus = order.OnlineStatus,
                OnlineStatusCode = order.OnlineStatusCode.ToString(),
                LocalStatus = order.LocalStatus,
                RequestedShipping = order.RequestedShipping,
                BillAddress = CreateAddress(new PersonAdapter(order, "Bill")),
                ShipAddress = CreateAddress(new PersonAdapter(order, "Ship")),
                ShipByDate = order.ShipByDate,
                Custom1 = order.Custom1,
                Custom2 = order.Custom2,
                Custom3 = order.Custom3,
                Custom4 = order.Custom4,
                Custom5 = order.Custom5,
                Charges = order.OrderCharges.Select(CreateCharge).ToList(),
                PaymentDetails = order.OrderPaymentDetails.Select(CreatePaymentDetail).ToList(),
                Notes = order.Notes.Select(CreateNote).ToList(),
                Items = order.OrderItems.Select(CreateOrderItem).ToList()
            };
        }
        
        /// <summary>
        /// Create a WarehouseOrderAddress from a PersonAdapter
        /// </summary>
        private static WarehouseOrderAddress CreateAddress(PersonAdapter order)
        {
            return new WarehouseOrderAddress
            {
                UnparsedName = order.UnparsedName,
                FirstName = order.FirstName,
                MiddleName = order.MiddleName,
                LastName = order.LastName,
                Company = order.Company,
                Street1 = order.Street1,
                Street2 = order.Street2,
                Street3 = order.Street3,
                City = order.City,
                StateProvCode = order.StateProvCode,
                PostalCode = order.PostalCode,
                CountryCode = order.CountryCode,
                Phone = order.Phone,
                Fax = order.Fax,
                Email = order.Email,
                Website = order.Website
            };
        }
        
        /// <summary>
        /// Create a WarehouseOrderCharge from a OrderChargeEntity
        /// </summary>
        private WarehouseOrderCharge CreateCharge(OrderChargeEntity charge)
        {
            return new WarehouseOrderCharge
            {
                Type = charge.Type,
                Description = charge.Description,
                Amount = charge.Amount
            };
        }

        /// <summary>
        /// Create a WarehouseOrderPaymentDetail from a OrderPaymentDetailEntity
        /// </summary>
        private WarehouseOrderPaymentDetail CreatePaymentDetail(OrderPaymentDetailEntity paymentDetail)
        {
            return new WarehouseOrderPaymentDetail
            {
                Label = paymentDetail.Label,
                Value = paymentDetail.Value
            };
        }        

        /// <summary>
        /// Create a WarehouseOrderNote from a NoteEntity
        /// </summary>
        private WarehouseOrderNote CreateNote(NoteEntity note)
        {
            return new WarehouseOrderNote
            {
                Edited = note.Edited,
                Text = note.Text,
                Visibility = note.Visibility
            };
        }

        /// <summary>
        /// Creates a WarehouseOrderItem from a WarehouseOrderItem
        /// </summary>
        private WarehouseOrderItem CreateOrderItem(OrderItemEntity item)
        {
            return new WarehouseOrderItem
            {
                Name = item.Name,
                Code = item.Code,
                SKU = item.SKU,
                ISBN = item.ISBN,
                UPC = item.UPC,
                HarmonizedCode = item.HarmonizedCode,
                Brand = item.Brand,
                MPN = item.MPN,
                Description = item.Description,
                Location = item.Location,
                Image = item.Image,
                Thumbnail = item.Thumbnail,
                UnitPrice = item.UnitPrice,
                UnitCost = item.UnitCost,
                Quantity = item.Quantity,
                Weight = item.Weight,
                Length = item.Length,
                Width = item.Width,
                Height = item.Height,
                Custom1 = item.Custom1,
                Custom2 = item.Custom2,
                Custom3 = item.Custom3,
                Custom4 = item.Custom4,
                Custom5 = item.Custom5,
                ItemAttributes = item.OrderItemAttributes.Select(CreateOrderItemAttributes).ToList()
            };
        }

        /// <summary>
        /// Creates a WarehouseOrderItemAttribute from an OrderItemAttributeEntity
        /// </summary>
        private WarehouseOrderItemAttribute CreateOrderItemAttributes(OrderItemAttributeEntity attribute)
        {
            return new WarehouseOrderItemAttribute
            {
                Name = attribute.Name,
                Description = attribute.Description,
                UnitPrice = attribute.UnitPrice
            };
        }
    }
}

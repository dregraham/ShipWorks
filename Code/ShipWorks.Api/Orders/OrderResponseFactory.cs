using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Factory for generating order responses
    /// </summary>
    public class OrderResponseFactory : IOrderResponseFactory
    {
        /// <summary>
        /// Create an order response using the given order
        /// </summary>
        public OrderResponse Create(IOrderEntity order) =>
            new OrderResponse()
            {
                OrderId = order.OrderID,
                OrderNumber = order.OrderNumberComplete,
                OrderDate = order.OrderDate,
                LastModifiedDate = order.OnlineLastModified,
                OrderTotal = order.OrderTotal,
                StoreStatus = order.OnlineStatus,
                ShipAddress = new Address()
                {
                    RecipientName = order.ShipUnparsedName,
                    Street1 = order.ShipStreet1,
                    Street2 = order.ShipStreet2,
                    Street3 = order.ShipStreet3,
                    City = order.ShipCity,
                    StateProvince = order.ShipStateProvCode,
                    CountryCode = order.ShipCountryCode,
                    PostalCode = order.ShipPostalCode
                },
                BillAddress = new Address()
                {
                    RecipientName = order.BillUnparsedName,
                    Street1 = order.BillStreet1,
                    Street2 = order.BillStreet2,
                    Street3 = order.BillStreet3,
                    City = order.BillCity,
                    StateProvince = order.BillStateProvCode,
                    CountryCode = order.BillCountryCode,
                    PostalCode = order.BillPostalCode
                },
            };
    }
}

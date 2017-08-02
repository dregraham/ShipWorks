using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet.DTO;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Load jet order data into OrderEntities
    /// </summary>
    [Component]
    public class JetOrderLoader : IJetOrderLoader
    {
        private readonly IOrderElementFactory orderElementFactory;
        private readonly IJetOrderItemLoader orderItemLoader;
        private readonly IOrderChargeCalculator orderChargeCalculator;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetOrderLoader(IOrderElementFactory orderElementFactory, Func<IOrderElementFactory, IJetOrderItemLoader> orderItemLoaderFactory, IOrderChargeCalculator orderChargeCalculator)
        {
            this.orderElementFactory = orderElementFactory;
            orderItemLoader = orderItemLoaderFactory(orderElementFactory);
            this.orderChargeCalculator = orderChargeCalculator;
        }

        /// <summary>
        /// Load the jetOrder into the order entity
        /// </summary>
        public void LoadOrder(JetOrderEntity order, JetOrderDetailsResult jetOrder, JetStoreEntity store)
        {
            order.ChangeOrderNumber(jetOrder.ReferenceOrderId.ToString());
            order.MerchantOrderId = jetOrder.MerchantOrderId;
            order.OrderDate = jetOrder.OrderPlacedDate;
            order.OnlineStatus = "Acknowledged";

            order.RequestedShipping = $"{jetOrder.OrderDetail.RequestShippingCarrier} {jetOrder.OrderDetail.RequestShippingMethod}";

            LoadBuyer(order, jetOrder.Buyer);
            LoadShipping(order, jetOrder.ShippingTo);

            // items
            orderItemLoader.LoadItems(order, jetOrder, store);

            // charges
            LoadCharges(order, jetOrder);

            // Update the total
            order.OrderTotal = orderChargeCalculator.CalculateTotal(order);

        }

        /// <summary>
        /// Load the order charges
        /// </summary>
        private void LoadCharges(JetOrderEntity order, JetOrderDetailsResult jetOrder)
        {
            decimal taxAmount = jetOrder.OrderTotals.ItemPrice.ItemShippingTax + jetOrder.OrderTotals.ItemPrice.ItemShippingTax;
            if (taxAmount > 0)
            {
                orderElementFactory.CreateCharge(order, "TAX", "Tax", taxAmount);
            }

            decimal shippingAmount = jetOrder.OrderTotals.ItemPrice.ItemShippingCost;
            if (shippingAmount > 0)
            {
                orderElementFactory.CreateCharge(order, "SHIPPING", "Shipping", shippingAmount);
            }
        }

        /// <summary>
        /// Load the shipping info
        /// </summary>
        private static void LoadShipping(OrderEntity order, JetShippingTo shipping)
        {
            new PersonAdapter(order, "Ship")
            {
                NameParseStatus = PersonNameParseStatus.Unparsed,
                UnparsedName = shipping.Recipient.Name,
                Phone = shipping.Recipient.PhoneNumber,
                Street1 = shipping.Address.Address1,
                Street2 = shipping.Address.Address2,
                City = shipping.Address.City,
                StateProvCode = shipping.Address.State,
                PostalCode = shipping.Address.ZipCode,
                CountryCode = "US"
            };
        }

        /// <summary>
        /// Load the buyer info
        /// </summary>
        private static void LoadBuyer(OrderEntity order, JetBuyer buyer)
        {
            new PersonAdapter(order, "Bill")
            {
                NameParseStatus = PersonNameParseStatus.Unparsed,
                UnparsedName = buyer.Name,
                Phone = buyer.PhoneNumber,
                CountryCode = "US"
            };
        }
    }
}
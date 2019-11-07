using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Platforms.Rakuten.Enums;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Populates a RakutenOrderEntity from a downloaded Rakuten order
    /// </summary>
    [Component(RegistrationType.Self)]
    public class RakutenOrderLoader
    {
        private readonly IOrderChargeCalculator orderChargeCalculator;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="RakutenOrderLoader"/> class.
        /// </summary>
        /// <param name="orderChargeCalculator">The order charge calculator.</param>
        public RakutenOrderLoader(IOrderChargeCalculator orderChargeCalculator,
            Func<Type, ILog> loggerFactory)
        {
            this.orderChargeCalculator = orderChargeCalculator;
            log = loggerFactory(typeof(RakutenOrderLoader));
        }

        /// <summary>
        /// Loads the order
        /// </summary>
        /// <remarks>
        /// Order to save must have store loaded
        /// </remarks>
        public void LoadOrder(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder,
            List<RakutenProduct> downloadedProducts, IOrderElementFactory orderElementFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderToSave.Store, "orderToSave.Store");

            orderToSave.OrderNumber = downloadedOrder.OrderNumber;
            orderToSave.OrderDate = downloadedOrder.OrderDate;
            orderToSave.OnlineLastModified = downloadedOrder.LastModifiedDate;

            LoadOrderStatuses(orderToSave, downloadedOrder);
            LoadAddresses(orderToSave, downloadedOrder);

            if (orderToSave.IsNew || string.IsNullOrWhiteSpace(orderToSave.RequestedShipping))
            {
                LoadRequestedShipping(orderToSave, downloadedOrder);
            }

            if (orderToSave.IsNew)
            {

                LoadNotes(orderToSave, downloadedOrder, orderElementFactory);

                // items
                LoadItems(orderToSave, downloadedOrder, downloadedProducts, orderElementFactory);

                // charges
                LoadCharges(orderToSave, downloadedOrder, orderElementFactory);

                // payments
                LoadPayments(orderToSave, downloadedOrder, orderElementFactory);

                SetOrderTotal(orderToSave, downloadedOrder, orderElementFactory);
            }
        }

        /// <summary>
        /// Set the order total
        /// </summary>
        private void SetOrderTotal(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderToSave.OrderTotal = downloadedOrder.OrderTotal;

            decimal calculatedTotal = orderChargeCalculator.CalculateTotal(orderToSave);

            // Sometimes Rakuten doesnt give us all of the discounts for the order, if the order total is different than what we calculate
            // we need to figure out the difference so that the user can see that there was an aditional discount or cost
            if (downloadedOrder.OrderTotal != calculatedTotal)
            {
                decimal adjustment = downloadedOrder.OrderTotal - calculatedTotal;

                log.Info($"Order total for {downloadedOrder.OrderNumber} does not match our calculated total, adding an adjustment charge of {adjustment} to compensate for the discrepancy.");

                orderElementFactory.CreateCharge(orderToSave, "ADDITIONAL COST OR DISCOUNT", "Additional Cost or Discount", adjustment);
            }
        }

        /// <summary>
        /// Loads the items.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="downloadedProducts">The downloaded products.</param>
        /// <param name="orderElementFactory">The order element factory.</param>
        private void LoadItems(RakutenOrderEntity orderToSave,
            RakutenOrder downloadedOrder,
            List<RakutenProduct> downloadedProducts,
            IOrderElementFactory orderElementFactory)
        {
            if (downloadedOrder.Items != null)
            {
                foreach (RakutenOrderItem downloadedItem in downloadedOrder.Items)
                {
                    RakutenOrderItemEntity itemToSave =
                        (RakutenOrderItemEntity) orderElementFactory.CreateItem(orderToSave);

                    LoadItem(itemToSave, downloadedOrder, downloadedItem, orderElementFactory);

                    RakutenProduct downloadedProduct = downloadedProducts.FirstOrDefault(p => p.ID == downloadedItem.ProductID);

                    if (downloadedProduct != null)
                    {
                        LoadProductDetails(itemToSave, downloadedProduct);
                        LoadProductAttributes(itemToSave, downloadedProduct, orderElementFactory);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the item.
        /// </summary>
        private void LoadItem(RakutenOrderItemEntity itemToSave,
            RakutenOrder downloadedOrder,
            RakutenOrderItem downloadedItem,
            IOrderElementFactory orderElementFactory)
        {
            itemToSave.SKU = downloadedItem.BaseSku;
            itemToSave.Discount = downloadedItem.Discount;
            itemToSave.ItemTotal = downloadedItem.ItemTotal;
            itemToSave.OrderItemID = downloadedItem.OrderItemID;
            itemToSave.Quantity = downloadedItem.Quantity;
            itemToSave.UnitPrice = downloadedItem.UnitPrice;

        }

        /// <summary>
        /// Loads the charges.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="orderElementFactory">The order element factory.</param>
        private void LoadCharges(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder,
            IOrderElementFactory orderElementFactory)
        {
            if (downloadedOrder.TotalShippingPrice != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "SHIPPING", "Shipping", downloadedOrder.TotalShippingPrice);
            }
        }

        /// <summary>
        /// Loads the notes.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="orderElementFactory">The order element factory.</param>
        private void LoadNotes(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.MerchantMemo, orderToSave.OrderDate, NoteVisibility.Public);
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.ShopperComment, orderToSave.OrderDate, NoteVisibility.Public);
        }

        /// <summary>
        /// Loads the shipping, billing, and email addresses.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private void LoadAddresses(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder)
        {
            PersonAdapter shipAdapter = new PersonAdapter(orderToSave, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(orderToSave, "Bill");

            LoadShippingAddress(downloadedOrder, shipAdapter);
            LoadBillingAddress(downloadedOrder, shipAdapter, billAdapter);

            billAdapter.Email = downloadedOrder.BuyerEmailAddress;

            // No shipping email provided, so if ship and bill are the same, copy the email to the ship
            if (shipAdapter.FirstName == billAdapter.FirstName &&
                shipAdapter.LastName == billAdapter.LastName &&
                shipAdapter.City == billAdapter.City &&
                shipAdapter.Street1 == billAdapter.Street1)
            {
                shipAdapter.Email = billAdapter.Email;
            }
        }

        /// <summary>
        /// Loads the shipping address.
        /// </summary>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="shipAdapter">The ship adapter.</param>
        private static void LoadShippingAddress(RakutenOrder downloadedOrder, PersonAdapter shipAdapter)
        {
            shipAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            shipAdapter.FirstName = downloadedOrder.ShippingName;
            shipAdapter.LastName = downloadedOrder.ShippingName;
            shipAdapter.Street1 = downloadedOrder.ShippingAddress1;
            shipAdapter.City = downloadedOrder.ShippingCity;
            shipAdapter.StateProvCode = dowloadedOrder.ShippingSateCode;
            shipAdapter.PostalCode = downloadedOrder.ShippingPostalCode;
            shipAdapter.CountryCode = downloadedOrder.BillingCountryCode;
            shipAdapter.Phone = downloadedOrder.ShippingPhoneNumber;
        }

        /// <summary>
        /// Loads the billing address.
        /// </summary>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="shipAdapter">The ship adapter.</param>
        /// <param name="billAdapter">The bill adapter.</param>
        private void LoadBillingAddress(RakutenOrder downloadedOrder, PersonAdapter shipAdapter, PersonAdapter billAdapter)
        {
            shipAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            shipAdapter.FirstName = downloadedOrder.BillingName;
            shipAdapter.LastName = downloadedOrder.BillingName;
            shipAdapter.Street1 = downloadedOrder.BillingAddress1;
            shipAdapter.City = downloadedOrder.BillingCity;
            shipAdapter.StateProvCode = dowloadedOrder.BillingSateCode;
            shipAdapter.PostalCode = downloadedOrder.BillingPostalCode;
            shipAdapter.CountryCode = downloadedOrder.BillingCountryCode;
            shipAdapter.Phone = downloadedOrder.BillingPhoneNumber;
        }

        /// <summary>
        /// Loads payment information for the order
        /// </summary>
        private void LoadPayments(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Type", downloadedOrder.PaymentMethod);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment ID", downloadedOrder.OrderPaymentID);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Status", downloadedOrder.PaymentStatus);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Point Amount", downloadedOrder.PointAmount);
        }

        /// <summary>
        /// Creates the payment detail if value is not null or whitespace
        /// </summary>
        private void CreatePaymentDetail(IOrderElementFactory orderElementFactory, RakutenOrderEntity orderToSave, string label, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                orderElementFactory.CreatePaymentDetail(orderToSave, label, value);
            }
        }

        /// <summary>
        /// Loads the requested shipping and prime status.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private static void LoadRequestedShipping(RakutenOrderEntity orderToSave,
            RakutenOrder downloadedOrder)
        {
            RakutenFulfillment fulfillment = downloadedOrder.Fulfillments.FirstOrDefault();
            string carrier = fulfillment?.ShippingCarrier;
            string shippingClass = fulfillment?.ShippingClass;

            if (!string.IsNullOrEmpty(carrier) || !string.IsNullOrEmpty(shippingClass))
            {
                orderToSave.RequestedShipping = $"{carrier} - {shippingClass}";
            }

            orderToSave.IsPrime = (int) RakutenHelper.GetIsPrime(fulfillment?.ShippingClass, fulfillment?.ShippingCarrier);
        }

        /// <summary>
        /// Loads the order statuses.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private static void LoadOrderStatuses(RakutenOrderEntity orderToSave,
            RakutenOrder downloadedOrder)
        {
            orderToSave.OnlinePaymentStatus =
                (int) (EnumHelper.TryParseEnum<RakutenPaymentStatus>(downloadedOrder.OrderStatus) ??
                       RakutenPaymentStatus.Unknown);
        }
    }
}
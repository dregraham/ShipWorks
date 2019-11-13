using System;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

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
        public Task LoadOrder(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder,
            IOrderElementFactory orderElementFactory)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderToSave.Store, "orderToSave.Store");

            orderToSave.RakutenOrderID = downloadedOrder.OrderNumber;
            orderToSave.OrderDate = downloadedOrder.OrderDate;
            orderToSave.OnlineLastModified = downloadedOrder.LastModifiedDate;

            LoadAddresses(orderToSave, downloadedOrder);

            if (orderToSave.IsNew || string.IsNullOrWhiteSpace(orderToSave.RequestedShipping))
            {
                LoadRequestedShipping(orderToSave, downloadedOrder);
            }

            if (orderToSave.IsNew)
            {
                // Load order data
                LoadNotes(orderToSave, downloadedOrder, orderElementFactory);
                LoadItems(orderToSave, downloadedOrder, orderElementFactory);
                LoadCharges(orderToSave, downloadedOrder, orderElementFactory);
                LoadPayments(orderToSave, downloadedOrder, orderElementFactory);
                SetOrderTotal(orderToSave, downloadedOrder, orderElementFactory);
            }

            return null;
        }

        /// <summary>
        /// Set the order total
        /// </summary>
        private void SetOrderTotal(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderToSave.OrderTotal = downloadedOrder.OrderTotal;

            decimal calculatedTotal = orderChargeCalculator.CalculateTotal(orderToSave);

            // Sometimes Rakuten doesn't give us all of the discounts for the order
            // If the order total is different than what we calculate we need to show the additional discount/cost
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
        private void LoadItems(RakutenOrderEntity orderToSave,
            RakutenOrder downloadedOrder,
            IOrderElementFactory orderElementFactory)
        {
            if (downloadedOrder.OrderItems != null)
            {
                foreach (RakutenOrderItem downloadedItem in downloadedOrder.OrderItems)
                {
                    RakutenOrderItemEntity itemToSave =
                        (RakutenOrderItemEntity) orderElementFactory.CreateItem(orderToSave);

                    LoadItem(itemToSave, downloadedItem);
                }
            }
        }

        /// <summary>
        /// Loads the item.
        /// </summary>
        private void LoadItem(RakutenOrderItemEntity itemToSave, RakutenOrderItem downloadedItem)
        {
            itemToSave.SKU = downloadedItem.SKU ?? downloadedItem.BaseSku;
            itemToSave.Discount = downloadedItem.Discount;
            itemToSave.ItemTotal = downloadedItem.ItemTotal;
            itemToSave.RakutenOrderItemID = downloadedItem.OrderItemID;
            itemToSave.Quantity = downloadedItem.Quantity;
            itemToSave.UnitPrice = downloadedItem.UnitPrice;
        }

        /// <summary>
        /// Loads the charges.
        /// </summary>
        private void LoadCharges(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder,
            IOrderElementFactory orderElementFactory)
        {
            if (downloadedOrder.Shipping.ShippingFee != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "SHIPPING", "Shipping", downloadedOrder.Shipping.ShippingFee);
            }
        }

        /// <summary>
        /// Loads the notes.
        /// </summary>
        private void LoadNotes(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.MerchantMemo, orderToSave.OrderDate, NoteVisibility.Public);
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.ShopperComment, orderToSave.OrderDate, NoteVisibility.Public);
        }

        /// <summary>
        /// Loads the shipping, billing, and email addresses.
        /// </summary>
        private void LoadAddresses(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder)
        {
            PersonAdapter shipAdapter = new PersonAdapter(orderToSave, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(orderToSave, "Bill");

            LoadShippingAddress(downloadedOrder, shipAdapter);
            LoadBillingAddress(downloadedOrder, shipAdapter, billAdapter);

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
        private static void LoadShippingAddress(RakutenOrder downloadedOrder, PersonAdapter shipAdapter)
        {
            var address = downloadedOrder.Shipping.DeliveryAddress;
            shipAdapter.NameParseStatus = PersonNameParseStatus.Unparsed;
            shipAdapter.UnparsedName = address.Name;
            shipAdapter.Street1 = address.Address1;
            shipAdapter.Street2 = address.Address2;
            shipAdapter.City = address.CityName;
            shipAdapter.StateProvCode = address.StateCode;
            shipAdapter.PostalCode = address.PostalCode;
            shipAdapter.CountryCode = address.CountryCode;
            shipAdapter.Phone = address.PhoneNumber;
        }

        /// <summary>
        /// Loads the billing address.
        /// </summary>
        private void LoadBillingAddress(RakutenOrder downloadedOrder, PersonAdapter shipAdapter, PersonAdapter billAdapter)
        {
            var address = downloadedOrder.Shipping.InvoiceAddress;
            billAdapter.NameParseStatus = PersonNameParseStatus.Unparsed;
            billAdapter.UnparsedName = address.Name;
            billAdapter.Street1 = address.Address1;
            billAdapter.Street2 = address.Address2;
            billAdapter.City = address.CityName;
            billAdapter.StateProvCode = address.StateCode;
            billAdapter.PostalCode = address.PostalCode;
            billAdapter.CountryCode = address.CountryCode;
            billAdapter.Phone = address.PhoneNumber;
        }

        /// <summary>
        /// Loads payment information for the order
        /// </summary>
        private void LoadPayments(RakutenOrderEntity orderToSave, RakutenOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment ID", downloadedOrder.Payment.OrderPaymentID);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Status", downloadedOrder.Payment.PaymentStatus);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Amount", downloadedOrder.Payment.PayAmount);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Point Amount", downloadedOrder.Payment.PointAmount);
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
        private static void LoadRequestedShipping(RakutenOrderEntity orderToSave,
            RakutenOrder downloadedOrder)
        {
            var shipping = downloadedOrder.Shipping;
            string carrier = shipping?.ShippingMethod;

            if (!string.IsNullOrEmpty(carrier))
            {
                orderToSave.RequestedShipping = carrier;
            }
        }
    }
}
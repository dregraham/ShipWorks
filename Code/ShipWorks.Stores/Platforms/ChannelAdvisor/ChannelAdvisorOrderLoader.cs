using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    [Component(RegistrationType.Self)]
    public class ChannelAdvisorOrderLoader
    {
        private readonly IOrderChargeCalculator orderChargeCalculator;
        private readonly IOrderRepository orderRepository;
        private readonly IChannelAdvisorRestClient webClient;

        public ChannelAdvisorOrderLoader(IOrderChargeCalculator orderChargeCalculator, IOrderRepository orderRepository, IChannelAdvisorRestClient webClient)
        {
            this.orderChargeCalculator = orderChargeCalculator;
            this.orderRepository = orderRepository;
            this.webClient = webClient;
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public void LoadOrder(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory, string accessToken)
        {
            orderToSave.OrderNumber = downloadedOrder.ID;
            orderToSave.OrderDate = downloadedOrder.CreatedDateUtc;
            orderToSave.OnlineLastModified = downloadedOrder.CreatedDateUtc;
            orderToSave.CustomOrderIdentifier = downloadedOrder.SiteOrderID;

            LoadOrderStatuses(orderToSave, downloadedOrder);
            LoadOrderFlag(orderToSave, downloadedOrder);
            LoadAddresses(orderToSave, downloadedOrder);

            if (orderToSave.IsNew || string.IsNullOrWhiteSpace(orderToSave.RequestedShipping))
            {
                LoadRequestedShippingAndPrimeStatus(orderToSave, downloadedOrder);
            }

            if (orderToSave.IsNew)
            {
                orderToSave.ResellerID = downloadedOrder.ResellerID;
                orderToSave.MarketplaceNames = downloadedOrder.SiteName;

                LoadNotes(orderToSave, downloadedOrder, orderElementFactory);

                // items
                LoadItems(orderToSave, downloadedOrder, orderElementFactory, accessToken);

                // charges
                LoadCharges(orderToSave, downloadedOrder, orderElementFactory);

                // payments
                LoadPayments(orderToSave, downloadedOrder, orderElementFactory);

                // Update the total
                orderToSave.OrderTotal = orderChargeCalculator.CalculateTotal(orderToSave);
            }
        }

        private void LoadItems(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory, string accessToken)
        {
            if (downloadedOrder.Items != null)
            {
                foreach (ChannelAdvisorOrderItem item in downloadedOrder.Items)
            {
                LoadItem(orderToSave, downloadedOrder, item, orderElementFactory, accessToken);
            }
                }
        }

        private void LoadItem(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, ChannelAdvisorOrderItem downloadedItem, IOrderElementFactory orderElementFactory, string accessToken)
        {
            ChannelAdvisorOrderItemEntity itemToSave = (ChannelAdvisorOrderItemEntity) orderElementFactory.CreateItem(orderToSave);

            itemToSave.Name = downloadedItem.Title;
            itemToSave.Quantity = downloadedItem.Quantity;
            itemToSave.UnitPrice = downloadedItem.UnitPrice;
            itemToSave.Code = downloadedItem.Sku;
            itemToSave.SKU = downloadedItem.Sku;

            // CA-specific
            itemToSave.MarketplaceName = downloadedItem.SiteOrderItemID;
            itemToSave.MarketplaceBuyerID = downloadedOrder.BuyerUserID;

            if (!string.IsNullOrWhiteSpace(downloadedItem.GiftNotes))
            {
                OrderItemAttributeEntity attribute = orderElementFactory.CreateItemAttribute(itemToSave);
                attribute.Name = "Gift Notes";
                attribute.Description = downloadedItem.GiftNotes;

                // gift wrap cost is already included as a Charge
                attribute.UnitPrice = 0M;
            }

            if (!string.IsNullOrWhiteSpace(downloadedItem.GiftMessage))
            {
                OrderItemAttributeEntity attribute = orderElementFactory.CreateItemAttribute(itemToSave);
                attribute.Name = "Gift Message";
                attribute.Description = downloadedItem.GiftMessage;
                attribute.UnitPrice = 0M;
            }
            ChannelAdvisorProduct product = webClient.GetProduct(downloadedItem.ProductID, accessToken);
            LoadProductDetails(itemToSave, product, orderElementFactory);

            // Appear in SOAP, but not REST
            // IsFBA, should be wrapped up in DC stuff
            // UnitWeight.UnitOfMeasure, default is lbs for US profiles, KG all else, so we could hook into that
            // SalesSourceID
            // UserName
        }

        private void LoadProductDetails(ChannelAdvisorOrderItemEntity itemToSave, ChannelAdvisorProduct product, IOrderElementFactory orderElementFactory)
        {
            itemToSave.Weight = (double) product.Weight;
            itemToSave.Location = product.WarehouseLocation;
            itemToSave.Classification = product.Classification;
            itemToSave.UnitCost = product.Cost;
            itemToSave.HarmonizedCode = product.HarmonizedCode;
            itemToSave.ISBN = product.ISBN;
            itemToSave.UPC = product.UPC;
            itemToSave.MPN = product.MPN;
            itemToSave.Description = product.Description;

            foreach (ChannelAdvisorProductAttribute attribute in product.Attributes)
            {
                orderElementFactory.CreateItemAttribute(itemToSave, attribute.Name, attribute.Value, 0, false);
            }

            ChannelAdvisorProductImage image = product.Images?.FirstOrDefault();
            itemToSave.Image = image?.Url ?? string.Empty;
            itemToSave.Thumbnail = itemToSave.Image;
        }

        /// <summary>
        /// Loads the charges.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="orderElementFactory">The order element factory.</param>
        private void LoadCharges(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder,
            IOrderElementFactory orderElementFactory)
        {
            if (!downloadedOrder.TotalTaxPrice.IsEquivalentTo(0))
            {
                // Total tax price includes shipping tax and gift wrap tax, so no need to load them
                orderElementFactory.CreateCharge(orderToSave, "Sales Tax", "TAX", downloadedOrder.TotalTaxPrice);
            }

            if (!downloadedOrder.TotalShippingPrice.IsEquivalentTo(0))
            {
                orderElementFactory.CreateCharge(orderToSave, "Shipping", "SHIPPING", downloadedOrder.TotalShippingPrice);
            }

            if (!downloadedOrder.TotalInsurancePrice.IsEquivalentTo(0))
            {
                orderElementFactory.CreateCharge(orderToSave, "Shipping Insurance", "INSURANCE", downloadedOrder.TotalInsurancePrice);
            }

            if (!downloadedOrder.TotalGiftOptionPrice.IsEquivalentTo(0))
            {
                orderElementFactory.CreateCharge(orderToSave, "Gift Wrap", "GIFT WRAP", downloadedOrder.TotalGiftOptionPrice);
            }

            if (!downloadedOrder.AdditionalCostOrDiscount.IsEquivalentTo(0))
            {
                orderElementFactory.CreateCharge(orderToSave, "Additional Cost or Discount",
                    "ADDITIONAL COST OR DISCOUNT", downloadedOrder.AdditionalCostOrDiscount);
            }
        }

        private void LoadNotes(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.PublicNotes, orderToSave.OrderDate, NoteVisibility.Public);
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.SpecialInstructions, orderToSave.OrderDate, NoteVisibility.Public);
            orderElementFactory.CreateNote(orderToSave, downloadedOrder.PrivateNotes, orderToSave.OrderDate, NoteVisibility.Internal);

            // A gift message can be associated with each item in the order, so we need to find any items containing
            // gift messages and add each message as a note
            if (downloadedOrder.Items != null)
            {
                foreach (ChannelAdvisorOrderItem item in downloadedOrder.Items)
                {
                    if (!string.IsNullOrWhiteSpace(item.GiftMessage))
                    {
                        orderElementFactory.CreateNote(orderToSave, $"Gift message for {item.Title}: {item.GiftMessage}",
                            orderToSave.OrderDate, NoteVisibility.Public);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the shipping, billing, and email addresses.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private void LoadAddresses(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder)
        {
            PersonAdapter shipAdapter = new PersonAdapter(orderToSave, "Ship");
            PersonAdapter billAdapter = new PersonAdapter(orderToSave, "Bill");

            LoadShippingAddress(downloadedOrder, shipAdapter);
            LoadBillingAddress(downloadedOrder, shipAdapter, billAdapter);

            billAdapter.Email = downloadedOrder.BuyerEmailAddress;

            // No shipping email provided, so if ship and bill are the same, copy the email to the ship
            if (shipAdapter.FirstName == billAdapter.FirstName&&
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
        private static void LoadShippingAddress(ChannelAdvisorOrder downloadedOrder, PersonAdapter shipAdapter)
        {
            shipAdapter.NameParseStatus = PersonNameParseStatus.Simple;
            shipAdapter.FirstName = downloadedOrder.ShippingFirstName;
            shipAdapter.LastName = downloadedOrder.ShippingLastName;
            shipAdapter.Company = downloadedOrder.ShippingCompanyName;
            shipAdapter.Street1 = downloadedOrder.ShippingAddressLine1;
            shipAdapter.Street2 = downloadedOrder.ShippingAddressLine2;
            shipAdapter.City = downloadedOrder.ShippingCity;
            shipAdapter.StateProvCode = ChannelAdvisorHelper.GetStateProvCode(downloadedOrder.ShippingStateOrProvince);
            shipAdapter.PostalCode = downloadedOrder.ShippingPostalCode;
            shipAdapter.CountryCode = ChannelAdvisorHelper.GetCountryCode(downloadedOrder.ShippingCountry);
            shipAdapter.Phone = downloadedOrder.ShippingDaytimePhone;
        }

        /// <summary>
        /// Loads the billing address.
        /// </summary>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="shipAdapter">The ship adapter.</param>
        /// <param name="billAdapter">The bill adapter.</param>
        private void LoadBillingAddress(ChannelAdvisorOrder downloadedOrder, PersonAdapter shipAdapter, PersonAdapter billAdapter)
        {
            // In ChannelAdvsior if the buyer selected Use Shipping as Billing during checkout,
            // the values get copied to billing, but that data doesn't come down in Billing.
            if (string.IsNullOrWhiteSpace(downloadedOrder.BillingFirstName)
                && string.IsNullOrWhiteSpace(downloadedOrder.BillingLastName)
                && string.IsNullOrWhiteSpace(downloadedOrder.BillingAddressLine1)
                && string.IsNullOrWhiteSpace(downloadedOrder.BillingCity))
            {
                // copy shipping to billing
                PersonAdapter.Copy(shipAdapter, billAdapter);
            }
            else
            {
                billAdapter.NameParseStatus = PersonNameParseStatus.Simple;
                billAdapter.FirstName = downloadedOrder.BillingFirstName;
                billAdapter.LastName = downloadedOrder.BillingLastName;
                billAdapter.Company = downloadedOrder.BillingCompanyName;
                billAdapter.Street1 = downloadedOrder.BillingAddressLine1;
                billAdapter.Street2 = downloadedOrder.BillingAddressLine2;
                billAdapter.City = downloadedOrder.BillingCity;
                billAdapter.StateProvCode = ChannelAdvisorHelper.GetStateProvCode(downloadedOrder.BillingStateOrProvince);
                billAdapter.PostalCode = downloadedOrder.BillingPostalCode;
                billAdapter.CountryCode = ChannelAdvisorHelper.GetCountryCode(downloadedOrder.BillingCountry);
                billAdapter.Phone = downloadedOrder.BillingDaytimePhone;
            }
        }

        /// <summary>
        /// Loads payment information for the order
        /// </summary>
        private void LoadPayments(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            CreatePaymentDetail(orderElementFactory, orderToSave, "Payment Type", downloadedOrder.PaymentMethod);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Card Number", downloadedOrder.PaymentCreditCardLast4);
            CreatePaymentDetail(orderElementFactory, orderToSave, "Reference", downloadedOrder.PaymentMerchantReferenceNumber);
            CreatePaymentDetail(orderElementFactory, orderToSave, "TransactionID", downloadedOrder.PaymentTransactionID);
        }

        /// <summary>
        /// Creates the payment detail if value is not null or whitespace
        /// </summary>
        private void CreatePaymentDetail(IOrderElementFactory orderElementFactory, ChannelAdvisorOrderEntity orderToSave, string label, string value)
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
        private static void LoadRequestedShippingAndPrimeStatus(ChannelAdvisorOrderEntity orderToSave,
            ChannelAdvisorOrder downloadedOrder)
        {
            string carrier = downloadedOrder.Fulfillments.FirstOrDefault()?.ShippingCarrier;
            string shippingClass = downloadedOrder.Fulfillments.FirstOrDefault()?.ShippingClass;

            if (!string.IsNullOrEmpty(carrier) || !string.IsNullOrEmpty(shippingClass))
            {
                orderToSave.RequestedShipping = $"{carrier} - {shippingClass}";
            }

            orderToSave.IsPrime = (int) ChannelAdvisorHelper.GetIsPrime(shippingClass, carrier);
        }

        /// <summary>
        /// Loads the order flag.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private void LoadOrderFlag(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder)
        {
            orderToSave.FlagStyle = EnumHelper.GetDescription((ChannelAdvisorRestFlagType) downloadedOrder.FlagID);
            orderToSave.FlagDescription = downloadedOrder.FlagDescription;
            orderToSave.FlagType = downloadedOrder.FlagID;
        }

        /// <summary>
        /// Loads the order statuses.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private static void LoadOrderStatuses(ChannelAdvisorOrderEntity orderToSave,
            ChannelAdvisorOrder downloadedOrder)
        {
            ChannelAdvisorRestPaymentStatus paymentStatus;
            orderToSave.OnlinePaymentStatus =
                (int) (Enum.TryParse(downloadedOrder.PaymentStatus, true, out paymentStatus) ?
                paymentStatus :
                ChannelAdvisorRestPaymentStatus.Unknown);

            ChannelAdvisorRestCheckoutStatus checkoutStatus;
            orderToSave.OnlineCheckoutStatus =
                (int) (Enum.TryParse(downloadedOrder.CheckoutStatus, true, out checkoutStatus) ?
                checkoutStatus :
                ChannelAdvisorRestCheckoutStatus.Unknown);

            ChannelAdvisorRestShippingStatus shippingStatus;
            orderToSave.OnlineShippingStatus =
                (int) (Enum.TryParse(downloadedOrder.ShippingStatus, true, out shippingStatus) ?
                shippingStatus :
                ChannelAdvisorRestShippingStatus.Unknown);
        }
    }
}
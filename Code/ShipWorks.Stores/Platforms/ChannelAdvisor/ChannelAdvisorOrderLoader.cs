﻿using System;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Stores.Platforms.Walmart;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorOrderLoader
    {
        private readonly IOrderChargeCalculator orderChargeCalculator;
        private readonly IOrderRepository orderRepository;

        public ChannelAdvisorOrderLoader(IOrderChargeCalculator orderChargeCalculator, IOrderRepository orderRepository)
        {
            this.orderChargeCalculator = orderChargeCalculator;
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        public void LoadOrder(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory)
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
                LoadItems(orderToSave, downloadedOrder, orderElementFactory);

                // charges
                LoadCharges(orderToSave, downloadedOrder, orderElementFactory);

                // payments
                LoadPayments(orderToSave, downloadedOrder, orderElementFactory);

                // Update the total
                orderToSave.OrderTotal = orderChargeCalculator.CalculateTotal(orderToSave);
            }
        }

        private void LoadItems(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            foreach (ChannelAdvisorOrderItem item in downloadedOrder.Items)
            {
                LoadItem(orderToSave, downloadedOrder, item, orderElementFactory);
            }
        }

        private void LoadItem(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder order, ChannelAdvisorOrderItem item, IOrderElementFactory orderElementFactory)
        {
            ChannelAdvisorOrderItemEntity itemToSave = (ChannelAdvisorOrderItemEntity) orderElementFactory.CreateItem(orderToSave);

            itemToSave.Name = item.Title;
            itemToSave.Quantity = item.Quantity;
            itemToSave.UnitPrice = item.UnitPrice;
            itemToSave.Code = item.Sku;
            itemToSave.SKU = item.Sku;

            // CA-specific
            itemToSave.MarketplaceName = item.SiteOrderItemID;
            itemToSave.MarketplaceBuyerID = order.BuyerUserId;

            if (!string.IsNullOrWhiteSpace(item.GiftNotes))
            {
                OrderItemAttributeEntity attribute = orderElementFactory.CreateItemAttribute(itemToSave);
                attribute.Name = "Gift Notes";
                attribute.Description = item.GiftNotes;

                // gift wrap cost is already included as a Charge
                attribute.UnitPrice = 0M;
            }

            if (!string.IsNullOrWhiteSpace(item.GiftMessage))
            {
                OrderItemAttributeEntity attribute = orderElementFactory.CreateItemAttribute(itemToSave);
                attribute.Name = "Gift Message";
                attribute.Description = item.GiftMessage;
                attribute.UnitPrice = 0M;
            }

            itemToSave.DistributionCenter = "";
            itemToSave.Classification = "";
            itemToSave.UnitCost = 0;
            itemToSave.HarmonizedCode = "";
            itemToSave.ISBN = "";
            itemToSave.UPC = "";
            itemToSave.MPN = "";

            // LoadItemAttributes();
            // LoadImages();

            // Get from Product endpoint
            // itemToSave.Weight =
            // itemToSave.Location = item.WarehouseLocation;

            // Appear in SOAP, but not REST

            // itemToSave.IsFBA = item.IsFBA;
            // // Convert KG to LBS if needed
            // if (item.UnitWeight.UnitOfMeasure == "KG")
            // {
            //    itemToSave.Weight = itemToSave.Weight * 2.20462262;
            // }
            // itemToSave.MarketplaceSalesID = item.SalesSourceID;
            // itemToSave.MarketplaceStoreName = !string.IsNullOrWhiteSpace(item.UserName) ? item.UserName : string.Empty;

            //
            //
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
            foreach (ChannelAdvisorOrderItem item in downloadedOrder.Items)
            {
                if (!string.IsNullOrWhiteSpace(item.GiftMessage))
                {
                    orderElementFactory.CreateNote(orderToSave, $"Gift message for {item.Title}: {item.GiftMessage}",
                        orderToSave.OrderDate, NoteVisibility.Public);
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
            if (downloadedOrder.BillingFirstName.Length == 0
                && downloadedOrder.BillingLastName.Length == 0
                && downloadedOrder.BillingAddressLine1.Length == 0
                && downloadedOrder.BillingCity.Length == 0)
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
                billAdapter.CountryCode = ChannelAdvisorHelper.GetCountryCode(downloadedOrder.BillingCountry.Trim());
                billAdapter.Phone = downloadedOrder.BillingDaytimePhone;
            }
        }

        /// <summary>
        /// Loads payment information for the order
        /// </summary>
        private void LoadPayments(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderElementFactory.CreatePaymentDetail(orderToSave, "Payment Type", downloadedOrder.PaymentMethod);
            orderElementFactory.CreatePaymentDetail(orderToSave, "Card Number", downloadedOrder.PaymentCreditCardLast4);
            orderElementFactory.CreatePaymentDetail(orderToSave, "Reference", downloadedOrder.PaymentMerchantReferenceNumber);
            orderElementFactory.CreatePaymentDetail(orderToSave, "TransactionID", downloadedOrder.PaymentTransactionID);
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
        private static void LoadOrderStatuses(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder)
        {
            int paymentStatus;
            orderToSave.OnlinePaymentStatus = Enum.TryParse(downloadedOrder.PaymentStatus, true, out paymentStatus) ?
                paymentStatus :
                (int) ChannelAdvisorPaymentStatus.Unknown;

            int checkoutStatus;
            orderToSave.OnlineCheckoutStatus = Enum.TryParse(downloadedOrder.CheckoutStatus, true, out checkoutStatus) ?
                paymentStatus :
                (int) ChannelAdvisorPaymentStatus.Unknown;

            int shippingStatus;
            orderToSave.OnlineShippingStatus = Enum.TryParse(downloadedOrder.ShippingStatus, true, out shippingStatus) ?
                paymentStatus :
                (int) ChannelAdvisorPaymentStatus.Unknown;
        }
    }
}
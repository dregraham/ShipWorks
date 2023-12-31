﻿using System;
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
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Populates a ChannelAdvisorOrderEntity from a downloaded ChannelAdvisor order
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ChannelAdvisorOrderLoader
    {
        private readonly IOrderChargeCalculator orderChargeCalculator;
        private readonly IEnumerable<ChannelAdvisorDistributionCenter> distributionCenters;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAdvisorOrderLoader"/> class.
        /// </summary>
        /// <param name="orderChargeCalculator">The order charge calculator.</param>
        /// <param name="distributionCenters"></param>
        public ChannelAdvisorOrderLoader(IOrderChargeCalculator orderChargeCalculator,
            IEnumerable<ChannelAdvisorDistributionCenter> distributionCenters,
            Func<Type, ILog> loggerFactory)
        {
            this.orderChargeCalculator = orderChargeCalculator;
            this.distributionCenters = distributionCenters;
            log = loggerFactory(typeof(ChannelAdvisorOrderLoader));
        }

        /// <summary>
        /// Loads the order
        /// </summary>
        /// <remarks>
        /// Order to save must have store loaded
        /// </remarks>
        public void LoadOrder(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder,
            List<ChannelAdvisorProduct> downloadedProducts, IOrderElementFactory orderElementFactory, string siteName)
        {
            MethodConditions.EnsureArgumentIsNotNull(orderToSave.Store, "orderToSave.Store");

            orderToSave.OrderNumber = downloadedOrder.ID;
            orderToSave.OrderDate = downloadedOrder.CreatedDateUtc;
            orderToSave.OnlineLastModified = downloadedOrder.PaymentDateUtc;
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
                LoadItems(orderToSave, downloadedOrder, downloadedProducts, orderElementFactory, siteName);

                // charges
                LoadCharges(orderToSave, downloadedOrder, orderElementFactory);

                // payments
                LoadPayments(orderToSave, downloadedOrder, orderElementFactory);

                SetOrderTotal(orderToSave, downloadedOrder, orderElementFactory);
            }

            if (orderToSave.OnlineLastModified == DateTime.MinValue)
            {
                orderToSave.OnlineLastModified = orderToSave.OrderDate;
            }
        }

        /// <summary>
        /// Set the order total
        /// </summary>
        private void SetOrderTotal(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder, IOrderElementFactory orderElementFactory)
        {
            orderToSave.OrderTotal = downloadedOrder.TotalPrice;

            decimal calculatedTotal = orderChargeCalculator.CalculateTotal(orderToSave);

            // Sometimes ChannelAdvisor doesnt give us all of the discounts for the order, if the order total is different than what we calculate
            // we need to figure out the difference so that the user can see that there was an aditional discount or cost
            if (downloadedOrder.TotalPrice != calculatedTotal)
            {
                decimal adjustment = downloadedOrder.TotalPrice - calculatedTotal;

                log.Info($"Order total for {downloadedOrder.ID} does not match our calculated total, adding an adjustment charge of {adjustment} to compensate for the discrepancy.");

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
        private void LoadItems(ChannelAdvisorOrderEntity orderToSave,
            ChannelAdvisorOrder downloadedOrder,
            List<ChannelAdvisorProduct> downloadedProducts,
            IOrderElementFactory orderElementFactory,
            string siteName)
        {
            if (downloadedOrder.Items != null)
            {
                foreach (ChannelAdvisorOrderItem downloadedItem in downloadedOrder.Items)
                {
                    ChannelAdvisorOrderItemEntity itemToSave =
                        (ChannelAdvisorOrderItemEntity) orderElementFactory.CreateItem(orderToSave);

                    itemToSave.MarketplaceStoreName = siteName;

                    LoadItem(itemToSave, downloadedOrder, downloadedItem, orderElementFactory);

                    ChannelAdvisorProduct downloadedProduct = downloadedProducts.FirstOrDefault(p => p.ID == downloadedItem.ProductID);

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
        private void LoadItem(ChannelAdvisorOrderItemEntity itemToSave,
            ChannelAdvisorOrder downloadedOrder,
            ChannelAdvisorOrderItem downloadedItem,
            IOrderElementFactory orderElementFactory)
        {
            itemToSave.Name = downloadedItem.Title;
            itemToSave.Quantity = downloadedItem.Quantity;
            itemToSave.UnitPrice = downloadedItem.UnitPrice;
            itemToSave.Code = downloadedItem.Sku;
            itemToSave.SKU = downloadedItem.Sku;

            // CA specific
            if (string.IsNullOrWhiteSpace(downloadedItem.SiteOrderItemID))
            {
                itemToSave.MarketplaceSalesID = downloadedItem.SiteListingID;
            }
            else
            {
                itemToSave.MarketplaceSalesID = downloadedItem.SiteOrderItemID;
            }
            
            itemToSave.MarketplaceName = downloadedOrder.SiteName;
            itemToSave.MarketplaceBuyerID = downloadedOrder.BuyerUserID;

            LoadDistributionCenter(itemToSave, downloadedOrder, downloadedItem);

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
        }

        /// <summary>
        /// Loads the distribution center for the given item
        /// </summary>
        private void LoadDistributionCenter(ChannelAdvisorOrderItemEntity itemToSave, ChannelAdvisorOrder downloadedOrder,
            ChannelAdvisorOrderItem downloadedItem)
        {
            ChannelAdvisorFulfillmentItem channelAdvisorFulfillmentItem = downloadedItem.FulfillmentItems.FirstOrDefault();
            if (channelAdvisorFulfillmentItem != null)
            {
                int fulfillmentID = channelAdvisorFulfillmentItem.FulfillmentID;
                ChannelAdvisorFulfillment channelAdvisorFulfillment =
                    downloadedOrder.Fulfillments.FirstOrDefault(f => f.ID == fulfillmentID);
                if (channelAdvisorFulfillment != null)
                {
                    long distributionCenterID = channelAdvisorFulfillment.DistributionCenterID;

                    itemToSave.DistributionCenterID = distributionCenterID;

                    ChannelAdvisorDistributionCenter channelAdvisorDistributionCenter = distributionCenters
                        .SingleOrDefault(d => d.ID == distributionCenterID);
                    if (channelAdvisorDistributionCenter != null)
                    {
                        itemToSave.DistributionCenter = channelAdvisorDistributionCenter.Code;
                        itemToSave.DistributionCenterName = channelAdvisorDistributionCenter.Name;
                        itemToSave.IsFBA = channelAdvisorDistributionCenter.IsExternallyManaged;
                    }
                }
            }
        }

        /// <summary>
        /// Populates the specified order item attributes
        /// </summary>
        private void LoadProductAttributes(ChannelAdvisorOrderItemEntity itemToSave,
            ChannelAdvisorProduct product,
            IOrderElementFactory orderElementFactory)
        {
            IEnumerable<string> attributesToDownload = ((ChannelAdvisorStoreEntity) itemToSave.Order.Store).ParsedAttributesToDownload;

            if (attributesToDownload.Any())
            {
                var attributesToSave = product.Attributes.Where(a => attributesToDownload.Contains(a.Name));
                foreach (ChannelAdvisorProductAttribute attribute in attributesToSave)
                {
                    orderElementFactory.CreateItemAttribute(itemToSave, attribute.Name, attribute.Value, 0, false);
                }
            }
        }

        /// <summary>
        /// Loads the product details.
        /// </summary>
        /// <param name="itemToSave">The item to save.</param>
        /// <param name="product">The product.</param>
        private void LoadProductDetails(ChannelAdvisorOrderItemEntity itemToSave, ChannelAdvisorProduct product)
        {
            // Default unit of measure for US profiles is pounds, kilograms for everything else
            itemToSave.Weight = itemToSave.Order.Store.CountryCode.ToUpperInvariant() == "US" ?
                Convert.ToDouble(product.Weight) :
                Convert.ToDouble(product.Weight.ConvertFromKilogramsToPounds());

            itemToSave.Length = product.Length;
            itemToSave.Width = product.Width;
            itemToSave.Height = product.Height;
            itemToSave.Location = product.WarehouseLocation;
            itemToSave.Classification = product.Classification;
            itemToSave.UnitCost = product.Cost;
            itemToSave.HarmonizedCode = product.HarmonizedCode;
            itemToSave.ISBN = product.ISBN;
            itemToSave.UPC = product.UPC;
            itemToSave.MPN = product.MPN;
            itemToSave.Description = product.Description;

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
            if (downloadedOrder.TotalTaxPrice != 0)
            {
                // Total tax price includes shipping tax and gift wrap tax, so no need to load them
                orderElementFactory.CreateCharge(orderToSave, "TAX", "Sales Tax", downloadedOrder.TotalTaxPrice);
            }

            if (downloadedOrder.TotalShippingPrice != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "SHIPPING", "Shipping", downloadedOrder.TotalShippingPrice);
            }

            if (downloadedOrder.TotalInsurancePrice != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "INSURANCE", "Shipping Insurance", downloadedOrder.TotalInsurancePrice);
            }

            if (downloadedOrder.TotalGiftOptionPrice != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "GIFT WRAP", "Gift Wrap", downloadedOrder.TotalGiftOptionPrice);
            }

            if (downloadedOrder.AdditionalCostOrDiscount != 0)
            {
                orderElementFactory.CreateCharge(orderToSave, "ADDITIONAL COST OR DISCOUNT", "Additional Cost or Discount",
                    downloadedOrder.AdditionalCostOrDiscount);
            }
        }

        /// <summary>
        /// Loads the notes.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        /// <param name="orderElementFactory">The order element factory.</param>
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
            ChannelAdvisorFulfillment fulfillment = downloadedOrder.Fulfillments.FirstOrDefault();
            string carrier = fulfillment?.ShippingCarrier;
            string shippingClass = fulfillment?.ShippingClass;

            if (!string.IsNullOrEmpty(carrier) || !string.IsNullOrEmpty(shippingClass))
            {
                orderToSave.RequestedShipping = $"{carrier} - {shippingClass}";
            }

            orderToSave.IsPrime = (int) ChannelAdvisorHelper.GetIsPrime(fulfillment?.ShippingClass, fulfillment?.ShippingCarrier);
        }

        /// <summary>
        /// Loads the order flag.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private void LoadOrderFlag(ChannelAdvisorOrderEntity orderToSave, ChannelAdvisorOrder downloadedOrder)
        {
            ChannelAdvisorFlagType flag = GetFlag(downloadedOrder);

            orderToSave.FlagStyle = EnumHelper.GetDescription(flag);
            orderToSave.FlagType = (int) flag;
            orderToSave.FlagDescription = downloadedOrder.FlagDescription;
        }

        /// <summary>
        /// Given a ChannelAdvisorOrder, get the flag
        /// </summary>
        private static ChannelAdvisorFlagType GetFlag(ChannelAdvisorOrder downloadedOrder)
        {
            // If an unknown flag is downloaded, use NoFlag
            ChannelAdvisorFlagType flag;
            if (Enum.IsDefined(typeof(ChannelAdvisorFlagType), downloadedOrder.FlagID))
            {
                flag = (ChannelAdvisorFlagType) downloadedOrder.FlagID;
            }
            else
            {
                flag = ChannelAdvisorFlagType.NoFlag;
            }

            return flag;
        }

        /// <summary>
        /// Loads the order statuses.
        /// </summary>
        /// <param name="orderToSave">The order to save.</param>
        /// <param name="downloadedOrder">The downloaded order.</param>
        private static void LoadOrderStatuses(ChannelAdvisorOrderEntity orderToSave,
            ChannelAdvisorOrder downloadedOrder)
        {
            orderToSave.OnlinePaymentStatus =
                (int) (EnumHelper.TryParseEnum<ChannelAdvisorPaymentStatus>(downloadedOrder.PaymentStatus) ??
                       ChannelAdvisorPaymentStatus.Unknown);

            orderToSave.OnlineCheckoutStatus =
                (int) (EnumHelper.TryParseEnum<ChannelAdvisorCheckoutStatus>(downloadedOrder.CheckoutStatus) ??
                       ChannelAdvisorCheckoutStatus.Unknown);

            orderToSave.OnlineShippingStatus =
                (int) (EnumHelper.TryParseEnum<ChannelAdvisorShippingStatus>(downloadedOrder.ShippingStatus) ??
                       ChannelAdvisorShippingStatus.Unknown);
        }
    }
}
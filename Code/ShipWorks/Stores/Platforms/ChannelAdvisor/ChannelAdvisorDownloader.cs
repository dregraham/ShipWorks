﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Constants;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Inventory;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Downloader for ChannelAdvisor
    /// </summary>
    [Component]
    public class ChannelAdvisorDownloader : StoreDownloader, IChannelAdvisorSoapDownloader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorDownloader));
        // total download count
        private int totalCount;

        private List<string> itemAttributesToDownload = new List<string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store"></param>
        public ChannelAdvisorDownloader(StoreEntity store, IStoreTypeManager storeTypeManager)
            : base(store, storeTypeManager.GetType(store))
        {
            XDocument attributesToDownload = XDocument.Parse(ChannelAdvisorStore.AttributesToDownload);

            itemAttributesToDownload.AddRange(attributesToDownload.Descendants("Attribute").Select(a => a.Value.ToUpperInvariant()));

            ItemAttributesEnabled = itemAttributesToDownload.Any();
        }

        /// <summary>
        /// Convenience property for quick access to the specific store entity
        /// </summary>
        private ChannelAdvisorStoreEntity ChannelAdvisorStore
        {
            get { return (ChannelAdvisorStoreEntity) Store; }
        }

        /// <summary>
        /// Property to hold whether or not the user has selected any item attributes to download
        /// </summary>
        private bool ItemAttributesEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Download data
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Checking for orders...";

                ChannelAdvisorSoapClient soapClient = new ChannelAdvisorSoapClient(ChannelAdvisorStore);

                DateTime? lastModified = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);

                // Initialize the download, which will also give us the total count to download
                totalCount = soapClient.InitializeDownload(lastModified);

                // exit if there's nothing to download
                if (totalCount == 0)
                {
                    Progress.Detail = "No orders to download.";
                    Progress.PercentComplete = 100;
                    return;
                }

                Progress.Detail = string.Format("Downloading {0} orders...", totalCount);

                // keep downloading until non are left
                while (true)
                {
                    // check for cancellation
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    bool morePages = await DownloadNextOrdersPage(soapClient).ConfigureAwait(false);
                    if (!morePages)
                    {
                        return;
                    }
                }
            }
            catch (ChannelAdvisorException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download the next page of results from CA
        /// </summary>
        private async Task<bool> DownloadNextOrdersPage(ChannelAdvisorSoapClient soapClient)
        {
            List<OrderResponseDetailComplete> caOrders = soapClient.GetNextOrders();
            if (caOrders.Count == 0)
            {
                Progress.Detail = "Done";
                return false;
            }

            // check for cancellation
            if (Progress.IsCancelRequested)
            {
                return true;
            }

            MarkOrdersAsExported(soapClient, caOrders);

            await LoadOrders(soapClient, caOrders).ConfigureAwait(false);

            return true;
        }

        /// <summary>
        /// Tells Channel Advisor we downloaded the orders.
        /// </summary>
        private void MarkOrdersAsExported(ChannelAdvisorSoapClient soapClient, IEnumerable<OrderResponseDetailComplete> caOrders)
        {
            soapClient.SetOrdersExportStatus(caOrders.Select(x => x.ClientOrderIdentifier));
        }

        /// <summary>
        /// Loads the orders into ShipWorks database.
        /// </summary>
        /// <param name="soapClient">The soapSoapClient.</param>
        /// <param name="caOrders">The ca orders.</param>
        private async Task LoadOrders(ChannelAdvisorSoapClient soapClient, List<OrderResponseDetailComplete> caOrders)
        {
            foreach (OrderResponseDetailComplete caOrder in caOrders)
            {
                // check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                Progress.Detail = String.Format("Processing order {0}...", (QuantitySaved + 1));
                await LoadOrder(soapClient, caOrder).ConfigureAwait(false);

                // update the status, 100 max
                Progress.PercentComplete = Math.Min(100 * QuantitySaved / totalCount, 100);
            }
        }

        /// <summary>
        /// Load order data from the CA response
        /// </summary>
        private async Task LoadOrder(ChannelAdvisorSoapClient soapClient, OrderResponseDetailComplete caOrder)
        {
            int orderNumber = caOrder.OrderID;

            // get the order instance
            GenericResult<OrderEntity> result = await InstantiateOrder(new OrderNumberIdentifier(orderNumber)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", orderNumber, result.Message);
                return;
            }

            ChannelAdvisorOrderEntity order = (ChannelAdvisorOrderEntity) result.Value;

            order.OrderDate = caOrder.OrderTimeGMT.Value;
            order.OnlineLastModified = caOrder.LastUpdateDate ?? order.OrderDate;

            // custom order identifier
            order.CustomOrderIdentifier = caOrder.ClientOrderIdentifier;

            // no customer id available to us
            order.OnlineCustomerID = null;

            // statuses
            order.OnlinePaymentStatus = (int) ChannelAdvisorHelper.GetShipWorksPaymentStatus(caOrder.OrderStatus.PaymentStatus);
            order.OnlineCheckoutStatus = (int) ChannelAdvisorHelper.GetShipWorksCheckoutStatus(caOrder.OrderStatus.CheckoutStatus);
            order.OnlineShippingStatus = (int) ChannelAdvisorHelper.GetShipWorksShippingStatus(caOrder.OrderStatus.ShippingStatus);

            // flags
            SetChannelAdvisorOrderFlags(order, caOrder);

            // Load address info
            LoadAddresses(order, caOrder);

            // For new orders - or if the requested shipping is not yet filled out
            if (order.IsNew || string.IsNullOrEmpty(order.RequestedShipping))
            {
                SetPrimeAndRequestedShipping(caOrder, order);
            }

            // only do the remainder for new orders
            if (order.IsNew)
            {
                await LoadOrderDetailsWhenNew(soapClient, caOrder, order).ConfigureAwait(false);
            }

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "ChannelAdvisorDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Load order details when the order is new
        /// </summary>
        private async Task LoadOrderDetailsWhenNew(ChannelAdvisorSoapClient soapClient, OrderResponseDetailComplete caOrder, ChannelAdvisorOrderEntity order)
        {
            order.ResellerID = caOrder.ResellerID;

            // Find the unique sale sources and combine them into a comma separated list
            IEnumerable<string> marketplaces = caOrder.ShoppingCart.LineItemSKUList.Select(item => item.ItemSaleSource).Distinct().OrderBy(source => source);
            order.MarketplaceNames = string.Join(", ", marketplaces);

            await LoadNotes(order, caOrder).ConfigureAwait(false);

            // items
            LoadItems(soapClient, order, caOrder);

            // charges
            LoadCharges(order, caOrder);

            // payments
            LoadPayments(order, caOrder);

            // Update the total
            order.OrderTotal = OrderUtility.CalculateTotal(order);
        }

        /// <summary>
        /// Sets Prime and Requested Shipping on the order using the caOrder
        /// </summary>
        private static void SetPrimeAndRequestedShipping(OrderResponseDetailComplete caOrder, ChannelAdvisorOrderEntity order)
        {
            // shipping
            if (caOrder.ShippingInfo != null && caOrder.ShippingInfo.ShipmentList.Length > 0)
            {
                string carrier = caOrder.ShippingInfo.ShipmentList[0].ShippingCarrier;
                string shippingClass = caOrder.ShippingInfo.ShipmentList[0].ShippingClass;

                if (!string.IsNullOrEmpty(carrier) || !string.IsNullOrEmpty(shippingClass))
                {
                    order.RequestedShipping = $"{carrier} - {shippingClass}";
                }

                order.IsPrime = (int) ChannelAdvisorHelper.GetIsPrime(shippingClass, carrier);
            }
        }

        /// <summary>
        /// Sets the ChannelAdvisor order flags.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="caOrder">The ca order.</param>
        private static void SetChannelAdvisorOrderFlags(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            order.FlagStyle = caOrder.FlagStyle;
            order.FlagDescription = caOrder.FlagDescription;

            // When using the Enum.TryParse method, if an new/unknown flag type is encountered,
            // the NoFlag value will be assigned to the output parameter, so there's not really
            // any reason to inspect the results of the TryParse here.
            ChannelAdvisorFlagType flagType = ChannelAdvisorFlagType.NoFlag;
            Enum.TryParse(caOrder.FlagStyle, true, out flagType);
            order.FlagType = (int) flagType;
        }

        /// <summary>
        /// Creates a new payment detail for the order
        /// </summary>
        private void LoadPaymentDetail(OrderEntity order, string label, string value)
        {
            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = label;
            detail.Value = value;
        }

        /// <summary>
        /// Imports payment information for the order
        /// </summary>
        private void LoadPayments(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            LoadPaymentDetail(order, "Payment Type", caOrder.PaymentInfo.PaymentType);
            LoadPaymentDetail(order, "Card Number", caOrder.PaymentInfo.CreditCardLast4);
            LoadPaymentDetail(order, "Reference", caOrder.PaymentInfo.MerchantReferenceNumber);
            LoadPaymentDetail(order, "TransactionID", caOrder.PaymentInfo.PaymentTransactionID);
        }

        /// <summary>
        /// Import order charges from CA shopping cart
        /// </summary>
        private void LoadCharges(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            foreach (OrderLineItemInvoice invoice in caOrder.ShoppingCart.LineItemInvoiceList)
            {
                string type = "";
                string name = "";

                switch (invoice.LineItemType)
                {
                    case LineItemTypeCodes.Listing:
                        name = "Listing";
                        type = "FEE";
                        break;

                    case LineItemTypeCodes.SalesTax:
                        name = "Sales Tax";
                        type = "TAX";
                        break;

                    case LineItemTypeCodes.Shipping:
                        name = "Shipping";
                        type = "SHIPPING";
                        break;

                    case LineItemTypeCodes.ShippingInsurance:
                        name = "Shipping Insurance";
                        type = "INSURANCE";
                        break;

                    case LineItemTypeCodes.VATShipping:
                        name = "VAT Shipping";
                        type = "VAT";
                        // Skip pulling in 'VAT Shipping'
                        // it is included in the tax
                        // see FreshDesk 593454
                        continue;

                    default:
                        // there are some undocumented enum values
                        name = invoice.LineItemType;
                        type = name.ToUpperInvariant();
                        break;
                }

                // don't bother if there isn't an amount
                if (invoice.UnitPrice != 0.0M)
                {
                    LoadCharge(order, type, name, invoice.UnitPrice);
                }
            }

            ExtractDiscountsAndPromotions(order, caOrder);
        }

        /// <summary>
        /// Extract discounts and promotions
        /// </summary>
        private void ExtractDiscountsAndPromotions(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            // There are two different types of promotions in v6 of the CA API - promotions at
            // the individual item level and those at the order level. The item level promotions
            // were added in v4.

            // We're going to extract the item-level discounts/promotions from the line items in the order first
            List<OrderLineItemItem> lineItems = caOrder.ShoppingCart.LineItemSKUList.ToList();
            List<OrderLineItemItemPromo> lineItemPromos = lineItems.Where(line => line.ItemPromoList != null).SelectMany(line => line.ItemPromoList).ToList();

            foreach (OrderLineItemItemPromo lineItemPromo in lineItemPromos)
            {
                // Load the item-level discounts/promotions on the unit price and the shipping price
                LoadPromotion(order, lineItemPromo.PromoCode, lineItemPromo.UnitPrice);
                LoadPromotion(order, lineItemPromo.PromoCode + " - Shipping", lineItemPromo.ShippingPrice);
            }

            // Now load order-level discounts/promotions
            foreach (OrderLineItemPromo promo in caOrder.ShoppingCart.LineItemPromoList)
            {
                LoadPromotion(order, promo.PromoCode, promo.UnitPrice);
            }
        }

        /// <summary>
        /// Loads the promotion.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="promoCode">The promo code.</param>
        /// <param name="amount">The amount.</param>
        private void LoadPromotion(ChannelAdvisorOrderEntity order, string promoCode, decimal amount)
        {
            // discounts need to have a negative value in ShipWorks
            if (amount > 0)
            {
                amount *= -1;
            }

            // only save charges if they have a value
            if (amount != 0.0M)
            {
                LoadCharge(order, "PROMO", string.Format("Promo ({0})", promoCode), amount);
            }
        }

        /// <summary>
        /// Creates and populates an order charge
        /// </summary>
        private void LoadCharge(OrderEntity order, string type, string description, decimal amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type;
            charge.Description = description;
            charge.Amount = amount;
        }

        /// <summary>
        /// Imports order items from the CA shopping cart
        /// </summary>
        private void LoadItems(ChannelAdvisorSoapClient soapClient, ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            foreach (OrderLineItemItemResponse caItem in caOrder.ShoppingCart.LineItemSKUList)
            {
                LoadItemDetails(order, caItem);
            }

            // Some data comes from a  CA Inventory service call
            List<string> skuList = caOrder.ShoppingCart.LineItemSKUList.Select(i => i.SKU).ToList();
            InventoryItemResponse[] inventoryItems = soapClient.GetInventoryItems(skuList);

            if (inventoryItems != null)
            {
                foreach (ChannelAdvisorOrderItemEntity orderItem in order.OrderItems.Where(item => item is ChannelAdvisorOrderItemEntity))
                {
                    LoadItemInventoryDetails(soapClient, inventoryItems, orderItem);
                }
            }
        }

        /// <summary>
        /// Load item inventory details
        /// </summary>
        private void LoadItemInventoryDetails(ChannelAdvisorSoapClient soapClient, InventoryItemResponse[] inventoryItems, ChannelAdvisorOrderItemEntity orderItem)
        {
            // find the response for this item's sku
            InventoryItemResponse matchingItem = inventoryItems
                .Where(x => x?.Sku != null)
                .FirstOrDefault(response => String.Compare(response.Sku, orderItem.SKU, StringComparison.OrdinalIgnoreCase) == 0);

            if (matchingItem == null)
            {
                return;
            }

            PopulateItemInventoryDetails(orderItem, matchingItem);
            PopulateItemAttributes(soapClient, orderItem);
            PopulateImages(soapClient, orderItem);
        }

        /// <summary>
        /// Populate the item inventory details
        /// </summary>
        private static void PopulateItemInventoryDetails(ChannelAdvisorOrderItemEntity orderItem, InventoryItemResponse matchingItem)
        {
            if (matchingItem.ShippingInfo != null)
            {
                orderItem.DistributionCenter = matchingItem.ShippingInfo.DistributionCenterCode ?? "";
            }

            if (!String.IsNullOrEmpty(matchingItem.Classification))
            {
                orderItem.Classification = matchingItem.Classification;
            }

            // pull out the item cost
            if (matchingItem.PriceInfo?.Cost != null)
            {
                orderItem.UnitCost = matchingItem.PriceInfo.Cost.Value;
            }

            // Harmonized Code
            if (!String.IsNullOrEmpty(matchingItem.HarmonizedCode))
            {
                orderItem.HarmonizedCode = matchingItem.HarmonizedCode;
            }

            // ISBN
            if (!String.IsNullOrEmpty(matchingItem.ISBN))
            {
                orderItem.ISBN = matchingItem.ISBN;
            }

            // UPC
            if (!String.IsNullOrEmpty(matchingItem.UPC))
            {
                orderItem.UPC = matchingItem.UPC;
            }

            // MPN
            if (!String.IsNullOrEmpty(matchingItem.MPN))
            {
                orderItem.MPN = matchingItem.MPN;
            }

            orderItem.Length = matchingItem.Length ?? 0;
            orderItem.Width = matchingItem.Width ?? 0;
            orderItem.Height = matchingItem.Height ?? 0;
        }

        /// <summary>
        /// Load item details
        /// </summary>
        private void LoadItemDetails(ChannelAdvisorOrderEntity order, OrderLineItemItemResponse caItem)
        {
            ChannelAdvisorOrderItemEntity item = (ChannelAdvisorOrderItemEntity) InstantiateOrderItem(order);

            item.Name = caItem.Title;
            item.Quantity = caItem.Quantity;
            item.UnitPrice = caItem.UnitPrice;
            item.Code = caItem.SKU;
            item.SKU = caItem.SKU;
            item.Weight = caItem.UnitWeight.Value;
            item.Location = caItem.WarehouseLocation;
            item.IsFBA = caItem.IsFBA;

            // Convert KG to LBS if needed
            if (caItem.UnitWeight.UnitOfMeasure == "KG")
            {
                item.Weight = item.Weight * 2.20462262;
            }

            // CA-specific
            item.MarketplaceName = caItem.ItemSaleSource;
            item.MarketplaceStoreName = !string.IsNullOrWhiteSpace(caItem.UserName) ? caItem.UserName : string.Empty;
            item.MarketplaceBuyerID = caItem.BuyerUserID;
            item.MarketplaceSalesID = caItem.SalesSourceID;

            if (!String.IsNullOrEmpty(caItem.GiftWrapLevel))
            {
                // add an attribute for gift wrap level
                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                attribute.Name = "Gift Wrap Level";
                attribute.Description = caItem.GiftWrapLevel;

                // gift wrap cost is already included as a Charge
                attribute.UnitPrice = 0M;
            }

            if (!String.IsNullOrEmpty(caItem.GiftMessage))
            {
                // add an attribute for gift message
                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);
                attribute.Name = "Gift Message";
                attribute.Description = caItem.GiftMessage;
                attribute.UnitPrice = 0M;
            }
        }

        /// <summary>
        /// Retrieves the attribute information from ChannelAdvisor and populates the order item attributes appropriately.
        /// </summary>
        private void PopulateItemAttributes(ChannelAdvisorSoapClient soapSoapClient, ChannelAdvisorOrderItemEntity orderItem)
        {
            // Only add item attributes if the user has specified ones they want to download.
            if (ItemAttributesEnabled)
            {
                IEnumerable<AttributeInfo> attributes = soapSoapClient.GetInventoryItemAttributes(orderItem.SKU);

                // ItemAttributesEnabled is true so we have at least one attribute to download.
                // Filter out all the others that don't match.
                attributes = attributes.Where(a => itemAttributesToDownload.Contains(a.Name.ToUpperInvariant()));

                foreach (AttributeInfo caAttribute in attributes)
                {
                    OrderItemAttributeEntity orderItemAttribute = StoreType.CreateOrderItemAttributeInstance();
                    orderItemAttribute.Name = caAttribute.Name ?? string.Empty;
                    orderItemAttribute.Description = caAttribute.Value ?? string.Empty;
                    orderItemAttribute.UnitPrice = 0;
                    orderItemAttribute.IsManual = false;

                    orderItem.OrderItemAttributes.Add(orderItemAttribute);
                }
            }
        }

        /// <summary>
        /// Retrieves Image information from ChannelAdvisor and populates the OrderItem appropriately
        /// </summary>
        private static void PopulateImages(ChannelAdvisorSoapClient soapClient, ChannelAdvisorOrderItemEntity orderItem)
        {
            ImageInfoResponse[] itemImages = soapClient.GetItemImages(orderItem.SKU);
            if (itemImages != null && itemImages.Length > 0)
            {
                ImageInfoResponse image = itemImages[0];

                // set the SW item image
                orderItem.Image = image.Url;

                // try to find the first thumbnail
                if (image.ImageThumbList != null && image.ImageThumbList.Length > 0)
                {
                    orderItem.Thumbnail = image.ImageThumbList[0].Url;
                }

                // if no thumbnail yet, just copy the image url
                if (String.IsNullOrEmpty(orderItem.Thumbnail))
                {
                    orderItem.Thumbnail = orderItem.Image;
                }
            }
        }

        /// <summary>
        /// Loads any notes from the CA order
        /// </summary>
        private async Task LoadNotes(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            if (caOrder.ShippingInfo != null && caOrder.ShippingInfo.ShippingInstructions.Length > 0)
            {
                await InstantiateNote(order, caOrder.ShippingInfo.ShippingInstructions, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            if (!string.IsNullOrEmpty(caOrder.TransactionNotes))
            {
                await InstantiateNote(order, caOrder.TransactionNotes, order.OrderDate, NoteVisibility.Internal).ConfigureAwait(false);
            }

            // A gift message can be associated with each item in the order, so we need to find any items containing
            // gift messages and add each message as a note
            List<OrderLineItemItem> giftItems = caOrder.ShoppingCart.LineItemSKUList.Select(item => item).Where(i => i.GiftMessage.Length > 0).ToList();
            foreach (OrderLineItemItem item in giftItems)
            {
                string giftMessage = string.Format("Gift message for {0}: {1}", item.Title, item.GiftMessage);
                await InstantiateNote(order, giftMessage, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Addresses
        /// </summary>
        private void LoadAddresses(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            // shipping address
            PopulateOrderAddress(order.ShipPerson, caOrder.ShippingInfo);

            // In ChannelAdvsior if the buyer selected Use Shipping as Billing during checkout,
            // the values get copied to billing, but that data doesn't come down in Billing.  It's all blank,
            // so we have to copy the values here.
            BillingInfo billing = caOrder.BillingInfo;
            if (IsAddressConsideredEmpty(billing))
            {
                order.ShipPerson.CopyTo(order.BillPerson);
            }
            else
            {
                PopulateOrderAddress(order.BillPerson, billing);
            }

            // email address
            order.BillEmail = caOrder.BuyerEmailAddress;

            // If ship and bill are the same, copy the email to the ship
            if (order.ShipLastName == order.BillLastName &&
                order.ShipFirstName == order.BillFirstName &&
                order.ShipCity == order.BillCity &&
                order.ShipStreet1 == order.BillStreet1)
            {
                order.ShipEmail = order.BillEmail;
            }
        }

        /// <summary>
        /// Is the address considered empty
        /// </summary>
        private static bool IsAddressConsideredEmpty(BillingInfo billing)
        {
            return billing.FirstName.Length == 0 &&
                billing.LastName.Length == 0 &&
                billing.AddressLine1.Length == 0 &&
                billing.City.Length == 0;
        }

        /// <summary>
        /// Populate the order address from the given info
        /// </summary>
        private static void PopulateOrderAddress(PersonAdapter person, ContactComplete address)
        {
            person.NameParseStatus = PersonNameParseStatus.Simple;
            person.FirstName = address.FirstName;
            person.LastName = address.LastName;
            person.Company = address.CompanyName;
            person.Street1 = address.AddressLine1;
            person.Street2 = address.AddressLine2;
            person.City = address.City;
            person.StateProvCode = ChannelAdvisorHelper.GetStateProvCode(address.Region);
            person.PostalCode = address.PostalCode;
            person.CountryCode = ChannelAdvisorHelper.GetCountryCode(address.CountryCode.Trim());
            person.Phone = address.PhoneNumberDay;
        }
    }
}
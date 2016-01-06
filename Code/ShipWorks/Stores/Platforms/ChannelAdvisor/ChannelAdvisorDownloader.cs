using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Constants;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Inventory;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using ShippingInfo = ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order.ShippingInfo;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Downloader for ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorDownloader : StoreDownloader
    {
        // total download count
        private int totalCount;

        private List<string> itemAttributesToDownload = new List<string>(); 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store"></param>
        public ChannelAdvisorDownloader(StoreEntity store)
            : base(store)
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
            get { return (ChannelAdvisorStoreEntity)Store; }
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
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Checking for orders...";

                ChannelAdvisorClient client = new ChannelAdvisorClient(ChannelAdvisorStore);

                DateTime? lastModified = GetOnlineLastModifiedStartingPoint();

                // Initialize the download, which will also give us the total count to download
                totalCount = client.InitializeDownload(lastModified);

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

                    if (!DownloadNextOrdersPage(client))
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
        private bool DownloadNextOrdersPage(ChannelAdvisorClient client)
        {
            List<OrderResponseDetailComplete> caOrders = client.GetNextOrders();
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

            MarkOrdersAsExported(client, caOrders);

            LoadOrders(client, caOrders);

            return true;
        }

        /// <summary>
        /// Tells Channel Advisor we downloaded the orders.
        /// </summary>
        private void MarkOrdersAsExported(ChannelAdvisorClient client, IEnumerable<OrderResponseDetailComplete> caOrders)
        {
            client.SetOrdersExportStatus(caOrders.Select(x => x.ClientOrderIdentifier));
        }

        /// <summary>
        /// Loads the orders into ShipWorks database.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="caOrders">The ca orders.</param>
        private void LoadOrders(ChannelAdvisorClient client, List<OrderResponseDetailComplete> caOrders)
        {
            foreach (OrderResponseDetailComplete caOrder in caOrders)
            {
                // check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                Progress.Detail = String.Format("Processing order {0}...", (QuantitySaved + 1));
                LoadOrder(client, caOrder);

                // update the status, 100 max
                Progress.PercentComplete = Math.Min(100*QuantitySaved/totalCount, 100);
            }
        }

        /// <summary>
        /// Load order data from the CA response
        /// </summary>
        private void LoadOrder(ChannelAdvisorClient client, OrderResponseDetailComplete caOrder)
        {
            int orderNumber = caOrder.OrderID;

            // get the order instance
            ChannelAdvisorOrderEntity order = (ChannelAdvisorOrderEntity)InstantiateOrder(new OrderNumberIdentifier(orderNumber));

            order.OrderDate = caOrder.OrderTimeGMT.Value;
            order.OnlineLastModified = caOrder.LastUpdateDate ?? order.OrderDate;

            // custom order identifier
            order.CustomOrderIdentifier = caOrder.ClientOrderIdentifier;

            // no customer id available to us
            order.OnlineCustomerID = null;

            // statuses
            order.OnlinePaymentStatus = (int)ChannelAdvisorHelper.GetShipWorksPaymentStatus(caOrder.OrderStatus.PaymentStatus);
            order.OnlineCheckoutStatus = (int)ChannelAdvisorHelper.GetShipWorksCheckoutStatus(caOrder.OrderStatus.CheckoutStatus);
            order.OnlineShippingStatus = (int)ChannelAdvisorHelper.GetShipWorksShippingStatus(caOrder.OrderStatus.ShippingStatus);

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
                order.ResellerID = caOrder.ResellerID;

                // Find the unique sale sources and combine them into a comma separated list
                IEnumerable<string> marketplaces = caOrder.ShoppingCart.LineItemSKUList.Select(item => item.ItemSaleSource).Distinct().OrderBy(source => source);
                order.MarketplaceNames = string.Join(", ", marketplaces);

                LoadNotes(order, caOrder);

                // items
                LoadItems(client, order, caOrder);

                // charges
                LoadCharges(order, caOrder);

                // payments
                LoadPayments(order, caOrder);

                // Update the total
                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "ChannelAdvisorDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
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

                order.IsPrime = (int)GetIsPrime(shippingClass);
            }
        }

        /// <summary>
        /// Gets the prime status based on the shippingClass
        /// </summary>
        public static ChannelAdvisorIsAmazonPrime GetIsPrime(string shippingClass)
        {
            if (string.IsNullOrEmpty(shippingClass))
            {
                return ChannelAdvisorIsAmazonPrime.Unknown;
            }

            if (shippingClass.IndexOf("Amazon", StringComparison.OrdinalIgnoreCase) >= 0 &&
                shippingClass.IndexOf("Prime", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ChannelAdvisorIsAmazonPrime.Yes;
            }
            return ChannelAdvisorIsAmazonPrime.No;
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
            order.FlagType = (int)flagType;
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
        [NDependIgnoreLongMethod]
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
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethod]
        private void LoadItems(ChannelAdvisorClient client, ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            foreach (OrderLineItemItemResponse caItem in caOrder.ShoppingCart.LineItemSKUList)
            {
                ChannelAdvisorOrderItemEntity item = (ChannelAdvisorOrderItemEntity)InstantiateOrderItem(order);

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
                    item.Weight = item.Weight*2.20462262;
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

            // Some data comes from a  CA Inventory service call
            List<string> skuList = caOrder.ShoppingCart.LineItemSKUList.Select(i => i.SKU).ToList();
            InventoryItemResponse[] inventoryItems = client.GetInventoryItems(skuList);
            if (inventoryItems != null)
            {
                foreach (ChannelAdvisorOrderItemEntity orderItem in order.OrderItems.Where(item => item is ChannelAdvisorOrderItemEntity))
                {
                    // find the response for this item's sku
                    InventoryItemResponse matchingItem = inventoryItems.FirstOrDefault(response =>
                                                                                       response != null &&
                                                                                       response.Sku != null &&
                                                                                       String.Compare(response.Sku, orderItem.SKU, StringComparison.OrdinalIgnoreCase) == 0);

                    if (matchingItem != null)
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
                        if (matchingItem.PriceInfo != null && matchingItem.PriceInfo.Cost.HasValue)
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
                        
                        PopulateItemAttributes(client, orderItem);
                        PopulateImages(client, orderItem);
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the attribute information from ChannelAdvisor and populates the order item attributes appropriately.
        /// </summary>
        private void PopulateItemAttributes(ChannelAdvisorClient client, ChannelAdvisorOrderItemEntity orderItem)
        {
            // Only add item attributes if the user has specified ones they want to download.
            if (ItemAttributesEnabled)
            {
                IEnumerable<AttributeInfo> attributes = client.GetInventoryItemAttributes(orderItem.SKU);

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
        private static void PopulateImages(ChannelAdvisorClient client, ChannelAdvisorOrderItemEntity orderItem)
        {
            ImageInfoResponse[] itemImages = client.GetItemImages(orderItem.SKU);
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
        private void LoadNotes(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            if (caOrder.ShippingInfo != null && caOrder.ShippingInfo.ShippingInstructions.Length > 0)
            {
                InstantiateNote(order, caOrder.ShippingInfo.ShippingInstructions, order.OrderDate, NoteVisibility.Public);
            }

            if (caOrder.TransactionNotes != null && caOrder.TransactionNotes.Length > 0)
            {
                InstantiateNote(order, caOrder.TransactionNotes, order.OrderDate, NoteVisibility.Internal);
            }

            // A gift message can be associated with each item in the order, so we need to find any items containing
            // gift messages and add each message as a note
            List<OrderLineItemItem> giftItems = caOrder.ShoppingCart.LineItemSKUList.Select(item => item).Where(i => i.GiftMessage.Length > 0).ToList();
            foreach (OrderLineItemItem item in giftItems)
            {
                string giftMessage = string.Format("Gift messsage for {0}: {1}", item.Title, item.GiftMessage);
                InstantiateNote(order, giftMessage, order.OrderDate, NoteVisibility.Public);
            }
        }

        /// <summary>
        /// Determine the state/provice based on the region from CA.  
        /// </summary>
        private static string GetStateProvCode(string region)
        {
            // CA will send 001 if they don't know what to do with the region.  
            // So we'll just return ""
            if (region == "001")
            {
                return string.Empty;
            }
            
            return Geography.GetStateProvCode(region);   
        }

        /// <summary>
        /// Addresses
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadAddresses(ChannelAdvisorOrderEntity order, OrderResponseDetailComplete caOrder)
        {
            // shipping address
            ShippingInfo shipping = caOrder.ShippingInfo;
            order.ShipNameParseStatus = (int)PersonNameParseStatus.Simple;
            order.ShipFirstName = shipping.FirstName;
            order.ShipLastName = shipping.LastName;
            order.ShipCompany = shipping.CompanyName;
            order.ShipStreet1 = shipping.AddressLine1;
            order.ShipStreet2 = shipping.AddressLine2;
            order.ShipCity = shipping.City;
            order.ShipStateProvCode = GetStateProvCode(shipping.Region);
            order.ShipPostalCode = shipping.PostalCode;
            order.ShipCountryCode = shipping.CountryCode.Trim();
            order.ShipPhone = shipping.PhoneNumberDay;

            // In ChannelAdvsior if the buyer selected Use Shipping as Billing during checkout,
            // the values get copied to billing, but that data doesn't come down in Billing.  It's all blank,
            // so we have to copy the values here.
            BillingInfo billing = caOrder.BillingInfo;
            if (billing.FirstName.Length == 0
                && billing.LastName.Length == 0
                && billing.AddressLine1.Length == 0
                && billing.City.Length == 0)
            {
                // copy shipping to billing
                PersonAdapter shipAdapter = new PersonAdapter(order, "Ship");
                PersonAdapter billAdapter = new PersonAdapter(order, "Bill");
                PersonAdapter.Copy(shipAdapter, billAdapter);
            }
            else
            {
                order.BillNameParseStatus = (int)PersonNameParseStatus.Simple;
                order.BillFirstName = billing.FirstName;
                order.BillLastName = billing.LastName;
                order.BillCompany = billing.CompanyName;
                order.BillStreet1 = billing.AddressLine1;
                order.BillStreet2 = billing.AddressLine2;
                order.BillCity = billing.City;
                order.BillStateProvCode = GetStateProvCode(billing.Region);
                order.BillPostalCode = billing.PostalCode;
                order.BillCountryCode = billing.CountryCode.Trim();
                order.BillPhone = billing.PhoneNumberDay;
            }

            // in some cases, we've seen CA provide invalid data here
            if (string.CompareOrdinal(order.BillCountryCode, "-1") == 0)
            {
                order.BillCountryCode = "";
            }

            if (string.CompareOrdinal(order.ShipCountryCode, "-1") == 0)
            {
                order.ShipCountryCode = "";
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
    }
}
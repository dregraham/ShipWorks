﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common.Logging;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.PayPal.WebServices;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Downloader for eBay
    /// </summary>
    public class EbayDownloader : StoreDownloader
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(EbayDownloader));

        // The current time according to eBay
        DateTime eBayOfficialTime;

        // WebClient to use for connetivity
        EbayWebClient webClient;

        // Total number of ordres expected during this download
        int expectedCount = -1;

        /// <summary>
        /// Create the new eBay downloader
        /// </summary>
        public EbayDownloader(StoreEntity store)
            : base(store)
        {
            webClient = new EbayWebClient(EbayToken.FromStore((EbayStoreEntity) store));
        }

        /// <summary>
        /// Begin the order download process
        /// </summary>
        protected override void Download()
        {
            try
            {
                Progress.Detail = "Connecting to eBay...";

                // Get the official eBay time in UTC
                eBayOfficialTime = webClient.GetOfficialTime();

                if (!DownloadOrders())
                {
                    return;
                }

                if (!DownloadFeedback())
                {
                    return;
                }

                // Done
                Progress.Detail = "Done";
                Progress.PercentComplete = 100;
            }
            catch (EbayException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (PayPalException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        #region Orders

        /// <summary>
        /// Download all orders from eBay
        /// </summary>
        private bool DownloadOrders()
        {
            // Controls wether we download using eBay paging, or using our typical sliding method where we always just adjust the start time and ask for page 1.
            bool usePagedDownload = true;

            // Get the date\time to start downloading from
            DateTime rangeStart = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-7);
            DateTime rangeEnd = eBayOfficialTime.AddMinutes(-5);

            // Ebay only allows going back 30 days
            if (rangeStart < eBayOfficialTime.AddDays(-30))
            {
                rangeStart = eBayOfficialTime.AddDays(-30).AddMinutes(5);
            }

            int page = 1;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                GetOrdersResponseType response = webClient.GetOrders(rangeStart, rangeEnd, page);

                // Grab the total expected account from the first page
                if (expectedCount < 0)
                {
                    expectedCount = response.PaginationResult.TotalNumberOfEntries;
                }

                // Process all of the downloaded orders
                foreach (OrderType orderType in response.OrderArray)
                {
                    // Check for user cancel
                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }

                    DateTime lastModified = orderType.CheckoutStatus.LastModifiedTime;
                    
                    // Skip any that are out of range.  We take any that are a day out of range back, b\c eBay's API sometimes returns stuff in the next page that should have been on the previous.
                    // I have open bug reports with them.  12/18/13
                    if (!usePagedDownload)
                    {
                        if (lastModified < rangeStart.AddDays(-1) || lastModified > rangeEnd)
                        {
                            log.WarnFormat("Skipping eBay order {0} since it's out of our date range {1}", orderType.OrderID, orderType.CheckoutStatus.LastModifiedTime);
                            continue;
                        }
                    }

                    ProcessOrder(orderType);

                    Progress.Detail = string.Format("Processing order {0} of {1}...", QuantitySaved, expectedCount);
                    Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);

                    // Update the range for the next time around.  Should always be ascending
                    if (!usePagedDownload)
                    {
                        if (lastModified > rangeStart)
                        {
                            rangeStart = lastModified;
                        }
                    }
                }

                // Quit if eBay says there aren't any more
                if (!response.HasMoreOrders)
                {
                    return true;
                }

                // Increment the page, if that's the method we are using
                if (usePagedDownload)
                {
                    page++;
                }
            }
        }

        /// <summary>
        /// Process the given eBay order
        /// </summary>
        private void ProcessOrder(OrderType orderType)
        {
            // Get the ShipWorks order.  This ends up calling our overriden FindOrder implementation
            EbayOrderEntity order = (EbayOrderEntity) InstantiateOrder(new EbayOrderIdentifier(orderType.OrderID));

            // Special processing for cancelled orders. If we'd never seen it before, there's no reason to do anything - just ignore it.
            if (orderType.OrderStatus == OrderStatusCodeType.Cancelled && order.IsNew)
            {
                log.WarnFormat("Skipping eBay order {0} due to we've never seen it and it's cancelled.", orderType.OrderID);
                return;
            }

            // If its new it needs a ShipWorks order number
            if (order.IsNew)
            {
                order.OrderNumber = GetNextOrderNumber();

                // We use the oldest auction date as the order date
                order.OrderDate = DetermineOrderDate(orderType);
            }

            // Update last modified
            order.OnlineLastModified = orderType.CheckoutStatus.LastModifiedTime;

            // Online status
            order.OnlineStatusCode = (int) orderType.OrderStatus;
            order.OnlineStatus = EbayUtility.GetOrderStatusName(orderType.OrderStatus);

            // SellingManager Pro
            order.SellingManagerRecord = orderType.ShippingDetails.SellingManagerSalesRecordNumberSpecified ? orderType.ShippingDetails.SellingManagerSalesRecordNumber : (int?) null;

            // Buyer , email, and address
            order.EbayBuyerID = orderType.BuyerUserID;
            order.ShipEmail = order.BillEmail = DetermineBuyerEmail(orderType);
            UpdateOrderAddress(order, orderType.ShippingAddress);

            // Requested shipping (but only if we actually have an address)
            if (!string.IsNullOrWhiteSpace(order.ShipLastName) || !string.IsNullOrWhiteSpace(order.ShipCity) || !string.IsNullOrWhiteSpace(order.ShipCountryCode))
            {
                order.RequestedShipping = EbayUtility.GetShipmentMethodName(orderType.ShippingServiceSelected.ShippingService);
            }
            else
            {
                order.RequestedShipping = "";
            }

            // Load all the transactions (line items) for the order
            List<OrderItemEntity> abandonedItems = LoadTransactions(order, orderType);

            // Update PayPal information
            UpdatePayPal(order, orderType);

            // Charges
            UpdateCharges(order, orderType);

            // Notes
            UpdateNotes(order, orderType);

            // Make sure we have the latest GSP data
            UpdateGlobalShippingProgramInfo(order, orderType.IsMultiLegShipping, orderType.MultiLegShippingDetails);

            // If all items are shipped, and the local status isn't set yet, set it to shipped
            if (string.IsNullOrWhiteSpace(order.LocalStatus) && order.OrderItems.OfType<EbayOrderItemEntity>().All(item => item.MyEbayShipped))
            {
                order.LocalStatus = "Shipped";
            }

            // Make totals adjustments
            double amount = orderType.AmountPaid != null ? orderType.AmountPaid.Value : orderType.Total.Value;
            BalanceOrderTotal(order, amount);

            SaveOrder(order, abandonedItems);
        }

        /// <summary>
        /// Save the given order, handling all the given abandoned items that have now moved to the new order
        /// </summary>
        private void SaveOrder(EbayOrderEntity order, List<OrderItemEntity> abandonedItems)
        {
            List<OrderEntity> affectedOrders = new List<OrderEntity>();

            // Build a distinct list of all the orders affected by the abandoned items
            foreach (long orderID in abandonedItems.Select(i => i.OrderID).Distinct())
            {
                // Load the order and all of it's items (which, will duplicate any abandoned item entities)
                OrderEntity affectedOrder = (OrderEntity) DataProvider.GetEntity(orderID);
                affectedOrder.OrderItems.AddRange(DataProvider.GetRelatedEntities(orderID, EntityType.OrderItemEntity).Cast<OrderItemEntity>());

                affectedOrders.Add(affectedOrder);
            }

            // We have to use the exact scope that SaveDownloadedOrder will, or a MSDTC exception will be thrown since the connection would be slightly different
            using (AuditBehaviorScope auditScope = CreateOrderAuditScope(order))
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Save the new order
                    SaveDownloadedOrder(order);

                    // Go through each abandoned item and delete it
                    foreach (OrderItemEntity item in abandonedItems)
                    {
                        // Detatch it from the order
                        affectedOrders.Single(o => o.OrderID == item.OrderID).OrderItems.Single(i => i.OrderItemID == item.OrderItemID).Order = null;

                        // Deleted all the attributes
                        foreach (OrderItemAttributeEntity attribute in item.OrderItemAttributes)
                        {
                            adapter.DeleteEntity(attribute);
                        }

                        // And delete it
                        adapter.DeleteEntity(item);
                    }

                    // Find all the orders that have no items.  We're going to have to delete them, since they are now empty and pointless.  But before
                    // we delete them, we need to migrate their shipments and notes so they don't just get lost.
                    foreach (OrderEntity fromOrder in affectedOrders.Where(o => o.OrderItems.Count == 0))
                    {
                        // Copy the notes from the old order
                        OrderUtility.CopyNotes(fromOrder.OrderID, order);

                        // Copy the shipments from the old order
                        OrderUtility.CopyShipments(fromOrder.OrderID, order);

                        // Delete the old order
                        DeletionService.DeleteOrder(fromOrder.OrderID, adapter);
                    }

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Load all the transactions (line items) for the given order. Returns a list of items for transactions that are being loaded that used to be apart
        /// of another order.  Those items must be deleted when the new order is saved, as they've now moved to the new order.
        /// </summary>
        private List<OrderItemEntity> LoadTransactions(EbayOrderEntity order, OrderType orderType)
        {
            List<OrderItemEntity> abandonedItems = new List<OrderItemEntity>();

            // Go through each transaction in the order
            foreach (TransactionType transaction in orderType.TransactionArray)
            {
                if (transaction.Item == null)
                {
                    log.InfoFormat("Encountered a NULL transaction.Item in combinedPayment.TransactionArray for eBay Order ID {0}.  Skipping.", orderType.OrderID);
                    continue;
                }

                // See if this item exists somewhere already in our database
                EbayOrderItemEntity orderItem = FindItem(new EbayOrderIdentifier(transaction.Item.ItemID, transaction.TransactionID));

                // If it does already exist, let's see if it's actually on a different order, we need to see if it exists on a different order
                if (orderItem != null)
                {
                    // If this order is new, or if the ID's don't match, then it was on a different order and we have to move it
                    if (order.IsNew || order.OrderID != orderItem.OrderID)
                    {
                        abandonedItems.Add(orderItem);

                        // This will force creating a brand new order item that will recreate this one fresh on this order.  The previous item will just be deleted, effectivly moving
                        // the item from the old order to the new order
                        orderItem = null;
                    }
                }

                // See if we need to create a band new order item, we need to create it
                if (orderItem == null)
                {
                    orderItem = (EbayOrderItemEntity) InstantiateOrderItem(order);

                    orderItem.LocalEbayOrderID = orderItem.OrderID;
                    orderItem.EbayItemID = long.Parse(transaction.Item.ItemID);
                    orderItem.EbayTransactionID = long.Parse(transaction.TransactionID);
                }
                // Attach it, so any items get saved with the order
                else
                {
                    order.OrderItems.Add(orderItem);
                }

                UpdateTransaction(orderItem, orderType, transaction);
            }

            return abandonedItems;
        }

        /// <summary>
        /// The date we use for an eBay order is the oldest of all the transactions in the order
        /// </summary>
        private DateTime DetermineOrderDate(OrderType orderType)
        {
            DateTime orderDate = orderType.CreatedTime;

            foreach (var transaction in orderType.TransactionArray)
            {
                orderDate = (transaction.CreatedDate < orderDate) ? transaction.CreatedDate : orderDate;
            }

            return orderDate;
        }

        /// <summary>
        /// Determine the email address to use for the buyer
        /// </summary>
        private string DetermineBuyerEmail(OrderType orderType)
        {
            var buyerNodes = orderType.TransactionArray.Select(t => t.Buyer).Where(b => b != null);

            // eBay doesn't always send the real email address. First check for the first real email address we find.
            string email = buyerNodes.Select(b => b.Email).FirstOrDefault(e => e != null && e.Contains('@'));

            // Then fall back to the alias.
            if (string.IsNullOrWhiteSpace(email))
            {
                email = buyerNodes.Select(b => b.StaticAlias).FirstOrDefault(e => e != null && e.Contains('@'));
            }

            return email ?? "";
        }

        /// <summary>
        /// Update the address of the ebay order with the address infomration provided
        /// </summary>
        private void UpdateOrderAddress(EbayOrderEntity order, ShipWorks.Stores.Platforms.Ebay.WebServices.AddressType address)
        {
            // Split the name
            PersonName personName = PersonName.Parse(address.Name);

            order.ShipNameParseStatus = (int) personName.ParseStatus;
            order.ShipUnparsedName = personName.UnparsedName;
            order.ShipCompany = address.CompanyName ?? "";
            order.ShipFirstName = personName.First;
            order.ShipMiddleName = personName.Middle;
            order.ShipLastName = personName.Last;
            order.ShipStreet1 = address.Street1 ?? "";
            order.ShipStreet2 = address.Street2 ?? "";
            order.ShipCity = address.CityName ?? "";
            order.ShipStateProvCode = address.StateOrProvince == null ? string.Empty : Geography.GetStateProvCode(address.StateOrProvince) ?? "";
            order.ShipPostalCode = address.PostalCode ?? "";
            order.ShipPhone = address.Phone ?? "";
            order.ShipCountryCode = address.CountrySpecified ? Enum.GetName(typeof(ShipWorks.Stores.Platforms.Ebay.WebServices.CountryCodeType), address.Country) : "";

            // Fill in billing address from the shipping
            PersonAdapter.Copy(order, "Ship", order, "Bill");
        }

        /// <summary>
        /// Update all of the charges for the order
        /// </summary>
        private void UpdateCharges(EbayOrderEntity order, OrderType orderType)
        {
            #region Shipping

            // Shipping
            OrderChargeEntity shipping = GetCharge(order, "SHIPPING", "Shipping");
            shipping.Amount = orderType.ShippingServiceSelected.ShippingServiceCost != null ? (decimal) orderType.ShippingServiceSelected.ShippingServiceCost.Value : 0;

            #endregion

            #region Adjustment

            // Only use the adjustment value if the order is considered complete.  Otherwise ebay seems to put an adjustment on non-complete orders that sets the total to zero.
            decimal adjustment = (order.OnlineStatusCode is int && (int) order.OnlineStatusCode == (int) OrderStatusCodeType.Completed) ?
                (decimal) orderType.AdjustmentAmount.Value :
                0m;

            OrderChargeEntity adjust = GetCharge(order, "ADJUST", "Adjustment", adjustment != 0);

            // Apply the adjustment
            if (adjust != null)
            {
                adjust.Amount = adjustment;
            }

            #endregion

            #region Insurance

            decimal insuranceTotal = 0;

            // Use insurance
            if (orderType.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Required ||
                orderType.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Optional)
            {
                if ((orderType.ShippingDetails.InsuranceWanted || orderType.ShippingDetails.InsuranceOption == InsuranceOptionCodeType.Required))
                {
                    if (orderType.ShippingServiceSelected != null && orderType.ShippingServiceSelected.ShippingInsuranceCost != null)
                    {
                        insuranceTotal = (decimal) orderType.ShippingServiceSelected.ShippingInsuranceCost.Value;
                    }
                    else if (orderType.ShippingDetails.InsuranceFee != null)
                    {
                        insuranceTotal = (decimal) orderType.ShippingDetails.InsuranceFee.Value;
                    }
                }
            }

            OrderChargeEntity insurance = GetCharge(order, "INSURANCE", "Insurance", insuranceTotal != 0);

            if (insurance != null)
            {
                insurance.Amount = insuranceTotal;
            }

            #endregion

            #region Sales Tax

            decimal salesTax = 0m;

            if (orderType.ShippingDetails.SalesTax != null && orderType.ShippingDetails.SalesTax.SalesTaxAmount != null)
            {
                salesTax = (decimal) orderType.ShippingDetails.SalesTax.SalesTaxAmount.Value;
            }

            // Tax
            OrderChargeEntity tax = GetCharge(order, "TAX", "Sales Tax", true);
            tax.Amount = salesTax;

            #endregion
        }

        /// <summary>
        /// Update the notes for the given order
        /// </summary>
        private void UpdateNotes(EbayOrderEntity order, OrderType orderType)
        {
            InstantiateNote(order, orderType.BuyerCheckoutMessage, order.OrderDate, NoteVisibility.Public, true);
        }

        /// <summary>
        /// Update external paypal information for the order
        /// </summary>
        private void UpdatePayPal(EbayOrderEntity order, OrderType orderType)
        {
            string transactionID = "";
            PayPalAddressStatus addressStatus = PayPalAddressStatus.None;

            // Only try to find PayPal details for PayPal orders
            if (orderType.CheckoutStatus.PaymentMethod == BuyerPaymentMethodCodeType.PayPal)
            {
                // See if we can find the transaction ID from the monetary details
                if (orderType.MonetaryDetails != null && orderType.MonetaryDetails.Payments != null && orderType.MonetaryDetails.Payments.Payment != null)
                {
                    transactionID = orderType.MonetaryDetails.Payments.Payment
                        .Where(p => p.ReferenceID != null && p.ReferenceID.Value != null && p.ReferenceID.Value.Length > 10)
                        .Select(p => p.ReferenceID.Value)
                        .FirstOrDefault() ?? "";
                }

                // See if we need to download paypal details
                if (((EbayStoreEntity) Store).DownloadPayPalDetails)
                {
                    // If we don't have the transaction id yet, try to find it
                    if (transactionID.Length == 0)
                    {
                        transactionID = FindPayPalTransactionID(order.OrderDate, order.ShipLastName, orderType.Total.Value);
                    }

                    // If we have it now, load the PayPal details
                    if (transactionID.Length > 0)
                    {
                        try
                        {
                            addressStatus = LoadPayPalTransactionData(order, transactionID);
                        }
                        catch (PayPalException)
                        {
                            // Transaction ID must have been bad, search again
                            transactionID = FindPayPalTransactionID(order.OrderDate, order.ShipLastName, orderType.Total.Value);

                            if (transactionID.Length > 0)
                            {
                                addressStatus = LoadPayPalTransactionData(order, transactionID);
                            }
                        }
                    }
                }
            }

            // For now apply this to each of the items - we need to move this data up to the order level
            foreach (EbayOrderItemEntity item in order.OrderItems.OfType<EbayOrderItemEntity>())
            {
                item.PayPalTransactionID = transactionID;
                item.PayPalAddressStatus = (int) addressStatus;
            }
        }

        /// <summary>
        /// Update the transaction (order item) info
        /// </summary>
        private void UpdateTransaction(EbayOrderItemEntity orderItem, OrderType orderType, TransactionType transaction)
        {
            if (string.IsNullOrWhiteSpace(orderItem.Code)) { orderItem.Code = transaction.Item.ItemID; }
            if (string.IsNullOrWhiteSpace(orderItem.Name)) { orderItem.Name = transaction.Item.Title; }
            UpdateTransactionSKU(orderItem, transaction.Item.SKU ?? "");

            orderItem.UnitPrice = (decimal) transaction.TransactionPrice.Value;
            orderItem.Quantity = transaction.QuantityPurchased;

            // Checkout (from order - these can be moved up in the database from the item to the order level)
            orderItem.PaymentMethod = (int) orderType.CheckoutStatus.PaymentMethod;
            orderItem.PaymentStatus = (int) orderType.CheckoutStatus.eBayPaymentStatus;
            orderItem.CompleteStatus = (int) orderType.CheckoutStatus.Status;

            // My eBay statuses - we set this at the line-item level, but the API provides them at the order level.
            orderItem.MyEbayPaid = orderType.PaidTimeSpecified;
            orderItem.MyEbayShipped = orderType.ShippedTimeSpecified || HasTrackingNumber(transaction);

            // Load variation information
            UpdateTransactionVariationDetail(orderItem, transaction);

            // We can only pull weight for calculated shipping
            UpdateTransactionWeight(orderItem, transaction);

            // SellingManager Pro
            orderItem.SellingManagerRecord = transaction.ShippingDetails.SellingManagerSalesRecordNumberSpecified ? transaction.ShippingDetails.SellingManagerSalesRecordNumber : 0;

            // Update the item with extra details that must be downloaded externally
            UpdateTransactionExtraDetails(orderItem, transaction);
        }

        /// <summary>
        /// Variations allow single auctions to have items that vary in size, color, etc.
        /// </summary>
        private void UpdateTransactionVariationDetail(EbayOrderItemEntity orderItem, TransactionType transaction)
        {
            if (transaction.Variation == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(transaction.Variation.VariationTitle))
            {
                orderItem.Name = transaction.Variation.VariationTitle;
            }

            // Overwrite the SKU if a variation SKU is provided.  
            if (!string.IsNullOrWhiteSpace(transaction.Variation.SKU))
            {
                UpdateTransactionSKU(orderItem, transaction.Variation.SKU);
            }

            // If this is a new item and one with variations, we can add attributes now
            if (orderItem.IsNew)
            {
                if (transaction.Variation.VariationSpecifics != null)
                {
                    // create attributes for the variations
                    foreach (NameValueListType variation in transaction.Variation.VariationSpecifics)
                    {
                        OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(orderItem);
                        attribute.Name = variation.Name;
                        attribute.Description = String.Join("; ", variation.Value);
                        attribute.UnitPrice = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Update the SKU of the given item with the given SKU
        /// </summary>
        private void UpdateTransactionSKU(EbayOrderItemEntity orderItem, string sku)
        {
            // Don't overwrite it if it's already there.  One important scenario is if the user applies their own SKUs and doesn't want eBay's to overrwrite, such as SKUVault
            if (!string.IsNullOrWhiteSpace(orderItem.SKU) && !orderItem.IsNew)
            {
                return;
            }

            // FreshDesk #517760 - SKU was coming down literally as "null"
            if (string.Compare(sku, "null", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                orderItem.SKU = sku;
            }
        }

        /// <summary>
        /// Update the weight of the given item based on the detail in the transaction
        /// </summary>
        private void UpdateTransactionWeight(EbayOrderItemEntity orderItem, TransactionType transaction)
        {
            if (transaction.ShippingDetails.CalculatedShippingRate != null)
            {
                ShipWorks.Stores.Platforms.Ebay.WebServices.MeasureType weightMajor = transaction.ShippingDetails.CalculatedShippingRate.WeightMajor;
                ShipWorks.Stores.Platforms.Ebay.WebServices.MeasureType weightMinor = transaction.ShippingDetails.CalculatedShippingRate.WeightMinor;

                if (weightMajor != null && weightMinor != null)
                {
                    if (weightMajor.unit == "POUNDS" || weightMajor.unit == "lbs")
                    {
                        orderItem.Weight = (double) (weightMajor.Value + weightMinor.Value / 16.0m);
                    }
                }
            }
        }

        /// <summary>
        /// Update the details of the transaction with information that we have to make an extra call for
        /// </summary>
        private void UpdateTransactionExtraDetails(EbayOrderItemEntity orderItem, TransactionType transaction)
        {
            // If updating external stuff is turned off, don't do it
            if (!((EbayStoreEntity) Store).DownloadItemDetails)
            {
                return;
            }

            // If this item isn't new, or we already have the extra info, then we don't need to waste time on this
            if (!orderItem.IsNew || !string.IsNullOrWhiteSpace(orderItem.Image))
            {
                return;
            }

            try
            {
                ItemType eBayItem = webClient.GetItem(transaction.Item.ItemID);

                PictureDetailsType pictureDetails = eBayItem.PictureDetails;

                // Teh first picture in PictureURL is the default
                if (pictureDetails != null && pictureDetails.PictureURL != null && pictureDetails.PictureURL.Length > 0)
                {
                    orderItem.Image = eBayItem.PictureDetails.PictureURL[0] ?? "";
                    orderItem.Thumbnail = orderItem.Image;
                }

                // If still no image, see if there is a stock image
                if (string.IsNullOrWhiteSpace(orderItem.Image))
                {
                    if (eBayItem.ProductListingDetails != null && eBayItem.ProductListingDetails.IncludeStockPhotoURL)
                    {
                        orderItem.Image = eBayItem.ProductListingDetails.StockPhotoURL ?? "";
                        orderItem.Thumbnail = orderItem.Image;
                    }
                }

                // See if there are other details we can use
                if (eBayItem.ProductListingDetails != null)
                {
                    orderItem.UPC = eBayItem.ProductListingDetails.UPC ?? "";
                    orderItem.ISBN = eBayItem.ProductListingDetails.ISBN ?? "";
                }
            }
            catch (EbayException exception)
            {
                // Check if we get error code 17 (the item has been deleted). eBay deletes items in certain
                // situations where the items falls outside of their user agreeements (counterfeit, selling illegal/trademarked items, etc.)
                if (exception.ErrorCode == "17" || exception.ErrorCode == "21917182")
                {
                    // Just log this exception otherwise the download process will not complete
                    log.WarnFormat("eBay returned an error when trying to download item details for item {0} (item ID {1}, transaction ID {2}). {3}", orderItem.Name, orderItem.EbayItemID, orderItem.EbayTransactionID, exception.Message);
                    log.WarnFormat("The seller may need to contact eBay support to determine why the order item may have been deleted.");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Indicates if the transaction has a shipment tracking number attached
        /// </summary>
        private bool HasTrackingNumber(TransactionType transaction)
        {
            return
                transaction.ShippingDetails.ShipmentTrackingDetails != null &&
                transaction.ShippingDetails.ShipmentTrackingDetails.Any() &&
                !string.IsNullOrWhiteSpace(transaction.ShippingDetails.ShipmentTrackingDetails.First().ShipmentTrackingNumber);
        }

        /// <summary>
        /// Reconciles the ShipWorks order total with what eBay has on record.
        /// Any adjustment are done via an OTHER charge
        /// </summary>
        private void BalanceOrderTotal(EbayOrderEntity order, double amountPaid)
        {
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            if (order.OrderTotal != Convert.ToDecimal(amountPaid))
            {
                // only make adjustments if it's considered complete
                if (order.OnlineStatusCode is int && (int) order.OnlineStatusCode == (int) OrderStatusCodeType.Completed)
                {
                    OrderChargeEntity otherCharge = GetCharge(order, "OTHER", "Other", true);
                    otherCharge.Description = "Other";
                    otherCharge.Amount += Convert.ToDecimal(amountPaid) - order.OrderTotal;
                }
            }
        }

        /// <summary>
        /// Updates the global shipping program info.
        /// </summary>
        /// <param name="order">The ebay order.</param>
        /// <param name="isGsp">if set to <c>true</c> [is global shipping program order].</param>
        /// <param name="gspDetails">The multi leg shipping details.</param>
        /// <exception cref="EbayException">eBay did not provide a reference ID for an order designated for the Global Shipping Program.</exception>
        private void UpdateGlobalShippingProgramInfo(EbayOrderEntity order, bool isGsp, MultiLegShippingDetailsType gspDetails)
        {
            order.GspEligible = isGsp;

            if (order.GspEligible)
            {
                // This is part of the global shipping program, so we need to pull out the address info 
                // of the international shipping provider but first make sure there aren't any null 
                // objects in the address heirarchy
                if (gspDetails != null &&
                    gspDetails.SellerShipmentToLogisticsProvider != null && 
                    gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress != null)
                {
                    // Pull out the name of the international shipping provider
                    PersonName name = PersonName.Parse(gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Name);
                    order.GspFirstName = name.First;
                    order.GspLastName = name.Last;

                    // Address info                    

                    // eBay includes "Suite 400" as part of street1 which some shipping carriers (UPS) don't recognize as a valid address.
                    // So, we'll try to split the street1 line (1850 Airport Exchange Blvd, Suite 400) into separate addresses based on 
                    // the presence of a comma in street 1

                    // We're ultimately going to populate the ebayOrder.GspStreet property values based on the elements in th streetLines list
                    List<string> streetLines = new List<string>
                    {
                        // Default the list to empty strings for the case where the Street1 and Street2 
                        // properties of the shipping address are null
                        gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1 ?? string.Empty,
                        gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street2 ?? string.Empty
                    };

                    if (gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1 != null)
                    {
                        // Try to split the Street1 property based on comma
                        List<string> splitStreetInfo = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Street1.Split(new char[] { ',' }).ToList();

                        // We'll always have at least one value in the result of the split which will be our value for street1
                        streetLines[0] = splitStreetInfo[0];

                        if (splitStreetInfo.Count > 1)
                        {
                            // There were multiple components to the original Street1 address provided by eBay; this second
                            // component will be the value we use for our street 2 address instead of the value provided by eBay
                            streetLines[1] = splitStreetInfo[1].Trim();
                        }
                    }

                    order.GspStreet1 = streetLines[0].Trim();
                    order.GspStreet2 = streetLines[1].Trim();

                    order.GspCity = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.CityName ?? string.Empty;
                    order.GspStateProvince = Geography.GetStateProvCode(gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.StateOrProvince) ?? string.Empty;
                    order.GspPostalCode = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.PostalCode ?? string.Empty;
                    order.GspCountryCode = Enum.GetName(typeof(ShipWorks.Stores.Platforms.Ebay.WebServices.CountryCodeType), gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.Country);

                    // Pull out the reference ID that will identify the order to the international shipping provider
                    order.GspReferenceID = gspDetails.SellerShipmentToLogisticsProvider.ShipToAddress.ReferenceID;

                    if (order.GspPostalCode.Length >= 5)
                    {
                        // Only grab the first five digits of the postal code; there have been incidents in the past where eBay 
                        // sends down an invalid 9 digit postal code (e.g. 41018-319) that prevents orders from being shipped
                        order.GspPostalCode = order.GspPostalCode.Substring(0, 5);
                    }

                    if (order.IsNew || order.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
                    {
                        // The seller has the choice to NOT ship GSP, so only default the shipping program to GSP for new orders
                        // or orders if the selected shipping method has not been manually overridden
                        order.SelectedShippingMethod = (int) EbayShippingMethod.GlobalShippingProgram;
                    }
                }

                if (string.IsNullOrEmpty(order.GspReferenceID))
                {
                    // We can't necessarily reference an ID number here since the ShipWorks order ID may not be assigned yet,
                    // so we'll reference the order date and the buyer that made the purchase
                    string message = string.Format("eBay did not provide a reference ID for an order designated for the Global Shipping Program. The order was placed on {0} from buyer {1}.",
                                        StringUtility.FormatFriendlyDateTime(order.OrderDate), order.BillUnparsedName);

                    throw new EbayMissingGspReferenceException(message);
                }
            }
            else
            {
                // This isn't a GSP order, so we're going to wipe the GSP data from the order in the event that
                // an order was previously marked as a GSP, but is no longer for some reason

                order.GspFirstName = string.Empty;
                order.GspLastName = string.Empty;

                // Address info
                order.GspStreet1 = string.Empty;
                order.GspStreet2 = string.Empty;
                order.GspCity = string.Empty;
                order.GspStateProvince = string.Empty;
                order.GspPostalCode = string.Empty;
                order.GspCountryCode = string.Empty;

                // Reset the reference ID and the shipping method to standard
                order.GspReferenceID = string.Empty;

                if (order.SelectedShippingMethod != (int) EbayShippingMethod.DirectToBuyerOverridden)
                {
                    // Only change the status if it has not been previously overridden; due to the individual transactions being downloaded 
                    // first then the combined orders being downloaded, this would inadvertently get set back to GSP if the combined order is
                    // a GSP order (if the same buyer purchases one item that is GSP and another that isn't, the GSP settings get applied
                    // to the combined order).
                    order.SelectedShippingMethod = (int) EbayShippingMethod.DirectToBuyer;
                }
            }
        }

        /// <summary>
        /// Get the specified charge for the order
        /// </summary>
        private OrderChargeEntity GetCharge(OrderEntity order, string type, string description, bool autoCreate = true)
        {
            OrderChargeEntity orderCharge = order.OrderCharges.FirstOrDefault(c => c.Type == type);

            if (orderCharge == null)
            {
                if (autoCreate)
                {
                    // create a new one
                    orderCharge = InstantiateOrderCharge(order);
                    orderCharge.Type = type;
                    orderCharge.Description = description;
                }
            }

            return orderCharge;
        }

        /// <summary>
        /// Locate an order with an OrderIdentifier
        /// </summary>
        protected override OrderEntity FindOrder(OrderIdentifier orderIdentifier)
        {
            EbayOrderIdentifier identifier = orderIdentifier as EbayOrderIdentifier;
            if (identifier == null)
            {
                throw new InvalidOperationException("OrderIdentifier of type EbayOrderIdentifier expected.");
            }

            return FindOrder(identifier, true);
        }

        /// <summary>
        /// Find and load an order of the given identifier, optionally including child charges
        /// </summary>
        private EbayOrderEntity FindOrder(EbayOrderIdentifier identifier, bool includeCharges)
        {
            PrefetchPath2 prefetch = null;

            // When we do load the order, see if we should include charges
            if (includeCharges)
            {
                prefetch = new PrefetchPath2(EntityType.OrderEntity);
                prefetch.Add(OrderEntity.PrefetchPathOrderCharges);
            }

            // A single-auction will have an OrderID of zero
            if (identifier.EbayOrderID == 0)
            {
                // doing a few joins, give llbl the information
                RelationCollection relations = new RelationCollection(OrderEntity.Relations.OrderItemEntityUsingOrderID);
                relations.Add(OrderItemEntity.Relations.GetSubTypeRelation("EbayOrderItemEntity"));

                // Look for any existing EbayOrderItem with matching ebayItemID and TransactionID
                // it's parent order will be the order we're looking for
                object objOrderID = SqlAdapter.Default.GetScalar(EbayOrderItemFields.OrderID,
                    null, AggregateFunction.None,
                    EbayOrderItemFields.EbayItemID == identifier.EbayItemID & EbayOrderItemFields.EbayTransactionID == identifier.TransactionID &
                        OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false,
                    null,
                    relations);

                if (objOrderID == null)
                {
                    // order does not exist
                    return null;
                }
                else
                {
                    // return the order entity
                    long orderID = (long) objOrderID;

                    EbayOrderEntity ebayOrder = new EbayOrderEntity(orderID);
                    SqlAdapter.Default.FetchEntity(ebayOrder, prefetch);

                    return ebayOrder;
                }
            }
            else
            {
                RelationPredicateBucket bucket = new RelationPredicateBucket(
                    EbayOrderFields.EbayOrderID == identifier.EbayOrderID & EbayOrderFields.StoreID == Store.StoreID);

                EntityCollection<EbayOrderEntity> collection = new EntityCollection<EbayOrderEntity>();
                SqlAdapter.Default.FetchEntityCollection(collection, bucket, prefetch);

                return collection.FirstOrDefault();
            }
        }

        /// <summary>
        /// Locate an item with the given identifier.  Can be optionally restricted to only loading the ItemID
        /// </summary>
        private EbayOrderItemEntity FindItem(EbayOrderIdentifier identifier, bool idOnly = false)
        {
            if (identifier.EbayOrderID != 0)
            {
                throw new InvalidOperationException("FindItem not valid for identifiers representing combined orders.");
            }

            object objItemID = SqlAdapter.Default.GetScalar(EbayOrderItemFields.OrderItemID,
                null, AggregateFunction.None,
                EbayOrderItemFields.EbayItemID == identifier.EbayItemID & EbayOrderItemFields.EbayTransactionID == identifier.TransactionID,
                null);

            if (objItemID == null)
            {
                // item does not exist
                return null;
            }
            else
            {
                // return the item entity
                long itemID = (long) objItemID;

                EbayOrderItemEntity item = new EbayOrderItemEntity(itemID);

                if (!idOnly)
                {
                    PrefetchPath2 prefetch = new PrefetchPath2(EntityType.OrderItemEntity);
                    prefetch.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);

                    SqlAdapter.Default.FetchEntity(item, prefetch);
                }

                return item;
            }
        }

        /// <summary>
        /// Search for a paypal transaction that matches up with these payment details
        /// </summary>
        private string FindPayPalTransactionID(DateTime start, string payerLastName, double amount)
        {
            TransactionSearchRequestType search = new TransactionSearchRequestType();

            search.StartDate = start;
            search.PayerName = new PersonNameType { LastName = payerLastName };

            // Perform the search
            TransactionSearchResponseType response;
            try
            {
                PayPalWebClient paypalClient = new PayPalWebClient(new PayPalAccountAdapter(Store, "PayPal"));

                response = (TransactionSearchResponseType) paypalClient.ExecuteRequest(search);
                if (response.PaymentTransactions == null)
                {
                    // no transaction found that matches the criteria
                    return "";
                }

                // consider only those with a payer specified
                List<PaymentTransactionSearchResultType> candidates = response.PaymentTransactions.Where(p => p.Payer.Length > 0).ToList();

                // single match, must be the one we're looking for
                if (candidates.Count == 1)
                {
                    // single reuslt
                    return candidates[0].TransactionID;
                }

                // try to further narrow it down, filter out Updates
                candidates.RemoveAll(p => p.Type != "Payment");

                // Did we remove them all
                if (candidates.Count == 0)
                {
                    return "";
                }

                // again return if there is only one
                if (candidates.Count == 1)
                {
                    return candidates[0].TransactionID;
                }

                // saw a case where we had multiple, but all were the same transaction ID
                if (candidates.All(p => p.TransactionID == candidates[0].TransactionID))
                {
                    return candidates[0].TransactionID;
                }

                if (candidates.Count > 1)
                {
                    // Pick the first one that matches the gross amount, if there's only one
                    var matches = candidates.Where(p => Math.Abs(Convert.ToDecimal(p.GrossAmount.Value)) == (decimal) amount);
                    if (matches.Count() == 1)
                    {
                        return matches.FirstOrDefault().TransactionID;
                    }
                }

                return "";
            }
            catch (PayPalException ex)
            {
                // log the error
                log.Error("Failed to retrieve PayPal transactions for buyer " + payerLastName, ex);
                return "";
            }
        }

        /// <summary>
        /// Updates the paypal address status, and adds any paypal-sourced notes to the order.
        /// </summary>
        private PayPalAddressStatus LoadPayPalTransactionData(EbayOrderEntity order, string transactionID)
        {
            GetTransactionDetailsRequestType request = new GetTransactionDetailsRequestType();
            request.TransactionID = transactionID;

            try
            {
                PayPalWebClient client = new PayPalWebClient(new PayPalAccountAdapter(Store, "PayPal"));
                GetTransactionDetailsResponseType response = (GetTransactionDetailsResponseType) client.ExecuteRequest(request);

                // TODO: need to specify which item it's for
                InstantiateNote(order, response.PaymentTransactionDetails.PaymentItemInfo.Memo, response.PaymentTransactionDetails.PaymentInfo.PaymentDate, NoteVisibility.Public, true);

                return (PayPalAddressStatus) (int) response.PaymentTransactionDetails.PayerInfo.Address.AddressStatus;
            }
            catch (PayPalException ex)
            {
                // 10007 means you didn't have permission to get the details of the transaction
                if (ex.Errors.Any(e => e.Code == "10007"))
                {
                    log.ErrorFormat("eBay had a correct transaction ({0}) but insufficient PayPal priveleges to get data for it.", transactionID);

                    return PayPalAddressStatus.None;
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region Feedback

        /// <summary>
        /// Download all feedback
        /// </summary>
        private bool DownloadFeedback()
        {
            Progress.Detail = "Checking for feedback...";
            Progress.PercentComplete = 0;

            DateTime? downloadThrough = ((EbayStoreEntity) Store).FeedbackUpdatedThrough;

            // The date to stop looking at feedback, even if more exists
            if (downloadThrough == null)
            {
                downloadThrough = GetOldestOrderDate();
                if (downloadThrough == null)
                {
                    return true;
                }
            }

            // Go back a max
            if (downloadThrough < DateTime.UtcNow.AddDays(-14))
            {
                downloadThrough = DateTime.UtcNow.AddDays(-14);
            }

            int feedbackCount = 0;

            // First download all feedback recieved
            DateTime newestRecieved = DownloadFeedback(null, downloadThrough.Value, ref feedbackCount);

            // Check for user cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Then download all feedback left
            DownloadFeedback(FeedbackTypeCodeType.FeedbackLeft, downloadThrough.Value, ref feedbackCount);

            // Check for user cancel
            if (Progress.IsCancelRequested)
            {
                return false;
            }

            // Use the latest feedback recieived date as our ending point for next time. Since we queried it first, this is safe.
            SaveFeedbackCheckpoint(newestRecieved);

            return true;
        }

        /// <summary>
        /// Download feedback of the given time, through the specified date
        /// </summary>
        private DateTime DownloadFeedback(FeedbackTypeCodeType? feedbackType, DateTime downloadThrough, ref int count)
        {
            int page = 1;

            DateTime newestFeedback = downloadThrough;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                GetFeedbackResponseType response = webClient.GetFeedback(feedbackType, page);

                // Quit if eBay says there aren't any more
                if (response.FeedbackDetailItemTotal == 0)
                {
                    return newestFeedback;
                }

                // Process all of the downloaded feedback
                foreach (FeedbackDetailType feedback in response.FeedbackDetailArray)
                {
                    DateTime feedbackDate = feedback.CommentTime.ToUniversalTime();

                    // Record this as the newest we've seen if necessary
                    if (feedbackDate > newestFeedback)
                    {
                        newestFeedback = feedbackDate;
                    }

                    // If this goes back prior to when we want to look for feedback, or the user has cancelled, we are doing
                    if (feedbackDate < downloadThrough || Progress.IsCancelRequested)
                    {
                        return newestFeedback;
                    }

                    ProcessFeedback(feedback);

                    Progress.Detail = string.Format("Processing feedback {0}...", count++);
                }

                // Next page
                page++;
            }
        }

        /// <summary>
        /// Save the checkpoint for how far back to go looking for feedback
        /// </summary>
        private void SaveFeedbackCheckpoint(DateTime newestFeedback)
        {
            EbayStoreEntity eBayStore = (EbayStoreEntity) Store;

            eBayStore.FeedbackUpdatedThrough = newestFeedback;
            SqlAdapter.Default.SaveAndRefetch(eBayStore);
        }

        /// <summary>
        /// Process the given feedback
        /// </summary>
        private void ProcessFeedback(FeedbackDetailType feedback)
        {
            EbayOrderItemEntity item = FindItem(new EbayOrderIdentifier(feedback.OrderLineItemID));
            if (item == null)
            {
                return;
            }

            log.DebugFormat("FEEDBACK: {0} - {1} - {2}", feedback.CommentTime, feedback.ItemID, feedback.CommentText);

            // Feedback we've recieved
            if (feedback.Role == TradingRoleCodeType.Seller)
            {
                item.FeedbackReceivedType = (int) feedback.CommentType;
                item.FeedbackReceivedComments = feedback.CommentText;
            }
            else
            {
                item.FeedbackLeftType = (int) feedback.CommentType;
                item.FeedbackLeftComments = feedback.CommentText;
            }

            // save the order item
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveEntity(item);
            }
        }

        /// <summary>
        /// Gets the smallest order date, combined or not, for this store in the database
        /// </summary>
        private DateTime? GetOldestOrderDate()
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                object result = adapter.GetScalar(
                    OrderFields.OrderDate,
                    null, AggregateFunction.Min,
                    OrderFields.StoreID == Store.StoreID & OrderFields.IsManual == false);

                DateTime? dateTime = result is DBNull ? null : (DateTime?) result;

                log.InfoFormat("MIN(OrderDate) = {0:u}", dateTime);

                return dateTime;
            }
        }

        /// <summary>
        /// Verify order totals are correct.
        /// </summary>
        protected override void VerifyOrderTotal(OrderEntity order)
        {
            // do nothing because during normal ebay operations, there are 
            // times when the orders don't always balance correctly temporarily.
        }

        #endregion

    }
}

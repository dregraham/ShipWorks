using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common.Logging;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Ebay.Enums;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.PayPal.WebServices;

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

        // Time range we are downloading from\to
        DateTime rangeStart;
        DateTime rangeEnd;

        // Total number of ordres expected during this download
        int expectedCount;

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

                // Get the date\time to start downloading from
                rangeStart = GetOnlineLastModifiedStartingPoint() ?? DateTime.UtcNow.AddDays(-7);
                rangeEnd = eBayOfficialTime.AddMinutes(-5);

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
            int page = 1;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                GetOrdersResponseType response = webClient.GetOrders(rangeStart, rangeEnd, page);

                // Grab the total expected account from the first page
                if (page == 1)
                {
                    expectedCount = response.PaginationResult.TotalNumberOfEntries;
                }

                // Process all of the downloaded orders
                foreach (OrderType orderType in response.OrderArray)
                {
                    ProcessOrder(orderType);

                    Progress.Detail = string.Format("Processing order {0} of {1}...", QuantitySaved, expectedCount);
                    Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);

                    // Check for user cancel
                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }
                }

                // Quit if eBay says there aren't any more
                if (!response.HasMoreOrders)
                {
                    return true;
                }

                // Next page
                page++;
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
            LoadTransactions(order, orderType);

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

            SaveDownloadedOrder(order);
        }

        /// <summary>
        /// Load all the transactions (line items) for the given order
        /// </summary>
        private void LoadTransactions(EbayOrderEntity order, OrderType orderType)
        {
            // Go through each transaction in the order
            foreach (TransactionType transaction in orderType.TransactionArray)
            {
                if (transaction.Item == null)
                {
                    log.InfoFormat("Encountered a NULL transaction.Item in combinedPayment.TransactionArray for eBay Order ID {0}.  Skipping.", orderType.OrderID);
                    continue;
                }

                // See if this transaction is already attached to the order and just needs updated
                EbayOrderItemEntity orderItem = order.OrderItems.OfType<EbayOrderItemEntity>().FirstOrDefault(i => transaction.OrderLineItemID == i.OrderLineItemID);

                // If it doesn't exist already in the order, we need to see if it exists on a different order
                if (orderItem == null)
                {
                    EbayOrderEntity otherOrder = FindOrder(new EbayOrderIdentifier(transaction.Item.ItemID, transaction.TransactionID), false);

                    // If another order owns this item right now, we have to delete it from that order.  Also if that other order has shipments, we're going to move
                    // them over to this order.  And then, if that other order has no more shipments, and no more items, we'll delete it.
                    if (otherOrder != null)
                    {
                        throw new NotImplementedException("Moving existing items to new combined orders.");
                    }
                }

                // If we still don't have an orderItem, we need to create it
                if (orderItem == null)
                {
                    orderItem = (EbayOrderItemEntity) InstantiateOrderItem(order);

                    orderItem.LocalEbayOrderID = orderItem.OrderID;
                    orderItem.EbayItemID = long.Parse(transaction.Item.ItemID);
                    orderItem.EbayTransactionID = long.Parse(transaction.TransactionID);
                }

                UpdateTransaction(orderItem, transaction, orderType.CheckoutStatus);
            }
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
            OrderChargeEntity shipping = GetCharge(order, "Shipping");
            shipping.Amount = orderType.ShippingServiceSelected.ShippingServiceCost != null ? (decimal) orderType.ShippingServiceSelected.ShippingServiceCost.Value : 0;

            #endregion

            #region Adjustment

            // Adjustment
            decimal adjustment = (decimal) orderType.AdjustmentAmount.Value;

            // Apply the adjustment
            if (adjustment != 0)
            {
                OrderChargeEntity adjust = GetCharge(order, "ADJUST", true);
                adjust.Description = "Adjustment";
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

            if (insuranceTotal != 0)
            {
                OrderChargeEntity insurance = GetCharge(order, "INSURANCE", true);
                insurance.Description = "Insurance";
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
            OrderChargeEntity tax = GetCharge(order, "TAX", true);
            tax.Description = "Sales Tax";
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
        private void UpdateTransaction(EbayOrderItemEntity orderItem, TransactionType transaction, CheckoutStatusType checkoutStatus)
        {
            orderItem.Code = transaction.Item.ItemID;
            orderItem.UnitPrice = (decimal) transaction.TransactionPrice.Value;
            orderItem.Quantity = transaction.QuantityPurchased;
            orderItem.Name = transaction.Item.Title;
            orderItem.SKU = transaction.Item.SKU ?? "";

            // Checkout (from order - these can be moved up in the database from the item to the order level)
            orderItem.PaymentMethod = (int) checkoutStatus.PaymentMethod;
            orderItem.PaymentStatus = (int) checkoutStatus.eBayPaymentStatus;
            orderItem.CompleteStatus = (int) checkoutStatus.Status;

            // My eBay
            orderItem.MyEbayPaid = transaction.PaidTimeSpecified;
            orderItem.MyEbayShipped = transaction.ShippedTimeSpecified;

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

            // Overwrite the SKU if a variation SKU is provided
            if (!string.IsNullOrEmpty(transaction.Variation.SKU))
            {
                orderItem.SKU = transaction.Variation.SKU;
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
        /// Reconciles the ShipWorks order total with what eBay has on record.
        /// Any adjustment are done via an OTHER charge
        /// </summary>
        private void BalanceOrderTotal(EbayOrderEntity order, double amountPaid)
        {
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            if (order.OrderTotal != Convert.ToDecimal(amountPaid))
            {
                // only make adjustments if all items are Complete
                if (order.OrderItems.OfType<EbayOrderItemEntity>().All(item => item.CompleteStatus == (int) CompleteStatusCodeType.Complete))
                {
                    OrderChargeEntity otherCharge = GetCharge(order, "OTHER", true);
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
        private OrderChargeEntity GetCharge(OrderEntity order, string description, bool autoCreate = true)
        {
            OrderChargeEntity orderCharge = order.OrderCharges.FirstOrDefault(c => c.Description == description);

            if (orderCharge == null)
            {
                if (autoCreate)
                {
                    // create a new one
                    orderCharge = InstantiateOrderCharge(order);
                    orderCharge.Description = description;
                    orderCharge.Type = description.ToUpper();
                }
            }

            return orderCharge;
        }

        /// <summary>
        /// Locate an order with an OrderIdentifier
        /// </summary>
        protected override OrderEntity FindOrder(OrderIdentifier orderIdentifier)
        {
            return FindOrder(orderIdentifier, true);
        }

        /// <summary>
        /// Find and load an order of the given identifier, optionall including all child records
        /// </summary>
        private EbayOrderEntity FindOrder(OrderIdentifier orderIdentifier, bool includeChildren)
        {
            EbayOrderIdentifier identifier = orderIdentifier as EbayOrderIdentifier;
            if (identifier == null)
            {
                throw new InvalidOperationException("OrderIdentifier of type EbayOrderIdentifier expected.");
            }

            PrefetchPath2 prefetch = null;

            // When we do load the order, we'll need to grab order items, charges, shipments since they'll be needed during processing
            if (includeChildren)
            {
                prefetch = new PrefetchPath2(EntityType.OrderEntity);
                prefetch.Add(OrderEntity.PrefetchPathOrderItems).SubPath.Add(OrderItemEntity.PrefetchPathOrderItemAttributes);
                prefetch.Add(OrderEntity.PrefetchPathOrderPaymentDetails);
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
                    // return the order entity, with associated items, charges, etc
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

            int page = 1;
            int count = 1;

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

            // Go back a max of 30 days
            if (downloadThrough < DateTime.UtcNow.AddDays(-30))
            {
                downloadThrough = DateTime.UtcNow.AddDays(-30);
            }

            // Tracks the newest feedback we've seen - which is where we will start next time.  But only if we complete this time.
            DateTime newestFeedback = downloadThrough.Value;

            // Keep going until the user cancels or there aren't any more.
            while (true)
            {
                GetFeedbackResponseType response = webClient.GetFeedback(page);

                // Quit if eBay says there aren't any more
                if (response.FeedbackDetailItemTotal == 0)
                {
                    if (page > 1)
                    {
                        SaveFeedbackCheckpoint(newestFeedback);
                    }

                    return true;
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

                    // If this goes back prior to when we want to look for feedback, we are done
                    if (feedbackDate < downloadThrough)
                    {
                        SaveFeedbackCheckpoint(newestFeedback);

                        return true;
                    }

                    ProcessFeedback(feedback);

                    Progress.Detail = string.Format("Processing feedback {0}...", count++);

                    // Check for user cancel
                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }
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
            EbayOrderEntity order = FindOrder(new EbayOrderIdentifier(feedback.OrderLineItemID), true);
            if (order == null)
            {
                return;
            }

            log.DebugFormat("FEEDBACK: {0} - {1} - {2}", feedback.CommentTime, feedback.ItemID, feedback.CommentText);

            EbayOrderItemEntity item = order.OrderItems.OfType<EbayOrderItemEntity>().First(i => i.OrderLineItemID == feedback.OrderLineItemID);

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

        #endregion

    }
}

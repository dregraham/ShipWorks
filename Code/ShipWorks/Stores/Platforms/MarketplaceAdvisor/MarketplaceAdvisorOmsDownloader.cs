using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.WebServices.Oms;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    /// <summary>
    /// Downloader for OMS MarketplaceAdvisor stores
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    [Component]
    public class MarketplaceAdvisorOmsDownloader : StoreDownloader, IMarketplaceAdvisorOmsDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MarketplaceAdvisorOmsDownloader));

        // Download page size
        const int pageSize = 200;

        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOmsDownloader(MarketplaceAdvisorStoreEntity store)
            : base(store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));
        }

        /// <summary>
        /// Download the orders
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                Progress.Detail = "Checking for orders...";

                int currentPage = 1;

                // Keep going until no more to download
                while (true)
                {
                    // Check if it has been canceled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    bool morePages = await DownloadNextOrdersPage(currentPage++).ConfigureAwait(false);
                    if (!morePages)
                    {
                        return;
                    }
                }
            }
            catch (MarketplaceAdvisorException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download the next page of MarketplaceAdvisor orders
        /// </summary>
        private async Task<bool> DownloadNextOrdersPage(int currentPage)
        {
            OMOrders orders = MarketplaceAdvisorOmsClient.Create((MarketplaceAdvisorStoreEntity) Store).GetOrders(currentPage);

            if (orders.Orders.Length == 0)
            {
                if (currentPage == 1)
                {
                    Progress.Detail = "No orders to download.";
                    Progress.PercentComplete = 100;
                    return false;
                }
                else
                {
                    Progress.Detail = "Done";
                    return false;
                }
            }
            else
            {
                await LoadOrders(orders).ConfigureAwait(false);

                return true;
            }
        }

        /// <summary>
        /// Load the given OMS orders into ShipWorks
        /// </summary>
        private async Task LoadOrders(OMOrders ordersResult)
        {
            OMOrder[] orders = ordersResult.Orders;

            // We have to update back to MW that we processed the orders so they don't get downloaded again
            List<long> markAsProcessed = new List<long>();

            // Go through each order
            foreach (OMOrder order in orders)
            {
                // check for cancel
                if (Progress.IsCancelRequested)
                {
                    // We can't just return, since we still have to mark the ones we did download as processed
                    break;
                }

                // Different from the base QuantitySaved b\c multi-parcel orders generate more than one saved order
                int quantityDownloaded = (Array.IndexOf(orders, order) + 1) + ((ordersResult.PageNumber - 1) * ordersResult.RecordsPerPage);

                // Update the status
                Progress.Detail = string.Format("Processing order {0} of {1}...",
                    quantityDownloaded,
                    ordersResult.TotalRecords);

                // Always add to this list, even if already downloaded
                markAsProcessed.Add(order.OrderUid);

                await LoadOrder(order).ConfigureAwait(false);

                // update the status
                Progress.PercentComplete = 100 * quantityDownloaded / ordersResult.TotalRecords;
            }

            if (MarketplaceAdvisorUtility.MarkProcessedAfterDownload)
            {
                // Only make the call if there are any to do
                if (markAsProcessed.Count > 0)
                {
                    MarketplaceAdvisorOmsClient.Create((MarketplaceAdvisorStoreEntity) Store).MarkOrdersProcessed(markAsProcessed);
                }
            }
        }

        /// <summary>
        /// Load the given OMS order into ShipWorks
        /// </summary>
        private async Task LoadOrder(OMOrder omsOrder)
        {
            bool isNew = await CreateMasterOrder(omsOrder).ConfigureAwait(false);

            if (isNew)
            {
                // Now, create an extra order for each additional parcels
                for (int i = 1; i < omsOrder.Parcels.OrderParcels.Length; i++)
                {
                    await CreateParcelOrder(omsOrder, omsOrder.Parcels.OrderParcels[i]).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Create the master order for the oms order.  If there is only one parcel,
        /// then this will be the only order.  Returns true if the order that was created was a newly downloaded order.
        /// </summary>
        private async Task<bool> CreateMasterOrder(OMOrder omsOrder)
        {
            // Create a new order instance
            GenericResult<OrderEntity> result = await InstantiateOrder(new MarketplaceAdvisorOrderNumberIdentifier(omsOrder.OrderUid)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", omsOrder.OrderUid, result.Message);
                return false;
            }

            MarketplaceAdvisorOrderEntity order = (MarketplaceAdvisorOrderEntity) result.Value;

            // Setup the basic properties
            LoadCommonOrderProperties(order, omsOrder);

            // First parcel
            OMOrderParcel parcel = omsOrder.Parcels.OrderParcels[0];

            // Load the parcel specific info for the order
            LoadOrderParcelInfo(order, omsOrder, parcel);

            bool isNew = order.IsNew;
            if (isNew)
            {
                // If there are more than 1 parcel, we have to add a charge to make up
                // for the fact that some line items aren't on the master order
                if (omsOrder.Parcels.OrderParcels.Length > 1)
                {
                    double otherParcelsTotal = 0;

                    // Add up all the line item totals for the other parcels, if any.
                    for (int i = 1; i < omsOrder.Parcels.OrderParcels.Length; i++)
                    {
                        otherParcelsTotal += GetParcelLineItemTotal(omsOrder.Parcels.OrderParcels[i]);
                    }

                    LoadCharge(order, "PARCEL-ADJUST", "Parcel Adjustment", otherParcelsTotal, false);
                }

                // Load all the charges
                LoadOrderCharges(order, omsOrder);

                // Payment details
                LoadPaymentDetails(order, omsOrder);

                // Notes
                await LoadNotes(order, omsOrder).ConfigureAwait(false);

                // Update the total
                order.OrderTotal = OrderUtility.CalculateTotal(order);
            }

            // Save the order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "MarketplaceAdvisorOmsDownloader.CreateMasterOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

            return isNew;
        }

        /// <summary>
        /// Create an order to represent the given additional parcel of the oms order.
        /// </summary>
        private async Task CreateParcelOrder(OMOrder omsOrder, OMOrderParcel parcel)
        {
            // Create a new order instance with parse information
            GenericResult<OrderEntity> result = await InstantiateOrder(new MarketplaceAdvisorOrderNumberIdentifier(
                omsOrder.OrderUid,
                Array.IndexOf(omsOrder.Parcels.OrderParcels, parcel) + 1)).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", omsOrder.OrderUid, result.Message);
                return;
            }

            MarketplaceAdvisorOrderEntity order = (MarketplaceAdvisorOrderEntity) result.Value;

            // Setup the basic properties
            LoadCommonOrderProperties(order, omsOrder);

            // Load the parcel specific info for the order
            LoadOrderParcelInfo(order, omsOrder, parcel);

            // We need the order total to come out as zero, since the master order
            // contains the full order total.
            LoadCharge(order, "PARCEL-ADJUST", "Parcel Adjustment", -GetParcelLineItemTotal(parcel), false);

            // Update the total
            order.OrderTotal = OrderUtility.CalculateTotal(order);

            // Save the order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "MarketplaceAdvisorOmsDownloader.CreateParcelOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the total value of all line items in the given parcel.
        /// </summary>
        private double GetParcelLineItemTotal(OMOrderParcel parcel) => parcel.MerchandiseTotal;

        /// <summary>
        /// Load order properties common to master orders and parcel orders.
        /// </summary>
        private void LoadCommonOrderProperties(MarketplaceAdvisorOrderEntity order, OMOrder omsOrder)
        {
            order.OrderNumber = (int) omsOrder.OrderUid;
            order.OrderDate = DateTime.Parse(omsOrder.OrderDate);

            order.BuyerNumber = omsOrder.BuyerInfo.BuyerNumber;
            order.InvoiceNumber = omsOrder.InvoiceNumber;
            order.SellerOrderNumber = omsOrder.SellerOrderNumber;
        }

        /// <summary>
        /// Load all parcel specific info for the order from the given parcel.
        /// </summary>
        private void LoadOrderParcelInfo(MarketplaceAdvisorOrderEntity order, OMOrder omsOrder, OMOrderParcel parcel)
        {
            // Requested shipping
            order.RequestedShipping = parcel.ShipMethodName;

            // Load address info
            LoadAddressInfo(order, omsOrder.BuyerInfo, parcel.Destination);

            // Only reload items for new orders
            if (order.IsNew)
            {
                order.ParcelID = parcel.ParcelUid;

                // Items
                foreach (OMLineItem omsItem in parcel.LineItems.LineItem)
                {
                    LoadItem(order, omsItem);
                }
            }
        }

        /// <summary>
        /// Update the notes for the order based on the OMS order.
        /// </summary>
        private async Task LoadNotes(MarketplaceAdvisorOrderEntity order, OMOrder omsOrder)
        {
            if (omsOrder.InvoiceNote.Length > 0)
            {
                await InstantiateNote(order, "Buyer note: " + omsOrder.InvoiceNote, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            // Instructions
            if (omsOrder.SpecialBuyerInstructions.Length > 0)
            {
                await InstantiateNote(order, "Special Instructions: " + omsOrder.SpecialBuyerInstructions, order.OrderDate, NoteVisibility.Public).ConfigureAwait(false);
            }

            // Private notes
            if (omsOrder.PrivateOrderNotes.Length > 0)
            {
                await InstantiateNote(order, omsOrder.PrivateOrderNotes, order.OrderDate, NoteVisibility.Internal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Load all charges for the order
        /// </summary>
        private void LoadOrderCharges(MarketplaceAdvisorOrderEntity order, OMOrder omsOrder)
        {
            OMOrderFinancials finance = omsOrder.Financials;

            foreach (OMAdjustment adjustment in finance.OrderAdjustments.Adjustments)
            {
                LoadCharge(order, adjustment.Reason, adjustment.Reason, adjustment.Amount, true);
            }

            // Insurance
            LoadCharge(order, "Insurance", "Insurance", finance.InsuranceTotal, true);

            // Handling
            LoadCharge(order, "Handling", "Handling", finance.HandlingTotal, true);

            // Shipping
            LoadCharge(order, "Shipping", "Shipping", finance.PostageTotal, false);

            // Tax
            LoadCharge(order, "Tax", "Tax", finance.TaxTotal, false);
        }

        /// <summary>
        /// Load the charge for the order
        /// </summary>
        private void LoadCharge(MarketplaceAdvisorOrderEntity order, string type, string name, double amount, bool ignoreZeroAmount)
        {
            if (amount.IsEquivalentTo(0) && ignoreZeroAmount)
            {
                return;
            }

            if (name.Length == 0)
            {
                name = type;
            }

            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = type.ToUpper();
            charge.Description = name;
            charge.Amount = (decimal) amount;
        }

        /// <summary>
        /// Load the appropriate address info from the objects
        /// </summary>
        private void LoadAddressInfo(MarketplaceAdvisorOrderEntity order, OMAddress bill, OMAddress ship)
        {
            LoadAddressInfo(new PersonAdapter(order, "Ship"), ship);
            LoadAddressInfo(new PersonAdapter(order, "Bill"), bill);
        }

        /// <summary>
        /// Load the given OMS address into the specified person adapter
        /// </summary>
        private void LoadAddressInfo(PersonAdapter person, OMAddress ship)
        {
            person.FirstName = ship.FirstName;
            person.LastName = ship.LastName;
            person.NameParseStatus = PersonNameParseStatus.Simple;
            person.Company = ship.CompanyName;
            person.Street1 = ship.AddressLine1;
            person.Street2 = ship.AddressLine2;
            person.Street3 = ship.County;
            person.City = ship.City;
            person.StateProvCode = Geography.GetStateProvCode(ship.State);
            person.PostalCode = ship.ZipCode;
            person.CountryCode = Geography.GetCountryCode(ship.Country);
            person.Email = ship.EmailAddress;
            person.Phone = ship.PhoneNumber;
            person.Fax = ship.FaxNumber;

            // If we had a state, and a province, use them both separated by a space
            if (person.StateProvCode.Length > 0 && ship.Province.Length > 0)
            {
                person.StateProvCode += " ";
            }

            // Specify the province, if any
            person.StateProvCode += Geography.GetStateProvCode(ship.Province);

            // Special case for MW giving a name for Russia that's not in Geography
            if (String.Compare(person.CountryCode, "Russian Federation", StringComparison.OrdinalIgnoreCase) == 0)
            {
                person.CountryCode = "RU";
            }

            if (String.Compare(person.CountryCode, "Slovakia", StringComparison.OrdinalIgnoreCase) == 0)
            {
                person.CountryCode = "SK";
            }

            if (String.Compare(person.CountryCode, "Korea, Republic of (South Korea)", StringComparison.OrdinalIgnoreCase) == 0)
            {
                person.CountryCode = "KR";
            }

            if (String.Compare(person.CountryCode, "Croatia, Republic of", StringComparison.OrdinalIgnoreCase) == 0)
            {
                person.CountryCode = "HR";
            }
        }

        /// <summary>
        /// Load the payment details for the order.
        /// </summary>
        private void LoadPaymentDetails(MarketplaceAdvisorOrderEntity order, OMOrder omsOrder)
        {
            OMOrderPayment payment = omsOrder.Payment;

            if (payment.MethodCode == 0)
            {
                return;
            }

            string methodCode = payment.MethodCode.ToString();

            // Extras for CC
            if (MarketplaceAdvisorUtility.IsMethodCC(methodCode))
            {
                string auth = payment.CreditCard.AuthCode;
                if (auth.Length > 0)
                {
                    LoadPaymentDetail(order, "Authorization", auth);
                }

                LoadPaymentDetail(order, "TransactionID", payment.CreditCard.GatewayTransId);
                LoadPaymentDetail(order, "Verification", payment.CreditCard.CVV);
                LoadPaymentDetail(order, "Expiration", payment.CreditCard.EndMonth + "/" + payment.CreditCard.EndYear);
                LoadPaymentDetail(order, "Cardholder", payment.CreditCard.AcctName);
                LoadPaymentDetail(order, "Card Number", payment.CreditCard.AcctNumber);
            }

            // Extras for PayPal
            if (MarketplaceAdvisorUtility.IsMethodPayPal(methodCode))
            {
                LoadPaymentDetail(order, "Authorization", payment.Other.AuthCode);

                string transID = payment.Other.TransactionId;
                transID = transID.Replace("Paypal Transaction ID#", "");
                LoadPaymentDetail(order, "Transaction ID", transID);
            }

            // Create a detail item for the method
            LoadPaymentDetail(order, "Method", payment.Method);
        }

        /// <summary>
        /// Load the payment detail for the order
        /// </summary>
        private void LoadPaymentDetail(MarketplaceAdvisorOrderEntity order, string name, string data)
        {
            // Handle new OMS case where CC data sometimes comes in null
            if (data == null)
            {
                return;
            }

            OrderPaymentDetailEntity detail = InstantiateOrderPaymentDetail(order);

            detail.Label = name;
            detail.Value = data;
        }

        /// <summary>
        /// Load the item information into the order
        /// </summary>
        private void LoadItem(MarketplaceAdvisorOrderEntity order, OMLineItem omsItem)
        {
            OrderItemEntity item = InstantiateOrderItem(order);

            long itemNumber = omsItem.MWInvUID;

            double unitPrice = omsItem.ItemFinancials.PriceNoTax;

            // If there is a unit price adjustment, apply it now
            if (omsItem.ItemFinancials.LineItemAdjustment != null)
            {
                unitPrice += omsItem.ItemFinancials.LineItemAdjustment.Amount / omsItem.LineItemQuantity;
            }

            item.Name = omsItem.Title;
            item.Code = omsItem.InventoryNumber;
            item.Quantity = omsItem.LineItemQuantity;
            item.UnitPrice = (decimal) unitPrice;
            item.Location = omsItem.InventoryLocation;

            // Get the details for the item
            MarketplaceAdvisorLegacyClient client = MarketplaceAdvisorLegacyClient.Create((MarketplaceAdvisorStoreEntity) Store);
            MarketplaceAdvisorInventoryItem inventoryItem = client.GetInventoryItem(itemNumber);

            item.Description = inventoryItem.Description;
            item.SKU = inventoryItem.SKU;
            item.ISBN = inventoryItem.ISBN;
            item.UPC = inventoryItem.UPC;
            item.Image = inventoryItem.ImageUrl;
            item.Thumbnail = inventoryItem.ImageUrl;
            item.Weight = inventoryItem.WeightLbs;
            item.UnitCost = inventoryItem.Cost;
        }
    }
}

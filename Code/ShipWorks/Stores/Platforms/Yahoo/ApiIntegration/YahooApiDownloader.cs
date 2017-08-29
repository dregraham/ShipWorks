using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    /// <summary>
    /// Downloader for Yahoo stores using the Yahoo Api
    /// </summary>
    [Component]
    public class YahooApiDownloader : StoreDownloader, IYahooApiDownloader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(YahooApiDownloader));

        private readonly IYahooApiWebClient webClient;
        private readonly ISqlAdapterRetry sqlAdapter;
        private const int InvalidStartRangeErrorCode = 20021;
        private const int CatalogAccessForbidden = 10010;
        private bool CatalogEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooApiDownloader"/> class.
        /// </summary>
        [NDependIgnoreTooManyParams(Justification =
            "These parameters are dependencies the store downloader already had, they're just explicit now")]
        public YahooApiDownloader(YahooStoreEntity store,
            IYahooApiWebClient webClient,
            ISqlAdapterRetryFactory sqlAdapterRetryFactory,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory,
            Func<StoreEntity, YahooStoreType> getStoreType) :
            base(store, getStoreType(store), configurationData, sqlAdapterFactory)
        {
            this.webClient = webClient;
            this.sqlAdapter = sqlAdapterRetryFactory.Create<SqlException>(5, -5, "YahooApiDownloader.LoadOrder");
        }

        /// <summary>
        /// Kicks off download for the store
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for new orders...";

            try
            {
                List<long> orderList = await CheckForNewOrders().ConfigureAwait(false);

                if (orderList == null)
                {
                    throw new YahooException("Error checking for orders");
                }

                // Here we remove the first order in the list, because we already
                // have it. In the case of the stores very first download, the first
                // order number will be set to -1, so that no orders are lost.
                if (orderList.Count != 0)
                {
                    orderList.RemoveAt(0);
                }

                if (orderList.Count == 0)
                {
                    // There's nothing to download, so update the progress accordingly
                    Progress.PercentComplete = 100;
                    Progress.Detail = "Done - No new orders to download";
                }
                else
                {
                    Progress.Detail = "Downloading new orders...";

                    await DownloadNewOrders(orderList).ConfigureAwait(false);

                    ((YahooStoreEntity) Store).BackupOrderNumber = null;
                    StoreManager.SaveStore(Store);
                }
            }
            catch (YahooException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Downloads the new orders.
        /// </summary>
        /// <param name="orderList">The order list.</param>
        private async Task DownloadNewOrders(List<long> orderList)
        {
            var store = Store as IYahooStoreEntity;
            int expectedCount = orderList.Count;

            foreach (long orderID in orderList)
            {
                YahooResponse response = webClient.GetOrder(store, orderID);

                if (response.ResponseResourceList?.OrderList?.Order?.FirstOrDefault() == null)
                {
                    if (response.ErrorResourceList != null)
                    {
                        foreach (YahooError error in response.ErrorResourceList.Error)
                        {
                            throw new DownloadException(error.Message);
                        }
                    }

                    throw new DownloadException($"Error downloading order {orderID}");
                }

                // Set the progress detail
                Progress.Detail = $"Processing order {QuantitySaved + 1} of {expectedCount} ...";
                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);

                await CreateOrder(response.ResponseResourceList.OrderList.Order.FirstOrDefault()).ConfigureAwait(false);

                // Check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Creates the order entity and calls load order to populate it
        /// </summary>
        /// <param name="order">The order DTO.</param>
        /// <exception cref="YahooException">$Failed to instantiate order {order.OrderID}</exception>
        public async Task CreateOrder(YahooOrder order)
        {
            GenericResult<OrderEntity> result = await InstantiateOrder(new YahooOrderIdentifier(order.OrderID.ToString())).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order '{0}': {1}.", order.OrderID.ToString(), result.Message);
                return;
            }

            YahooOrderEntity orderEntity = result.Value as YahooOrderEntity;

            if (orderEntity == null)
            {
                throw new YahooException($"Failed to instantiate order {order.OrderID}");
            }

            if (orderEntity.IsNew)
            {
                orderEntity = await LoadOrder(order, orderEntity).ConfigureAwait(false);

                await sqlAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(orderEntity)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Populates order entity with fields from YahooOrder DTO
        /// </summary>
        /// <param name="order">The order DTO.</param>
        /// <param name="orderEntity">The order entity.</param>
        public async Task<YahooOrderEntity> LoadOrder(YahooOrder order, YahooOrderEntity orderEntity)
        {
            orderEntity.OnlineStatusCode = GetOnlineStatusCode(order);
            orderEntity.OnlineStatus = GetOnlineStatus(orderEntity);
            orderEntity.RequestedShipping = GetRequestedShipping(order);
            orderEntity.OrderDate = ParseYahooDateTime(order.CreationTime);
            orderEntity.OnlineLastModified = ParseYahooDateTime(order.LastUpdatedTime);
            orderEntity.OrderNumber = order.OrderID;
            orderEntity.YahooOrderID = order.OrderID.ToString();

            LoadAddress(orderEntity, order);
            LoadOrderItems(orderEntity, order);
            LoadOrderTotals(orderEntity, order);
            LoadOrderCharges(orderEntity, order);
            LoadOrderGiftMessages(orderEntity, order);
            await LoadOrderNotes(orderEntity, order).ConfigureAwait(false);
            LoadOrderPayments(orderEntity, order);

            return orderEntity;
        }

        /// <summary>
        /// Get Online Status
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private string GetOnlineStatusCode(YahooOrder order)
        {
            string code = order.StatusList.OrderStatus.First().StatusID;

            return code.Equals("0") ?
                order.StatusList.OrderStatus.Last().StatusID :
                code;
        }

        /// <summary>
        /// Get the online status code for this order
        /// </summary>
        /// <remarks>
        /// Yahoo gives a list of all that statuses an order has had. Typically, the
        /// most recent one is the last in the list. However, if you manually change
        /// an order status on Yahoo's back-end, it comes down as the first status in
        /// the list. So here we grab the first status, if it's 0 that means it hasn't
        /// been updated manually, so take the last one instead. If not, that is the
        /// manually updated status, so keep it
        /// </remarks>
        private string GetOnlineStatus(YahooOrderEntity order)
        {
            int statusID = int.Parse(order.OnlineStatusCode.ToString());

            if (statusID >= 0 && statusID <= EnumHelper.GetEnumList<YahooApiOrderStatus>().Count())
            {
                return EnumHelper.GetDescription((YahooApiOrderStatus) statusID);
            }

            var store = Store as IYahooStoreEntity;
            string status = webClient.GetCustomOrderStatus(store, statusID)
                .ResponseResourceList?.CustomOrderStatusList?.CustomOrderStatus?.FirstOrDefault()?
                .Code;

            if (status != null)
            {
                return status;
            }

            return order.OnlineStatus;
        }

        /// <summary>
        /// Get the requested shipping method
        /// </summary>
        /// <remarks>We've seen CartShipmentInfo not included in the response from Yahoo</remarks>
        private string GetRequestedShipping(YahooOrder order)
        {
            IEnumerable<string> requestedShippingPieces = new[] { order.CartShipmentInfo?.Shipper, order.ShipMethod }
                .Where(x => !string.IsNullOrWhiteSpace(x));

            return string.Join(" ", requestedShippingPieces);
        }

        /// <summary>
        /// Loads the address.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private void LoadAddress(YahooOrderEntity orderEntity, YahooOrder order)
        {
            orderEntity.ShipFirstName = order.ShipToInfo.GeneralInfo.FirstName;
            orderEntity.ShipLastName = order.ShipToInfo.GeneralInfo.LastName;
            orderEntity.ShipPhone = order.ShipToInfo.GeneralInfo.PhoneNumber;
            orderEntity.ShipCompany = order.ShipToInfo.GeneralInfo.Company;
            orderEntity.ShipEmail = order.ShipToInfo.GeneralInfo.Email;
            orderEntity.ShipStreet1 = order.ShipToInfo.AddressInfo.Address1;
            orderEntity.ShipStreet2 = order.ShipToInfo.AddressInfo.Address2;
            orderEntity.ShipCity = order.ShipToInfo.AddressInfo.City;
            // Yahoo gives the country code followed by the country name, so split it and grab just the code
            orderEntity.ShipCountryCode = Geography.GetCountryCode(order.ShipToInfo.AddressInfo.Country.Split(' ')[0]);
            orderEntity.ShipStateProvCode = Geography.GetStateProvCode(order.ShipToInfo.AddressInfo.State);
            orderEntity.ShipPostalCode = order.ShipToInfo.AddressInfo.Zip;

            orderEntity.BillFirstName = order.BillToInfo.GeneralInfo.FirstName;
            orderEntity.BillLastName = order.BillToInfo.GeneralInfo.LastName;
            orderEntity.BillPhone = order.BillToInfo.GeneralInfo.PhoneNumber;
            orderEntity.BillCompany = order.BillToInfo.GeneralInfo.Company;
            orderEntity.BillEmail = order.BillToInfo.GeneralInfo.Email;
            orderEntity.BillStreet1 = order.BillToInfo.AddressInfo.Address1;
            orderEntity.BillStreet2 = order.BillToInfo.AddressInfo.Address2;
            orderEntity.BillCity = order.BillToInfo.AddressInfo.City;
            orderEntity.BillCountryCode = Geography.GetCountryCode(order.ShipToInfo.AddressInfo.Country.Split(' ')[0]);
            orderEntity.BillStateProvCode = Geography.GetStateProvCode(order.BillToInfo.AddressInfo.State);
            orderEntity.BillPostalCode = order.BillToInfo.AddressInfo.Zip;
        }

        /// <summary>
        /// Loads the order payments.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO</param>
        private void LoadOrderPayments(YahooOrderEntity orderEntity, YahooOrder order)
        {
            OrderPaymentDetailEntity payment = InstantiateOrderPaymentDetail(orderEntity);

            payment.Label = "Payment Type";
            payment.Value = order.PaymentType;
        }

        /// <summary>
        /// Loads the order notes.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private async Task LoadOrderNotes(YahooOrderEntity orderEntity, YahooOrder order)
        {
            if (!order.MerchantNotes.IsNullOrWhiteSpace())
            {
                await InstantiateNote(orderEntity, order.MerchantNotes, ParseYahooDateTime(order.CreationTime),
                    NoteVisibility.Internal).ConfigureAwait(false);
            }

            if (!order.BuyerComments.IsNullOrWhiteSpace())
            {
                await InstantiateNote(orderEntity, order.BuyerComments, ParseYahooDateTime(order.CreationTime),
                    NoteVisibility.Public).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Loads the order gift messages.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private void LoadOrderGiftMessages(YahooOrderEntity orderEntity, YahooOrder order)
        {
            if (order.OrderTotals.GiftWrap != 0 || !order.GiftMessage.IsNullOrWhiteSpace())
            {
                YahooOrderItemEntity item = (YahooOrderItemEntity) InstantiateOrderItem(orderEntity);

                item.YahooProductID = "giftwrap";
                item.Code = "GIFTWRAP";
                item.Name = "Gift Wrap";
                item.Quantity = 1;
                item.UnitPrice = order.OrderTotals.GiftWrap;

                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(item);

                attribute.Name = "Message";
                attribute.Description = order.GiftMessage;
                attribute.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Loads the order charges.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private void LoadOrderCharges(YahooOrderEntity orderEntity, YahooOrder order)
        {
            LoadOrderCharge(orderEntity, "SHIPPING", "Shipping", order.OrderTotals.Shipping);
            LoadOrderCharge(orderEntity, "TAX", "Tax", order.OrderTotals.Tax);

            if (order.OrderTotals.Coupon != 0)
            {
                LoadOrderCharge(orderEntity, "COUPON", "Coupon", order.OrderTotals.Coupon);
            }

            if (order.OrderTotals.Discount != 0)
            {
                LoadOrderCharge(orderEntity, "DISCOUNT", "Discount", order.OrderTotals.Discount);
            }

            List<YahooAppliedPromotion> promotions = order.OrderTotals?.Promotions?.AppliedPromotion;

            if (promotions == null)
            {
                return;
            }

            foreach (YahooAppliedPromotion promotion in promotions)
            {
                LoadOrderCharge(orderEntity, "PROMOTION", promotion.Name, -promotion.Discount);
            }
        }

        /// <summary>
        /// Loads the order totals.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private void LoadOrderTotals(YahooOrderEntity orderEntity, YahooOrder order)
        {
            orderEntity.OrderTotal = order.OrderTotals.Total;
        }

        /// <summary>
        /// Loads an order charge.
        /// </summary>
        /// <param name="order">The order entity.</param>
        /// <param name="chargeType">Type of the charge.</param>
        /// <param name="chargeDescription">The charge description.</param>
        /// <param name="amount">The amount.</param>
        private void LoadOrderCharge(YahooOrderEntity order, string chargeType, string chargeDescription, decimal amount)
        {
            OrderChargeEntity charge = InstantiateOrderCharge(order);

            charge.Type = chargeType;
            charge.Description = chargeDescription;
            charge.Amount = amount;
        }

        /// <summary>
        /// Loads the order items.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private void LoadOrderItems(YahooOrderEntity orderEntity, YahooOrder order)
        {
            foreach (YahooItem item in order.ItemList.Item)
            {
                LoadOrderItem(orderEntity, item);
            }
        }

        /// <summary>
        /// Loads an order item.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="item">The item DTO.</param>
        private void LoadOrderItem(YahooOrderEntity orderEntity, YahooItem item)
        {
            YahooOrderItemEntity itemEntity = (YahooOrderItemEntity) InstantiateOrderItem(orderEntity);

            itemEntity.YahooProductID = item.ItemID;
            itemEntity.Code = WebUtility.HtmlDecode(item.ItemCode);
            itemEntity.Name = WebUtility.HtmlDecode(item.Description);
            itemEntity.Quantity = item.Quantity;
            itemEntity.UnitPrice = item.UnitPrice;
            itemEntity.Url = item.URL;

            if (!item.ThumbnailUrl.IsNullOrWhiteSpace())
            {
                // Thumbnail node format - <img border=0 width=42 height=70 src=Actual_Thumbnail_URL>
                string[] thumbnailSplit = item.ThumbnailUrl.Split(new[] { "src=" }, StringSplitOptions.RemoveEmptyEntries);

                if (thumbnailSplit.Length == 2)
                {
                    itemEntity.Thumbnail = thumbnailSplit[1].TrimEnd('>');
                }
                else
                {
                    log.Error("An error occurred retrieving the item thumbnail url");
                }
            }

            if (CatalogEnabled)
            {
                itemEntity.Weight = GetItemWeight(item.ItemID);
            }

            LoadOrderItemAttributes(itemEntity, item);
        }

        /// <summary>
        /// Attempt to get the item weight from the cache, if it is not there get it from Yahoo
        /// </summary>
        /// <param name="itemID">The item ID.</param>
        /// <returns></returns>
        /// <exception cref="YahooException">Attempted to perform Yahoo actions on a non-Yahoo store</exception>
        private double GetItemWeight(string itemID)
        {
            YahooStoreEntity store = Store as YahooStoreEntity;
            if (store == null)
            {
                throw new YahooException("Attempted to perform Yahoo actions on a non-Yahoo store");
            }

            LruCache<string, YahooCatalogItem> productWeightCache = YahooProductWeightCache.Instance.GetStoreProductWeightCache(store.YahooStoreID);

            YahooCatalogItem item = productWeightCache[itemID];

            if (item != null)
            {
                return item.ShipWeight;
            }

            YahooResponse response;
            try
            {
                log.Info("Attempting to get item information from yahoo api.");

                // If item is null, that means it is not in the cache
                // So make the call to get the item weight and cache it
                response = webClient.GetItem(store, itemID);

                log.Info("Retrieved item information from yahoo api.");
            }
            catch (NullReferenceException ex)
            {
                log.Error("Error retrieving item information", ex);
                return 0;
            }

            if (response == null)
            {
                return 0;
            }

            // Check the get item response for the catalog access forbidden error code. If
            // we get the error, set CatalogEnabled to false and return 0
            if (response.ErrorMessages?.Error?.FirstOrDefault()?.Code == CatalogAccessForbidden)
            {
                CatalogEnabled = false;
                return 0;
            }

            YahooCatalogItem newItem = response.ResponseResourceList?.Catalog?.ItemList?.Item?.FirstOrDefault();

            if (newItem == null)
            {
                log.Error("Error deserializing XML response from Yahoo Catalog API");
                return 0;
            }

            productWeightCache[itemID] = newItem;

            return newItem.ShipWeight;
        }

        /// <summary>
        /// Loads the item attributes.
        /// </summary>
        /// <param name="itemEntity">The item entity.</param>
        /// <param name="item">The item DTO.</param>
        private void LoadOrderItemAttributes(YahooOrderItemEntity itemEntity, YahooItem item)
        {
            if (item.SelectedOptionList?.Option == null)
            {
                return;
            }

            foreach (YahooOption option in item.SelectedOptionList.Option)
            {
                OrderItemAttributeEntity attribute = InstantiateOrderItemAttribute(itemEntity);

                attribute.Name = WebUtility.HtmlDecode(option.Name);
                attribute.Description = WebUtility.HtmlDecode(option.Value);
                attribute.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Checks if there are new orders to download.
        /// </summary>
        /// <returns>List of order numbers to be downloaded</returns>
        private async Task<List<long>> CheckForNewOrders()
        {
            YahooStoreEntity store = Store as YahooStoreEntity;

            if (store == null)
            {
                throw new DownloadException("Attempted to check for orders on a null store");
            }

            // We want to use the highest order number we have as our next
            // starting number. We want to make sure the order number we
            // start with exists in Yahoo, so we don't get an error.
            long currentOrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);
            long nextOrderNumber = currentOrderNumber - 1;

            long? backupNumber = store.BackupOrderNumber;

            List<long> orders = new List<long>();

            bool initialDownload = false;

            // This should only happen on the store's initial download
            if (nextOrderNumber == 0 && backupNumber != null)
            {
                nextOrderNumber = backupNumber.Value;
                initialDownload = true;
                orders.Add(-1);
            }

            orders.AddRange(await GetOrderNumbers(nextOrderNumber).ConfigureAwait(false));

            // This means no new orders retrieved
            if (!initialDownload && orders.Count == 1)
            {
                return new List<long>();
            }

            return orders;
        }

        /// <summary>
        /// Gets a list of order numbers to download
        /// </summary>
        /// <param name="nextOrderNumber">The next order number to start from</param>
        private async Task<List<long>> GetOrderNumbers(long nextOrderNumber)
        {
            bool done = false;

            var store = Store as IYahooStoreEntity;
            List<long> orders = new List<long>();

            while (!done)
            {
                YahooResponse response = webClient.GetOrderRange(store, nextOrderNumber);

                long? nextNumberToTry = await CheckForErrors(response, nextOrderNumber).ConfigureAwait(false);

                if (nextNumberToTry != null)
                {
                    nextOrderNumber = nextNumberToTry.Value;
                    response = webClient.GetOrderRange(store, nextOrderNumber);
                }

                // After getting more orders, Check if the last order number in the list is the same
                // as the starting order number last used to retrieve orders. If so we know we
                // have retrieved all of the orders.
                if (orders.Count != 0 && response.ResponseResourceList.OrderList.Order.Last().OrderID == nextOrderNumber)
                {
                    done = true;
                    continue;
                }

                // We know we haven't reached to end of the list, so add the list of orders just retrieved
                // to the running list of orders to download.
                if (response != null)
                {
                    // if orders isn't empty we want to remove the last in the list,
                    // since it will be the first in the new list.
                    if (orders.Count > 0)
                    {
                        orders.RemoveAt(orders.Count - 1);
                    }

                    orders.AddRange(
                        response.ResponseResourceList.OrderList.Order.Select(order => order.OrderID).ToList());
                }

                // When the loop starts over, we want to use the last order number we retrieved
                // as the starting order number for the next retrieval
                nextOrderNumber = orders.Last();
            }

            return orders;
        }

        /// <summary>
        /// Checks for invalid start range error and handle it appropriately
        /// </summary>
        /// <param name="response">The response to check for errors.</param>
        /// <param name="backupOrderNumber">The backup order number.</param>
        /// <returns>A new valid Yahoo order ID or null if no errors</returns>
        public async Task<long?> CheckForErrors(YahooResponse response, long? backupOrderNumber)
        {
            // Check if any errors in response. If not, return null
            if (response?.ErrorResourceList?.Error == null)
            {
                return null;
            }

            // We know we have errors, lets check for the invalid start range error.
            // If it is, try to use the backup order number if it is set.
            // If it is not set, try the last 5 order numbers ShipWorks has for that store
            // and throw a DownloadException telling the user to set a new starting order number
            // If its not an invalid start range error, throw DownloadException with Yahoo's error message.
            foreach (YahooError error in response.ErrorResourceList.Error.Where(error => error.Code != InvalidStartRangeErrorCode))
            {
                throw new DownloadException(error.Message);
            }

            var store = Store as IYahooStoreEntity;

            // If backup order number exists, try that and check for errors
            if (backupOrderNumber != null)
            {
                YahooResponse newResponse = webClient.GetOrderRange(store, backupOrderNumber.Value);

                if (newResponse?.ErrorResourceList?.Error == null)
                {
                    return backupOrderNumber;
                }
            }

            // -2 here because the original order number we tried was GetNextOrderNumberAsync() - 1
            long currentOrderNumber = await GetNextOrderNumberAsync().ConfigureAwait(false);
            long nextOrderNumber = currentOrderNumber - 2;

            // Backup order number didn't exist, lets try the last 5 order numbers
            for (int i = 0; i < 5; i++)
            {
                YahooResponse newResponse = webClient.GetOrderRange(store, nextOrderNumber);

                if (newResponse?.ErrorResourceList?.Error == null)
                {
                    return nextOrderNumber;
                }

                nextOrderNumber--;
            }

            // If we're here, the user doesn't have a backup order number set and the last 5 order
            // numbers didn't work. So throw a download exception with a message telling them to
            // set a new starting order number.
            throw new DownloadException("You either have no orders to download or need to set a new starting order number in store settings");
        }

        /// <summary>
        /// Parses the yahoo date string into a DateTime
        /// </summary>
        /// <param name="yahooDateTime">The yahoo date time.</param>
        public DateTime ParseYahooDateTime(string yahooDateTime)
        {
            // We get DateTime format - Fri Mar 26 12:25:15 2010 GMT
            // But DateTime.Parse can't parse it, but it can parse a very similar format of Mon, 15 Jun 2009 20:45:30 GMT
            // So here we just switch things around then parse
            string[] tokens = yahooDateTime.Split(' ');

            tokens[0] = tokens[0] + ",";

            string temp = tokens[1];
            tokens[1] = tokens[2];
            tokens[2] = temp;

            temp = tokens[3];
            tokens[3] = tokens[4];
            tokens[4] = temp;

            temp = tokens.Aggregate(string.Empty, (current, token) => $"{current} {token}");

            DateTime result;

            // When try parse fails, it returns DateTime.MinValue. We don't want to actually set that
            // as the date, so throw an error. If Yahoo ever changes their date format, this method will
            // more than likely have to change.
            if (DateTime.TryParse(temp, out result) == false)
            {
                throw new YahooException("Error parsing date in Yahoo's response");
            }

            return result.ToUniversalTime();
        }
    }
}

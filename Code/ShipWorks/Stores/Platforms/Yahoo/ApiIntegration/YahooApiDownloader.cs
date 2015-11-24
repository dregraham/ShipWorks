using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using Quartz.Util;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration.DTO;

namespace ShipWorks.Stores.Platforms.Yahoo.ApiIntegration
{
    public class YahooApiDownloader : StoreDownloader
    {
        public YahooApiDownloader(StoreEntity store) : base(store)
        {
        }

        public YahooApiDownloader(StoreEntity store, StoreType storeType) : base(store, storeType)
        {
        }

        public void ForceDownload()
        {
            Download();
        }

        /// <summary>
        /// Kicks off download for the store
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Checking for new orders...";

            try
            {
                List<long> orderList = CheckForNewOrders();

                ((YahooStoreEntity) Store).BackupOrderNumber = null;

                if (orderList == null)
                {
                    throw new YahooException("Error checking for orders");
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
                    DownloadNewOrders(orderList);

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
        private void DownloadNewOrders(List<long> orderList)
        {
            int expectedCount = orderList.Count;

            foreach (long orderID in orderList)
            {
                YahooApiWebClient client = new YahooApiWebClient(Store as YahooStoreEntity);

                YahooResponse response = DeserializeResponse<YahooResponse>(client.GetOrder(orderID));

                // Set the progress detail
                Progress.Detail = $"Processing order {QuantitySaved + 1} of {expectedCount} ...";
                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / expectedCount);

                LoadOrder(response.ResponseResourceList.OrderList.Order.FirstOrDefault());

                // Check for cancellation
                if (Progress.IsCancelRequested)
                {
                    return;
                } 
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        /// <param name="order">The order DTO.</param>
        /// <exception cref="YahooException">$Failed to instantiate order {order.OrderID}</exception>
        public YahooOrderEntity LoadOrder(YahooOrder order)
        {
            YahooOrderEntity orderEntity = InstantiateOrder(new YahooOrderIdentifier((order.OrderID.ToString()))) as YahooOrderEntity;
            if (orderEntity == null)
            {
                throw new YahooException($"Failed to instantiate order {order.OrderID}");
            }

            if (orderEntity.IsNew)
            {
                orderEntity.OnlineStatusCode = order.StatusList.OrderStatus.Last().StatusID;
                orderEntity.OnlineStatus = EnumHelper.GetDescription((YahooApiOrderStatus)int.Parse(orderEntity.OnlineStatusCode.ToString()));
                orderEntity.RequestedShipping = $"{order.CartShipmentInfo.Shipper} {order.ShipMethod}";
                orderEntity.OrderDate = ParseYahooDateTime(order.CreationTime);
                orderEntity.OnlineLastModified = ParseYahooDateTime(order.LastUpdatedTime);
                orderEntity.OrderNumber = order.OrderID;
                orderEntity.YahooOrderID = order.OrderID.ToString();

                LoadAddress(orderEntity, order);
                LoadOrderItems(orderEntity, order);
                LoadOrderTotals(orderEntity, order);
                LoadOrderCharges(orderEntity, order);
                LoadOrderGiftMessages(orderEntity, order);
                LoadOrderNotes(orderEntity, order);
                LoadOrderPayments(orderEntity, order); 
            }

            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "YahooApiDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(orderEntity));

            return orderEntity;
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
        private void LoadOrderNotes(YahooOrderEntity orderEntity, YahooOrder order)
        {
            if (!order.MerchantNotes.IsNullOrWhiteSpace())
            {
                // Merchant notes combines all notes into one string, separated by a '>'
                string[] noteArray = order.MerchantNotes.Split('>');

                foreach (string note in noteArray)
                {
                    if (note.IsNullOrWhiteSpace())
                    {
                        continue;
                    }
                    // Here we are extracting the date that prefixes the note, the date and note are seperated by a ':'
                    // We want to substring off of the second ":" though, since one appears in the dates time
                    int dateEndIndex = note.IndexOf(":", note.IndexOf(":", StringComparison.Ordinal) + 1, StringComparison.Ordinal);
                    
                    DateTime noteDate = DateTime.Parse(note.Substring(0, dateEndIndex));

                    string noteText = note.Substring(dateEndIndex + 1);

                    InstantiateNote(orderEntity, noteText, noteDate, NoteVisibility.Internal);
                }
            }

            if (!order.BuyerComments.IsNullOrWhiteSpace())
            {
                InstantiateNote(orderEntity, order.BuyerComments, ParseYahooDateTime(order.CreationTime),
                    NoteVisibility.Public);
            }
        }

        /// <summary>
        /// Loads the order gift messages.
        /// </summary>
        /// <param name="orderEntity">The order entity.</param>
        /// <param name="order">The order DTO.</param>
        private void LoadOrderGiftMessages(YahooOrderEntity orderEntity, YahooOrder order)
        {
            if (order.OrderTotals.GiftWrap !=0 || !order.GiftMessage.IsNullOrWhiteSpace())
            {
                YahooOrderItemEntity item = (YahooOrderItemEntity)InstantiateOrderItem(orderEntity);

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
            YahooOrderItemEntity itemEntity = (YahooOrderItemEntity)InstantiateOrderItem(orderEntity);

            itemEntity.YahooProductID = item.ItemID;
            itemEntity.Code = item.ItemCode;
            itemEntity.Name = item.ItemCode;
            itemEntity.Quantity = item.Quantity;
            itemEntity.UnitPrice = item.UnitPrice;
            itemEntity.Description = item.Description;
            itemEntity.Url = item.URL; 
            itemEntity.Thumbnail = item.ThumbnailUrl;
            itemEntity.Weight = GetItemWeight(item.ItemID);

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

            // If item is null, that means it is not in the cache
            // So make the call to get the item weight and cache it
            YahooApiWebClient client = new YahooApiWebClient(store);
            YahooResponse response = DeserializeResponse<YahooResponse>(client.GetItem(itemID));

            YahooCatalogItem newItem = response.ResponseResourceList.Catalog.ItemList.Item.FirstOrDefault();

            if (newItem == null)
            {
                throw new YahooException("Error deserializing XML response from Yahoo Catalog API");
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

                attribute.Name = option.Name;
                attribute.Description = option.Value;
                attribute.UnitPrice = 0;
            }
        }

        /// <summary>
        /// Checks if there are new orders to download.
        /// </summary>
        /// <returns>List of order numbers to be downloaded</returns>
        private List<long> CheckForNewOrders()
        {
            YahooStoreEntity store = Store as YahooStoreEntity;

            YahooApiWebClient client = new YahooApiWebClient(store);

            List<long> orders = new List<long>();

            bool done = false;

            long nextOrderNumber = GetNextOrderNumber() - 1;

            long? backupNumber = store?.BackupOrderNumber;

            // This should only happen on the stores initial download
            if (nextOrderNumber == 0 && backupNumber != null)
            {
               nextOrderNumber = backupNumber.Value;
            }

            int backupTries = 2;
            while (!done)
            {
                string responseXml = client.GetOrderRange(nextOrderNumber);

                YahooResponse response = DeserializeResponse<YahooResponse>(responseXml);
                
                if (CheckForErrors(response, ref backupNumber, ref nextOrderNumber, ref backupTries))
                {
                    continue;
                }

                // check if the last order in the last after getting more orders is the same
                // as the starting order number last used to retrieve orders. If so we know we
                // have retrieved all of the orders.
                if (orders.Count != 0 && orders.Last() == nextOrderNumber)
                {
                    done = true;
                    continue;
                }

                // We know we haven't reached to end of the list, so add the list of orders just retrieved
                // to the running list of orders to download.
                if (response != null)
                {
                    orders.AddRange(
                        response.ResponseResourceList.OrderList.Order.Select(order => order.OrderID).ToList());
                }

                // When the loop starts over, we want to use the last order number we retrieved
                // as the starting order number for the next retrieval
                nextOrderNumber = orders.Last();

                backupNumber = null;
            }

            return orders;
        }

        private bool CheckForErrors(YahooResponse response, ref long? backupNumber, ref long nextOrderNumber,
            ref int backupTries)
        {
            // If invalid start range error occurs try to use the backup order number.
            // If it is not set, attempt to use the last 5 highest order numbers in ShipWorks.
            // If none of those work, throw a download error telling the user to go to the 
            // store settings and change the starting order number
            if (response?.ErrorResourceList?.Error != null)
            {
                if (backupNumber != null)
                {
                    nextOrderNumber = backupNumber.Value;
                    backupNumber = null;
                    return true;
                }

                if (backupTries >= 7)
                {
                    throw new DownloadException(
                        "You either have no orders to download or need to set a new starting order number in store settings");
                }

                nextOrderNumber = GetNextOrderNumber() - backupTries;
                backupTries++;
                return true;
            }
            return false;
        }

        private DateTime ParseYahooDateTime(string yahooDateTime)
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

            return DateTime.Parse(temp);
        }

        /// <summary>
        /// Deserializes the response XML
        /// </summary>
        public static T DeserializeResponse<T>(string xml)
        {
            try
            {
                return SerializationUtility.DeserializeFromXml<T>(xml);
            }
            catch (InvalidOperationException ex)
            {
                if (!xml.Contains("ErrorResourceList"))
                {
                    YahooResponse errorResponse = SerializationUtility.DeserializeFromXml<YahooResponse>(xml);

                    foreach (YahooError error in errorResponse.ErrorResourceList.Error)
                    {
                        throw new YahooException(error.Message, ex);
                    }
                }

                throw new YahooException($"Error Deserializing {typeof(T).Name}", ex);
            }
        }
    }
}

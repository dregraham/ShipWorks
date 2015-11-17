using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using Quartz.Util;
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

        /// <summary>
        /// Kicks off download for the store
        /// </summary>
        protected override void Download()
        {
            Progress.Detail = "Checking for new orders...";

            try
            {
                List<long> orderList = CheckForNewOrders();

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
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Downloads the new orders.
        /// </summary>
        /// <param name="orderList">The order list.</param>
        private void DownloadNewOrders(List<long> orderList)
        {
            foreach (long orderID in orderList)
            {
                YahooApiWebClient client = new YahooApiWebClient(Store as YahooStoreEntity);

                YahooOrder order = DeserializeResponse<YahooOrder>(client.GetOrder(orderID));

                LoadOrder(order);
            }
        }

        /// <summary>
        /// Loads the order.
        /// </summary>
        /// <param name="order">The order DTO.</param>
        /// <exception cref="YahooException">$Failed to instantiate order {order.OrderID}</exception>
        private void LoadOrder(YahooOrder order)
        {
            YahooOrderEntity orderEntity = InstantiateOrder(new YahooOrderIdentifier((order.OrderID.ToString()))) as YahooOrderEntity;
            if (orderEntity == null)
            {
                throw new YahooException($"Failed to instantiate order {order.OrderID}");
            }

            orderEntity.OnlineStatusCode = order.StatusList.OrderStatus.Last().StatusID;
            orderEntity.OnlineStatus = EnumHelper.GetDescription((YahooApiOrderStatus) int.Parse(orderEntity.OnlineStatusCode.ToString()));
            orderEntity.RequestedShipping = $"{order.CartShipmentInfo.Shipper} {order.ShipMethod}";
            orderEntity.OrderDate = DateTime.Parse(order.CreationTime);
            orderEntity.OnlineLastModified = DateTime.Parse(order.LastUpdatedTime);

            LoadAddress(orderEntity, order);
            LoadOrderItems(orderEntity, order);
            LoadOrderTotals(orderEntity, order);
            LoadOrderCharges(orderEntity, order);
            LoadOrderGiftMessages(orderEntity, order);
            LoadOrderNotes(orderEntity, order);
            LoadOrderPayments(orderEntity, order);
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
            orderEntity.ShipCountryCode = Geography.GetCountryCode(order.ShipToInfo.AddressInfo.Country);
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
            orderEntity.BillCountryCode = Geography.GetCountryCode(order.BillToInfo.AddressInfo.Country);
            orderEntity.BillStateProvCode = Geography.GetStateProvCode(order.BillToInfo.AddressInfo.State);
            orderEntity.BillPostalCode = order.BillToInfo.AddressInfo.Zip;
        }

        private void LoadOrderPayments(YahooOrderEntity orderEntity, YahooOrder order)
        {
            throw new NotImplementedException();
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
                    // Here we are extracting the date that prefixes the note, the date and note are seperated by a ':'
                    // We want to substring off of the second ":" though, since one appears in the dates time
                    int dateEndIndex = note.IndexOf(":", note.IndexOf(":", StringComparison.Ordinal) + 1, StringComparison.Ordinal) + 1;

                    DateTime noteDate = DateTime.Parse(note.Substring(0, dateEndIndex));
                    string noteText = note.Substring(dateEndIndex + 1);
                    InstantiateNote(orderEntity, noteText, noteDate, NoteVisibility.Internal);
                }
            }

            if (!order.BuyerComments.IsNullOrWhiteSpace())
            {
                InstantiateNote(orderEntity, order.BuyerComments, DateTime.Parse(order.CreationTime),
                    NoteVisibility.Public);
            }
        }

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

            //if (order.OrderTotals.GiftWrap != 0)
            //{
            //    LoadOrderCharge(orderEntity, "GIFT WRAP", "Gift Wrap", order.OrderTotals.GiftWrap);
            //}

            foreach (YahooAppliedPromotion promotion in order.OrderTotals.Promotions.AppliedPromotion)
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
        /// Loads the order charge.
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

        private void LoadOrderItem(YahooOrderEntity orderEntity, YahooItem item)
        {
            YahooOrderItemEntity itemEntity = (YahooOrderItemEntity)InstantiateOrderItem(orderEntity);

            itemEntity.YahooProductID = item.ItemID;
            itemEntity.Code = item.ItemCode;
            itemEntity.Quantity = item.Quantity;
            itemEntity.UnitPrice = item.UnitPrice;
            itemEntity.Description = item.Description;
            itemEntity.Url = item.URL; // Add URL to YahooOrderItem
            itemEntity.Thumbnail = item.ThumbnailUrl;
            itemEntity.Weight = GetItemWeight(item.ItemID);



            LoadOrderItemAttributes(item);
        }

        private double GetItemWeight(string itemID)
        {
            throw new NotImplementedException();
        }

        private void LoadOrderItemAttributes(YahooItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for new orders.
        /// </summary>
        /// <returns>List of order numbers to be downloaded</returns>
        private List<long> CheckForNewOrders()
        {
            YahooApiWebClient client = new YahooApiWebClient(Store as YahooStoreEntity);

            long lastOrderNumber = GetOrderNumberStartingPoint();

            string response = client.GetOrderRange(lastOrderNumber + 1);

            YahooResponseResourceList responseResourceList = DeserializeResponse<YahooResponseResourceList>(response);

            return responseResourceList.OrderList.Order.Select(order => order.OrderID).ToList();
        }

        /// <summary>
        /// Deserializes the response XML
        /// </summary>
        private static T DeserializeResponse<T>(string xml)
        {
            try
            {
                return SerializationUtility.DeserializeFromXml<T>(xml);
            }
            catch (InvalidOperationException ex)
            {
                if (xml.Contains("ErrorResponse"))
                {
                    YahooErrorResourceList errorResponse = SerializationUtility.DeserializeFromXml<YahooErrorResourceList>(xml);
                    throw new YahooException(errorResponse.Error.Message, ex);
                }

                throw new YahooException($"Error Deserializing {typeof(T).Name}", ex);
            }
        }
    }
}

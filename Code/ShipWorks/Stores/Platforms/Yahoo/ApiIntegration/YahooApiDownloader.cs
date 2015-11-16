using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
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

        private void DownloadNewOrders(List<long> orderList)
        {
            foreach (long orderID in orderList)
            {
                YahooApiWebClient client = new YahooApiWebClient(Store as YahooStoreEntity);

                YahooOrder order = DeserializeResponse<YahooOrder>(client.GetOrder(orderID));

                LoadOrder(order);
            }
        }

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

        private void LoadOrderNotes(YahooOrderEntity orderEntity, YahooOrder order)
        {
            throw new NotImplementedException();
        }

        private void LoadOrderGiftMessages(YahooOrderEntity orderEntity, YahooOrder order)
        {
            throw new NotImplementedException();
        }

        private void LoadOrderCharges(YahooOrderEntity orderEntity, YahooOrder order)
        {
            throw new NotImplementedException();
        }

        private void LoadOrderTotals(YahooOrderEntity orderEntity, YahooOrder order)
        {
            throw new NotImplementedException();
        }

        private void LoadOrderItems(YahooOrderEntity orderEntity, YahooOrder order)
        {
            LoadOrderItem();
        }

        private void LoadOrderItem()
        {
            LoadOrderItemAttributes();
        }

        private void LoadOrderItemAttributes()
        {
            throw new NotImplementedException();
        }

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

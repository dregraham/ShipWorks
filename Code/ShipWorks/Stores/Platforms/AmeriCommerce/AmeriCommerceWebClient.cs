using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.AmeriCommerce.WebServices;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using System.Web.Services.Protocols;
using log4net;
using System.Globalization;
using ShipWorks.Shipping;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Web client for communicating with the AmeriCommerce SOAP api
    /// </summary>
    public class AmeriCommerceWebClient
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmeriCommerceWebClient));

        // API security token (encrypted)
        const string securityToken = "aRSxLBYFG1sWpcfl0Bh6PNtFW+58arxEIfu+cK7kv/jVSvXsuykhtQ==";

        Dictionary<int, string> stateCache = new Dictionary<int, string>();
        Dictionary<int, string> countryCache = new Dictionary<int, string>();

        // the store
        AmeriCommerceStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceWebClient(AmeriCommerceStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Tests communication
        /// </summary>
        public void TestConnection()
        {
            GetStore(-1);
        }

        /// <summary>
        /// Gets a Store object from its code
        /// </summary>
        public StoreTrans GetStore(int storeCode)
        {
            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("GetStore"))
                {
                    if (storeCode < 0)
                    {
                        return service.Store_GetCurrent();
                    }
                    else
                    {
                        return service.Store_GetByKey(storeCode);
                    }
                }
            }
            catch (SoapException ex)
            {
                string message = ex.Message;

                if (message.Contains("Unauthorized Request"))
                {
                    message = "Invalid Username or Password.";
                }

                throw new AmeriCommerceException(message, ex);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Gets a list of stores associated with the online account
        /// </summary>
        public List<StoreTrans> GetStores()
        {
            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("GetStores"))
                {
                    return service.Store_GetAll().ToList();
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Create the Web Service proxy
        /// </summary>
        private AmeriCommerceDatabaseIO CreateWebService(string logName)
        {
            AmeriCommerceDatabaseIO service = new AmeriCommerceDatabaseIO(new ApiLogEntry(ApiLogSource.AmeriCommerce, logName));

            // setup the authentication headers
            AmeriCommerceHeaderInfo headerInfo = new AmeriCommerceHeaderInfo();
            headerInfo.SecurityToken = SecureText.Decrypt(securityToken, "shipworks");
            headerInfo.UserName = store.Username;
            headerInfo.Password = SecureText.Decrypt(store.Password, store.Username);
            service.AmeriCommerceHeaderInfoValue = headerInfo;

            // configure the url
            service.Url = BuildServiceUrl();

            return service;
        }

        /// <summary>
        /// Constructs the url to the web service based on the store url provided
        /// </summary>
        private string BuildServiceUrl()
        {
            string userUrl = store.StoreUrl;

            if (userUrl.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                userUrl = "https://" + userUrl;
            }

            Uri uri = new Uri(userUrl);
            return string.Format("{0}://{1}/store/ws/AmeriCommerceDb.asmx", uri.Scheme, uri.Host);
        }

        /// <summary>
        /// Gets the available status codes for AmeriCommerce
        /// </summary>
        /// <returns></returns>
        public List<OrderStatusTrans> GetStatusCodes()
        {
            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("GetStatusCodes"))
                {
                    return service.OrderStatus_GetAll().ToList();
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Get orders by modified time
        /// </summary>
        public List<OrderTrans> GetOrders(DateTime? lastModified)
        {
            if (!lastModified.HasValue)
            {
                // By default if there is no other setting go back a year
                lastModified = DateTime.Now.AddYears(-1);
            }

            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("GetOrders"))
                {
                    // retrieve the orders, request using Local time so that timezone information is included in the request.  AmeriCommerce doesn't handle UTC times right.
                    List<OrderTrans> orders = service.Order_GetByEditDateRangeAndStoreID(lastModified.Value.ToLocalTime(), DateTime.UtcNow, store.StoreCode).ToList();

                    // AmeriCommerce sometimes returns orders from other stores, so filter the list down to those for the requested store.
                    orders = orders.Where(o => o.StoreID.Value == store.StoreCode).ToList();

                    // Very important to sort by EditDate so we don't get gaps in the download
                    orders.Sort((a, b) => a.EditDate.GetValue(DateTime.MinValue).CompareTo(b.EditDate.GetValue(DateTime.MinValue)));

                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Fills a stub OrderTrans object
        /// </summary>
        public OrderTrans FillOrderDetail(OrderTrans order)
        {
            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("FillOrderDetail"))
                {
                    // fill related objects
                    order = service.Order_FillOrderItemCollection(order);
                    order = service.Order_FillOrderPaymentCollection(order);
                    order = service.Order_FillCustomFields(order);

                    return order;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Gets an AmeriCommerce address by key
        /// </summary>
        public OrderAddressTrans GetAddress(int addressId)
        {
            if (addressId == 0)
            {
                // return a blank address
                return new OrderAddressTrans();
            }

            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("GetAddress"))
                {
                    return service.OrderAddress_GetByKey(addressId);
                }
            }
            catch (SoapException ex)
            {
                if (ex.Message.IndexOf("AC.SystemException.DBRecordNotFound") > -1)
                {
                    // this is a "test" system problem that shouldn't happen live, return an empty object
                    return new OrderAddressTrans();
                }
                else
                {
                    throw new AmeriCommerceException(ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Returns the state name for a given State ID
        /// </summary>
        public string GetStateCode(int stateId)
        {
            if (stateId == 0)
            {
                return "";
            }

            try
            {
                // if we haven't seen this value, get it from AmeriCommerce
                if (!stateCache.ContainsKey(stateId))
                {
                    using (AmeriCommerceDatabaseIO service = CreateWebService("GetStateCode"))
                    {
                        stateCache[stateId] = service.State_GetByKey(stateId).stateCode;
                    }
                }

                return stateCache[stateId];
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Gets the country code for the americommerce country id
        /// </summary>
        public string GetCountryCode(int countryId)
        {
            if (countryId == 0)
            {
                return "";
            }

            try
            {
                // if we haven't seen this countryId yet, get it from AmeriCommerce
                if (!countryCache.ContainsKey(countryId))
                {
                    using (AmeriCommerceDatabaseIO service = CreateWebService("GetCountryCode"))
                    {
                        countryCache[countryId] = service.Country_GetByKey(countryId).countryCode;
                    }
                }

                return countryCache[countryId];
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Gets the AmeriCommerce customer by key
        /// </summary>
        public CustomerTrans GetCustomer(int customerId)
        {
            // we've had this happen, an order without a customer.  Return an empty record
            if (customerId == 0)
            {
                CustomerTrans blankCustomer = new CustomerTrans();
                blankCustomer.firstName = "";
                blankCustomer.lastName = "";
                blankCustomer.phoneNumber = "";
                blankCustomer.email = "";
                blankCustomer.DisplayName = "";
                blankCustomer.Company = "";

                return blankCustomer;
            }

            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("GetCustomer"))
                {
                    return service.Customer_GetByKey(customerId);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Gets the weight in pounds of the value passed in.
        ///
        /// Per Dustin Holmes email, there are only 2 weight unites (pounds and kg), so
        /// for now we'll just do a hard coded conversion but keep this here so it can easily
        /// be handed off to AmeriCommerce.
        /// </summary>
        public double GetWeightInPounds(decimal weight, int weightUnitId)
        {
            if (weightUnitId == 1)
            {
                // weight is in lbs
                return Convert.ToDouble(weight);
            }
            else if (weightUnitId == 2)
            {
                // weight is in KG
                return Convert.ToDouble(weight) * 2.20462262;
            }
            else
            {
                // don't do any conversion, but log it
                log.WarnFormat("Unknown AmeriCommerce weight unit id '{0}'", weightUnitId);

                // just assuming it's in lbs due to a known Phantom Item weight bug in AmeriCommerce.  Case 89427
                return Convert.ToDouble(weight);
            }
        }

        /// <summary>
        /// Updates the online status of orders
        /// </summary>
        public void UpdateOrderStatus(int orderNumber, int statusCode)
        {
            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("UpdateOrderStatus"))
                {
                    // find the order to be edited
                    OrderTrans orderTrans = service.Order_GetByKey(orderNumber);

                    // update the status id
                    orderTrans.orderStatusID = new DataInt32();
                    orderTrans.orderStatusID.Value = statusCode;

                    // validate the order
                    string result = service.Order_Validate(orderTrans);
                    if (string.Compare(result, "ok", true, CultureInfo.InvariantCulture) != 0)
                    {
                        throw new AmeriCommerceException(string.Format("An error occurred while validating order status: {0}", result));
                    }

                    // save the order with its changes
                    if (!service.Order_Save(orderTrans))
                    {
                        throw new AmeriCommerceException(string.Format("An unknown error occurred while saving order status."));
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Uploads the tracking number for shipments related to order OrderNumber
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;

            if (order.IsManual)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", order.OrderID);
                return;
            }

            try
            {
                using (AmeriCommerceDatabaseIO service = CreateWebService("UploadShipmentDetails"))
                {
                    // retrieve the order
                    OrderTrans orderTrans = service.Order_GetByKey(Convert.ToInt32(order.OrderNumber));
                    orderTrans = service.Order_FillOrderShippingCollection(orderTrans);

                    // get the existing shipment records
                    List<OrderShippingTrans> shipmentRecords = orderTrans.OrderShippingColTrans.ToList();

                    // create and populate the OrderShipping record
                    OrderShippingTrans shippingTrans = CreateOrderShippingTrans(order, shipment);
                    shipmentRecords.Add(shippingTrans);
                    orderTrans.OrderShippingColTrans = shipmentRecords.ToArray();

                    // set the order's tracking number
                    orderTrans.trackingCode = shipment.TrackingNumber;

                    // validate and save
                    string result = service.Order_Validate(orderTrans);
                    if (string.Compare(result, "ok", true, CultureInfo.InvariantCulture) != 0)
                    {
                        throw new AmeriCommerceException(string.Format("An error occurred while validating shipment details: {0}", result));
                    }

                    // perform the save
                    if (!service.Order_Save(orderTrans))
                    {
                        throw new AmeriCommerceException(string.Format("An unknown error occurred while saving tracking number."));
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(AmeriCommerceException));
            }
        }

        /// <summary>
        /// Creates the shipping record to be sent to AmeriCommerce
        /// </summary>
        private OrderShippingTrans CreateOrderShippingTrans(OrderEntity order, ShipmentEntity shipment)
        {
            OrderShippingTrans shippingTrans = new OrderShippingTrans();

            // we're creating a new record here
            shippingTrans.IsNew = true;

            shippingTrans.OrderID = new DataInt32();
            shippingTrans.OrderID.Value = Convert.ToInt32(order.OrderNumber);

            shippingTrans.NumberOfPackages = new DataInt32();
            shippingTrans.NumberOfPackages.Value = GetPackageCount(shipment);
            shippingTrans.EmailSent = false;

            shippingTrans.EditDate = new DataDateTime();
            shippingTrans.EditDate.Value = DateTime.UtcNow;

            shippingTrans.EnterDate = new DataDateTime();
            shippingTrans.EnterDate.Value = shipment.ProcessedDate.Value;

            shippingTrans.EnteredBy = "ShipWorks";

            shippingTrans.ProviderTotalShippingCost = new DataMoney();
            shippingTrans.ProviderTotalShippingCost.Value = shipment.ShipmentCost;

            shippingTrans.TotalWeight = new DataMoney();
            shippingTrans.TotalWeight.Value = Convert.ToDecimal(shipment.TotalWeight);

            shippingTrans.ShippingDate = new DataDateTime();
            shippingTrans.ShippingDate.Value = shipment.ShipDate;

            shippingTrans.ShippingMethod = ShippingManager.GetCarrierName((ShipmentTypeCode)shipment.ShipmentType) + " " + ShippingManager.GetServiceUsed(shipment);
            shippingTrans.TrackingNumbers = shipment.TrackingNumber;

            return shippingTrans;
        }

        /// <summary>
        /// Get the number of packages on the shipment
        /// </summary>
        private int GetPackageCount(ShipmentEntity shipment)
        {
            try
            {
                ShipmentTypeCode shipmentType = (ShipmentTypeCode) shipment.ShipmentType;
                if (shipmentType == ShipmentTypeCode.UpsOnLineTools ||
                    shipmentType == ShipmentTypeCode.UpsWorldShip)
                {
                    // load the particular shipment type details
                    ShipmentTypeManager.GetType(shipment).LoadShipmentData(shipment, true);

                    return shipment.Ups.Packages.Count;
                }
                else if (shipmentType == ShipmentTypeCode.FedEx)
                {
                    // load the particular shipment type details
                    ShipmentTypeManager.GetType(shipment).LoadShipmentData(shipment, true);

                    return shipment.FedEx.Packages.Count;
                }
            }
            catch (ORMConcurrencyException) { }
            catch (ObjectDeletedException) { }
            catch (SqlForeignKeyException) { }

            // All other services - or if there was an error with LoadShipmentData - just have 1 "package"
            return 1;
        }
    }
}

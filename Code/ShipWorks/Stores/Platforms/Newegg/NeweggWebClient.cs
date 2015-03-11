using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using log4net;

using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using System.Diagnostics;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Class that acts as a facade for funneling all requests to the Newegg API. 
    /// </summary>
    public class NeweggWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(NeweggWebClient));

        private NeweggStoreEntity store;
        private IRequestFactory requestFactory;
        private Credentials credentials; 

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggWebClient"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        public NeweggWebClient(NeweggStoreEntity store)
            : this (store, new LiveRequestFactory())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggWebClient"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="requestFactory">The request factory.</param>
        public NeweggWebClient(NeweggStoreEntity store, IRequestFactory requestFactory)
        {
            if (store == null)
            {
                string message = "A null store value was provided to the NeweggWebClient.";
                log.Error(message);
                throw new InvalidOperationException(message);
            }

            this.store = store;
            this.requestFactory = requestFactory;

            credentials = new Credentials(store.SellerID, store.SecretKey);
        }

        /// <summary>
        /// Checks whether the credentials are valid by making a request to the Newegg API.
        /// </summary>
        /// <returns>True if the credentials are valid; otherwise false.</returns>
        public bool AreCredentialsValid()
        {
            ICheckCredentialRequest credentialRequest = requestFactory.CreateCheckCredentialRequest();
            return credentialRequest.AreCredentialsValid(credentials);
        }

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="orderType">Types of orders to be downloaded.</param>
        /// <returns>A DownloadInfo object.</returns>
        public DownloadInfo GetDownloadInfo(DateTime startingPoint, NeweggOrderType orderType)
        {
            // Just create an instance of IDownloadOrderRequest and use the staring point
            // provided to download the orders from Newegg. We're going to download orders
            // up until 3 minutes ago to avoid potential race conditions such as orders
            // being placed at the same time as orders are being downloaded
            DateTime endingPoint = DateTime.UtcNow.AddMinutes(-3);
            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);

            DownloadInfo downloadInfo = downloadRequest.GetDownloadInfo(startingPoint, endingPoint, orderType);
            
            return downloadInfo;
        }

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns>A DownloadInfo object.</returns>
        public DownloadInfo GetDownloadInfo(IEnumerable<NeweggOrderEntity> orderEntities)
        {
            List<Order> neweggOrders = CreateNeweggOrders(orderEntities);

            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);
            DownloadInfo downloadInfo = downloadRequest.GetDownloadInfo(neweggOrders);

            return downloadInfo;
        }

        /// <summary>
        /// Downloads the specified orders from Newegg
        /// </summary>
        /// <param name="orderEntities">The order entities.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An IEnumerable of Newegg Order objects.</returns>
        public IEnumerable<Order> DownloadOrders(IEnumerable<NeweggOrderEntity> orderEntities, int pageNumber)
        {
            List<Order> neweggOrders = CreateNeweggOrders(orderEntities);

            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);
            DownloadResult result = downloadRequest.Download(neweggOrders, pageNumber);

            return result.Body.Orders;
        }

        /// <summary>
        /// Downloads the orders from Newegg.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="endingPoint">The ending point.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="orderType">The type of orders to be downloaded.</param>
        /// <returns>An IEnumerable of Order objects.</returns>
        public IEnumerable<Order> DownloadOrders(DateTime startingPoint, DateTime endingPoint, int pageNumber, NeweggOrderType orderType)
        {            
            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);
            DownloadResult result = downloadRequest.Download(startingPoint, endingPoint, pageNumber, orderType);

            log.InfoFormat("Downloaded {0} orders (\"{1}\" order type(s)) from Newegg between {2} and {3} for seller {4}", 
                result.Body.Orders.Count, Enum.GetName(typeof(NeweggOrderType), orderType), startingPoint.ToString(), endingPoint.ToString(), credentials.SellerId);

            return result.Body.Orders;
        }

        /// <summary>
        /// Converts a list of NeweggOrderEntity objects to a list of Newegg API order iobjects.
        /// </summary>
        /// <param name="orderEntities">The order entities.</param>
        /// <returns>A List of Order objects.</returns>
        private static List<Order> CreateNeweggOrders(IEnumerable<NeweggOrderEntity> orderEntities)
        {
            // Convert the order entities to Newegg orders to be used in the request
            List<Order> neweggOrders = new List<Order>();
            foreach (NeweggOrderEntity entity in orderEntities)
            {
                Order neweggOrder = new Order 
                { 
                    OrderNumber = entity.OrderNumber,                    
                    // All times must be in PST for the Newegg API
                    OrderDateInPacificStandardTime = TimeZoneInfo.ConvertTimeFromUtc(entity.OrderDate, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"))
                };

                neweggOrders.Add(neweggOrder);
            }

            return neweggOrders;
        }

        /// <summary>
        /// Uploads the shipping details.
        /// </summary>
        /// <param name="shipmentEntity">The shipment.</param>
        /// <returns>A ShippingResult containing the results from the request to Newegg.</returns>
        public ShippingResult UploadShippingDetails(ShipmentEntity shipmentEntity)
        {
            // Convert the shipment entity to a shipment object the Newegg API is expecting
            Shipment apiShipment = CreateNeweggShipment(shipmentEntity);
            
            IShippingRequest shippingRequest = requestFactory.CreateShippingRequest(credentials);
            ShippingResult result = shippingRequest.Ship(apiShipment);

            // We'll inspect the result for any failure before returning the result to the caller.
            CheckForShippingFailures(result);
            return result;
        }
        
        /// <summary>
        /// Inspects a ShippingResult for any packages that failed to get updated in Newegg. A NeweggException
        /// containing the details of the failures is thrown if any failures are encountered.
        /// </summary>
        /// <param name="shippingResult">The shipping result.</param>
        private static void CheckForShippingFailures(ShippingResult shippingResult)
        {
            if (shippingResult.PackageSummary.FailedCount > 0)
            {
                // There a packages that failed to ship, so try to pick off the reason why and wrap
                // that in a NeweggException so the user is informed of it.
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendFormat("An error occurred updating the shipping details for order {0}:{1}", shippingResult.Detail.OrderNumber, System.Environment.NewLine);

                foreach (ShipmentPackage package in shippingResult.Detail.Shipment.Packages)
                {
                    // We need to find which packages failed and append the reason for failure
                    // to our error message.
                    if (!package.IsSuccessfullyProcessed)
                    {
                        if (!string.IsNullOrWhiteSpace(package.TrackingNumber))
                        {
                            // There is a tracking number contained in the results so we will 
                            // include it in the error message.
                            errorMessage.AppendFormat("Tracking number {0}: ", package.TrackingNumber);
                        }
                        errorMessage.AppendLine(package.ProcessingDescription);
                    }
                }

                // Throw the exception now that all the information about the failures has been collected
                throw new NeweggException(errorMessage.ToString());
            }
        }

        /// <summary>
        /// Factory method to translate a ShipWorks shipment entity into an object
        /// that the Newegg API is expecting.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A Newegg Shipment object.</returns>
        private Shipment CreateNeweggShipment(ShipmentEntity shipmentEntity)
        {
            Shipment apiShipment = new Shipment();

            try
            {
                ShippingManager.EnsureShipmentLoaded(shipmentEntity);
            }
            catch (ObjectDeletedException)
            {
                // Just continue, we'll assume it's OK to upload the info we do have.
            }
            catch (SqlForeignKeyException)
            {
                // Just continue, we'll assume it's OK to upload the info we do have.
            }

            // The only fields required by Newegg are header, tracking number, carrier, service, 
            // and item's seller number and the quantity of each item shipped. 
            apiShipment.Header.OrderNumber = shipmentEntity.Order.OrderNumber;
            apiShipment.Header.SellerId = credentials.SellerId;

            ShipmentPackage package = new ShipmentPackage();
            package.TrackingNumber = shipmentEntity.TrackingNumber;
            
            package.ShipDateInPacificStandardTime = ConvertUtcToPacificStandardTime(shipmentEntity.ShipDate);
            package.ShipCarrier = GetCarrierCode(shipmentEntity);

            package.ShipService = GetShipService(shipmentEntity);

            package.ShipFromAddress1 = shipmentEntity.OriginStreet1;
            package.ShipFromAddress2 = shipmentEntity.OriginStreet2;
            package.ShipFromCity = shipmentEntity.OriginCity;
            package.ShipFromState = shipmentEntity.OriginStateProvCode;
            package.ShipFromZipCode = shipmentEntity.OriginPostalCode;

            foreach (NeweggOrderItemEntity item in shipmentEntity.Order.OrderItems)
            {
                // We won't be shipping partially shipping items, so the quantity of items shipped will
                // always be the same as those ordered.
                package.Items.Add(new ShippedItem { SellerPartNumber = item.SellerPartNumber, QuantityShipped = (int)item.Quantity});
            }
            
            apiShipment.Packages.Add(package);

            return apiShipment;
        }

        /// <summary>
        /// Gets the carrier code.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns></returns>
        public static string GetCarrierCode(ShipmentEntity shipmentEntity)
        {
            string carrierCode = string.Empty;
            switch(((ShipmentTypeCode)shipmentEntity.ShipmentType))
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    carrierCode = "USPS";
                    break;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia shipment, check to see if it's DHL
                    if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        carrierCode = "DHL";
                    }
                    else if (shipmentEntity.Postal != null && ShipmentTypeManager.IsEndiciaConsolidator((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        carrierCode = "Consolidator";
                    }
                    else
                    {
                        // Use the default carrier for other Endicia types
                        carrierCode = "USPS";
                    }
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = "FedEX";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierCode = "UPS";

                    // Adjust tracking details per Mail Innovations and others
                    if (UpsUtility.IsUpsMiService((UpsServiceType)shipmentEntity.Ups.Service))
                    {
                        if (shipmentEntity.Ups.UspsTrackingNumber.Length > 0)
                        {
                            carrierCode = "UPS MI";
                        }
                    }

                    break;

                case ShipmentTypeCode.OnTrac:
                    carrierCode = "OnTrac";
                    break;

                case ShipmentTypeCode.iParcel:
                    carrierCode = "i-parcel";
                    break;

                case ShipmentTypeCode.Other:
                    carrierCode = shipmentEntity.Other.Carrier;
                    break;

                default:
                    Debug.Fail("Unhandled ShipmentTypeCode in NewEgg.GetCarrierCode");
                    carrierCode = "Other";
                    break;
            }
            
            return carrierCode;
        }

        /// <summary>
        /// Gets the ship service.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns></returns>
        private static string GetShipService(ShipmentEntity shipmentEntity)
        {
            ShippingManager.EnsureShipmentLoaded(shipmentEntity);
            ShipmentTypeCode type = (ShipmentTypeCode)shipmentEntity.ShipmentType;

            string service = "NONE";

            switch (type)
            {
                case ShipmentTypeCode.Other:
                    service = shipmentEntity.Other.Service;
                    break;

                case ShipmentTypeCode.FedEx:
                    FedExServiceType fedExServiceType = (FedExServiceType)shipmentEntity.FedEx.Service;
                    service = EnumHelper.GetDescription(fedExServiceType);
                    break;
                     
                case ShipmentTypeCode.UpsWorldShip:
                case ShipmentTypeCode.UpsOnLineTools:
                    UpsServiceType upsServiceType = (UpsServiceType)shipmentEntity.Ups.Service;
                    service = EnumHelper.GetDescription(upsServiceType);
                    break;

                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Usps:
                    PostalServiceType uspsType = (PostalServiceType)shipmentEntity.Postal.Service;
                    service = EnumHelper.GetDescription(uspsType);
                    break;

                case ShipmentTypeCode.iParcel:
                    var serviceType = (iParcelServiceType) shipmentEntity.IParcel.Service;
                    service = EnumHelper.GetDescription(serviceType);
                    break;
            }

            return service;
        }

        /// <summary>
        /// Converts the UTC date to pacific standard time.
        /// </summary>
        /// <param name="utcDateTime">The UTC date time.</param>
        /// <returns>A DateTime in PST.</returns>
        private static DateTime ConvertUtcToPacificStandardTime(DateTime utcDateTime)
        {
            TimeZoneInfo pacificStandardTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, pacificStandardTimeZone);            
        }
    }
}

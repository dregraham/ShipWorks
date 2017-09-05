using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.CredentialValidation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Class that acts as a facade for funneling all requests to the Newegg API.
    /// </summary>
    [Component]
    public class NeweggWebClient : INeweggWebClient
    {
        // Logger
        private readonly ILog log;
        private readonly IRequestFactory requestFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggWebClient"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="requestFactory">The request factory.</param>
        public NeweggWebClient(IRequestFactory requestFactory, Func<Type, ILog> createLogger)
        {
            this.requestFactory = requestFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Checks whether the credentials are valid by making a request to the Newegg API.
        /// </summary>
        /// <returns>True if the credentials are valid; otherwise false.</returns>
        public async Task<bool> AreCredentialsValid(INeweggStoreEntity store)
        {
            var credentials = GetCredentialsFrom(store);
            ICheckCredentialRequest credentialRequest = requestFactory.CreateCheckCredentialRequest();
            return await credentialRequest.AreCredentialsValid(credentials).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="orderType">Types of orders to be downloaded.</param>
        /// <returns>A DownloadInfo object.</returns>
        public async Task<DownloadInfo> GetDownloadInfo(INeweggStoreEntity store, DateTime startingPoint, NeweggOrderType orderType)
        {
            var credentials = GetCredentialsFrom(store);

            // Just create an instance of IDownloadOrderRequest and use the staring point
            // provided to download the orders from Newegg. We're going to download orders
            // up until 3 minutes ago to avoid potential race conditions such as orders
            // being placed at the same time as orders are being downloaded
            DateTime endingPoint = DateTime.UtcNow.AddMinutes(-3);
            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);

            DownloadInfo downloadInfo = await downloadRequest.GetDownloadInfo(startingPoint, endingPoint, orderType).ConfigureAwait(false);

            return downloadInfo;
        }

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns>A DownloadInfo object.</returns>
        public DownloadInfo GetDownloadInfo(INeweggStoreEntity store, IEnumerable<NeweggOrderEntity> orderEntities)
        {
            var credentials = GetCredentialsFrom(store);
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
        public async Task<IEnumerable<Order>> DownloadOrders(INeweggStoreEntity store, IEnumerable<NeweggOrderEntity> orderEntities, int pageNumber)
        {
            var credentials = GetCredentialsFrom(store);
            List<Order> neweggOrders = CreateNeweggOrders(orderEntities);

            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);
            DownloadResult result = await downloadRequest.Download(neweggOrders, pageNumber).ConfigureAwait(false);

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
        public async Task<IEnumerable<Order>> DownloadOrders(INeweggStoreEntity store, Range<DateTime> downloadRange, int pageNumber, NeweggOrderType orderType)
        {
            var credentials = GetCredentialsFrom(store);

            IDownloadOrderRequest downloadRequest = requestFactory.CreateDownloadOrderRequest(credentials);
            DownloadResult result = await downloadRequest.Download(downloadRange.Start, downloadRange.End, pageNumber, orderType).ConfigureAwait(false);

            log.InfoFormat("Downloaded {0} orders (\"{1}\" order type(s)) from Newegg between {2} and {3} for seller {4}",
                result.Body.Orders.Count, Enum.GetName(typeof(NeweggOrderType), orderType), downloadRange.Start.ToString(), downloadRange.End.ToString(), credentials.SellerId);

            return result.Body.Orders;
        }

        /// <summary>
        /// Converts a list of NeweggOrderEntity objects to a list of Newegg API order objects.
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
        /// <returns>Status of the shipment</returns>
        public async Task<string> UploadShippingDetails(INeweggStoreEntity store, ShipmentEntity shipmentEntity, long orderNumber, IEnumerable<ItemDetails> items)
        {
            var credentials = GetCredentialsFrom(store);

            // Convert the shipment entity to a shipment object the Newegg API is expecting
            Shipment apiShipment = CreateNeweggShipment(shipmentEntity, orderNumber, items, credentials);

            IShippingRequest shippingRequest = requestFactory.CreateShippingRequest(credentials);
            var result = await shippingRequest.Ship(apiShipment).ConfigureAwait(false);

            // We'll inspect the result for any failure before returning the result to the caller.
            CheckForShippingFailures(result);

            return result.Detail.OrderStatus;
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
        /// <param name="shipment">The shipment entity.</param>
        /// <returns>A Newegg Shipment object.</returns>
        private Shipment CreateNeweggShipment(ShipmentEntity shipment, long orderNumber, IEnumerable<ItemDetails> items, Credentials credentials)
        {
            Shipment apiShipment = new Shipment();

            try
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
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
            apiShipment.Header.OrderNumber = orderNumber;
            apiShipment.Header.SellerId = credentials.SellerId;

            ShipmentPackage package = new ShipmentPackage();
            package.TrackingNumber = shipment.TrackingNumber;

            package.ShipDateInPacificStandardTime = ConvertUtcToPacificStandardTime(shipment.ShipDate);
            package.ShipCarrier = GetCarrierCode(shipment);

            package.ShipService = GetShipService(shipment);

            package.ShipFromAddress1 = shipment.OriginStreet1;
            package.ShipFromAddress2 = shipment.OriginStreet2;
            package.ShipFromCity = shipment.OriginCity;
            package.ShipFromState = shipment.OriginStateProvCode;
            package.ShipFromZipCode = shipment.OriginPostalCode;

            // We won't be shipping partially shipping items, so the quantity of items shipped will
            // always be the same as those ordered.
            package.Items = items
                .Select(x => new ShippedItem { SellerPartNumber = x.SellerPartNumber, QuantityShipped = (int) x.Quantity })
                .ToList();

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
            switch (((ShipmentTypeCode) shipmentEntity.ShipmentType))
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    carrierCode = "USPS";
                    break;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia shipment, check to see if it's DHL
                    if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType) shipmentEntity.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        carrierCode = "DHL";
                    }
                    else if (shipmentEntity.Postal != null && ShipmentTypeManager.IsConsolidator((PostalServiceType) shipmentEntity.Postal.Service))
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
                    if (UpsUtility.IsUpsMiService((UpsServiceType) shipmentEntity.Ups.Service))
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
            ShipmentTypeCode type = (ShipmentTypeCode) shipmentEntity.ShipmentType;

            string service = "NONE";

            switch (type)
            {
                case ShipmentTypeCode.Other:
                    service = shipmentEntity.Other.Service;
                    break;

                case ShipmentTypeCode.FedEx:
                    FedExServiceType fedExServiceType = (FedExServiceType) shipmentEntity.FedEx.Service;
                    service = EnumHelper.GetDescription(fedExServiceType);
                    break;

                case ShipmentTypeCode.UpsWorldShip:
                case ShipmentTypeCode.UpsOnLineTools:
                    UpsServiceType upsServiceType = (UpsServiceType) shipmentEntity.Ups.Service;
                    service = EnumHelper.GetDescription(upsServiceType);
                    break;

                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Usps:
                    PostalServiceType uspsType = (PostalServiceType) shipmentEntity.Postal.Service;
                    if (uspsType == PostalServiceType.GlobalPostEconomyIntl || uspsType == PostalServiceType.GlobalPostSmartSaverEconomyIntl)
                    {
                        uspsType = PostalServiceType.InternationalFirst;
                    }
                    if (uspsType == PostalServiceType.GlobalPostStandardIntl || uspsType == PostalServiceType.GlobalPostSmartSaverStandardIntl)
                    {
                        uspsType = PostalServiceType.InternationalPriority;
                    }

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

        /// <summary>
        /// Create credentials from a store
        /// </summary>
        private Credentials GetCredentialsFrom(INeweggStoreEntity store) =>
            new Credentials(store.SellerID, store.SecretKey, (NeweggChannelType) store.Channel);
    }
}

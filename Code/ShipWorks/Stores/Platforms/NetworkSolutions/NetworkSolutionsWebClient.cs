using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.NetworkSolutions.WebServices;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using System.Web.Services.Protocols;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Interapptive.Shared.UI;
using System.Xml;
using log4net;
using Interapptive.Shared.Utility;
using System.Xml.XPath;
using Interapptive.Shared.Win32;
using ShipWorks.Shipping;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Web client for interfacing with the NetworkSolutions SOAP api
    /// </summary>
    public class NetworkSolutionsWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(NetworkSolutionsWebClient));
						  
        // Communication requirements
        private static string application = "Interapptive";
        private static string certificate = "NoeE8IbA1ne9OhnEX2w872Lp";

        // store communicating for
        NetworkSolutionsStoreEntity store;

        // the current page of orders being downloaded
        int currentPage = 0;

        // total number of orders that exist to be downloaded
        int totalCount = 0;

        // whether or not more orders exist to download
        bool hasMoreOrders = true;

        /// <summary>
        /// Gets whether or not orders exist to be downloaded 
        /// </summary>
        public bool HasMoreOrders
        {
            get
            {
                return hasMoreOrders;
            }
        }

        /// <summary>
        /// Gets the total number of orders that exist to download
        /// </summary>
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
        }

        /// <summary>
        /// Returns the system-configured batch size to retrieve from eBay with.
        /// </summary>
        public static int DownloadBatchSize
        {
            get
            {
                int batchSize = InterapptiveOnly.Registry.GetValue("NetworkSolutionsDownloadBatchSize", 100);

                // restrict to 1..500
                return Math.Min(Math.Max(batchSize, 1), 500);
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsWebClient(NetworkSolutionsStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Gets the user token from the user key, as phase 2 of the authentication process
        /// </summary>
        public string FetchUserToken(string userKey)
        {
            try
            {
                using (NetSolEcomService service = CreateWebService("GetUserToken"))
                {
                    // create the request for getting the token from the key
                    GetUserTokenRequestType request = new GetUserTokenRequestType
                    {
                        UserToken = new UserTokenType
                        {
                             UserKey = userKey
                        }
                    };

                    GetUserTokenResponseType response = service.GetUserToken(request);
                    HandleErrors(response);

                    return response.UserToken.UserToken;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }

        /// <summary>
        /// Gets the User Key and Login Url for phase 1 of authentication/signup
        /// </summary>
        public NetworkSolutionsUserKey FetchUserKey()
        {
            try
            {
                using (NetSolEcomService service = CreateWebService("GetUserKey"))
                {
                    GetUserKeyRequestType request = new GetUserKeyRequestType();

                    GetUserKeyResponseType response = service.GetUserKey(request);

                    HandleErrors(response);

                    return new NetworkSolutionsUserKey()
                    {
                        UserKey = response.UserKey.UserKey,
                        LoginUrl = response.UserKey.LoginUrl,
                        FailureUrl = response.UserKey.FailureUrl
                    };
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }

        /// <summary>
        /// Create the web service proxy object configured for communication
        /// </summary>
        private NetSolEcomService CreateWebService(string logName)
        {
            NetSolEcomService service = new NetSolEcomService(new ApiLogEntry(ApiLogSource.NetworkSolutions, logName));

            service.SecurityCredential = new SecurityCredentialType()
            {
                Application = application,
                Certificate = certificate,
                UserToken = store.UserToken
            };

            return service;
        }

        /// <summary>
        /// Checks a SOAP response for errors and raises a NetworkSolutionsException if one exists
        /// </summary>
        private void HandleErrors(BaseResponseType response)
        {
            if (response.Status != StatusCodeType.Success)
            {
                // get only those errors that are actual failures, as opposed to warnings
                List<ErrorType> errors = response.ErrorList.Where(e => e.SeveritySpecified && e.Severity == SeverityCodeType.Error).ToList();

                List<NetworkSolutionsError> nsErrors = new List<NetworkSolutionsError>();
                errors.ForEach(e =>
                {
                    nsErrors.Add(new NetworkSolutionsError { Number = e.NumberSpecified ? e.Number : -1, 
                                                             Message = e.Message, 
                                                             FieldInError = e.FieldInfo == null ? "" : e.FieldInfo.Field });
                });


                // build and throw a NetSol Exception
                NetworkSolutionsFailureSeverity severity = NetworkSolutionsFailureSeverity.Other;
                switch (response.Status)
                {
                    case StatusCodeType.Failure:
                        severity = NetworkSolutionsFailureSeverity.Failure;
                        break;
                    case StatusCodeType.PartialFailure:
                        severity = NetworkSolutionsFailureSeverity.PartialFailure;
                        break;
                }

                if (nsErrors.Count(e => e.Number == 607 && e.FieldInError == "SecurityCredential.UserToken") > 0)
                {
                    // invalid user token error, raise a more useful exception
                    throw new NetworkSolutionsException("The NetworkSolutions user access token appears to be invalid.\nCreate a new token in the Manage Stores window.");
                }
                else
                {
                    throw new NetworkSolutionsException(severity, nsErrors);
                }
            }
        }

        /// <summary>
        /// Imports a token file into the provided NetworkSolutions store
        /// </summary>
        public static bool ImportTokenFile(NetworkSolutionsStoreEntity store, IWin32Window owner)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Network Solutions Token File (*.nst)|*.nst";

                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    // load the file
                    try
                    {
                        string contents = File.ReadAllText(dlg.FileName);
                        string tokenXml = SecureText.Decrypt(contents, "token");

                        XmlDocument tokenDoc = new XmlDocument();
                        tokenDoc.LoadXml(tokenXml);

                        // load the token into the store
                        LoadTokenXml(store, tokenDoc);

                        return true;
                    }
                    catch (NetworkSolutionsException ex)
                    {
                        log.Error(ex.Message, ex);

                        MessageHelper.ShowError(owner, ex.Message);
                    }
                    catch (IOException ex)
                    {
                        // device failure most likely
                        string message = String.Format("Failure reading token file '{0}'", dlg.FileName);
                        log.Error(message, ex);

                        MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", ex.Message));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        // No access
                        string message = String.Format("Failure reading token file '{0}'", dlg.FileName);
                        log.Error(message, ex);

                        // show the error
                        MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", ex.Message));
                    }
                    catch (XmlException ex)
                    {
                        // Not the XML we were expecting
                        string message = String.Format("Invalid token file format in '{0}'", dlg.FileName);
                        log.Error(message, ex);

                        // show the error
                        MessageHelper.ShowError(owner, "Unable to import token file, invalid format.");
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Parses the Token Xml file and imports the contained token
        /// </summary>
        private static void LoadTokenXml(NetworkSolutionsStoreEntity store, IXPathNavigable tokenXml)
        {
            XPathNavigator xpath = tokenXml.CreateNavigator();

            string token = (string)xpath.Evaluate("string(//Token)");

            // ensure the token exists
            if (token.Length == 0)
            {
                throw new NetworkSolutionsException("Unable to load the Network Solutions token from the token file.");
            }

            string oldToken = store.UserToken;

            // test the token out
            try
            {
                // apply the new token to test
                store.UserToken = token;

                NetworkSolutionsWebClient client = new NetworkSolutionsWebClient(store);
                client.TestConnection();
            }
            catch (NetworkSolutionsException ex)
            {
                store.UserToken = oldToken;

                log.Error(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Retrieves the site settings for the configured store
        /// </summary>
        public SiteSettingType GetSiteSettings()
        {
            try
            {
                using (NetSolEcomService service = CreateWebService("GetSiteSettings"))
                {
                    ReadSiteSettingRequestType request = new ReadSiteSettingRequestType()
                    {
                        DetailSize = SizeCodeType.Small,
                        DetailSizeSpecified = true
                    };

                    ReadSiteSettingResponseType response = service.ReadSiteSetting(request);
                    HandleErrors(response);

                    return response.SiteSetting;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }

        /// <summary>
        /// Tests connectivity and the registered UserToken
        /// </summary>
        private void TestConnection()
        {
            GetSiteSettings();
        }

        /// <summary>
        /// Gets the next page of orders
        /// </summary>
        public List<OrderType> GetNextOrders()
        {
            try
            {

                List<FilterType> filters = new List<FilterType>();

                bool one = true;
                foreach (string status in store.DownloadOrderStatuses.Split(','))
                {
                    filters.Add(new FilterType
                    {
                        Field = "Status.OrderStatusID",
                        ValueList = new string[]{ status },
                        Operator = OperatorCodeType.Equal,
                        OperatorSpecified = true,
                        OrClause = !one,
                        OrClauseSpecified = !one
                    });

                    one = false;
                }

                using (NetSolEcomService service = CreateWebService("GetOrders"))
                {
                    // configure the request
                    ReadOrderRequestType request = new ReadOrderRequestType
                    {
                        DetailSize = SizeCodeType.Large,
                        DetailSizeSpecified = true,

                        // paging
                        PageRequest = new PaginationType
                        {
                            Page = currentPage + 1,
                            PageSpecified = true,
                            Size = DownloadBatchSize,
                            SizeSpecified = true,
                        },

                        // select only orders of a certain type
                        FilterList = filters.ToArray() 
                    };

                    // execute the request, and response to errors in the response
                    ReadOrderResponseType response = service.ReadOrder(request);
                    HandleErrors(response);

                    // store the paging data for the next call
                    currentPage = response.PageResponse.PageSpecified ? response.PageResponse.Page : 1;
                    hasMoreOrders = response.PageResponse.HasMoreSpecified ? response.PageResponse.HasMore : false;
                    totalCount = response.PageResponse.TotalSizeSpecified ? response.PageResponse.TotalSize : 0;

                    // return the collection
                    if (response.OrderList == null)
                    {
                        return new List<OrderType>();
                    }
                    else
                    {
                        return response.OrderList.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }
        
        /// <summary>
        /// Returns the possible status codes in NetworkSolutions
        /// </summary>
        public List<OrderStatusType> GetStatusCodes()
        {
            try
            {
                using (NetSolEcomService service = CreateWebService("GetStatusCodes"))
                {
                    ReadOrderStatusRequestType request = new ReadOrderStatusRequestType
                    {
                        DetailSize = SizeCodeType.Large,
                        DetailSizeSpecified = true
                    };

                    ReadOrderStatusResponseType response = service.ReadOrderStatus(request);
                    HandleErrors(response);

                    return new List<OrderStatusType>(response.OrderStatusList);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }

        /// <summary>
        /// Update the online status of an order
        /// </summary>
        public void UpdateOrderStatus(long networkSolutionsOrderId, long currentStatus, long targetStatus, string comments)
        {
            try
            {
                // figure out what sequence of status updates need to be made.  Network Solutions
                // enforces a workflow from one status to another
                List<long> statusPath = GetStatusPath(currentStatus, targetStatus);

                if (statusPath.Count == 0)
                {
                    statusPath.Add(targetStatus);
                }

                int startIndex = 0;
                if (statusPath.Count > 1)
                {
                    startIndex = 1;
                }

                using (NetSolEcomService service = CreateWebService("UpdateOrderStatus"))
                {
                    for (int x = startIndex; x < statusPath.Count; x++)
                    {
                        // Create the request to set the status and comments
                        UpdateOrderRequestType request = new UpdateOrderRequestType
                        {
                            Order = new OrderType
                            {
                                OrderId = networkSolutionsOrderId,
                                OrderIdSpecified = true,

                                Status = new OrderStatusType
                                {
                                    OrderStatusId = statusPath[x],
                                    OrderStatusIdSpecified = true
                                },

                                Notes = comments
                            }
                        };

                        UpdateOrderResponseType response = (UpdateOrderResponseType)service.UpdateOrder(request);
                        HandleErrors(response);
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }

        /// <summary>
        /// Determines the series of status updates an order must go through to
        /// get from the currentStatus to the targetStatus
        /// </summary>
        private List<long> GetStatusPath(long currentStatus, long targetStatus)
        {
            // create a status code provider
            NetworkSolutionsStatusCodeProvider statusCodeProvider = new NetworkSolutionsStatusCodeProvider(store);

            // create and populate a status graph so we can find a path between the two status codes
            NetworkSolutionsStatusGraph graph = new NetworkSolutionsStatusGraph();

            // graph vertices
            List<long> statusCodes = statusCodeProvider.CodeValues.Cast<long>().ToList();
            foreach (long statusCode in statusCodes)
            {
                graph.AddVertex(statusCode);
            }

            // edges
            foreach (long statusCode in statusCodes)
            {
                foreach (long nextCode in statusCodeProvider.GetNextOrderStatusCodes(statusCode))
                {
                    graph.AddDirectedEdge(statusCode, nextCode);
                }
            }

            long[] path = graph.GetPath(currentStatus, targetStatus);
            if (path == null)
            {
                return new List<long>();
            }
            else
            {
                // get the path
                return new List<long>(path);
            }
        }

        /// <summary>
        /// Uploads the tracking number for a shipment
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment)
        {
            NetworkSolutionsOrderEntity order = shipment.Order as NetworkSolutionsOrderEntity;

            try
            {

                using (NetSolEcomService service = CreateWebService("UploadShipmentDetails"))
                {
                    CarrierCodeType carrier;
                    string serviceName;
                    string trackingNumber;

                    GetShipmentUploadValues(shipment, out carrier, out serviceName, out trackingNumber);

                    // Create the request to set the tracking number
                    UpdateOrderRequestType request = new UpdateOrderRequestType
                    {
                        Order = new OrderType
                        {
                            OrderId = order.NetworkSolutionsOrderID,
                            OrderIdSpecified = true,

                            // shipment information
                            Shipping = new ShippingType
                            {
                                // carrier
                                Carrier = carrier,
                                CarrierSpecified = true,

                                // service name?
                                Name = serviceName,

                                // Shipment has a package which has a tracking number
                                PackageList = new PackageType[] {
                                    new PackageType{
                                        TrackingNumber = trackingNumber
                                    }
                                }
                            }
                        }
                    };

                    UpdateOrderResponseType response = (UpdateOrderResponseType)service.UpdateOrder(request);
                    HandleErrors(response);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(NetworkSolutionsException));
            }
        }

        /// <summary>
        /// Gets values appropriate for upload to Network Solutions
        /// </summary>
        private void GetShipmentUploadValues(ShipmentEntity shipment, out CarrierCodeType carrier, out string serviceName, out string trackingNumber)
        {
            CarrierCodeType tempCarrier = GetShipmentCarrierCodeType(shipment);
            string tempServiceName = ShippingManager.GetServiceUsed(shipment);
            string tempTrackingNumber = shipment.TrackingNumber;


            // Adjust tracking details per Mail Innovations and others
            WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                {
                    if (track.Length > 0)
                    {
                        tempCarrier = CarrierCodeType.USPS;
                        tempTrackingNumber = track;
                    }
                    else
                    {
                        tempCarrier = CarrierCodeType.Custom;
                    }
                });

            carrier = tempCarrier;
            serviceName = tempServiceName;
            trackingNumber = tempTrackingNumber;
        }

        /// <summary>
        /// Translates ShipWorks shipment type to Network Solutions carrier type
        /// </summary>
        private CarrierCodeType GetShipmentCarrierCodeType(ShipmentEntity shipment)
        {
            switch ((ShipmentTypeCode)shipment.ShipmentType)
            {
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Stamps:
                    return CarrierCodeType.USPS;

                case ShipmentTypeCode.FedEx:
                    return CarrierCodeType.FedEx;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return CarrierCodeType.USPS;

                default:
                    return CarrierCodeType.Custom;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Enums;

namespace ShipWorks.Stores.Platforms.Miva
{
    /// <summary>
    /// Web client for connecting to the miva module.  Builds on top of the generic client.
    /// </summary>
    [Component(RegistrationType.Self)]
    public class MivaWebClient : GenericStoreWebClient
    {
        const string TemporarySessionQueryString = "TemporarySession=1";
        static readonly ILog log = LogManager.GetLogger(typeof(MivaWebClient));
        MivaStoreEntity store = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public MivaWebClient(MivaStoreEntity store) : base(store)
        {
            this.store = store;
        }

        /// <summary>
        /// Validate the response against the generic schema
        /// </summary>
        protected override void ValidateSchema(string resultXml, string action)
        {
            // Don't validate our miva custom stuff that's not in the schema
            if (action == "getstores" ||
                action == "getnextorderid")
            {
                return;
            }

            // allow standard validation
            base.ValidateSchema(resultXml, action);
        }

        /// <summary>
        /// Finish preparing the request with miva specific data
        /// </summary>
        protected override void TransformRequest(IHttpVariableRequestSubmitter request, string action)
        {
            // Everything but GetStors requires the specific store code
            if (action == "getstores")
            {
                request.Variables.Remove("storecode");
            }

            // For getorders we have to passalong if we want the sebenza data and the encryption data
            if (action == "getorders")
            {
                // Additional Checkout Data
                request.Variables.Add("sebenzaAcd", store.SebenzaCheckoutDataEnabled ? "1" : "0");

                // Online Status
                request.Variables.Add("statusSource", GetOnlineStatusSource((MivaOnlineUpdateStrategy) store.OnlineUpdateStrategy));

                // For encryption passphrase
                if (store.ModuleUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase) && store.EncryptionPassphrase.Length > 0)
                {
                    request.Variables.Add("swpassphrase", SecureText.Decrypt(store.EncryptionPassphrase, store.ModuleUsername));
                }
            }

            // for getstatuscodes we need to tell the module which status codes are desired
            if (action == "getstatuscodes")
            {
                // add online status strategy
                request.Variables.Add("statusSource", GetOnlineStatusSource((MivaOnlineUpdateStrategy) store.OnlineUpdateStrategy));
            }

            // If the request contains the "start" parameter that's a date
            HttpVariable startVariable = request.Variables.SingleOrDefault(v => v.Name == "start");
            if (startVariable != null)
            {
                // It could be a long if not downloading by date
                DateTime dateTime;
                if (DateTime.TryParse(startVariable.Value, out dateTime))
                {
                    TimeSpan timeSinceEpoch = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

                    // Update the start variable to be the unix time.
                    startVariable.Value = Math.Max(0, timeSinceEpoch.TotalSeconds).ToString();
                }
            }
        }

        /// <summary>
        /// Get the value to send to the module as the source for the online status value
        /// </summary>
        private string GetOnlineStatusSource(MivaOnlineUpdateStrategy strategy)
        {
            switch (strategy)
            {
                case MivaOnlineUpdateStrategy.MivaNative: return "miva";
                case MivaOnlineUpdateStrategy.Sebenza: return "sebenza";
                case MivaOnlineUpdateStrategy.None: return "none";
            }

            throw new InvalidOperationException("Invalid strategy value: " + strategy);
        }

        /// <summary>
        /// Get the list of miva stores from the configured miva module
        /// </summary>
        public List<MivaStoreHeader> GetMivaStores()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            // execute the request
            GenericModuleResponse response = ProcessRequest(request, "getstores");

            List<MivaStoreHeader> stores = new List<MivaStoreHeader>();

            // Pull the results out of the response
            foreach (XPathNavigator node in response.XPath.Select("//Store"))
            {
                MivaStoreHeader header = new MivaStoreHeader
                {
                    Code = XPathUtility.Evaluate(node, "Code", ""),
                    Name = XPathUtility.Evaluate(node, "Name", "")
                };

                stores.Add(header);
            }

            return stores;
        }

        /// <summary>
        /// Get and reserve the next OrderID from the online store
        /// </summary>
        public long GetNextOrderID()
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Variables.Add("KeyType", "Orders");

            // execute the request
            GenericModuleResponse response = ProcessRequest(request, "getnextorderid");

            long result = XPathUtility.Evaluate(response.XPath, "//OrderID", (long) -1);

            if (result == -1)
            {
                throw new GenericStoreException("Invalid order number returned from Miva Merchant.");
            }

            return result;
        }

        /// <summary>
        /// Send the order status update
        /// </summary>
        public override async Task<IResult> UpdateOrderStatus(OrderEntity order, string orderIdentifier, object code, string comment)
        {
            if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", order.OrderID);
                return Result.FromSuccess();
            }

            try
            {
                switch ((MivaOnlineUpdateStrategy) store.OnlineUpdateStrategy)
                {
                    case MivaOnlineUpdateStrategy.None:
                        return Result.FromError("The store is not configured for updating online status.");
                    case MivaOnlineUpdateStrategy.Sebenza:
                        await UpdateOnlineStatusThroughSebenza(order, orderIdentifier, code, comment, null).ConfigureAwait(false);
                        break;
                }
            }
            catch (GenericStoreException ex)
            {
                return Result.FromError(ex);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Send shipment details up to miva
        /// </summary>
        public override async Task<IResult> UploadShipmentDetails(OrderEntity order, string orderIdentifier, ShipmentEntity shipment)
        {
            if (order.IsManual && order.CombineSplitStatus == CombineSplitStatusType.None)
            {
                log.InfoFormat("Not updating order {0} online since its manual.", order.OrderID);
                return Result.FromSuccess();
            }

            if (!shipment.Processed || shipment.Voided)
            {
                log.InfoFormat("Not uploading shipment details for shipment {0}, either not processed or has been voided.", shipment.ShipmentID);
                return Result.FromSuccess();
            }

            try
            {
                switch ((MivaOnlineUpdateStrategy) store.OnlineUpdateStrategy)
                {
                    case MivaOnlineUpdateStrategy.None:
                        return Result.FromError("The store is not configured for updating online status.");
                    case MivaOnlineUpdateStrategy.Sebenza:
                        await UpdateOnlineStatusThroughSebenza(order, orderIdentifier, order.OnlineStatusCode, null, shipment).ConfigureAwait(false);
                        break;
                    case MivaOnlineUpdateStrategy.MivaNative:
                        await ExecuteNativeOnlineUpdate(order, orderIdentifier, shipment);
                        break;
                }
            }
            catch (GenericStoreException ex)
            {
                return Result.FromError(ex);
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Update online status using Sebenza
        /// </summary>
        private async Task UpdateOnlineStatusThroughSebenza(OrderEntity order, string orderIdentifier, object code, string comment, ShipmentEntity shipment)
        {
            var uploadXml = CreateSebenzaOnlineUpdateXml(orderIdentifier, order.OnlineStatusCode, code, comment, shipment);
            await ExecuteSebenzaOnlineUpdate(uploadXml).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute a Miva native online update
        /// </summary>
        private async Task ExecuteNativeOnlineUpdate(OrderEntity order, string orderIdentifier, ShipmentEntity shipment)
        {
            IHttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            request.Variables.Add("order", orderIdentifier);
            request.Variables.Add("tracking", shipment.TrackingNumber);
            request.Variables.Add("carrier", ShippingManager.GetCarrierName((ShipmentTypeCode) shipment.ShipmentType));
            request.Variables.Add("shipdate", shipment.ShipDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString());

            GenericModuleResponse response = await ProcessRequestAsync(request, "updateshipment").ConfigureAwait(false);

            // extract the new status
            string newStatus = XPathUtility.Evaluate(response.XPath, "//OrderStatus", "failed_after_update");

            // set status to what was returned
            order.OnlineStatusCode = newStatus;

            GenericModuleStoreType genericStoreType = (GenericModuleStoreType) StoreTypeManager.GetType(store);
            order.OnlineStatus = genericStoreType.CreateStatusCodeProvider().GetCodeName(newStatus);

            // save the order
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // update the base order table
                await adapter.SaveEntityAsync(order, true).ConfigureAwait(false);
                adapter.Commit();
            }
        }

        /// <summary>
        /// Create the xml to use to push to the sebenza module
        /// </summary>
        private string CreateSebenzaOnlineUpdateXml(string orderNumber, object onlineStatusCode, object statusCode, string statusComment, IShipmentEntity shipment)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
                using (xmlWriter.WriteStartDocumentDisposable())
                {
                    using (xmlWriter.WriteStartElementDisposable("OrderStatus"))
                    {
                        using (xmlWriter.WriteStartElementDisposable("Order"))
                        {
                            xmlWriter.WriteElementString("Id", orderNumber);
                            xmlWriter.WriteElementString("Note", statusComment ?? "");

                            // Whether to send an email on this update to the buyer
                            bool sendEmail = store.OnlineUpdateStatusChangeEmail && statusCode != null;

                            if (statusCode != null)
                            {
                                sendEmail = sendEmail && ((string) statusCode != (string) onlineStatusCode);

                                xmlWriter.WriteElementString("Status", statusCode.ToString());
                            }

                            xmlWriter.WriteElementString("SendEmail", sendEmail ? "1" : "0");

                            WriteShipmentDetails(shipment, xmlWriter);
                        }
                    }
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Write shipment details to the writer
        /// </summary>
        private static void WriteShipmentDetails(IShipmentEntity shipment, XmlTextWriter xmlWriter)
        {
            if (shipment == null)
            {
                return;
            }

            string carrierCode = "";

            switch ((ShipmentTypeCode) shipment.ShipmentType)
            {
                case ShipmentTypeCode.FedEx:
                    carrierCode = "FEDEX";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierCode = "UPS";
                    break;

                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    carrierCode = "USPS";
                    break;

                default:
                    carrierCode = "OTHER";
                    break;
            }

            xmlWriter.WriteElementString("Carrier", carrierCode);
            xmlWriter.WriteElementString("Tracking", shipment.TrackingNumber);
        }

        /// <summary>
        /// Execute the Sebenza online update request
        /// </summary>
        private async Task ExecuteSebenzaOnlineUpdate(string xml)
        {
            IHttpVariableRequestSubmitter request = PrepareRequest(xml);

            // log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.Miva, "SebenzaOnlineUpdate");
            logger.LogRequest(request);

            // execute the request
            try
            {
                using (IHttpResponseReader postResponse = await request.GetResponseAsync().ConfigureAwait(false))
                {
                    string resultXml = await postResponse.ReadResultAsync().ConfigureAwait(false);

                    ProcessResults(logger, resultXml);
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(GenericStoreException));
            }
        }

        /// <summary>
        /// Prepare a request for upload
        /// </summary>
        private IHttpVariableRequestSubmitter PrepareRequest(string xml)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Variables.Add("action", "ImportStatus");

            // Content
            request.Variables.Add("XML", xml);

            // Add the username and password as parameters
            request.Variables.Add("username", store.ModuleUsername);
            request.Variables.Add("password", SecureText.Decrypt(store.ModulePassword, store.ModuleUsername));

            // Store Code
            request.Variables.Add("Store_Code", store.ModuleOnlineStoreCode);

            // Setup the request
            request.Uri = new Uri(Regex.Replace(GetUrlFromStore(store).AbsoluteUri, "util/.*?[.]mvc", "fulfill/orderstatus5.mvc", RegexOptions.IgnoreCase).Replace("http:", "https:"));
            request.Timeout = TimeSpan.FromMinutes(1);
            return request;
        }

        /// <summary>
        /// Process the results of a request
        /// </summary>
        private static void ProcessResults(ApiLogEntry logger, string resultXml)
        {
            // An XML Document cannot start with whitespace, and I'm not positive the sebenza module never would
            if (!resultXml.StartsWith("<"))
            {
                resultXml = resultXml.Trim();
            }

            // log the response
            logger.LogResponse(resultXml);

            // Strip invalid input characters
            resultXml = XmlUtility.StripInvalidXmlCharacters(resultXml);

            // look for the <ERROR:......>
            Match match = Regex.Match(resultXml, @"\<ERROR:(.*)\>");
            if (match.Success)
            {
                throw new WebException(match.Groups[1].Value);
            }

            // check response
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(resultXml);

                XmlNode completeNode = xmlDocument.SelectSingleNode("//Complete");
                if (completeNode != null)
                {
                    bool success = (completeNode.InnerText.Trim() == "1");
                    if (!success)
                    {
                        throw new GenericStoreException("The Sebenza module reports that the order was not updated.");
                    }
                }
                else
                {
                    throw new GenericStoreException("The Sebenza module is returning results in an unknown format.");
                }
            }
            catch (XmlException ex)
            {
                throw new GenericStoreException("The Sebenza module is returning results in an unknown format.", ex);
            }
        }

        /// <summary>
        /// Adds the TemporarySession=1 query string variable
        /// </summary>
        protected override Uri GetUrlFromStore(GenericModuleStoreEntity genericStore)
        {
            UriBuilder builder = new UriBuilder(base.GetUrlFromStore(genericStore));
            if (builder.Query != null && builder.Query.Length > 1)
            {
                builder.Query = builder.Query.Substring(1) + "&" + TemporarySessionQueryString;
            }
            else
            {
                builder.Query = TemporarySessionQueryString;
            }
            return builder.Uri;
        }
    }
}

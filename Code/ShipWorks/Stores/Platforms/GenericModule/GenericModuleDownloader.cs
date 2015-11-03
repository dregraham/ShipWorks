using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using ShipWorks.Data.Connection;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using System.Text.RegularExpressions;
using System.Xml;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore;
using System.Windows.Forms;
using Interapptive.Shared;
using ShipWorks.Data.Import;
using ShipWorks.Data.Import.Xml;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Provides the entrypoint into the order download processes for Generic
    /// </summary>
    class GenericModuleDownloader : OrderElementFactoryDownloaderBase, IGenericXmlOrderLoadObserver
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(GenericModuleDownloader));

        // total download count
        int totalCount = 0;

        // Status code container 
        GenericStoreStatusCodeProvider statusCodeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleDownloader(StoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Convenience property for quick access to the specific entity
        /// </summary>
        protected GenericModuleStoreEntity GenericModuleStoreEntity
        {
            get
            {
                return (GenericModuleStoreEntity)Store;
            }
        }

        /// <summary>
        /// Begin order download
        /// </summary>
        [NDependIgnoreLongMethod]
        protected override void Download()
        {
            try
            {
                bool supportMode = InterapptiveOnly.MagicKeysDown;

                GenericModuleStoreType storeType = (GenericModuleStoreType)StoreTypeManager.GetType(Store);

                if (!supportMode)
                {
                    // If the platform\developer or capabilities changed we need to update the store
                    storeType.UpdateOnlineModuleInfo();
                    if (Store.IsDirty)
                    {
                        SqlAdapter.Default.SaveAndRefetch(Store);
                    }
                }

                // Create the web client to download with
                GenericStoreWebClient webClient = storeType.CreateWebClient();

                // If status codes are supported download them
                if (GenericModuleStoreEntity.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.None)
                {
                    // Update the status codes
                    Progress.Detail = "Updating status codes...";

                    statusCodeProvider = storeType.CreateStatusCodeProvider();

                    if (!supportMode)
                    {
                        statusCodeProvider.UpdateFromOnlineStore();
                    }
                }

                Progress.Detail = "Checking for orders...";

                if (!supportMode)
                {
                    // Get the largest last modified time.  We start downloading there.
                    if (GenericModuleStoreEntity.ModuleDownloadStrategy == (int) GenericStoreDownloadStrategy.ByModifiedTime)
                    {
                        // Downloading based on the last modified time
                        DateTime? lastModified = GetOnlineLastModifiedStartingPoint();

                        totalCount = webClient.GetOrderCount(lastModified);
                    }
                    else
                    {
                        // Downloading based on the last ordernumber we've downloaded
                        long lastOrderNumber = GetOrderNumberStartingPoint();

                        // Get the number of orders that need downloading
                        totalCount = webClient.GetOrderCount(lastOrderNumber);
                    }

                    if (totalCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }
                }

                Progress.Detail = string.Format("Downloading {0} orders...", totalCount);

                // keep going until none are left
                while (true)
                {
                    // support mode bypasses regular download mechanisms to load a response from disk
                    if (supportMode)
                    {
                        DownloadOrdersFromFile(webClient);

                        return;
                    }

                    // Check if it has been cancelled
                    if (Progress.IsCancelRequested)
                    {
                        return;
                    }

                    if (!DownloadNextOrdersPage(webClient))
                    {
                        return;
                    }
                }
            }
            catch (GenericModuleConfigurationException ex)
            {
                string message = String.Format("The ShipWorks module returned invalid configuration information.  Please contact the module developer with the following information.\n\n{0}", ex.Message);

                throw new DownloadException(message, ex);
            }
            catch (GenericStoreException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Presents the support staff with a way to load orders from a module response file directly
        /// </summary>
        private void DownloadOrdersFromFile(GenericStoreWebClient client)
        {
            using (GenericStoreResponseLoadDlg dlg = new GenericStoreResponseLoadDlg(client))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    GenericModuleResponse response = dlg.LoadedResponse;

                    log.InfoFormat("Manually loading Order Response File {0}...", dlg.LoadedFileName);

                    XPathNavigator xpath = response.XPath;
                    XPathNodeIterator orderNodes = xpath.Select("//Order");

                    // see if there are any orders in the response
                    if (orderNodes.Count > 0)
                    {
                        // progress tracking
                        totalCount = orderNodes.Count;

                        // import the downloaded orders
                        LoadOrders(orderNodes);
                    }

                    Progress.Detail = "Done";
                }
            }
        }

        /// <summary>
        /// Downloads and imports the next batch of orders into ShipWorks
        /// </summary>
        private bool DownloadNextOrdersPage(GenericStoreWebClient webClient)
        {
            // Get the largest last modified time.  We start downloading there.
            GenericModuleResponse response;
            if (GenericModuleStoreEntity.ModuleDownloadStrategy == (int) GenericStoreDownloadStrategy.ByModifiedTime)
            {
                // Downloading baed on the last modified time
                DateTime? lastModified = GetOnlineLastModifiedStartingPoint();

                response = webClient.GetNextOrderPage(lastModified);
            }
            else
            {
                // pickup where we left off
                long lastOrderNumber = GetOrderNumberStartingPoint();

                // retrieve the next page of data from the web module
                response = webClient.GetNextOrderPage(lastOrderNumber);
            }

            XPathNavigator xpath = response.XPath;
            XPathNodeIterator orderNodes = xpath.Select("//Order");

            // see if there are any orders in the response
            if (orderNodes.Count > 0)
            {
                // import the downloaded orders
                LoadOrders(orderNodes);

                // signal that we imported some orders
                return true;
            }
            else
            {
                Progress.Detail = "Done";

                // signal that none were imported
                return false;
            }
        }

        /// <summary>
        /// Imports the orders contained in the iterator
        /// </summary>
        private void LoadOrders(XPathNodeIterator orderNodes)
        {
            // go through each order in the batch
            while (orderNodes.MoveNext())
            {
                // check for cancel again
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                XPathNavigator order = orderNodes.Current.Clone();
                LoadOrder(order);

                // update the status
                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / totalCount);
            }
        }

        /// <summary>
        /// Create the order identifier based on incoming xml
        /// </summary>
        protected virtual OrderIdentifier CreateOrderIdentifier(XPathNavigator orderXPath)
        {
            // pull out the order number
            long orderNumber = XPathUtility.Evaluate(orderXPath, "OrderNumber", 0L);

            // pull in pre/postfix options
            string prefix = XPathUtility.Evaluate(orderXPath, "OrderNumberPrefix", "");
            string postfix = XPathUtility.Evaluate(orderXPath, "OrderNumberPostfix", "");

            // create the identifier
            return new GenericOrderIdentifier(orderNumber, prefix, postfix);
        }

        /// <summary>
        /// Assigns an order number to the order
        /// </summary>
        protected virtual void AssignOrderNumber(OrderEntity order)
        {
            // this is an extension point for derived class.  The GenericStoreDownloader
            // implementation uses the OrderNumberIdentifier which has already applied an order number 
        }

        /// <summary>
        /// Instantiate the generic order based on the configured mapping and the specified XPath
        /// </summary>
        protected OrderEntity InstantiateOrder(XPathNavigator xpath)
        {
            // Construct the order identifier based on the incoming xml
            OrderIdentifier orderIdentifier = CreateOrderIdentifier(xpath);

            // get the order instance; Change this to our derived class once it's needed and exists
            OrderEntity order = InstantiateOrder(orderIdentifier);

            AssignOrderNumber(order);

            return order;
        }

        /// <summary>
        /// Extract the order from the xml
        /// </summary>
        private void LoadOrder(XPathNavigator xpath)
        {
            OrderEntity order = InstantiateOrder(xpath);

            GenericXmlOrderLoader.LoadOrder(order, this, this, xpath);

            // last modified
            order.OnlineLastModified = DateTime.Parse(XPathUtility.Evaluate(xpath, "LastModified", order.OrderDate.ToString("s")));

            // If Parse can tell what timezone it's in, it automatically converts it to local.  We need UTC.
            if (order.OnlineLastModified.Kind == DateTimeKind.Local)
            {
                order.OnlineLastModified = order.OnlineLastModified.ToUniversalTime();
            }

            // CustomerID
            LoadCustomerIdentifier(order, xpath);

            // OnlineStatus and custom OnlineStatusCode
            if (GenericModuleStoreEntity.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.None)
            {
                try
                {
                    object statusCode = GenericModuleStoreEntity.ModuleOnlineStatusDataType == (int) GenericVariantDataType.Numeric ?
                        (object) XPathUtility.Evaluate(xpath, "StatusCode", 0) :
                        (object) XPathUtility.Evaluate(xpath, "StatusCode", "");

                    order.OnlineStatusCode = statusCode;
                    order.OnlineStatus = statusCodeProvider.GetCodeName(statusCode);
                }
                catch (FormatException ex)
                {
                    log.Error(ex);

                    if (GenericModuleStoreEntity.ModuleOnlineStatusDataType == (int) GenericVariantDataType.Numeric)
                    {
                        throw new DownloadException(string.Format("The module is configured to use numeric status codes, but the value '{0}' for order '{1}' is not numeric.", XPathUtility.Evaluate(xpath, "StatusCode", ""), order.OrderNumberComplete));
                    }
                    else
                    {
                        // Shouldn't happen if its Text
                        throw;
                    }
                }
            }
            else
            {
                order.OnlineStatusCode = null;
                order.OnlineStatus = "";
            }

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericModuleDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Sets the order's customer identifier
        /// </summary>
        private void LoadCustomerIdentifier(OrderEntity order, XPathNavigator xpath)
        {
            order.OnlineCustomerID = null;

            if (GenericModuleStoreEntity.ModuleOnlineCustomerSupport)
            {
                string xmlValue = XPathUtility.Evaluate(xpath, "CustomerID", "").Trim();
                if (xmlValue.Length > 0)
                {
                    switch ((GenericVariantDataType) GenericModuleStoreEntity.ModuleOnlineCustomerDataType)
                    {
                        case GenericVariantDataType.Text:
                            {
                                order.OnlineCustomerID = xmlValue;
                                break;
                            }

                        case GenericVariantDataType.Numeric:
                            {
                                long customerID;
                                if (long.TryParse(xmlValue, out customerID))
                                {
                                    if (customerID > 0)
                                    {
                                        order.OnlineCustomerID = customerID;
                                    }
                                }
                                else
                                {
                                    log.ErrorFormat("Unable to convert CustomerID \"{0}\" returned from module to type long.", xmlValue);
                                }

                                break;
                            }
                    }
                }
            }
        }

        #region IGenericXmlOrderLoadObserver Implementation

        /// <summary>
        /// A single order has been loaded from XML
        /// </summary>
        public virtual void OnOrderLoadComplete(OrderEntity order, XPathNavigator xpath)
        {
        }

        /// <summary>
        /// As single order item has been loaded from Xml
        /// </summary>
        public virtual void OnItemLoadComplete(OrderItemEntity item, XPathNavigator xpath)
        {
        }

        /// <summary>
        /// As single option has been loaded from Xml
        /// </summary>
        public virtual void OnItemAttributeLoadComplete(OrderItemAttributeEntity item, XPathNavigator xpath)
        {
        }

        #endregion
    }
}

using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Import.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Provides the entry point into the order download processes for Generic
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Amosoft)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Brightpearl)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Cart66Lite)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Cart66Pro)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.ChannelSale)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Choxi)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.CloudConversion)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.CreLoaded)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.CsCart)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Fortune3)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.GenericModule)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.GeekSeller)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.InfiPlex)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.InstaStore)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Jigoshop)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.LimeLightCRM)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.LiveSite)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.LoadedCommerce)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.nopCommerce)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.OpenCart)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.OpenSky)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.OrderBot)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.OrderDesk)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.OrderDynamics)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.osCommerce)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.PowersportsSupport)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.PrestaShop)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.RevolutionParts)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SearchFit)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SellerActive)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SellerCloud)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SellerExpress)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SellerVantage)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Shopperpress)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Shopp)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SolidCommerce)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.StageBloc)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.SureDone)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.VirtueMart)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.WebShopManager)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.WooCommerce)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.WPeCommerce)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.XCart)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.ZenCart)]
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.Zenventory)]
    public class GenericModuleDownloader : OrderElementFactoryDownloaderBase, IGenericXmlOrderLoadObserver
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(GenericModuleDownloader));

        // total download count
        private int totalCount;

        // Status code container
        private GenericStoreStatusCodeProvider statusCodeProvider;

        private readonly GenericModuleStoreType storeType;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericModuleDownloader(StoreEntity store, IStoreTypeManager storeTypeManager, IConfigurationData configurationData, ISqlAdapterFactory sqlAdapterFactory)
            : base(store, storeTypeManager.GetType(store), configurationData, sqlAdapterFactory)
        {
            storeType = StoreType as GenericModuleStoreType;
        }

        /// <summary>
        /// Convenience property for quick access to the specific entity
        /// </summary>
        protected GenericModuleStoreEntity GenericModuleStoreEntity => (GenericModuleStoreEntity) Store;

        /// <summary>
        /// Begin order download
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            try
            {
                bool supportModeActive = InterapptiveOnly.MagicKeysDown;

                if (!supportModeActive)
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

                GetOnlineStatusCodes(storeType, supportModeActive);

                Progress.Detail = "Checking for orders...";

                if (!supportModeActive)
                {
                    await GetOrderCount(webClient).ConfigureAwait(false);

                    if (totalCount == 0)
                    {
                        Progress.Detail = "No orders to download.";
                        Progress.PercentComplete = 100;
                        return;
                    }
                }

                Progress.Detail = $"Downloading {totalCount} orders...";

                await DownloadOrders(supportModeActive, webClient).ConfigureAwait(false);
            }
            catch (GenericModuleConfigurationException ex)
            {
                string message =
                    "The ShipWorks module returned invalid configuration information. " +
                    $"Please contact the module developer with the following information.\n\n{ex.Message}";

                throw new DownloadException(message, ex);
            }
            catch (Exception ex) when (ex is GenericStoreException || ex is SqlForeignKeyException)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Downloads the orders.
        /// </summary>
        private async Task DownloadOrders(bool supportMode, GenericStoreWebClient webClient)
        {
            // keep going until none are left
            while (true)
            {
                // support mode bypasses regular download mechanisms to load a response from disk
                if (supportMode)
                {
                    await DownloadOrdersFromFile(webClient).ConfigureAwait(false);

                    return;
                }

                // Check if it has been canceled
                if (Progress.IsCancelRequested)
                {
                    return;
                }

                bool morePages = await DownloadNextOrdersPage(webClient).ConfigureAwait(false);
                if (!morePages)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Gets the online status codes.
        /// </summary>
        /// <param name="genericModuleStoreType">Type of the store.</param>
        /// <param name="supportMode">if set to <c>true</c> [support mode].</param>
        private void GetOnlineStatusCodes(GenericModuleStoreType genericModuleStoreType, bool supportMode)
        {
            // If status codes are supported download them
            if (GenericModuleStoreEntity.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.None)
            {
                // Update the status codes
                Progress.Detail = "Updating status codes...";

                statusCodeProvider = genericModuleStoreType.CreateStatusCodeProvider();

                if (!supportMode)
                {
                    statusCodeProvider.UpdateFromOnlineStore();
                }
            }
        }

        /// <summary>
        /// Gets the order count.
        /// </summary>
        private async Task GetOrderCount(GenericStoreWebClient webClient)
        {
            // Get the largest last modified time.  We start downloading there.
            if (GenericModuleStoreEntity.ModuleDownloadStrategy == (int) GenericStoreDownloadStrategy.ByModifiedTime)
            {
                // Downloading based on the last modified time
                DateTime? lastModified = await GetOnlineLastModifiedStartingPoint().ConfigureAwait(false);

                totalCount = webClient.GetOrderCount(lastModified);
            }
            else
            {
                // Downloading based on the last order number we've downloaded
                long lastOrderNumber = await GetOrderNumberStartingPoint().ConfigureAwait(false);

                // Get the number of orders that need downloading
                totalCount = webClient.GetOrderCount(lastOrderNumber);
            }
        }

        /// <summary>
        /// Presents the support staff with a way to load orders from a module response file directly
        /// </summary>
        private async Task DownloadOrdersFromFile(GenericStoreWebClient client)
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
                        await LoadOrders(orderNodes).ConfigureAwait(false);
                    }

                    Progress.Detail = "Done";
                }
            }
        }

        /// <summary>
        /// Downloads and imports the next batch of orders into ShipWorks
        /// </summary>
        private async Task<bool> DownloadNextOrdersPage(GenericStoreWebClient webClient)
        {
            // Get the largest last modified time.  We start downloading there.
            GenericModuleResponse response;
            if (GenericModuleStoreEntity.ModuleDownloadStrategy == (int) GenericStoreDownloadStrategy.ByModifiedTime)
            {
                // Downloading based on the last modified time
                DateTime? lastModified = await GetOnlineLastModifiedStartingPoint();

                response = webClient.GetNextOrderPage(lastModified);
            }
            else
            {
                // pickup where we left off
                long lastOrderNumber = await GetOrderNumberStartingPoint();

                // retrieve the next page of data from the web module
                response = webClient.GetNextOrderPage(lastOrderNumber);
            }

            XPathNavigator xpath = response.XPath;
            XPathNodeIterator orderNodes = xpath.Select("//Order");

            // see if there are any orders in the response
            if (orderNodes.Count > 0)
            {
                // import the downloaded orders
                return await LoadOrders(orderNodes).ConfigureAwait(false);
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
        private async Task<bool> LoadOrders(XPathNodeIterator orderNodes)
        {
            bool anyProcessed = false;

            // go through each order in the batch
            while (orderNodes.MoveNext())
            {
                // check for cancel again
                if (Progress.IsCancelRequested)
                {
                    return false;
                }

                // Update the status
                Progress.Detail = string.Format("Processing order {0}...", (QuantitySaved + 1));

                XPathNavigator order = orderNodes.Current.Clone();
                anyProcessed |= await LoadOrder(order).ConfigureAwait(false);

                // update the status
                Progress.PercentComplete = Math.Min(100, 100 * QuantitySaved / totalCount);
            }

            return anyProcessed;
        }

        /// <summary>
        /// Create the order identifier based on incoming xml
        /// </summary>
        protected virtual OrderIdentifier CreateOrderIdentifier(XPathNavigator orderXPath)
        {
            // pull out the order number
            string orderNumber = XPathUtility.Evaluate(orderXPath, "OrderNumber", "");

            // We strip out leading 0's. If all 0's, TrimStart would make it an empty string,
            // so in that case, we leave a single 0.
            orderNumber = orderNumber.All(n => n == '0') ? "0" : orderNumber.TrimStart('0');

            if (GenericModuleStoreEntity.ModuleDownloadStrategy == (int) GenericStoreDownloadStrategy.ByOrderNumber)
            {
                long parsedOrderNumber;
                if (!long.TryParse(orderNumber, out parsedOrderNumber))
                {
                    throw new DownloadException("When downloading by order number, all order numbers must be a number.\r\n\r\n" +
                                                $"Non-numeric order number found: {orderNumber}");
                }
            }

            // pull in pre/postfix options
            string prefix = XPathUtility.Evaluate(orderXPath, "OrderNumberPrefix", "");
            string postfix = XPathUtility.Evaluate(orderXPath, "OrderNumberPostfix", "");

            // create the identifier
            return storeType.CreateOrderIdentifier(orderNumber, prefix, postfix);
        }

        /// <summary>
        /// Assigns an order number to the order
        /// </summary>
        protected virtual Task AssignOrderNumber(OrderEntity order)
        {
            // this is an extension point for derived class.  The GenericStoreDownloader
            // implementation uses the OrderNumberIdentifier which has already applied an order number
            return Task.CompletedTask;
        }

        /// <summary>
        /// Instantiate the generic order based on the configured mapping and the specified XPath
        /// </summary>
        protected Task<GenericResult<OrderEntity>> InstantiateOrder(XPathNavigator xpath)
        {
            // Construct the order identifier based on the incoming xml
            OrderIdentifier orderIdentifier = CreateOrderIdentifier(xpath);

            // get the order instance; Change this to our derived class once it's needed and exists
            return InstantiateOrder(orderIdentifier);
        }

        /// <summary>
        /// Extract the order from the xml
        /// </summary>
        private async Task<bool> LoadOrder(XPathNavigator xpath)
        {
            GenericResult<OrderEntity> result = await InstantiateOrder(xpath).ConfigureAwait(false);
            if (result.Failure)
            {
                return false;
            }

            OrderEntity order = result.Value;
            await AssignOrderNumber(order).ConfigureAwait(false);

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
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

            return true;
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

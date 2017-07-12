using System.Data.SqlClient;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Import.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    /// <summary>
    /// Downloader implementation for importing from XML files
    /// </summary>
    public class GenericFileXmlDownloader : GenericFileDownloaderBase
    {
        // Transform to use, if any
        XslCompiledTransform xslTransform = null;

        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileXmlDownloader));

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileXmlDownloader(GenericFileStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Do initialization needed at the beginning of the download
        /// </summary>
        protected override void InitializeDownload()
        {
            // Prepare the transform
            xslTransform = GenericFileXmlUtility.LoadXslTransform(GenericStore.XmlXsltContent);
        }

        /// <summary>
        /// Load the orders from the given GenericFileInstance
        /// </summary>
        protected override bool ImportFile(GenericFileInstance file)
        {
            XmlDocument xmlDocument = GenericFileXmlUtility.LoadAndValidateDocument(file.ReadAllText(), xslTransform);

            XPathNavigator xpath = xmlDocument.CreateNavigator();
            XPathNodeIterator orderNodes = xpath.Select("//Order");

            // go through each order in the batch
            while (orderNodes.MoveNext())
            {
                // Update the status
                Progress.Detail = string.Format("Importing order {0}...", (QuantitySaved + 1));

                XPathNavigator order = orderNodes.Current.Clone();
                LoadOrder(order);

                if (Progress.IsCancelRequested)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Extract the order from the xml
        /// </summary>
        private void LoadOrder(XPathNavigator xpath)
        {
            GenericResult<OrderEntity> result = InstantiateOrder(xpath);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order: {1}.", result.Message);
                return;
            }

            OrderEntity order = result.Value;

            GenericXmlOrderLoader.LoadOrder(order, this, null, xpath);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericFileXmlDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Instantiate the generic order based on the configured mapping and the specified XPath
        /// </summary>
        private GenericResult<OrderEntity> InstantiateOrder(XPathNavigator xpath)
        {
            // pull out the order number
            int orderNumber = XPathUtility.Evaluate(xpath, "OrderNumber", 0);

            // pull in pre/postfix options
            string prefix = XPathUtility.Evaluate(xpath, "OrderNumberPrefix", "");
            string postfix = XPathUtility.Evaluate(xpath, "OrderNumberPostfix", "");

            // create the identifier
            GenericFileOrderIdentifier orderIdentifier = new GenericFileOrderIdentifier(orderNumber, prefix, postfix);

            // get the order instance; Change this to our derived class once it's needed and exists
            return InstantiateOrder(orderIdentifier);
        }
    }
}

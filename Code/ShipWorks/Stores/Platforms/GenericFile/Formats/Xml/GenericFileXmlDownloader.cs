using System;
using System.Data.SqlClient;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using log4net;
using System.Xml;
using System.Xml.Xsl;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Import.Xml;

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
                Progress.Detail = $"Importing order {QuantitySaved + 1}...";

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
            OrderEntity order = InstantiateOrder(xpath);

            GenericXmlOrderLoader.LoadOrder(order, this, null, xpath);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericFileXmlDownloader.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Instantiate the generic order based on the configured mapping and the specified XPath
        /// </summary>
        private OrderEntity InstantiateOrder(XPathNavigator xpath)
        {
            // pull out the order number
            string orderNumber = XPathUtility.Evaluate(xpath, "OrderNumber", String.Empty).TrimStart('0');

            // pull in pre/postfix options
            string prefix = XPathUtility.Evaluate(xpath, "OrderNumberPrefix", "");
            string postfix = XPathUtility.Evaluate(xpath, "OrderNumberPostfix", "");

            // create the identifier
            OrderIdentifier orderIdentifier = storeType.CreateOrderIdentifier(orderNumber, prefix, postfix);

            // get the order instance; Change this to our derived class once it's needed and exists
            OrderEntity order = InstantiateOrder(orderIdentifier);

            return order;
        }
    }
}

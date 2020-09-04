﻿using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import.Xml;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    /// <summary>
    /// Downloader implementation for importing from XML files
    /// </summary>
    [Component]
    public class GenericFileXmlDownloader : GenericFileDownloaderBase, IGenericFileXmlDownloader
    {
        // Transform to use, if any
        private XslCompiledTransform xslTransform = null;

        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(GenericFileXmlDownloader));

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public GenericFileXmlDownloader(GenericFileStoreEntity store,
            Func<StoreEntity, GenericFileStoreType> getStoreType,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory,
            IWarehouseOrderClient warehouseOrderClient,
            ILicenseService licenseService)
            : base(store, getStoreType, configurationData, sqlAdapterFactory, warehouseOrderClient, licenseService)
        {

        }

        /// <summary>
        /// Do initialization needed at the beginning of the download
        /// </summary>
        protected override void InitializeDownload()
        {
            // Prepare the transform
            xslTransform = GenericFileXmlUtility.LoadXslTransform(GenericStoreEntity.XmlXsltContent);
        }

        /// <summary>
        /// Load the orders from the given GenericFileInstance
        /// </summary>
        protected override async Task<bool> ImportFile(GenericFileInstance file)
        {
            string fileText = await file.ReadAllTextAsync().ConfigureAwait(false);
            XmlDocument xmlDocument = GenericFileXmlUtility.LoadAndValidateDocument(fileText, xslTransform);

            XPathNavigator xpath = xmlDocument.CreateNavigator();
            XPathNodeIterator orderNodes = xpath.Select("//Order");

            // go through each order in the batch
            while (orderNodes.MoveNext())
            {
                // Update the status
                Progress.Detail = $"Importing order {QuantitySaved + 1}...";

                XPathNavigator order = orderNodes.Current.Clone();
                await LoadOrder(order).ConfigureAwait(false);

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
        private async Task LoadOrder(XPathNavigator xpath)
        {
            GenericResult<OrderEntity> result = await InstantiateOrder(xpath).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order: {0}.", result.Message);
                return;
            }

            OrderEntity order = result.Value;

            GenericXmlOrderLoader.LoadOrder(order, this, null, xpath);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericFileXmlDownloader.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Instantiate the generic order based on the configured mapping and the specified XPath
        /// </summary>
        private Task<GenericResult<OrderEntity>> InstantiateOrder(XPathNavigator xpath)
        {
            // pull out the order number
            string orderNumber = XPathUtility.Evaluate(xpath, "OrderNumber", String.Empty);

            // pull in pre/postfix options
            string prefix = XPathUtility.Evaluate(xpath, "OrderNumberPrefix", "");
            string postfix = XPathUtility.Evaluate(xpath, "OrderNumberPostfix", "");

            // create the identifier
            OrderIdentifier orderIdentifier = storeType.CreateOrderIdentifier(orderNumber, prefix, postfix);

            // get the order instance; Change this to our derived class once it's needed and exists
            return InstantiateOrder(orderIdentifier);
        }
    }
}

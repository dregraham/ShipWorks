using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Downloader for Buy.com stores.
    /// Heavily leverages GenericSpreadsheetOrderLoader to load orders.
    /// </summary>
    [KeyedComponent(typeof(IStoreDownloader), StoreTypeCode.BuyDotCom)]
    public class BuyDotComDownloader : OrderElementFactoryDownloaderBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BuyDotComDownloader));
        GenericCsvMap csvMap;
        private readonly IFtpClientFactory ftpClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComDownloader(StoreEntity store,
            Func<StoreEntity, BuyDotComStoreType> getStoreType,
            IConfigurationData configurationData,
            IFtpClientFactory ftpClientFactory,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, getStoreType(store), configurationData, sqlAdapterFactory)
        {
            this.ftpClientFactory = ftpClientFactory;
            string mapXml = ResourceUtility.ReadString("ShipWorks.Stores.Platforms.BuyDotCom.BuyDotComOrderImportMap.swcsvm");

            csvMap = new GenericCsvMap(new BuyDotComOrderImportSchema(), mapXml);
        }

        /// <summary>
        /// Download Orders
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to
        /// associate any store-specific download properties/metrics.</param>
        protected override async Task Download(TrackedDurationEvent trackedDurationEvent)
        {
            Progress.Detail = "Checking for orders...";

            try
            {
                if (InterapptiveOnly.MagicKeysDown)
                {
                    string fileName = (string) Program.MainForm.Invoke(new Func<string>(() =>
                        {
                            using (OpenFileDialog dlg = new OpenFileDialog())
                            {
                                if (dlg.ShowDialog() == DialogResult.OK)
                                {
                                    return dlg.FileName;
                                }
                            }

                            return "";
                        }));

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string fileText = await ReadAllTextAsync(fileName).ConfigureAwait(false);
                        await LoadOrdersFromCsv(fileText).ConfigureAwait(false);

                        Progress.Detail = "Done";
                    }
                }
                else
                {
                    using (IFtpClient ftpClient = await ftpClientFactory.LoginAsync(Store as IBuyDotComStoreEntity).ConfigureAwait(false))
                    //using (BuyDotComFtpClient ftpClient = new BuyDotComFtpClient((BuyDotComStoreEntity) Store))
                    {
                        List<string> fileNames = await ftpClient.GetOrderFileNames().ConfigureAwait(false);

                        if (fileNames.Count == 0)
                        {
                            Progress.Detail = "No orders to download.";
                            Progress.PercentComplete = 100;
                            return;
                        }

                        // Get all the files we know about
                        foreach (string fileName in fileNames)
                        {
                            // Check if it has been canceled
                            if (Progress.IsCancelRequested)
                            {
                                return;
                            }

                            await DownloadFtpFile(fileName, ftpClient).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (BuyDotComException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Read all text async
        /// </summary>
        private async Task<string> ReadAllTextAsync(string fileName)
        {
            using (Stream stream = File.OpenRead(fileName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Download File and process orders
        /// </summary>
        private async Task<bool> DownloadFtpFile(string fileName, IFtpClient ftpClient)
        {
            try
            {
                // Download the content of the file
                string fileContent = await ftpClient.GetOrderFileContent(fileName).ConfigureAwait(false);

                // Load orders from the content
                bool result = await LoadOrdersFromCsv(fileContent).ConfigureAwait(false);

                // Move the file to the Archive folder if configured (default)
                if (BuyDotComUtility.ArchiveFileAfterDownload)
                {
                    await ftpClient.ArchiveOrder(fileName).ConfigureAwait(false);
                }

                return result;
            }
            catch (GenericSpreadsheetException ex)
            {
                throw new BuyDotComException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Load Buy.com orders from the given file content
        /// </summary>
        private async Task<bool> LoadOrdersFromCsv(string csvContent)
        {
            using (GenericCsvReader csvReader = new GenericCsvReader(csvMap, csvContent))
            {
                while (csvReader.NextRecord())
                {
                    // Update the status
                    Progress.Detail = string.Format("Importing record {0}...", (QuantitySaved + 1));

                    // Create and load in the order data
                    GenericResult<OrderEntity> result = await InstantiateOrder(csvReader).ConfigureAwait(false);
                    if (result.Failure)
                    {
                        log.InfoFormat("Skipping order: {1}.", result.Message);
                        continue;
                    }

                    OrderEntity order = result.Value;

                    BuyDotComOrderLoader loader = new BuyDotComOrderLoader();
                    loader.Load(order, csvReader, this);

                    SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "BuyDotComDownloader.LoadOrder");
                    await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);

                    if (Progress.IsCancelRequested)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Instantiate the generic order based on the reader
        /// </summary>
        private Task<GenericResult<OrderEntity>> InstantiateOrder(GenericCsvReader reader)
        {
            // pull out the order number
            string orderNumber = reader.ReadField("Order.Number", "0");

            // get the order instance; Change this to our derived class once it's needed and exists
            return InstantiateOrder(new AlphaNumericOrderIdentifier(orderNumber));
        }
    }
}

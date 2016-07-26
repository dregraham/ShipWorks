using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Stores.Communication;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using System.IO;
using System.Reflection;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Stores.Content;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using System.Windows.Forms;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Downloader for Buy.com stores.
    /// Heavily leverages GenericSpreadsheetOrderLoader to load orders.
    /// </summary>
    public class BuyDotComDownloader : OrderElementFactoryDownloaderBase
    {        
        GenericCsvMap csvMap;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComDownloader(BuyDotComStoreEntity store) 
            : base(store)
        {
            string mapXml = ResourceUtility.ReadString("ShipWorks.Stores.Platforms.BuyDotCom.BuyDotComOrderImportMap.swcsvm");

            csvMap = new GenericCsvMap(new BuyDotComOrderImportSchema(), mapXml);
        }

        /// <summary>
        /// Download Orders
        /// </summary>
        /// <param name="trackedDurationEvent">The telemetry event that can be used to 
        /// associate any store-specific download properties/metrics.</param>
        protected override void Download(TrackedDurationEvent trackedDurationEvent)
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
                        LoadOrdersFromCsv(File.ReadAllText(fileName));

                        Progress.Detail = "Done";
                    }
                }
                else
                {
                    using (BuyDotComFtpClient ftpClient = new BuyDotComFtpClient((BuyDotComStoreEntity) Store))
                    {
                        List<string> fileNames = ftpClient.GetOrderFileNames();

                        if (fileNames.Count == 0)
                        {
                            Progress.Detail = "No orders to download.";
                            Progress.PercentComplete = 100;
                            return;
                        }

                        // Get all the files we know about
                        foreach (string fileName in fileNames)
                        {
                            // Check if it has been cancelled
                            if (Progress.IsCancelRequested)
                            {
                                return;
                            }

                            if (!DownloadFtpFile(fileName, ftpClient))
                            {
                                return;
                            }
                        }
                    }
                }
            }
            catch (BuyDotComException ex)
            { 
                throw new DownloadException(ex.Message,ex);
            }
            catch (SqlForeignKeyException ex)
            {
                throw new DownloadException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Download File and process orders
        /// </summary>
        private bool DownloadFtpFile(string fileName, BuyDotComFtpClient ftpClient)
        {
            try
            {
                // Download the content of the file
                string fileContent = ftpClient.GetOrderFileContent(fileName);

                // Load orders from the content
                bool result = LoadOrdersFromCsv(fileContent);

                // Move the file to the Archive folder if configured (default)
                if (BuyDotComUtility.ArchiveFileAfterDownload)
                {
                    ftpClient.ArchiveOrder(fileName);
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
        private bool LoadOrdersFromCsv(string csvContent)
        {
            using (GenericCsvReader csvReader = new GenericCsvReader(csvMap, csvContent))
            {
                while (csvReader.NextRecord())
                {
                    // Update the status
                    Progress.Detail = string.Format("Importing record {0}...", (QuantitySaved + 1));

                    // Create and load in the order data
                    OrderEntity order = InstantiateOrder(csvReader);

                    BuyDotComOrderLoader loader = new BuyDotComOrderLoader();
                    loader.Load(order, csvReader, this);

                    SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "BuyDotComDownloader.LoadOrder");
                    retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));

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
        private OrderEntity InstantiateOrder(GenericCsvReader reader)
        {
            // pull out the order number
            long orderNumber = reader.ReadField("Order.Number", 0L, false);

            // get the order instance; Change this to our derived class once it's needed and exists
            OrderEntity order = InstantiateOrder(new OrderNumberIdentifier(orderNumber));

            return order;
        }
    }
}

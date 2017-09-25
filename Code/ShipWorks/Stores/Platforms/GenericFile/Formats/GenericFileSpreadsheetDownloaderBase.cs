using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericFile.Sources;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats
{
    /// <summary>
    /// Base downloader for spreadsheets
    /// </summary>
    public abstract class GenericFileSpreadsheetDownloaderBase : GenericFileDownloaderBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileSpreadsheetDownloaderBase));

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericFileSpreadsheetDownloaderBase(GenericFileStoreEntity store,
            Func<StoreEntity, GenericFileStoreType> getStoreType,
            IConfigurationData configurationData,
            ISqlAdapterFactory sqlAdapterFactory)
            : base(store, getStoreType, configurationData, sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Create a spreadsheet reader for reading the given file instance
        /// </summary>
        protected abstract GenericSpreadsheetReader CreateReader(GenericFileInstance file);

        /// <summary>
        /// Load the order from the reader that is positioned on the row we want to load
        /// </summary>
        protected async Task LoadOrder(GenericSpreadsheetReader reader)
        {
            GenericResult<OrderEntity> result = await InstantiateOrder(reader).ConfigureAwait(false);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order: {0}.", result.Message);
                return;
            }

            OrderEntity order = result.Value;

            GenericSpreadsheetOrderLoader loader = new GenericSpreadsheetOrderLoader();
            loader.Load(order, reader, this);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericFileSpreadsheetDownloaderBase.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Instantiate the generic order based on the reader
        /// </summary>
        private Task<GenericResult<OrderEntity>> InstantiateOrder(GenericSpreadsheetReader reader)
        {
            // pull out the order number
            string orderNumber = reader.ReadField("Order.Number", "");

            // We strip out leading 0's. If all 0's, TrimStart would make it an empty string, 
            // so in that case, we leave a single 0.
            orderNumber = orderNumber.All(n => n == '0') ? "0" : orderNumber.TrimStart('0');

            // create the identifier
            OrderIdentifier orderIdentifier = storeType.CreateOrderIdentifier(orderNumber, string.Empty, string.Empty);

            // get the order instance; Change this to our derived class once it's needed and exists
            return InstantiateOrder(orderIdentifier);
        }

        /// <summary>
        /// Load the orders from the given GenericFileInstance
        /// </summary>
        protected override async Task<bool> ImportFile(GenericFileInstance file)
        {
            try
            {
                using (GenericSpreadsheetReader reader = CreateReader(file))
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    using (new LoggedStopwatch(LogManager.GetLogger(typeof(GenericFileSpreadsheetDownloaderBase)), "GenFile Importing orders time to run."))
                    {
                        while (reader.NextRecord())
                        {
                            // Update the status
                            Progress.Detail = $"Importing record {(QuantitySaved + 1)}...";

                            await LoadOrder(reader).ConfigureAwait(false);

                            if (Progress.IsCancelRequested)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (GenericSpreadsheetException ex)
            {
                throw new GenericFileStoreException(ex.Message, ex);
            }
        }
    }
}

using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats
{
    /// <summary>
    /// Base downloader for spreadsheets
    /// </summary>
    public abstract class GenericFileSpreadsheetDownloaderBase : GenericFileDownloaderBase
    {
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
            OrderEntity order = await InstantiateOrder(reader).ConfigureAwait(false);

            GenericSpreadsheetOrderLoader loader = new GenericSpreadsheetOrderLoader();
            loader.Load(order, reader, this);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericFileSpreadsheetDownloaderBase.LoadOrder");
            await retryAdapter.ExecuteWithRetryAsync(() => SaveDownloadedOrder(order)).ConfigureAwait(false);
        }

        /// <summary>
        /// Instantiate the generic order based on the reader
        /// </summary>
        private Task<OrderEntity> InstantiateOrder(GenericSpreadsheetReader reader)
        {
            // pull out the order number
            long orderNumber = reader.ReadField("Order.Number", 0L, false);

            // pull in pre/postfix options
            string prefix = "";
            string postfix = "";

            // create the identifier
            GenericFileOrderIdentifier orderIdentifier = new GenericFileOrderIdentifier(orderNumber, prefix, postfix);

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
                    double speed = 0;
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    using (new LoggedStopwatch(LogManager.GetLogger(typeof(GenericFileSpreadsheetDownloaderBase)), "GenFile Importing orders time to run."))
                    {
                        while (reader.NextRecord())
                        {
                            // Update the status
                            Progress.Detail = $"Importing record {(QuantitySaved + 1)}... {speed:##.##} ms/order";

                            await LoadOrder(reader).ConfigureAwait(false);

                            if (Progress.IsCancelRequested)
                            {
                                return false;
                            }

                            speed = sw.ElapsedMilliseconds * 1.0 / QuantitySaved * 1.0;
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

using System.Data.SqlClient;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Administration.Retry;
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
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileSpreadsheetDownloaderBase));

        /// <summary>
        /// Constructor
        /// </summary>
        protected GenericFileSpreadsheetDownloaderBase(GenericFileStoreEntity store)
            : base(store)
        {

        }

        /// <summary>
        /// Create a spreadsheet reader for reading the given file instance
        /// </summary>
        protected abstract GenericSpreadsheetReader CreateReader(GenericFileInstance file);

        /// <summary>
        /// Load the order from the reader that is positioned on the row we want to load
        /// </summary>
        protected void LoadOrder(GenericSpreadsheetReader reader)
        {
            GenericResult<OrderEntity> result = InstantiateOrder(reader);
            if (result.Failure)
            {
                log.InfoFormat("Skipping order: {1}.", result.Message);
                return;
            }

            OrderEntity order = result.Value;

            GenericSpreadsheetOrderLoader loader = new GenericSpreadsheetOrderLoader();
            loader.Load(order, reader, this);

            // Save the downloaded order
            SqlAdapterRetry<SqlException> retryAdapter = new SqlAdapterRetry<SqlException>(5, -5, "GenericFileSpreadsheetDownloaderBase.LoadOrder");
            retryAdapter.ExecuteWithRetry(() => SaveDownloadedOrder(order));
        }

        /// <summary>
        /// Instantiate the generic order based on the reader
        /// </summary>
        private GenericResult<OrderEntity> InstantiateOrder(GenericSpreadsheetReader reader)
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
        protected override bool ImportFile(GenericFileInstance file)
        {
            try
            {
                using (GenericSpreadsheetReader reader = CreateReader(file))
                {
                    while (reader.NextRecord())
                    {
                        // Update the status
                        Progress.Detail = string.Format("Importing record {0}...", (QuantitySaved + 1));

                        LoadOrder(reader);

                        if (Progress.IsCancelRequested)
                        {
                            return false;
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

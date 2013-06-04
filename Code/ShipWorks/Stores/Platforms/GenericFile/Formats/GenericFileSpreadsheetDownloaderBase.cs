using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
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
            OrderEntity order = InstantiateOrder(reader);

            GenericSpreadsheetOrderLoader loader = new GenericSpreadsheetOrderLoader();
            loader.Load(order, reader, this);

            // Save the downloaded order
            SaveDownloadedOrder(order);
        }

        /// <summary>
        /// Instantiate the generic order based on the reader
        /// </summary>
        private OrderEntity InstantiateOrder(GenericSpreadsheetReader reader)
        {
            // pull out the order number
            long orderNumber = reader.ReadField("Order.Number", 0L, false);

            // pull in pre/postfix options
            string prefix = "";
            string postfix = "";

            // create the identifier
            GenericFileOrderIdentifier orderIdentifier = new GenericFileOrderIdentifier(orderNumber, prefix, postfix);

            // get the order instance; Change this to our derived class once it's needed and exists
            OrderEntity order = InstantiateOrder(orderIdentifier);

            return order;
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

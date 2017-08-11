extern alias rebex2015;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using rebex2015::Rebex.IO;
using rebex2015::Rebex.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Wraps all FTP operations for dealing with buy.com
    /// </summary>
    [Component]
    public class BuyDotComFtpClient : IFtpClient
    {
        private readonly ILog log;
        private IFtp ftp;

        public BuyDotComFtpClient(IFtp ftp, Func<Type, ILog> createLogger)
        {
            this.ftp = ftp;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Gets list of order files queued for process
        /// </summary>
        public async Task<List<string>> GetOrderFileNames()
        {
            try
            {
                await ftp.ChangeDirectoryAsync("/Orders").ConfigureAwait(false);

                FileSystemItemCollection list = await ftp.GetListAsync().ConfigureAwait(false);

                return list
                    .Where(item =>
                        !item.Name.EndsWith("resp", StringComparison.OrdinalIgnoreCase) &&
                        !item.IsDirectory)
                    .Select(item => item.Name)
                    .ToList();
            }
            catch (NetworkSessionException ex)
            {
                throw new BuyDotComException("ShipWorks could not get the list of file names to download:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Given a name of the order file, return a stream that is ready to read
        /// </summary>
        public async Task<string> GetOrderFileContent(string fileName)
        {
            try
            {
                string filePath = string.Format("/Orders/{0}", fileName);

                ApiLogEntry apiLogger = new ApiLogEntry(ApiLogSource.BuyDotCom, "GetOrderFile");
                apiLogger.LogRequest(new XElement("Filename", filePath).ToString());

                using (MemoryStream stream = new MemoryStream())
                {
                    await ftp.GetFileAsync(filePath, stream).ConfigureAwait(false);
                    stream.Position = 0;

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string fileContent = await reader.ReadToEndAsync().ConfigureAwait(false);

                        apiLogger.LogResponse(fileContent, "txt");

                        return fileContent;
                    }
                }
            }
            catch (NetworkSessionException ex)
            {
                throw new BuyDotComException("ShipWorks could not download orders from Buy.com:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Move order file to archive directory.
        /// </summary>
        public async Task ArchiveOrder(string fileName)
        {
            try
            {
                await ftp.RenameAsync(string.Format("/Orders/{0}", fileName),
                    string.Format("/Orders/Archive/{0}", fileName)).ConfigureAwait(false);
            }
            catch (NetworkSessionException ex)
            {
                throw new BuyDotComException("ShipWorks could not archive an order:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Upload ship confirmation information.
        /// </summary>
        public async Task UploadShipConfirmation(List<BuyDotComShipConfirmation> confirmations)
        {
            StringWriter writer = new StringWriter();

            writer.WriteLine("receipt-id\treceipt-item-id\tquantity\ttracking-type\ttracking-number\tship-date");

            foreach (var confirmation in confirmations)
            {
                foreach (var orderLine in confirmation.OrderLines)
                {
                    writer.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                        confirmation.ReceiptID,
                        orderLine.ReceiptItemID,
                        (int) orderLine.Quantity,
                        (int) confirmation.TrackingType,
                        confirmation.TrackingNumber,
                        confirmation.ShipDate.ToString("MM/dd/yyyy")));
                }
            }

            ApiLogEntry apiLogger = new ApiLogEntry(ApiLogSource.BuyDotCom, "UploadShipConfirmation");
            apiLogger.LogRequest(writer.ToString(), "txt");

            try
            {
                await ftp.ChangeDirectoryAsync("/Fulfillment").ConfigureAwait(false);

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString())))
                {
                    await ftp.PutFileAsync(stream, string.Format("shipworks_{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now)).ConfigureAwait(false);
                }
            }
            catch (NetworkSessionException ex)
            {
                throw new BuyDotComException("ShipWorks could not upload the fulfillment to Buy.com:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose suggested by code analyzer.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ftp != null)
                {
                    try
                    {
                        ftp.Disconnect();
                        ftp.Dispose();
                        ftp = null;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to disconnect and dispose of FTP", ex);
                    }
                }
            }
        }
    }
}

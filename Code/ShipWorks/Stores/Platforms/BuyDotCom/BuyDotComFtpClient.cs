extern alias rebex2015;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rebex2015::Rebex.IO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.FileTransfer;
using rebex2015::Rebex.Net;
using System.IO;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;
using System.Xml.Linq;
using log4net;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Wraps all FTP operations for dealing with buy.com
    /// </summary>
    public class BuyDotComFtpClient : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(BuyDotComFtpClient));

        IFtp ftp;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComFtpClient(BuyDotComStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            try
            {
                ftp = FtpUtility.LogonToFtp(BuyDotComUtility.GetFtpAccount(store));
            }
            catch (FileTransferException ex)
            {
                throw new BuyDotComException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets list of order files queued for process
        /// </summary>
        public List<string> GetOrderFileNames()
        {
            try
            {
                ftp.ChangeDirectory("/Orders");

                FileSystemItemCollection list = ftp.GetList();

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
        public string GetOrderFileContent(string fileName)
        {
            try
            {
                string filePath = string.Format("/Orders/{0}", fileName);

                ApiLogEntry apiLogger = new ApiLogEntry(ApiLogSource.BuyDotCom, "GetOrderFile");
                apiLogger.LogRequest(new XElement("Filename", filePath).ToString());

                using (MemoryStream stream = new MemoryStream())
                {
                    ftp.GetFile(filePath, stream);
                    stream.Position = 0;

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string fileContent = reader.ReadToEnd();

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
        public void ArchiveOrder(string fileName)
        {
            try
            {
                ftp.Rename(string.Format("/Orders/{0}", fileName),
                    string.Format("/Orders/Archive/{0}", fileName));
            }
            catch (NetworkSessionException ex)
            {
                throw new BuyDotComException("ShipWorks could not archive an order:\n\n" + ex.Message, ex);
            }
        }

        /// <summary>
        /// Upload ship confirmation information.
        /// </summary>
        public void UploadShipConfirmation(List<BuyDotComShipConfirmation> confirmations)
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
                ftp.ChangeDirectory("/Fulfillment");

                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString())))
                {
                    ftp.PutFile(stream, string.Format("shipworks_{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now));
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

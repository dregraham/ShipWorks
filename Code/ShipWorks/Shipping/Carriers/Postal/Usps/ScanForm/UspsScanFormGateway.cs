using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Interapptive.Shared.Pdf;
using Rebex.Mail;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm
{
    /// <summary>
    /// An implementation of the IScanFormGateway interface that communicates with the USPS API
    /// for creating/obtaining SCAN forms.
    /// </summary>
    public class UspsScanFormGateway : IScanFormGateway
    {
        private string invalidCarrierMessage;
        private string invalidShipmentMessage;
        private IUspsWebClient webClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsScanFormGateway(IUspsWebClient webClient)
        {
            invalidCarrierMessage = "An attempt to create a USPS SCAN form was made for a carrier other than USPS.";
            invalidShipmentMessage = "Cannot create a USPS SCAN form for a shipment that was not shipped with USPS.";
            this.webClient = webClient;
        }

        /// <summary>
        /// Gets and sets the message that is displayed when an invalid shipment is used
        /// </summary>
        protected string InvalidShipmentMessage
        {
            get { return invalidShipmentMessage; }
            set { invalidShipmentMessage = value; }
        }

        /// <summary>
        /// Gets and sets the message that is displayed when an invalid carrier is used
        /// </summary>
        protected string InvalidCarrierMessage
        {
            get { return invalidCarrierMessage; }
            set { invalidCarrierMessage = value; }
        }

        /// <summary>
        /// Creates scan forms from the shipping carrier
        /// </summary>
        /// <param name="scanFormBatch">The batch to which the created scan forms should belong.</param>
        /// <param name="shipments">The shipments the scan form is being generated for.</param>
        /// <returns>A carrier-specific collection of scan form entity object.</returns>
        public IEnumerable<IEntity2> CreateScanForms(ScanFormBatch scanFormBatch, IEnumerable<ShipmentEntity> shipments)
        {
            UspsAccountEntity accountEntity = scanFormBatch.AccountEntity as UspsAccountEntity;
            if (accountEntity == null)
            {
                throw new UspsException(invalidCarrierMessage);
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new UspsException("There must be at least one shipment to create a SCAN form.");
            }

            // Grab all the USPS-specific shipments (this should be all of the shipments)
            IEnumerable<UspsShipmentEntity> uspsShipments = shipments.Select(s => s.Postal.Usps).Where(s => s != null);
            if (uspsShipments.Count() != shipments.Count())
            {
                throw new UspsException(invalidShipmentMessage);
            }

            // We have our list of USPS shipments, so call the API to create the SCAN form
            XDocument xDocument = webClient.CreateScanForm(uspsShipments, accountEntity);

            // Ensure that we have the correct amount of transactions and urls
            if (xDocument.Descendants("TransactionId").Count() != xDocument.Descendants("Url").Count())
            {
                throw new UspsException("Transactions and SCAN forms must be equal length.");
            }

            // Create entities for each returned scan form
            List<UspsScanFormEntity> entities = new List<UspsScanFormEntity>();

            for (int i = 0; i < xDocument.Descendants("TransactionId").Count(); i++)
            {
                // Populate the USPS scan form entity based on the response from the API
                UspsScanFormEntity scanEntity = new UspsScanFormEntity();
                scanEntity.UspsAccountID = accountEntity.UspsAccountID;
                scanEntity.CreatedDate = DateTime.UtcNow;
                scanEntity.ScanFormTransactionID = xDocument.Descendants("TransactionId").ElementAt(i).Value;
                scanEntity.ScanFormUrl = xDocument.Descendants("Url").ElementAt(i).Value;
                scanEntity.Description = "Non-cubic shipments";

                // Notify the batch of the new scan form
                scanFormBatch.CreateScanForm(scanEntity.Description, shipments, scanEntity, DownloadFormImages(scanEntity.ScanFormUrl));

                entities.Add(scanEntity);
            }

            return entities;
        }

        /// <summary>
        /// Downloads the SCAN form images.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A byte array containing the image data.</returns>
        private List<byte[]> DownloadFormImages(string url)
        {
            try
            {
                List<byte[]> images = new List<byte[]>();

                string[] scanFormUrls = new[] { url };
                if (url.Contains(" "))
                {
                    // This is really only applicable for DHL SCAN forms/manifest files as those are on 
                    // multiple pages. For USPS SCAN forms, the first URL contains the actual SCAN form 
                    // for Stamps.com
                    scanFormUrls = url.Split(new char[] { ' ' });
                }

                foreach (string scanFormUrl in scanFormUrls)
                {
                    using (WebClient client = new WebClient())
                    {
                        byte[] imageBytes = client.DownloadData(scanFormUrl);
                        images.Add(imageBytes);
                    }
                }

                return images;
            }
            catch (Exception ex)
            {
                throw new UspsException("ShipWorks was unable to download the SCAN form from USPS", ex);
            }
        }
    }
}

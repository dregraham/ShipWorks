﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.ScanForms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using System.IO;
using System.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// An implementation of the IScanFormGateway interface that communicates with the Stamps.com API
    /// for creating/obtaining SCAN forms.
    /// </summary>
    public class StampsScanFormGateway : IScanFormGateway
    {
        private string invalidCarrierMessage;
        private string invalidShipmentMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsScanFormGateway()
        {
            invalidCarrierMessage = "An attempt to create a Stamps.com SCAN form was made for a carrier other than Stamps.com.";
            invalidShipmentMessage = "Cannot create a Stamps.com SCAN form for a shipment that was not shipped with Stamps.com.";
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
            StampsAccountEntity accountEntity = scanFormBatch.AccountEntity as StampsAccountEntity;
            if (accountEntity == null)
            {
                throw new StampsException(invalidCarrierMessage);
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new StampsException("There must be at least one shipment to create a SCAN form.");
            }

            // Grab all the stamps-specific shipments (this should be all of the shipments)
            IEnumerable<StampsShipmentEntity> stampsShipments = shipments.Select(s => s.Postal.Stamps).Where(s => s != null);
            if (stampsShipments.Count() != shipments.Count())
            {
                throw new StampsException(invalidShipmentMessage);
            }

            // We have our list of Stamps.com shipments, so call the API to create the SCAN form
            XDocument xDocument = new StampsApiSession().CreateScanForm(stampsShipments, accountEntity);

            // Ensure that we have the correct amount of transactions and urls
            if (xDocument.Descendants("TransactionId").Count() != xDocument.Descendants("Url").Count())
            {
                throw new StampsException("Transactions and SCAN forms must be equal length.");
            }

            // Create entities for each returned scan form
            List<StampsScanFormEntity> entities = new List<StampsScanFormEntity>();

            for (int i = 0; i < xDocument.Descendants("TransactionId").Count(); i++)
            {
                // Populate the stamps scan form entity based on the response from the API
                StampsScanFormEntity scanEntity = new StampsScanFormEntity();
                scanEntity.StampsAccountID = accountEntity.StampsAccountID;
                scanEntity.CreatedDate = DateTime.UtcNow;
                scanEntity.ScanFormTransactionID = xDocument.Descendants("TransactionId").ElementAt(i).Value;
                scanEntity.ScanFormUrl = xDocument.Descendants("Url").ElementAt(i).Value;
                scanEntity.Description = "Non-cubic shipments";

                // Notify the batch of the new scan form
                scanFormBatch.CreateScanForm(scanEntity.Description, shipments, scanEntity, DownloadFormImage(scanEntity.ScanFormUrl));

                entities.Add(scanEntity);
            }

            return entities;
        }

        /// <summary>
        /// Downloads the form image.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>A byte array containing the image data.</returns>
        private byte[] DownloadFormImage(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] image = webClient.DownloadData(url);
                    return image;
                }
            }
            catch (Exception ex)
            {
                throw new StampsException("ShipWorks was unable to download the SCAN form from Stamps.com", ex);
            }
        }
    }
}

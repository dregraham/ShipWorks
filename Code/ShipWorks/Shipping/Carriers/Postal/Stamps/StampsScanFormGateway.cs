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
        /// <summary>
        /// Gets the scan form from the shipping carrier and populates the properties of the given scan form.
        /// </summary>
        /// <param name="scanForm">The scan form being populated.</param>
        /// <param name="shipments">The shipments the scan form is being generated for.</param>
        /// <returns>A carrier-specific scan form entity object.</returns>
        public IEntity2 FetchScanForm(ScanForm scanForm, IEnumerable<ShipmentEntity> shipments)
        {
            StampsAccountEntity accountEntity = scanForm.CarrierAccount.GetAccountEntity() as StampsAccountEntity;
            if (accountEntity == null)
            {
                throw new StampsException("An attempt to create a Stamps.com SCAN form was made for a carrier other than Stamps.com.");
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new StampsException("There must be at least one shipment to create a SCAN form.");
            }

            // Grab all the stamps-specific shipments (this should be all of the shipments)
            IEnumerable<StampsShipmentEntity> stampsShipments = shipments.Select(s => s.Postal.Stamps).Where(s => s != null);
            if (stampsShipments.Count() != shipments.Count())
            {
                throw new StampsException("Cannot create a Stamps.com SCAN form for a shipment that was not shipped with Stamps.com.");
            }

            // We have our list of Stamps.com shipments, so call the API to create the SCAN form
            XDocument xDocument = StampsApiSession.CreateScanForm(stampsShipments, accountEntity);

            // Populate the stamps scan form entity based on the response from the API
            StampsScanFormEntity scanEntity = new StampsScanFormEntity();
            scanEntity.StampsAccountID = accountEntity.StampsAccountID;            
            scanEntity.CreatedDate = DateTime.UtcNow;
            scanEntity.ShipmentCount = shipments.Count();
            scanEntity.ScanFormTransactionID = (string)xDocument.Descendants("TransactionId").Single();
            scanEntity.ScanFormUrl = (string)xDocument.Descendants("Url").Single();
            scanEntity.Description = scanForm.Description;

            // Now populate the scan form object itself based on the entity values and the image
            scanForm.CreatedDate = scanEntity.CreatedDate;
            scanForm.ShipmentCount = scanEntity.ShipmentCount;
            scanForm.Image = DownloadFormImage(scanEntity.ScanFormUrl);


            return scanEntity;
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

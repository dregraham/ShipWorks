﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.ScanForms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using Interapptive.Shared.Pdf;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// An implementation of the IScanFormGateway interface that communicates with the Endicia API
    /// for creating/obtaining SCAN forms.
    /// </summary>
    [Component(RegistrationType.Self)]
    public class EndiciaScanFormGateway : IScanFormGateway
    {
        private readonly IEndiciaApiClient endiciaApiClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaScanFormGateway(IEndiciaApiClient endiciaApiClient)
        {
            this.endiciaApiClient = endiciaApiClient;
        }

        /// <summary>
        /// Creates scan forms from the shipping carrier
        /// </summary>
        /// <param name="scanFormBatch">The batch to which the created scan forms should belong.</param>
        /// <param name="shipments">The shipments the scan form is being generated for.</param>
        /// <returns>A carrier-specific collection of scan form entity object.</returns>
        public IEnumerable<IEntity2> CreateScanForms(ScanFormBatch scanFormBatch, IEnumerable<ShipmentEntity> shipments)
        {
            EndiciaAccountEntity accountEntity = scanFormBatch.AccountEntity as EndiciaAccountEntity;
            if (accountEntity == null)
            {
                throw new EndiciaException("An attempt to create an Endicia SCAN form was made for a carrier other than Endicia.");
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new EndiciaException("There must be at least one shipment to create a SCAN form.");
            }

            // We have a scan form for an Endicia account, so we can obtain the scan form via the carrier API
            SCANResponse scanResponse = endiciaApiClient.GetScanForm(accountEntity, shipments);

            //Endicia can return an error without throwing a WebException
            if (!string.IsNullOrEmpty(scanResponse.ErrorMessage))
            {
                throw new EndiciaException(scanResponse.ErrorMessage);
            }

            EndiciaScanFormEntity scanEntity = new EndiciaScanFormEntity();
            scanEntity.EndiciaAccountID = accountEntity.EndiciaAccountID;
            scanEntity.EndiciaAccountNumber = accountEntity.AccountNumber;
            scanEntity.SubmissionID = scanResponse.SubmissionID;
            scanEntity.CreatedDate = DateTime.UtcNow;
            scanEntity.Description = "Non-cubic shipments";

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(scanResponse.SCANForm)))
            {
                // Notify the batch of the new scan form
                scanFormBatch.CreateScanForm(scanEntity.Description, shipments, scanEntity, new List<byte[]> { stream.ToArray()});
            }

            return new[] { scanEntity };
        }
    }
}
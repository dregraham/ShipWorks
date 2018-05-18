using System;
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
    public class EndiciaScanFormGateway : IScanFormGateway
    {
        private readonly IPdfDocumentFactory pdfDocumentFactory;
        private readonly IEndiciaApiClient endiciaApiClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaScanFormGateway(IPdfDocumentFactory pdfDocumentFactory, IEndiciaApiClient endiciaApiClient)
        {
            this.pdfDocumentFactory = pdfDocumentFactory;
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

            EndiciaScanFormEntity scanEntity = new EndiciaScanFormEntity();
            scanEntity.EndiciaAccountID = accountEntity.EndiciaAccountID;
            scanEntity.EndiciaAccountNumber = accountEntity.AccountNumber;
            scanEntity.SubmissionID = scanResponse.SubmissionID;
            scanEntity.CreatedDate = DateTime.UtcNow;
            scanEntity.Description = "Non-cubic shipments";

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(scanResponse.SCANForm)))
            {
                IPdfDocument doc = pdfDocumentFactory.Create(PdfDocumentType.BlackAndWhite);
                List<byte[]> scanFormPages = doc.SavePages(stream, (MemoryStream arg1, int arg2) => { return arg1.ToArray(); }).ToList();

                // Notify the batch of the new scan form
                scanFormBatch.CreateScanForm(scanEntity.Description, shipments, scanEntity, scanFormPages);
            }

            return new[] { scanEntity };
        }
    }
}
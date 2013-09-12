using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.ScanForms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Linq;
using System.IO;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An implementation of the IScanFormGateway interface that communicates with the Express1 API
    /// for creating/obtaining SCAN forms.
    /// </summary>
    public class Express1ScanFormGateway : IScanFormGateway
    {
        /// <summary>
        /// Gets the scan form from the shipping carrier and populates the properties of the given scan form.
        /// </summary>
        /// <param name="scanForm">The scan form being populated.</param>
        /// <param name="shipments">The shipments the scan form is being generated for.</param>
        /// <returns>A carrier-specific scan form entity object.</returns>
        public IEntity2 FetchScanForm(ScanForm scanForm, IEnumerable<ShipmentEntity> shipments)
        {
            EndiciaAccountEntity accountEntity = scanForm.CarrierAccount.GetAccountEntity() as EndiciaAccountEntity;
            if (accountEntity == null)
            {
                throw new Express1Exception("An attempt to create an Express1 SCAN form was made for a carrier other than Express1.");
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new Express1Exception("There must be at least one shipment to create a SCAN form.");
            }

            // We have a scan form for an Endicia account, so we can obtain the scan form via the Express1 API
            XDocument xDocument = Express1CustomerServiceClient.CreateScanForm(shipments);

            EndiciaScanFormEntity scanEntity = new EndiciaScanFormEntity();
            scanEntity.EndiciaAccountID = accountEntity.EndiciaAccountID;
            scanEntity.EndiciaAccountNumber = accountEntity.AccountNumber;
            scanEntity.SubmissionID = (string)xDocument.Descendants("SubmissionID").Single();
            scanEntity.CreatedDate = DateTime.UtcNow;
            scanEntity.ShipmentCount = shipments.Count();
            scanEntity.Description = scanForm.Description;

            // The scan form does not have a concept of the underlying entity type, so we also need to populate
            // the properties of the ScanForm object based on the entity
            scanForm.CreatedDate = scanEntity.CreatedDate;
            scanForm.ShipmentCount = scanEntity.ShipmentCount;

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String((string)xDocument.Descendants("SCANForm").Single())))
            {
                // Write the byte array of the form to the scan form's image property
                scanForm.Image = stream.ToArray();
            }

            return scanEntity;
        }
    }
}

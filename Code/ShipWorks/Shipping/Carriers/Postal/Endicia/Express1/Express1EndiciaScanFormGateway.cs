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
    public class Express1EndiciaScanFormGateway : IScanFormGateway
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
                throw new Express1EndiciaException("An attempt to create an Express1 SCAN form was made for a carrier other than Express1.");
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new Express1EndiciaException("There must be at least one shipment to create a SCAN form.");
            }

            // We have a scan form for an Endicia account, so we can obtain the scan form via the Express1 API
            XDocument xDocument = Express1EndiciaCustomerServiceClient.CreateScanForm(shipments);

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
                throw new Express1EndiciaException("An attempt to create an Express1 SCAN form was made for a carrier other than Express1.");
            }

            if (shipments == null || shipments.Count() == 0)
            {
                throw new Express1EndiciaException("There must be at least one shipment to create a SCAN form.");
            }

            List<EndiciaScanFormEntity> entities = new List<EndiciaScanFormEntity>();

            // Express 1 Endicia cubic shipments are created on a separate Endicia account on the Express 1 backend, so we need 
            // to separate the Express 1 Endicia cubic shipments into a separate SCAN form and request a scan form for those separately
            // (we don't want the user to have to split these out)
            List<ShipmentEntity> express1EndiciaCubicShipments = shipments.Where(selected => (ShipmentTypeCode)selected.ShipmentType == ShipmentTypeCode.Express1Endicia && (PostalPackagingType)selected.Postal.PackagingType == PostalPackagingType.Cubic).ToList();
            CreateScanForm(entities, scanFormBatch, shipments, accountEntity, "Cubic shipments");

            List<ShipmentEntity> allOtherShipments = shipments.Except(express1EndiciaCubicShipments).ToList();
            CreateScanForm(entities, scanFormBatch, allOtherShipments, accountEntity, "Non-cubic shipments");

            return entities;
        }

        private void CreateScanForm(List<EndiciaScanFormEntity> entities, ScanFormBatch scanFormBatch, IEnumerable<ShipmentEntity> shipments, EndiciaAccountEntity accountEntity, string description)
        {
            // Don't do anything if there are no shipments
            if (!shipments.Any())
            {
                return;
            }

            // We have a scan form for an Endicia account, so we can obtain the scan form via the Express1 API
            XDocument xDocument = Express1EndiciaCustomerServiceClient.CreateScanForm(shipments);

            EndiciaScanFormEntity scanEntity = new EndiciaScanFormEntity();
            scanEntity.EndiciaAccountID = accountEntity.EndiciaAccountID;
            scanEntity.EndiciaAccountNumber = accountEntity.AccountNumber;
            scanEntity.SubmissionID = (string) xDocument.Descendants("SubmissionID").Single();
            scanEntity.CreatedDate = DateTime.UtcNow;
            scanEntity.ShipmentCount = shipments.Count();
            scanEntity.Description = description;

            using (MemoryStream stream = new MemoryStream(Convert.FromBase64String((string) xDocument.Descendants("SCANForm").Single())))
            {
                // Notify the batch of the new scan form
                scanFormBatch.CreateScanForm(scanEntity.Description, shipments, stream.ToArray());
            }

            entities.Add(scanEntity);
        }
    }
}

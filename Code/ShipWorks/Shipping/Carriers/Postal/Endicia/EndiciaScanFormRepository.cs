using System;
using System.Collections.Generic;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// An Endicia implementation of the IScanFormEntityRepository that saves an Endicia
    /// SCAN form entity to the database along with the image of the form.
    /// </summary>
    public class EndiciaScanFormRepository : IScanFormRepository
    {
        private bool isEndiciaCarrier;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaScanFormRepository"/> class.
        /// </summary>
        /// <param name="isEndiciaCarrier">if set to <c>true</c> [is endicia carrier].</param>
        public EndiciaScanFormRepository(bool isEndiciaCarrier)
        {
            this.isEndiciaCarrier = isEndiciaCarrier;
        }

        /// <summary>
        /// Saves the specified scan form.
        /// </summary>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>The ID value that is generated as a result saving the scan form.</returns>
        /// <exception cref="EndiciaException"></exception>
        public long Save(ScanFormBatch scanFormBatch)
        {
            ScanFormBatchEntity batchEntity = BuildBatchEntity(scanFormBatch);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the scan record.
                adapter.SaveAndRefetch(batchEntity);

                foreach (ScanForm scanForm in scanFormBatch.ScanForms)
                {
                    // Set the ID on our scan form object to the ID value on the entity
                    EndiciaScanFormEntity endiciaScanFormEntity = scanForm.ScanFormEntity as EndiciaScanFormEntity;
                    scanForm.ScanFormId = endiciaScanFormEntity.EndiciaScanFormID;

                    // Should have the scan form ID populated now so save the image to the data source. The Endicia-based 
                    // APIs always generates a single image for the SCAN form
                    DataResourceManager.CreateFromBytes(scanForm.Images.FirstOrDefault(), endiciaScanFormEntity.EndiciaScanFormID, "SCAN Form");

                    // Now we have to update each shipment with the scan form record ID
                    foreach (ShipmentEntity shipment in scanForm.Shipments)
                    {
                        shipment.Postal.Endicia.ScanFormBatchID = batchEntity.ScanFormBatchID;
                        adapter.SaveAndRefetch(shipment.Postal.Endicia);
                    }
                }

                adapter.Commit();
            }

            return batchEntity.ScanFormBatchID;
        }

        /// <summary>
        /// Builds the LLBL Gen SCAN form batch entity form the ScanFormBatch object provided.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <returns>A ScanFormBatchEntity populated using the ScanFormBatch object.</returns>
        /// <exception cref="EndiciaException">The SCAN forms in the batch are not Endicia/Express1 SCAN forms.</exception>
        private ScanFormBatchEntity BuildBatchEntity(ScanFormBatch batch)
        {
            ScanFormBatchEntity batchEntity = new ScanFormBatchEntity();
            batchEntity.CreatedDate = batch.CreatedDate;
            batchEntity.ShipmentType = (int) batch.ShipmentType;
            batchEntity.ShipmentCount = batch.ShipmentCount;

            foreach (ScanForm scanForm in batch.ScanForms)
            {
                EndiciaScanFormEntity endiciaScanFormEntity = scanForm.ScanFormEntity as EndiciaScanFormEntity;
                if (endiciaScanFormEntity == null)
                {
                    string message = string.Format("The SCAN form provided was not an {0} SCAN form.", isEndiciaCarrier ? "Endicia" : "Express1");
                    throw new EndiciaException(message);
                }

                batchEntity.EndiciaScanForms.Add(endiciaScanFormEntity);
            }

            return batchEntity;
        }
        
        /// <summary>
        /// Gets existing scan forms for a carrier.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <returns>A collection of ScanForm objects.</returns>
        public IEnumerable<ScanForm> GetExistingScanForms(IScanFormCarrierAccount carrierAccount)
        {
            EndiciaAccountEntity accountEntity = carrierAccount.GetAccountEntity() as EndiciaAccountEntity;
            if (accountEntity == null)
            {
                string message = string.Format("ShipWorks was unable to retrieve existing SCAN forms: the {0} account could not be loaded.", isEndiciaCarrier ? "Endicia" : "Express1");
                throw new EndiciaException(message);
            }

            List<ScanForm> scanForms = new List<ScanForm>();

            // Get all the forms for this account from the last 8 days
            EndiciaScanFormCollection endiciaForms = EndiciaScanFormCollection.Fetch(SqlAdapter.Default,
                EndiciaScanFormFields.EndiciaAccountID == accountEntity.EndiciaAccountID &
                EndiciaScanFormFields.CreatedDate > DateTime.UtcNow.Subtract(TimeSpan.FromDays(8)));

            foreach (EndiciaScanFormEntity endiciaForm in endiciaForms)
            {
                // Create a general scan form using the data from the Endicia-specific form
                ScanForm scanForm = new ScanForm(carrierAccount, endiciaForm.EndiciaScanFormID, endiciaForm.ScanFormBatchID, endiciaForm.Description, endiciaForm.CreatedDate);
                scanForms.Add(scanForm);
            }

            return scanForms;
        }

        /// <summary>
        /// Gets the IDs of the shipments eligible for a SCAN form.
        /// </summary>
        /// <param name="bucket">The bucket containing the predicate for the query.</param>
        /// <returns>A collection of shipment ID values.</returns>
        public IEnumerable<long> GetShipmentIDs(RelationPredicateBucket bucket)
        {
            // We just need shipment ID's
            ResultsetFields resultFields = new ResultsetFields(1);
            resultFields.DefineField(ShipmentFields.ShipmentID, 0, "ShipmentID", "");
                        
            // Do the fetch
            using (SqlDataReader reader = (SqlDataReader)SqlAdapter.Default.FetchDataReader(resultFields, bucket, CommandBehavior.CloseConnection, 0, true))
            {
                List<long> keys = new List<long>();

                while (reader.Read())
                {
                    keys.Add(reader.GetInt64(0));
                }

                return keys;
            }
        }


        /// <summary>
        /// Gets the existing scan form batches for a carrier account.
        /// </summary>
        /// <param name="carrierAccount">The carrier account.</param>
        /// <returns>A collection of ScanFormBatch objects.</returns>
        public IEnumerable<ScanFormBatch> GetExistingScanFormBatches(IScanFormCarrierAccount carrierAccount)
        {
            EndiciaAccountEntity accountEntity = carrierAccount.GetAccountEntity() as EndiciaAccountEntity;
            if (accountEntity == null)
            {
                string message = string.Format("ShipWorks was unable to retrieve existing SCAN forms: the {0} account could not be loaded.", isEndiciaCarrier ? "Endicia" : "Express1");
                throw new EndiciaException(message);
            }

            List<ScanFormBatch> batches = new List<ScanFormBatch>();

            using (ScanFormBatchCollection batchEntities = new ScanFormBatchCollection())
            {
                // Get all the batches for this account from the last 8 days
                RelationPredicateBucket predicate = new RelationPredicateBucket
                    (
                    EndiciaScanFormFields.EndiciaAccountID == accountEntity.EndiciaAccountID &
                    ScanFormBatchFields.CreatedDate > DateTime.UtcNow.Subtract(TimeSpan.FromDays(8))
                    );

                predicate.Relations.Add(ScanFormBatchEntity.Relations.EndiciaScanFormEntityUsingScanFormBatchID);
                SqlAdapter.Default.FetchEntityCollection(batchEntities, predicate);


                foreach (ScanFormBatchEntity batchEntity in batchEntities)
                {
                    List<ScanForm> scanForms = new List<ScanForm>();

                    // Get all the forms for this account from the last 8 days
                    EndiciaScanFormCollection endiciaForms = EndiciaScanFormCollection.Fetch(SqlAdapter.Default,
                                                                                             EndiciaScanFormFields.ScanFormBatchID == batchEntity.ScanFormBatchID);

                    foreach (EndiciaScanFormEntity scanFormEntity in endiciaForms)
                    {
                        ScanForm scanForm = new ScanForm(carrierAccount, scanFormEntity.EndiciaScanFormID, scanFormEntity.ScanFormBatchID, scanFormEntity.Description, scanFormEntity.CreatedDate);
                        scanForms.Add(scanForm);
                    }

                    // Create a general scan form using the data from the Endicia-specific form
                    ScanFormBatch batch = new ScanFormBatch(carrierAccount, new DefaultScanFormPrinter(), scanForms, new DefaultScanFormBatchShipmentRepository())
                    {
                        BatchId = batchEntity.ScanFormBatchID,
                        CreatedDate = batchEntity.CreatedDate,
                        ShipmentType = (ShipmentTypeCode) batchEntity.ShipmentType
                    };

                    batches.Add(batch);
                }
            }

            return batches;
        }
    }
}

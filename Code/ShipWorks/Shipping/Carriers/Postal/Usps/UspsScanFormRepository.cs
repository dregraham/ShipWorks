using System;
using System.Collections.Generic;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data.SqlClient;
using System.Data;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A Stamps.com implementation of the IScanFormEntityRepository that saves a Stamps
    /// SCAN form entity to the database along with the image of the form.
    /// </summary>
    public class UspsScanFormRepository : IScanFormRepository
    {
        private readonly StampsResellerType stampsResellerType;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsScanFormRepository"/> class.
        /// </summary>
        /// <param name="stampsResellerType">The stamps reseller type.</param>
        public UspsScanFormRepository(StampsResellerType stampsResellerType)
        {
            this.stampsResellerType = stampsResellerType;
        }

        /// <summary>
        /// Saves the specified scan form.
        /// </summary>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>The ID value that is generated as a result saving the scan form.</returns>
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
                    UspsScanFormEntity uspsScanFormEntity = scanForm.ScanFormEntity as UspsScanFormEntity;
                    scanForm.ScanFormId = uspsScanFormEntity.UspsScanFormID;

                    // Should have the scan form ID populated now so save the image to the data source
                    DataResourceManager.CreateFromBytes(scanForm.Image, uspsScanFormEntity.UspsScanFormID, "SCAN Form");

                    // Now we have to update each shipment with the scan form record ID
                    foreach (ShipmentEntity shipment in scanForm.Shipments)
                    {
                        shipment.Postal.Usps.ScanFormBatchID = batchEntity.ScanFormBatchID;
                        adapter.SaveAndRefetch(shipment.Postal.Usps);
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
        /// <exception cref="StampsException">The SCAN forms in the batch are not Stamps.com SCAN forms.</exception>
        private ScanFormBatchEntity BuildBatchEntity(ScanFormBatch batch)
        {
            ScanFormBatchEntity batchEntity = new ScanFormBatchEntity();
            batchEntity.CreatedDate = batch.CreatedDate;
            batchEntity.ShipmentType = (int)batch.ShipmentType;
            batchEntity.ShipmentCount = batch.ShipmentCount;

            foreach (ScanForm scanForm in batch.ScanForms)
            {
                UspsScanFormEntity uspsScanFormEntity = scanForm.ScanFormEntity as UspsScanFormEntity;
                if (uspsScanFormEntity == null)
                {
                    string carrierName = StampsAccountManager.GetResellerName(stampsResellerType);
                    throw new StampsException(string.Format("The SCAN form provided was not a/an {0} SCAN form.", carrierName));
                }

                batchEntity.UspsScanForms.Add(uspsScanFormEntity);
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
            // We should have a stamps account entity
            UspsAccountEntity accountEntity = carrierAccount.GetAccountEntity() as UspsAccountEntity;

            if (accountEntity == null)
            {
                string carrierName = StampsAccountManager.GetResellerName(stampsResellerType);
                throw new StampsException(string.Format("ShipWorks was unable to retrieve existing SCAN forms: the {0} account could not be loaded.", carrierName));
            }
            
            List<ScanForm> scanForms = new List<ScanForm>();
            
            UspsScanFormCollection uspsForms = UspsScanFormCollection.Fetch(SqlAdapter.Default,
                UspsScanFormFields.UspsAccountID == accountEntity.UspsAccountID &
                UspsScanFormFields.CreatedDate > DateTime.UtcNow.Subtract(TimeSpan.FromDays(8)));

            foreach (UspsScanFormEntity stampsForm in uspsForms)
            {
                ScanForm scanForm = new ScanForm(carrierAccount, stampsForm.UspsScanFormID, stampsForm.ScanFormBatchID, stampsForm.Description, stampsForm.CreatedDate);
                scanForms.Add(scanForm);
            }

            return scanForms;
        }

        /// <summary>
        /// Gets the IDs of the shipments based on the given query predicate.
        /// </summary>
        /// <param name="bucket">The bucket containing the predicate for the query.</param>
        /// <returns>A collection of shipment ID values.</returns>
        public IEnumerable<long> GetShipmentIDs(RelationPredicateBucket bucket)
        {
            // We just need shipment IDs
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
        /// <exception cref="StampsException">ShipWorks was unable to retrieve existing SCAN forms: the Stamps.com account could not be loaded.</exception>
        public IEnumerable<ScanFormBatch> GetExistingScanFormBatches(IScanFormCarrierAccount carrierAccount)
        {
            UspsAccountEntity accountEntity = carrierAccount.GetAccountEntity() as UspsAccountEntity;
            if (accountEntity == null)
            {
                string carrierName = StampsAccountManager.GetResellerName(stampsResellerType);
                throw new StampsException(string.Format("ShipWorks was unable to retrieve existing SCAN forms: the {0} account could not be loaded.", carrierName));
            }

            List<ScanFormBatch> batches = new List<ScanFormBatch>();

            using (ScanFormBatchCollection batchEntities = new ScanFormBatchCollection())
            {
                // Get all the batches for this account from the last 8 days
                RelationPredicateBucket predicate = new RelationPredicateBucket
                    (
                    UspsScanFormFields.UspsAccountID == accountEntity.UspsAccountID &
                    ScanFormBatchFields.CreatedDate > DateTime.UtcNow.Subtract(TimeSpan.FromDays(8))
                    );

                predicate.Relations.Add(ScanFormBatchEntity.Relations.UspsScanFormEntityUsingScanFormBatchID);
                SqlAdapter.Default.FetchEntityCollection(batchEntities, predicate);

                foreach (ScanFormBatchEntity batchEntity in batchEntities)
                {
                    List<ScanForm> scanForms = new List<ScanForm>();

                    // Get all the forms for this account from the last 8 days
                    UspsScanFormCollection uspsForms = UspsScanFormCollection.Fetch(SqlAdapter.Default,
                                                                                          UspsScanFormFields.ScanFormBatchID == batchEntity.ScanFormBatchID);

                    foreach (UspsScanFormEntity scanFormEntity in uspsForms)
                    {
                        ScanForm scanForm = new ScanForm(carrierAccount, scanFormEntity.UspsScanFormID, scanFormEntity.ScanFormBatchID, scanFormEntity.Description, scanFormEntity.CreatedDate);
                        scanForms.Add(scanForm);
                    }

                    // Create a general scan form using the data from the Stamps-specific form
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

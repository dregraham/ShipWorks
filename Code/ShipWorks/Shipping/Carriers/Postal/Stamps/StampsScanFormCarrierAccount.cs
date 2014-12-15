using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// A Stamps.com implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class StampsScanFormCarrierAccount : IScanFormCarrierAccount
    {
        private readonly StampsAccountEntity accountEntity;
        private readonly IScanFormRepository repository;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsScanFormCarrierAccount" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public StampsScanFormCarrierAccount(IScanFormRepository repository, StampsAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(StampsScanFormCarrierAccount)))
        { }

        /// <summary>
        /// A constructor for testing purposes. Initializes a new instance of the <see cref="StampsScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public StampsScanFormCarrierAccount(IScanFormRepository repository, StampsAccountEntity accountEntity, ILog log)
        {
            this.repository = repository;
            this.accountEntity = accountEntity;
            this.log = log;
        }

        /// <summary>
        /// Gets the account entity.
        /// </summary>
        /// <returns>A carrier-specific account entity.</returns>
        public IEntity2 GetAccountEntity()
        {
            return accountEntity;
        }

        /// <summary>
        /// Gets the name of the shipping carrier.
        /// </summary>
        /// <value>The name of the shipping carrier.</value>
        public virtual string ShippingCarrierName
        {
            get { return EnumHelper.GetDescription(ShipmentTypeCode); }
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        /// <value>The shipment type code.</value>
        public virtual ShipmentTypeCode ShipmentTypeCode 
        { 
            get { return ShipmentTypeCode.Stamps; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>A string describing the account</returns>
        public string GetDescription()
        {
            // The stamps account doesn't have a description field, so we'll just build it here
            return string.Format("{0} - {1}", 
                EnumHelper.GetDescription(ShipmentTypeCode),
                accountEntity.Description);
        }
        
        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public virtual IScanFormGateway GetGateway()
        {
            return new StampsScanFormGateway(new StampsWebClient((StampsResellerType)accountEntity.StampsReseller));
        }
        
        /// <summary>
        /// Gets the printer to use for printing a carrier's SCAN form.
        /// </summary>
        /// <returns>An IScanFormPrinter object.</returns>
        public IScanFormPrinter GetPrinter()
        {
            return new DefaultScanFormPrinter();
        }


        /// <summary>
        /// Gets the existing scan form batches.
        /// </summary>
        /// <returns>A collection fo ScanFormBatch objects.</returns>
        public IEnumerable<ScanFormBatch> GetExistingScanFormBatches()
        {
            return repository.GetExistingScanFormBatches(this);
        }

        /// <summary>
        /// Saves the specified scan form batch.
        /// </summary>
        /// <param name="scanFormBatch">The scan form batch.</param>
        /// <returns>A long value representing the SCAN form batch ID.</returns>
        /// <exception cref="ShippingException">The ScanFormBatch is null.</exception>
        public long Save(ScanFormBatch scanFormBatch)
        {
            if (scanFormBatch != null)
            {
                // Delegate to  the  IScanFormRepository to carry out the saving and return the scan form batch ID value.
                return repository.Save(scanFormBatch);
            }
            else
            {
                string message = string.Format("ShipWorks was unable to create a SCAN form through {0} at this time. Please try again later.", ShippingCarrierName);

                log.Error(message + " (A null scan form batch tried to be saved.)");
                throw new ShippingException(message);
            }
        }

        /// <summary>
        /// Gets the IDs of the shipments eligible for a SCAN form.
        /// </summary>
        /// <returns>A collection of shipment ID values.</returns>
        public IEnumerable<long> GetEligibleShipmentIDs()
        {
            // Create the predicate for the query to determine which shipments are eligible
            RelationPredicateBucket bucket = new RelationPredicateBucket
            (
                // Shipment has to be processed and not yet voided
                ShipmentFields.Processed == true & ShipmentFields.Voided == false &

                // Shipment isn't a return
                ShipmentFields.ReturnShipment == false &

                // Has to not have been scanned yet and is for the selected account
                StampsShipmentFields.ScanFormBatchID == DBNull.Value &
                StampsShipmentFields.StampsAccountID == accountEntity.StampsAccountID &

                // And has to have been processed today.  This will get all shipments that were processed since midnight locally.
                ShipmentFields.ProcessedDate > DateTime.Now.Date.ToUniversalTime() & 

                // Exclude first class envelopes
                !(PostalShipmentFields.Service == (int) PostalServiceType.FirstClass & PostalShipmentFields.PackagingType == (int) PostalPackagingType.Envelope)
            );

            

            bucket.PredicateExpression.Add(ShipmentFields.ShipmentType == (int)ShipmentTypeCode);

            bucket.Relations.Add(ShipmentEntity.Relations.PostalShipmentEntityUsingShipmentID);
            bucket.Relations.Add(PostalShipmentEntity.Relations.StampsShipmentEntityUsingShipmentID);

            // Defer to the repository to perform the actual lookup
            return repository.GetShipmentIDs(bucket);
        }
    }
}

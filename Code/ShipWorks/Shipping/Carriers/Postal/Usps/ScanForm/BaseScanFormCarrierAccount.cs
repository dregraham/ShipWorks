using System.Collections.Generic;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm
{
    /// <summary>
    /// Base scan form carrier account
    /// </summary>
    public abstract class BaseScanFormCarrierAccount : IScanFormCarrierAccount
    {
        private readonly IScanFormRepository repository;
        private readonly ILog log;

        /// <summary>
        /// A constructor for testing purposes. Initializes a new instance of the <see cref="UspsScanFormCarrierAccount"/> class.
        /// </summary>
        protected BaseScanFormCarrierAccount(IScanFormRepository repository, ILog log, ShipmentTypeCode shipmentType)
        {
            this.repository = repository;
            this.log = log;
            ShipmentTypeCode = shipmentType;
        }

        /// <summary>
        /// Gets the account entity.
        /// </summary>
        /// <returns>A carrier-specific account entity.</returns>
        public abstract IEntity2 GetAccountEntity();

        /// <summary>
        /// Gets the name of the shipping carrier.
        /// </summary>
        /// <value>The name of the shipping carrier.</value>
        public abstract string ShippingCarrierName { get; }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        /// <value>The shipment type code.</value>
        public virtual ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>A string describing the account</returns>
        public string GetDescription()
        {
            // The USPS account doesn't have a description field, so we'll just build it here
            return string.Format("{0} - {1}",
                EnumHelper.GetDescription(ShipmentTypeCode),
                AccountDescription);
        }

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public abstract IScanFormGateway GetGateway();

        /// <summary>
        /// Gets the printer to use for printing a carrier's SCAN form.
        /// </summary>
        /// <returns>An IScanFormPrinter object.</returns>
        public virtual IScanFormPrinter GetPrinter()
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
            
            string message = string.Format("ShipWorks was unable to create a SCAN form through {0} at this time. Please try again later.", ShippingCarrierName);

            log.Error(message + " (A null scan form batch tried to be saved.)");
            throw new ShippingException(message);
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
                ShipmentFields.Processed == true &
                ShipmentFields.Voided == false &
                ShipmentFields.ReturnShipment == false &
                ShipmentFields.ShipmentType == (int)ShipmentTypeCode
                );

            bucket.Relations.Add(ShipmentEntity.Relations.PostalShipmentEntityUsingShipmentID);

            // Add carrier specific 
            AddPredicateFilters(bucket);

            // Defer to the repository to perform the actual lookup
            return repository.GetShipmentIDs(bucket);
        }

        /// <summary>
        /// Get the description of the account
        /// </summary>
        protected abstract string AccountDescription { get; }

        /// <summary>
        /// Add any carrier specific filters or relations to the bucket
        /// </summary>
        protected abstract void AddPredicateFilters(RelationPredicateBucket bucket);
    }
}
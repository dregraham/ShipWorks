﻿using System;
using System.Collections.Generic;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// An Endicia implementation of the ScanFormCarrierAccount interface.
    /// </summary>
    public class EndiciaScanFormCarrierAccount : IScanFormCarrierAccount
    {
        private readonly EndiciaAccountEntity accountEntity;
        private readonly IScanFormRepository repository;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(EndiciaScanFormCarrierAccount)))
        { }

        /// <summary>
        /// Constructor for testing purposes. Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity, ILog log)
        {
            this.repository = repository;
            this.accountEntity = accountEntity;
            this.log = log;

            ShipmentTypeCode = ShipmentTypeCode.Endicia;
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
            get { return "Endicia"; }
        }

        /// <summary>
        /// Gets or sets the shipment type code.
        /// </summary>
        /// <value>The shipment type code.</value>
        public ShipmentTypeCode ShipmentTypeCode { get; protected set; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>A string describing the account</returns>
        public string GetDescription()
        {
            return accountEntity.Description;
        }        
        
        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public virtual IScanFormGateway GetGateway()
        {
            return new EndiciaScanFormGateway();
        }
        
        /// <summary>
        /// Gets the printer to use for printing a carrier's SCAN form.
        /// </summary>
        /// <returns>An IScanFormPrinter object.</returns>
        public virtual IScanFormPrinter GetPrinter()
        {
            return new DefaultScanFormPrinter();
        }
        
        /// <summary>
        /// Gets the existing SCAN form batches.
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
        /// <exception cref="ShippingException"></exception>
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

                // Has to not have been scanned yet and is for the selected account
                EndiciaShipmentFields.ScanFormID == DBNull.Value &
                EndiciaShipmentFields.EndiciaAccountID == accountEntity.EndiciaAccountID &

                // And has to have been processed today.  This will get all shipments that were processed since midnight locally.
                ShipmentFields.ProcessedDate > DateTime.Now.Date.ToUniversalTime()
            );

            bucket.PredicateExpression.Add(ShipmentFields.ShipmentType == (int)ShipmentTypeCode);

            bucket.Relations.Add(ShipmentEntity.Relations.PostalShipmentEntityUsingShipmentID);
            bucket.Relations.Add(PostalShipmentEntity.Relations.EndiciaShipmentEntityUsingShipmentID);

            // Defer to the repository to perform the actual lookup
            return repository.GetShipmentIDs(bucket);
        }
    }
}

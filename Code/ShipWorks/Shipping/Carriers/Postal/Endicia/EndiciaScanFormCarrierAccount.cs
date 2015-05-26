using System;
using ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// An Endicia implementation of the ScanFormCarrierAccount interface.
    /// </summary>
    public class EndiciaScanFormCarrierAccount : BaseScanFormCarrierAccount
    {
        private readonly EndiciaAccountEntity accountEntity;
        //private readonly IScanFormRepository repository;
        //private readonly ILog log;
        private readonly IScanFormShipmentTypeName scanFormShipmentTypeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(EndiciaScanFormCarrierAccount)), new ScanFormShipmentTypeName())
        { }

        /// <summary>
        /// Constructor for testing purposes. Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        /// <param name="scanFormShipmentTypeName"></param>
        public EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity, ILog log, IScanFormShipmentTypeName scanFormShipmentTypeName) :
            this (repository, accountEntity, log, scanFormShipmentTypeName, ShipmentTypeCode.Endicia)
        {

        }

        /// <summary>
        /// Constructor for testing purposes. Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        /// <param name="scanFormShipmentTypeName"></param>
        public EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity, ILog log, IScanFormShipmentTypeName scanFormShipmentTypeName, ShipmentTypeCode shipmentTypeCode) :
            base(repository, log, shipmentTypeCode)
        {
            this.accountEntity = accountEntity;
            this.scanFormShipmentTypeName = scanFormShipmentTypeName;
        }

        /// <summary>
        /// Gets the account entity.
        /// </summary>
        /// <returns>A carrier-specific account entity.</returns>
        public override IEntity2 GetAccountEntity()
        {
            return accountEntity;
        }

        /// <summary>
        /// Gets the name of the shipping carrier.
        /// </summary>
        /// <value>The name of the shipping carrier.</value>
        public override string ShippingCarrierName
        {
            get { return scanFormShipmentTypeName.GetShipmentTypeName(ShipmentTypeCode); }
        }     
        
        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public override IScanFormGateway GetGateway()
        {
            return new EndiciaScanFormGateway();
        }

        /// <summary>
        /// Get the description of the account
        /// </summary>
        protected override string AccountDescription
        {
            get
            {
                return accountEntity.Description;
            }
        }

        /// <summary>
        /// Add any carrier specific filters or relations to the bucket
        /// </summary>
        protected override void AddPredicateFilters(RelationPredicateBucket bucket)
        {
            bucket.PredicateExpression.Add(
                // Has to not have been scanned yet and is for the selected account
                EndiciaShipmentFields.ScanFormBatchID == DBNull.Value &
                EndiciaShipmentFields.EndiciaAccountID == accountEntity.EndiciaAccountID &

                // Exclude first class envelopes
                !(PostalShipmentFields.Service == (int) PostalServiceType.FirstClass & PostalShipmentFields.PackagingType == (int) PostalPackagingType.Envelope));

            bucket.Relations.Add(PostalShipmentEntity.Relations.EndiciaShipmentEntityUsingShipmentID);
        }
    }
}

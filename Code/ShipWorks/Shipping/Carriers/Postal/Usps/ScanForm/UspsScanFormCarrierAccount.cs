using System;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm
{
    /// <summary>
    /// A USPS implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class UspsScanFormCarrierAccount : BaseScanFormCarrierAccount
    {
        private readonly UspsAccountEntity accountEntity;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsScanFormCarrierAccount" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public UspsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(UspsScanFormCarrierAccount)))
        { }

        /// <summary>
        /// A constructor for testing purposes. Initializes a new instance of the <see cref="UspsScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public UspsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity, ILog log) :
            base(repository, log, ShipmentTypeCode.Usps)
        {
            this.accountEntity = accountEntity;
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
            get { return EnumHelper.GetDescription(ShipmentTypeCode); }
        }

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public override IScanFormGateway GetGateway()
        {
            return new UspsScanFormGateway(new UspsWebClient((UspsResellerType)accountEntity.UspsReseller));
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
            int[] dhlServiceTypes = EnumHelper.GetEnumList<PostalServiceType>(ShipmentTypeManager.IsStampsDhl)
                .Select(entry => (int)entry.Value)
                .ToArray();

            bucket.PredicateExpression.Add(
                // Only allow shipments processed today, unless they are DHL which can be on a SCAN form at any time
                (ShipmentFields.ProcessedDate > DateTime.Now.Date.ToUniversalTime() |
                    PostalShipmentFields.Service == dhlServiceTypes) &

                // Has to not have been scanned yet and is for the selected account
                UspsShipmentFields.ScanFormBatchID == DBNull.Value &
                UspsShipmentFields.UspsAccountID == accountEntity.UspsAccountID &

                // Exclude first class envelopes
                !(PostalShipmentFields.Service == (int)PostalServiceType.FirstClass &
                    (PostalShipmentFields.PackagingType == (int)PostalPackagingType.Envelope | 
                        PostalShipmentFields.PackagingType == (int)PostalPackagingType.LargeEnvelope)));


            bucket.Relations.Add(PostalShipmentEntity.Relations.UspsShipmentEntityUsingShipmentID);
        }
    }
}

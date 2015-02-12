using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.ScanForms;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// An USPS (Stamps.com Expedited) implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class UspsScanFormCarrierAccount : StampsScanFormCarrierAccount
    {
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
        public UspsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity, ILog log)
            : base(repository, accountEntity, log)
        { }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Usps; }
        }
    }
}

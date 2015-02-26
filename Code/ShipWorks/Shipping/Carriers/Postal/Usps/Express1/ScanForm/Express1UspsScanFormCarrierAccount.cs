using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.ScanForm
{
    /// <summary>
    /// An Express1 implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class Express1UspsScanFormCarrierAccount : UspsScanFormCarrierAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1UspsScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public Express1UspsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(Express1UspsScanFormCarrierAccount)))
        { }

        /// <summary>
        /// Constructor for testing. Initializes a new instance of the <see cref="Express1UspsScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public Express1UspsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity, ILog log)
            : base(repository, accountEntity, log)
        {
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        /// <value>The shipment type code.</value>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Express1Usps; }
        }

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public override IScanFormGateway GetGateway()
        {
            // The Express1 gateway is very similar to that of USPS, but need to override this method
            // since we need to call into the Express1 API rather than USPS
            return new Express1UspsScanFormGateway(new Express1UspsWebClient());
        }
    }
}

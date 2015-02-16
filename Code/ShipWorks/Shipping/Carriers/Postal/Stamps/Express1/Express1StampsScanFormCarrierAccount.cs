using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;
using log4net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// An Express1 implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class Express1StampsScanFormCarrierAccount : StampsScanFormCarrierAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1StampsScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public Express1StampsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(Express1StampsScanFormCarrierAccount)))
        { }

        /// <summary>
        /// Constructor for testing. Initializes a new instance of the <see cref="Express1StampsScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public Express1StampsScanFormCarrierAccount(IScanFormRepository repository, UspsAccountEntity accountEntity, ILog log)
            : base(repository, accountEntity, log)
        {
        }

        /// <summary>
        /// Gets the shipment type code.
        /// </summary>
        /// <value>The shipment type code.</value>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Express1Stamps; }
        }

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public override IScanFormGateway GetGateway()
        {
            // The Express1 gateway is very similar to that of Stamps, but need to override this method
            // since we need to call into the Express1 API rather than Stamps
            return new Express1StampsScanFormGateway(new Express1UspsWebClient());
        }
    }
}

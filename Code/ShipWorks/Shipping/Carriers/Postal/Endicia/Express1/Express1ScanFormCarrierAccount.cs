using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An Express1 implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class Express1ScanFormCarrierAccount : EndiciaScanFormCarrierAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public Express1ScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(Express1ScanFormCarrierAccount)))
        { }

        /// <summary>
        /// Constructor for testing. Initializes a new instance of the <see cref="Express1ScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public Express1ScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity, ILog log)
            : base(repository, accountEntity, log)
        {
            // Just need to set the shipment type code for express 1 that 
            // gets used in the GetEligibleShipmentIDs method
            ShipmentTypeCode = ShipmentTypeCode.PostalExpress1;
        }

        /// <summary>
        /// Gets the name of the shipping carrier.
        /// </summary>
        /// <value>The name of the shipping carrier.</value>
        public override string ShippingCarrierName
        {
            get { return "Express 1"; }
        }

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public override IScanFormGateway GetGateway()
        {
            // The Express1 gateway is very similar to that of Endicia, but need to override this method
            // since we need to call into the Express1 API rather than Endicia
            return new Express1ScanFormGateway();
        }
    }
}

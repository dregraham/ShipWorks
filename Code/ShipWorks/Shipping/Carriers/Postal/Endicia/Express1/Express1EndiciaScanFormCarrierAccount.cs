using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An Express1 implementation of the IScanFormCarrierAccount interface.
    /// </summary>
    public class Express1EndiciaScanFormCarrierAccount : EndiciaScanFormCarrierAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        public Express1EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity)
            : this(repository, accountEntity, LogManager.GetLogger(typeof(Express1EndiciaScanFormCarrierAccount)), new ScanFormShipmentTypeName())
        { }

        /// <summary>
        /// Constructor for testing. Initializes a new instance of the <see cref="Express1EndiciaScanFormCarrierAccount"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="accountEntity">The account entity.</param>
        /// <param name="log">The log.</param>
        public Express1EndiciaScanFormCarrierAccount(IScanFormRepository repository, EndiciaAccountEntity accountEntity, ILog log, IScanFormShipmentTypeName scanFormShipmentTypeName)
            : base(repository, accountEntity, log, scanFormShipmentTypeName, ShipmentTypeCode.Express1Endicia)
        {
            // Just need to set the shipment type code for express 1 that 
            // gets used in the GetEligibleShipmentIDs method
           // ShipmentTypeCode = ShipmentTypeCode.Express1Endicia;
        }

        /// <summary>
        /// Gets the gateway object to use for communicating with the shipping carrier API for generating SCAN forms.
        /// </summary>
        /// <returns>An IScanFormGateway object.</returns>
        public override IScanFormGateway GetGateway()
        {
            // The Express1 gateway is very similar to that of Endicia, but need to override this method
            // since we need to call into the Express1 API rather than Endicia
            return new Express1EndiciaScanFormGateway();
        }
    }
}

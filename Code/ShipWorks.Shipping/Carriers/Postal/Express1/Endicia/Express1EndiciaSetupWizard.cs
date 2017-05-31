using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Endicia
{
    /// <summary>
    /// Setup wizard for Express1 Endicia accounts
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.Express1Endicia)]
    public class Express1EndiciaSetupWizard : Express1SetupWizard
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaSetupWizard(EndiciaBuyPostageDlg buyPostageDialog,
            EndiciaAccountManagerControl accountManagerControl,
            Express1EndiciaOptionsControl optionsControl,
            Express1EndiciaRegistration registration,
            Express1EndiciaAccountRepository accountRepository) :
            base(buyPostageDialog, accountManagerControl, optionsControl, registration, accountRepository)
        {
        }
    }
}

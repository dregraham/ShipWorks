using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Usps
{
    /// <summary>
    /// Setup wizard for Express1 Usps accounts
    /// </summary>
    [KeyedComponent(typeof(ShipmentTypeSetupWizardForm), ShipmentTypeCode.Express1Usps)]
    public class Express1UspsSetupWizard : Express1SetupWizard
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsSetupWizard(UspsPurchasePostageDlg buyPostageDialog,
            UspsAccountManagerControl accountManagerControl,
            Express1UspsOptionsControl optionsControl,
            Express1UspsRegistration registration,
            Express1UspsAccountRepository accountRepository) :
            base(buyPostageDialog, accountManagerControl, optionsControl, registration, accountRepository)
        {
        }
    }
}

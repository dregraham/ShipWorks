using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// The view model for the OneBalanceCarrierAccountsControl
    /// </summary>
    [KeyedComponent(typeof(IOneBalanceShowSetupDialogViewModel), ShipmentTypeCode.DhlExpress)]

    public class OneBalanceCarrierAccountsControlViewModel : OneBalanceShowSetupDialogViewModel
    {
        private SolidColorBrush dhlTextColor;
        private bool dhlAccountEnabled;
        private readonly IUspsAccountManager accountManager;
        private readonly IMessageHelper messageHelper;
        private IUspsWebClient webClient;
        private bool remoteDhlEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceCarrierAccountsControlViewModel(IIndex<ShipmentTypeCode, IOneBalanceSetupWizard> setupWizardFactory,
            IWin32Window window,
            IUspsAccountManager accountManager,
            IMessageHelper messageHelper) 
            : base(setupWizardFactory, window)
        {
            this.accountManager = accountManager;
            this.messageHelper = messageHelper;
            SetupWebClient();
            Refresh();
        }

        /// <summary>
        /// The color of the Dhl Express Text
        /// </summary>
        public SolidColorBrush DhlTextColor
        {
            get => dhlTextColor;
            set => Set(ref dhlTextColor, value);
        }

        /// <summary>
        /// A flag to indicate if the DhlAccount is still enabled
        /// </summary>
        public bool LocalDhlAccountEnabled
        {
            get => dhlAccountEnabled;
            set => Set(ref dhlAccountEnabled, value);
        }

        /// <summary>
        /// Refreshes the account dependent attributes of the controls 
        /// </summary>
        public void Refresh()
        {
            remoteDhlEnabled = RemoteDhlAccountEnabled();
            DhlTextColor =  remoteDhlEnabled ? Brushes.Black : Brushes.DarkGray;
            LocalDhlAccountEnabled = LocalDhlAccountExists();
        }

        /// <summary>
        /// Sets up the web client
        /// </summary>
        private void SetupWebClient()
        {
            UspsShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Usps) as UspsShipmentType;
            webClient = shipmentType.CreateWebClient();
        }

        /// <summary>
        /// Get the account from stamps
        /// </summary>
        private bool RemoteDhlAccountEnabled()
        {
            var uspsAccount = GetUspsAccount();
            if(uspsAccount != null)
            {
                var response = (AccountInfoV41) webClient.GetAccountInfo(uspsAccount);
                return response.Capabilities.CanPrintDX;
            }

            return false;
        }

        /// <summary>
        /// Get the account from stamps
        /// </summary>
        private UspsAccountEntity GetUspsAccount() => accountManager.GetAccounts(UspsResellerType.None).FirstOrDefault(e => e.ShipEngineCarrierId != null);

        protected override void ShowSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            try
            {
                if (!remoteDhlEnabled)
                {
                    AddCarrierAccount();
                }
                base.ShowSetupWizard(shipmentTypeCode);
                Refresh();
            }
            catch (Exception ex)
            {
                if (ex is UspsException)
                {
                    messageHelper.ShowError(ex.Message);
                }
                else
                {
                    messageHelper.ShowError("There was an error adding DHL Express to your One Balance account.");
                }
            }
        }

        /// <summary>
        /// Make an Api call to stamps to add the account
        /// </summary>
        private void AddCarrierAccount()
        {
            var account = GetUspsAccount();
            webClient.AddDhlExpress(account);   
        }

        /// <summary>
        /// Checks to see if the Dhl account exists locally
        /// </summary>
        private bool LocalDhlAccountExists() => DhlExpressAccountManager.Accounts.FirstOrDefault(e => e.UspsAccountId != null) != null;
    }
}

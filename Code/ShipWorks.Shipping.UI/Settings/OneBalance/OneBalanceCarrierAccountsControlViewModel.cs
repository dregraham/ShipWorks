using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.UPS;
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
        private SolidColorBrush upsTextColor;
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
        /// The color of the UPS Text
        /// </summary>
        public SolidColorBrush UpsTextColor
        {
            get => upsTextColor;
            set => Set(ref upsTextColor, value);
        }

        /// <summary>
        /// A flag to indicate if the Dhl Account is still enabled
        /// </summary>
        public bool LocalDhlAccountEnabled
        {
            get => dhlAccountEnabled;
            set => Set(ref dhlAccountEnabled, value);
        }

        /// <summary>
        /// Refreshes the account dependent attributes of the controls 
        /// </summary>
        public override void Refresh()
        {
            remoteDhlEnabled = RemoteDhlAccountEnabled();
            DhlTextColor = remoteDhlEnabled ? Brushes.Black : Brushes.DarkGray;
            LocalDhlAccountEnabled = LocalDhlAccountExists();
            UpsTextColor = LocalUpsAccountExists() ? Brushes.Black : Brushes.DarkGray;
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
            var result = GetUspsAccount();

            if (result.Success)
            {
                try
                {
                    var response = (AccountInfoV41) webClient.GetAccountInfo(result.Value);
                    return response.Capabilities.CanPrintDX;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the account from stamps
        /// </summary>
        private GenericResult<UspsAccountEntity> GetUspsAccount()
        {
            var accounts = accountManager.GetAccounts(UspsResellerType.None);

            if (!accounts.Any())
            {
                return new UspsException("You must have a USPS account to enable DHL Express from ShipWorks.");
            }

            // If there's only one account it's a One Balance account
            if (accounts.Count == 1)
            {
                return accounts.First();
            }

            // If there are multiple accounts the one with a ShipEngineCarrierId is the One Balance account
            var account = accounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);

            if (account == null)
            {
                return new UspsException("Unable to determine which USPS account to use. Please call ShipWorks support at 1-800-952-7784");
            }

            return account;
        }

        /// <summary>
        /// Show the DHL Express setup wizard
        /// </summary>
        protected override void ShowSetupWizard(ShipmentTypeCode shipmentTypeCode)
        {
            if ((!remoteDhlEnabled && AddDhlExpress()) || remoteDhlEnabled)
            {
                base.ShowSetupWizard(shipmentTypeCode);
                Refresh();
            }
        }

        /// <summary>
        /// Make an Api call to stamps to add the account
        /// </summary>
        private bool AddDhlExpress()
        {
            var result = GetUspsAccount();

            if (result.Failure)
            {
                messageHelper.ShowError(result.Exception.Message);
                return false;
            }

            try
            {
                webClient.AddDhlExpress(result.Value);
                return true;
            }
            catch (UspsException ex)
            {
                // We don't need to log the exception because the webclient already does it
                messageHelper.ShowError($"There was an error adding DHL Express to your One Balance account:\n{ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Get the dhl account from the local database
        /// </summary>
        private bool LocalDhlAccountExists() => DhlExpressAccountManager.Accounts.FirstOrDefault(e => e.UspsAccountId != null) != null;

        /// <summary>
        /// Get the ups account from the local database
        /// </summary>
        private bool LocalUpsAccountExists() => UpsAccountManager.Accounts.FirstOrDefault(e => e.ShipEngineCarrierId != null) != null;
    }
}

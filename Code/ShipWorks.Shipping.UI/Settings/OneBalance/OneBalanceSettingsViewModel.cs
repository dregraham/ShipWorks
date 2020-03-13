using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceSettingsControl
    /// </summary>
    [Component]
    public class OneBalanceSettingsControlViewModel : ViewModelBase, IOneBalanceSettingsControlViewModel
    {
        private IPostageWebClient postageWebClient;
        private readonly IUspsAccountManager uspsAccountManager;
        private UpsAccountEntity upsAccount;
        private readonly Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory;

        private decimal balance;
        private string getBalanceError;
        private bool showGetBalanceError = false;
        private bool showBanner;
        private bool loading = false;
        private bool addMoneyEnabled = true;

        public OneBalanceSettingsControlViewModel(IUspsAccountManager uspsAccountManager,
            Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory,
            IIndex<ShipmentTypeCode, IOneBalanceShowSetupDialogViewModel> setupDialogLookup,
            IOneBalanceAutoFundControlViewModel autoFundViewModel)
        {
            this.uspsAccountManager = uspsAccountManager;
            SetupWebClients();
            upsAccount = GetUpsAccount();

            this.addMoneyDialogFactory = addMoneyDialogFactory;
            ShowBanner = upsAccount == null;

            BannerContext = setupDialogLookup[ShipmentTypeCode.UpsOnLineTools];
            CarrierAccountsContext = setupDialogLookup[ShipmentTypeCode.DhlExpress];
            AutoFundContext = autoFundViewModel;

            BannerContext.SetupComplete += OnOneBalanceSetupComplete;
            CarrierAccountsContext.SetupComplete += OnOneBalanceSetupComplete;

            GetInitialValuesCommand = new RelayCommand(GetInitialValues);
            ShowAddMoneyDialogCommand = new RelayCommand(ShowAddMoneyDialog);
        }

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal Balance
        {
            get => balance;
            set => Set(ref balance, value);
        }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string GetBalanceError
        {
            get => getBalanceError;
            set => Set(ref getBalanceError, value);
        }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowGetBalanceError
        {
            get => showGetBalanceError;
            set => Set(ref showGetBalanceError, value);
        }

        /// <summary>
        /// A flag to indicate if we are still trying to load the balance
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Loading
        {
            get => loading;
            set => Set(ref loading, value);
        }

        /// <summary>
        /// A flag to indicate if we are in a state that the user should be allowed to add money
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool AddMoneyEnabled
        {
            get => addMoneyEnabled;
            set => Set(ref addMoneyEnabled, value);
        }

        /// <summary>
        /// A flag to indicate if we are in a state where we should show the banner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowBanner
        {
            get => showBanner;
            set => Set(ref showBanner, value);
        }

        /// <summary>
        /// The data context for the enable ups banner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOneBalanceShowSetupDialogViewModel BannerContext { get; }

        /// <summary>
        /// The data context for the carrier accounts control
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOneBalanceShowSetupDialogViewModel CarrierAccountsContext { get; }

        /// <summary>
        /// The data context for the enable ups banner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IOneBalanceAutoFundControlViewModel AutoFundContext { get; }

        /// <summary>
        /// RelayCommand for getting initial values to populate fields with
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetInitialValuesCommand { get; }

        /// <summary>
        /// Relay command for showing the add money dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowAddMoneyDialogCommand { get; }

        /// <summary>
        /// Retrieve the accounts balance if we have a One Balance account
        /// </summary>
        private void GetAccountBalance()
        {
            ShowBanner = upsAccount == null;

            if (postageWebClient != null)
            {
                Task.Run(() =>
                {
                    GetBalance();
                    AddMoneyEnabled = postageWebClient != null && !showGetBalanceError;
                });
            }
        }

        /// <summary>
        /// Retrieve the accounts balance
        /// </summary>
        private void GetBalance()
        {
            int tries = 5;
            Loading = true;
            while (tries-- > 0)
            {
                try
                {
                    Balance = new ShipWorks.Shipping.Carriers.Postal.PostageBalance(postageWebClient).Value;
                    Loading = false;
                    break;
                }
                catch (Exception ex)
                {
                    bool keepTrying = false;
                    
                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        GetBalanceError = $"Your One Balance account is not ready yet.";

                        keepTrying = true;
                    }
                    else
                    {
                        GetBalanceError = "There was an error retrieving your account balance";
                    }

                    Loading = false;
                    ShowGetBalanceError = true;

                    if (keepTrying)
                    {
                        // Sleep for a few seconds to allow the registration time to go through
                        Thread.Sleep(3000);
                    }
                }
            }
        }

        /// <summary>
        /// Show the add money dialog
        /// </summary>
        private void ShowAddMoneyDialog()
        {
            var addMoneyDialog = addMoneyDialogFactory(postageWebClient);

            var dlgResult = addMoneyDialog.ShowDialog();
            if (dlgResult == true)
            {
                GetAccountBalance();
            }
        }

        /// <summary>
        /// Sets up the web client
        /// </summary>
        private void SetupWebClients()
        {
            var account = GetOneBalanceAccount();
            postageWebClient = account == null ? null : new UspsPostageWebClient(account);
        }

        /// <summary>
        /// Get the Ups account
        /// </summary>
        private UpsAccountEntity GetUpsAccount() => UpsAccountManager.Accounts.FirstOrDefault(e => e.ShipEngineCarrierId != null);

        /// <summary>
        /// Event handler for the banners SetupComplete event
        /// </summary>
        private void OnOneBalanceSetupComplete(object sender, EventArgs e)
        {
            SetupWebClients();
            upsAccount = GetUpsAccount();
            GetAccountBalance();
            CarrierAccountsContext.Refresh();
        }

        /// <summary>
        /// Get initial values for fields that need to be populated
        /// </summary>
        private void GetInitialValues()
        {
            GetAccountBalance();
        }

        /// <summary>
        /// Send the auto fund settings to Stamps
        /// </summary>
        public void SaveAutoFundSettings() => AutoFundContext.SaveSettings();

        /// <summary>
        /// Determines which USPS account is One Balance, and if one exists returns it
        /// </summary>
        private UspsAccountEntity GetOneBalanceAccount()
        {
            /// If there's only one account it's a One Balance account
            if (uspsAccountManager.UspsAccounts.Count == 1)
            {
                return uspsAccountManager.UspsAccounts.First();
            }

            // If there are multiple accounts the one with a ShipEngineCarrierId is the One Balance account
            return uspsAccountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);
        }
    }
}

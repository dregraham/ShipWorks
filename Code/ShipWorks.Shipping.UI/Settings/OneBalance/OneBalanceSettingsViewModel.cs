using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class OneBalanceSettingsControlViewModel : ViewModelBase
    {
        private IPostageWebClient webClient;
        private readonly IUspsAccountManager accountManager;
        private UpsAccountEntity upsAccount;
        private readonly Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory;

        private decimal balance;
        private string message;
        private bool showMessage = false;
        private bool showBanner;
        private bool loading = true;
        private bool addMoneyEnabled = true;
        private readonly IOneBalanceEnableUpsBannerWpfViewModel bannerContext;

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public OneBalanceSettingsControlViewModel(IUspsAccountManager accountManager, Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory, IOneBalanceEnableUpsBannerWpfViewModel bannerViewModel)
        {
            this.accountManager = accountManager;
            SetupWebClient();
            upsAccount = GetUpsAccount();

            this.webClient = webClient;
            this.addMoneyDialogFactory = addMoneyDialogFactory;
            ShowBanner = upsAccount == null;

            bannerContext = bannerViewModel;

            bannerContext.SetupComplete += OnOneBalanceSetupComplete;

            GetBalanceCommand = new RelayCommand(GetAccountBalance);
            ShowAddMoneyDialogCommand = new RelayCommand(ShowAddMoneyDialog);
        }

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        /// 
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
        public string Message
        {
            get => message;
            set => Set(ref message, value);
        }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowMessage
        {
            get => showMessage;
            set => Set(ref showMessage, value);
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
        public IOneBalanceEnableUpsBannerWpfViewModel BannerContext
        {
            get => bannerContext;
        }

        /// <summary>
        /// RelayCommand for getting the account balance
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand GetBalanceCommand { get; }

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
            Loading = true;
            ShowBanner = upsAccount == null;

            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle,
                new Action(() =>
                {
                    if (webClient != null)
                    {
                        GetBalance();
                    }

                    AddMoneyEnabled = webClient != null && !showMessage;
                    Loading = false;
                }));
        }

        /// <summary>
        /// Retrieve the accounts balance
        /// </summary>
        private void GetBalance()
        {
            int tries = 5;

            while (tries-- > 0)
            {
                try
                {
                    Balance = new PostageBalance(webClient).Value;

                    break;
                }
                catch (UspsException ex)
                {
                    bool keepTrying = false;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        Message = $"Your One Balance account is not ready yet.";

                        keepTrying = true;
                    }
                    else
                    {
                        Message = "There was an error retrieving your account balance";
                    }
                   
                    ShowMessage = true;

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
            var addMoneyDialog = addMoneyDialogFactory(webClient);
           
            var dlgResult = addMoneyDialog.ShowDialog();
            if(dlgResult == true)
            {
                GetAccountBalance();
            }
        }

        /// <summary>
        /// Sets up the web client
        /// </summary>
        private void SetupWebClient()
        {
            var account = accountManager.UspsAccounts.FirstOrDefault(a => a.ShipEngineCarrierId != null);
            webClient = account == null ? null : new UspsPostageWebClient(account);
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
            SetupWebClient();
            upsAccount = GetUpsAccount();
            GetAccountBalance();
        }
    }
}

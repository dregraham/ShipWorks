using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Threading;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceSettingsControl
    /// </summary>
    public class OneBalanceSettingsControlViewModel : ViewModelBase
    {
        private readonly IPostageWebClient webClient;
        private readonly Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        private decimal balance;
        private string message;
        private bool showMessage = false;
        private bool loading = true;

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        /// 
        [Obfuscation(Exclude = true)]
        public decimal Balance 
        { 
            get { return balance; }
            set 
            {
                Set(ref balance, value);
            }
        }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message
        {
            get { return message; }
            set
            {
                Set(ref message, value);
            }
        }

        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowMessage
        {
            get { return showMessage; }
            set
            {
                Set(ref showMessage, value);
            }
        }

        /// <summary>
        /// A flag to indicate if we are still trying to load the balance
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Loading
        {
            get { return loading; }
            set
            {
                Set(ref loading, value);
            }
        }

        /// <summary>
        /// RelayCommand for getting the account balance
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand GetBalanceCommand => new RelayCommand(GetAccountBalance);

        /// <summary>
        /// Relay command for showing the add money dialog
        /// </summary>
        public RelayCommand ShowAddMoneyDialogCommand => new RelayCommand(ShowAddMoneyDialog);

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public OneBalanceSettingsControlViewModel(IPostageWebClient webClient, Func<IPostageWebClient, IOneBalanceAddMoneyDialog> addMoneyDialogFactory)
        {
            this.webClient = webClient;
            this.addMoneyDialogFactory = addMoneyDialogFactory;
        }

        /// <summary>
        /// Retrieve the accounts balance if we have a One Balance account
        /// </summary>
        private void GetAccountBalance()
        {
			this.Loading = true;
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.ApplicationIdle,
                new Action(() =>
                {
                    if (webClient != null)
                    {
                        GetBalance();
                        Loading = false;
                    }
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
                    string message = ex.Message;

                    // This message means we created a new account, but it wasn't ready to go yet
                    if (ex.Message.Contains("Registration timed out while authenticating."))
                    {
                        message = $"Your One Balance account is not ready yet.";

                        keepTrying = true;
                    }

                    Message = message;

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
        /// Raise the INotifyPropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

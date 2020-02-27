using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceSettingsControl
    /// </summary>
    public class OneBalanceSettingsControlViewModel : INotifyPropertyChanged
    {
        private readonly IPostageWebClient webClient;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The current balance of the one balance account
        /// </summary>
        /// 
        private decimal balance;
        public decimal Balance 
        { 
            get { return balance; }
            set 
            {
                balance = value;
                RaisePropertyChanged(nameof(Balance));
            }
        }

        /// <summary>
        /// The message to be displayed in place of the account balance if needed
        /// </summary>
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }
        /// <summary>
        /// A flag to indicate if we should show the message
        /// </summary>
        private bool showMessage = false;
        public bool ShowMessage
        {
            get { return showMessage; }
            set
            {
                showMessage = value;
                RaisePropertyChanged(nameof(ShowMessage));
            }
        }

        /// <summary>
        /// A flag to indicate if we are still trying to load the balance
        /// </summary>
        private bool loading = true;
        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = false;
                RaisePropertyChanged(nameof(Loading));
            }
        }

        /// <summary>
        /// RelayCommand for getting the account balance
        /// </summary>
        public RelayCommand GetBalanceCommand => new RelayCommand(GetAccountBalance);

        /// <summary>
        /// Initialize the control to display information for the given account
        /// </summary>
        public OneBalanceSettingsControlViewModel(IPostageWebClient webClient)
        {
            this.webClient = webClient;
        }

        /// <summary>
        /// Retrieve the accounts balance if we have a One Balance account
        /// </summary>
        private void GetAccountBalance()
        {
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
        /// Raise the INotifyPropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

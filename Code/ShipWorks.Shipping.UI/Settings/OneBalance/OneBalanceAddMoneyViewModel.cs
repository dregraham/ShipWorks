using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceAddMoneyDialog
    /// </summary>
    public class OneBalanceAddMoneyViewModel : INotifyPropertyChanged
    {
        private readonly IPostageWebClient webClient;
        private readonly Window window;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The amount of money to add to the stamps account
        /// </summary>
        private decimal amount;
        public decimal Amount { 
            get 
            {
                return amount;
            }
            set
            {
                amount = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }

        /// <summary>
        /// Relay command to for buying postage
        /// </summary>
        public RelayCommand BuyPostageCommand => new RelayCommand(BuyPostage);

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAddMoneyViewModel(IPostageWebClient webClient, Window window)
        {
            this.webClient = webClient;
            this.window = window;
        }

        /// <summary>
        /// Purchases postage from Stamps
        /// </summary>
        private void BuyPostage()
        {
            try
            {
                webClient.Purchase(Amount);
                window.DialogResult = true;
            }
            catch(UspsException ex)
            {
                var message = $"There was an error purchasing postage:\n{ex.Message}";
                var errorDlg = new OneBalanceErrorDialog(message);
                errorDlg.ShowDialog();
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

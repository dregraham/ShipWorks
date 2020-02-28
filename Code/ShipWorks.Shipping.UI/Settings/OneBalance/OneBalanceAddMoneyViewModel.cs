using System.ComponentModel;
using System.Reflection;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// View model for the OneBalanceAddMoneyDialog
    /// </summary>
    public class OneBalanceAddMoneyViewModel : ViewModelBase
    {
        private readonly IPostageWebClient webClient;
        private readonly Window window;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMessageHelper messageHelper;

        private decimal amount;

        /// <summary>
        /// The amount of money to add to the stamps account
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal Amount { 
            get 
            {
                return amount;
            }
            set
            {
                Set(ref amount,value);
            }
        }

        /// <summary>
        /// Relay command to for buying postage
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RelayCommand BuyPostageCommand => new RelayCommand(BuyPostage);

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAddMoneyViewModel(IPostageWebClient webClient, Window window, IMessageHelper messageHelper)
        {
            this.webClient = webClient;
            this.window = window;
            this.messageHelper = messageHelper;
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
                messageHelper.ShowError(message);
            }
        }
    }
}

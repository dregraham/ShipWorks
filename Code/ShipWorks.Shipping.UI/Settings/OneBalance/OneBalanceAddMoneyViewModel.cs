using System.Reflection;
using System.Windows;
using System.Windows.Input;
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
        private readonly IMessageHelper messageHelper;

        private decimal amount;
        private Cursor cursor;

        /// <summary>
        /// Constructor
        /// </summary>
        public OneBalanceAddMoneyViewModel(IPostageWebClient webClient, Window window, IMessageHelper messageHelper)
        {
            this.webClient = webClient;
            this.window = window;
            this.messageHelper = messageHelper;
            BuyPostageCommand = new RelayCommand(BuyPostage);
        }

        /// <summary>
        /// The amount of money to add to the stamps account
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal Amount
        {
            get => amount;
            set => Set(ref amount, value);
        }

        /// <summary>
        /// The cursor to show the user
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Cursor Cursor
        {
            get => cursor;
            set => Set(ref cursor, value);
        }

        /// <summary>
        /// Relay command to for buying postage
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand BuyPostageCommand { get; }

        /// <summary>
        /// Purchases postage from Stamps
        /// </summary>
        private void BuyPostage()
        {
            try
            {
                Cursor = Cursors.Wait;
                webClient.Purchase(Amount);
                window.DialogResult = true;
            }
            catch(UspsException ex)
            {
                var message = $"There was an error purchasing postage:\n{ex.Message}";
                messageHelper.ShowError(message);
            }
            Cursor = Cursors.Arrow;
        }
    }
}

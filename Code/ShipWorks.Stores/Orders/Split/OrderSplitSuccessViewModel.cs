using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Users;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// View model for the OrderSplitSuccessDialog
    /// </summary>
    [Component]
    public class OrderSplitSuccessViewModel : IOrderSplitSuccessViewModel, INotifyPropertyChanged
    {
        private const UserConditionalNotificationType notificationType = UserConditionalNotificationType.SplitOrders;
        private readonly IOrderSplitSuccessDialog dialog;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IAsyncMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;

        private IEnumerable<string> combinedOrders;
        private bool doNotShowAgain;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderSplitSuccessViewModel(IOrderSplitSuccessDialog dialog, ICurrentUserSettings currentUserSettings, IAsyncMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.dialog = dialog;
            this.currentUserSettings = currentUserSettings;

            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            Dismiss = new RelayCommand(() => DismissAction());
        }

        /// <summary>
        /// Notify of changing properties
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Dismiss the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand Dismiss { get; }

        /// <summary>
        /// SplitOrders to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<string> SplitOrders
        {
            get { return combinedOrders; }
            set { handler.Set(nameof(SplitOrders), ref combinedOrders, value); }
        }

        /// <summary>
        /// Should the dialog be shown again
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DoNotShowAgain
        {
            get { return doNotShowAgain; }
            set { handler.Set(nameof(DoNotShowAgain), ref doNotShowAgain, value); }
        }

        /// <summary>
        /// Show a success dialog after an order has been split
        /// </summary>
        public async Task ShowSuccessConfirmation(IEnumerable<string> orderNumbers)
        {
            if (!currentUserSettings.ShouldShowNotification(notificationType))
            {
                return;
            }

            SplitOrders = orderNumbers.ToImmutableList();

            await messageHelper.ShowDialog(SetupDialog);
        }

        /// <summary>
        /// Setup the success dialog
        /// </summary>
        private IDialog SetupDialog()
        {
            dialog.DataContext = this;
            return dialog;
        }

        /// <summary>
        /// Dismiss the dialog
        /// </summary>
        private void DismissAction()
        {
            if (DoNotShowAgain)
            {
                currentUserSettings.StopShowingNotification(notificationType);
            }

            dialog.Close();
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// View model for the combination success dialog
    /// </summary>
    [Component]
    public class OrderCombinationSuccessViewModel : IOrderCombinationSuccessViewModel
    {
        private readonly IOrderCombinationSuccessDialog dialog;
        readonly ICurrentUserSettings currentUserSettings;
        private readonly PropertyChangedHandler handler;

        private string orderNumber;
        private IEnumerable<string> combinedOrders;
        private bool doNotShowAgain;
        private UserConditionalNotificationType notificationType;
        readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombinationSuccessViewModel(IOrderCombinationSuccessDialog dialog, ICurrentUserSettings currentUserSettings, IMessageHelper messageHelper)
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
        /// OrderNumber of the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string OrderNumber
        {
            get { return orderNumber; }
            set { handler.Set(nameof(OrderNumber), ref orderNumber, value); }
        }

        /// <summary>
        /// CombinedOrders to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IEnumerable<string> CombinedOrders
        {
            get { return combinedOrders; }
            set { handler.Set(nameof(CombinedOrders), ref combinedOrders, value); }
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
        /// Show the combination successful dialog
        /// </summary>
        public void ShowSuccessConfirmation(string orderNumber, IEnumerable<IOrderEntity> orders)
        {
            this.notificationType = notificationType;

            if (!currentUserSettings.ShouldShowNotification(notificationType))
            {
                return;
            }

            OrderNumber = orderNumber;
            CombinedOrders = orders.Select(x => "#" + x.OrderNumberComplete).ToReadOnly();

            dialog.DataContext = this;
            messageHelper.ShowDialog(dialog);
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

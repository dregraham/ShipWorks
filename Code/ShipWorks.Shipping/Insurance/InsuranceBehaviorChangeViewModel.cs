using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Users;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// View model for notifying the user of the change in insurance behavior
    /// </summary>
    [Component]
    public class InsuranceBehaviorChangeViewModel : IInsuranceBehaviorChangeViewModel
    {
        private const UserConditionalNotificationType notificationType = UserConditionalNotificationType.InsuranceBehaviorChange;
        private readonly IInsuranceBehaviorChangeDialog dialog;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IMessageHelper messageHelper;
        private readonly PropertyChangedHandler handler;

        private bool doNotShowAgain;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceBehaviorChangeViewModel(IInsuranceBehaviorChangeDialog dialog, ICurrentUserSettings currentUserSettings, IMessageHelper messageHelper)
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
        /// Should the dialog be shown again
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DoNotShowAgain
        {
            get { return doNotShowAgain; }
            set { handler.Set(nameof(DoNotShowAgain), ref doNotShowAgain, value); }
        }
        /// <summary>
        /// Notify the user of the change
        /// </summary>
        public void Notify(bool originalInsuranceSelection, bool newInsuranceSelection)
        {
            // Only show the dialog if the shipment went from using insurance to not using insurance.
            if (newInsuranceSelection || !originalInsuranceSelection)
            {
                return;
            }

            if (!currentUserSettings.ShouldShowNotification(notificationType))
            {
                return;
            }

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
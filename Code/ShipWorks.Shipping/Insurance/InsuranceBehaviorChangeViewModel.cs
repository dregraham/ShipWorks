using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Core.UI;
using ShipWorks.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private readonly Func<IInsuranceBehaviorChangeViewModel, IInsuranceBehaviorChangeDialog> createDialog;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IMessageHelper messageHelper;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly PropertyChangedHandler handler;

        private bool doNotShowAgain;
        private IInsuranceBehaviorChangeDialog dialog;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceBehaviorChangeViewModel(
            Func<IInsuranceBehaviorChangeViewModel, IInsuranceBehaviorChangeDialog> createDialog,
            ICurrentUserSettings currentUserSettings,
            IMessageHelper messageHelper,
            ISchedulerProvider schedulerProvider
            )
        {
            this.schedulerProvider = schedulerProvider;
            this.messageHelper = messageHelper;
            this.createDialog = createDialog;
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
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
        public ICommand Dismiss { get; }

        /// <summary>
        /// Should the dialog be shown again
        /// </summary>
        [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
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
            if (ShouldBeNotified(originalInsuranceSelection, newInsuranceSelection))
            {
                PerformNotification();
            }
        }

        /// <summary>
        /// Notify the user of the change
        /// </summary>
        public void Notify(IDictionary<long, bool> originalInsuranceSelection, IDictionary<long, bool> newInsuranceSelection)
        {
            bool shouldBeNotified = originalInsuranceSelection.LeftJoin(newInsuranceSelection, x => x.Key, x => x.Key)
                .Any(x => ShouldBeNotified(x.Item1.Value, x.Item2.Value));

            // Only show the dialog if the shipment went from using insurance to not using insurance.
            if (shouldBeNotified)
            {
                PerformNotification();
            }
        }

        /// <summary>
        /// Should the customer be notified
        /// </summary>
        private static bool ShouldBeNotified(bool originalInsuranceSelection, bool newInsuranceSelection) =>
            !newInsuranceSelection && originalInsuranceSelection;

        /// <summary>
        /// Perform the actual notification, if necessary
        /// </summary>
        private void PerformNotification()
        {
            if (!currentUserSettings.ShouldShowNotification(notificationType))
            {
                return;
            }

            dialog = createDialog(this);
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

            dialog?.Close();
        }
    }
}
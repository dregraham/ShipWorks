using System;
using System.Reactive.Linq;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    public class ToggleAutoPrintPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IMessageHelper messageHelper;
        private readonly IUserSession usersession;
        private readonly ICurrentUserSettings currentUserSettings;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ToggleAutoPrintPipeline(
            IMessenger messenger,
            IMessageHelper messageHelper,
            IUserSession usersession,
            ICurrentUserSettings currentUserSettings)
        {
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.usersession = usersession;
            this.currentUserSettings = currentUserSettings;
        }

        /// <summary>
        /// Subscribe to the ToggleAutoPrint shortcut message 
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ToggleAutoPrint))
                .Where(m => usersession.User?.Settings?.SingleScanSettings != null &&
                            usersession.User.Settings.SingleScanSettings != (int) SingleScanSettings.Disabled)
                .Subscribe(HandleToggleAutoPrint);
        }

        /// <summary>
        /// End the session
        /// </summary>
        public void Dispose() => EndSession();

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscription?.Dispose();

        /// <summary>
        /// Toggle AutoPrint
        /// </summary>
        private void HandleToggleAutoPrint(ShortcutMessage shortcutMessage)
        {
            if (usersession.User.Settings.SingleScanSettings == (int) SingleScanSettings.AutoPrint)
            {
                usersession.User.Settings.SingleScanSettings = (int) SingleScanSettings.Scan;
            }
            else
            {
                usersession.User.Settings.SingleScanSettings = (int) SingleScanSettings.AutoPrint;
            }

            if (currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator))
            {
                string stateName = usersession.User.Settings.SingleScanSettings == (int) SingleScanSettings.AutoPrint ? "ON" : "OFF";

                if (shortcutMessage.Trigger == ShortcutTriggerType.Hotkey)
                {
                    messageHelper.ShowKeyboardPopup($"{shortcutMessage.Value}: Auto Print {stateName}");
                }
                else
                {
                    messageHelper.ShowBarcodePopup($"Barcode: Auto Print {stateName}");
                }
            }
        }
    }
}
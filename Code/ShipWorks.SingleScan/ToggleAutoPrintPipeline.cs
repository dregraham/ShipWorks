using System;
using System.Reactive.Linq;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Toggle autoprint via hotkeys or barcode scan
    /// </summary>
    public class ToggleAutoPrintPipeline : IInitializeForCurrentUISession
    {
        private readonly IMessenger messenger;
        private readonly IMessageHelper messageHelper;
        private readonly IUserSession userSession;
        private readonly ICurrentUserSettings currentUserSettings;
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ToggleAutoPrintPipeline(
            IMessenger messenger,
            IMessageHelper messageHelper,
            IUserSession userSession,
            ICurrentUserSettings currentUserSettings)
        {
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.userSession = userSession;
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
                .Where(m => userSession.User?.Settings?.SingleScanSettings != null &&
                            userSession.User.Settings.SingleScanSettings != (int) SingleScanSettings.Disabled)
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
            ChangeSingleScanSettings();
            ShowShortcutIndicator(shortcutMessage);
        }

        /// <summary>
        /// Update SingleScanSettings based on scan
        /// </summary>
        private void ChangeSingleScanSettings()
        {
            if (userSession.User.Settings.SingleScanSettings == (int) SingleScanSettings.AutoPrint)
            {
                userSession.User.Settings.SingleScanSettings = (int) SingleScanSettings.Scan;
            }
            else
            {
                userSession.User.Settings.SingleScanSettings = (int) SingleScanSettings.AutoPrint;
            }
        }

        /// <summary>
        /// Show shortcut indicator
        /// </summary>
        private void ShowShortcutIndicator(ShortcutMessage shortcutMessage)
        {
            if (currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator))
            {
                string stateName = userSession.User.Settings.SingleScanSettings == (int) SingleScanSettings.AutoPrint ? "ON" : "OFF";

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
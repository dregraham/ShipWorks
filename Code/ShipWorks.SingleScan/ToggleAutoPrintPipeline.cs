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
        private IDisposable subscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public ToggleAutoPrintPipeline(
            IMessenger messenger, 
            IMessageHelper messageHelper,
            IUserSession usersession)
        {
            this.messenger = messenger;
            this.messageHelper = messageHelper;
            this.usersession = usersession;
        }

        /// <summary>
        /// Subscribe to the ToggleAutoPrint shortcut message 
        /// </summary>
        public void InitializeForCurrentSession()
        {
            EndSession();

            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ToggleAutoPrint))
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
            if (usersession.User?.Settings?.SingleScanSettings != null && 
                usersession.User.Settings.SingleScanSettings != (int) SingleScanSettings.Disabled)
            {
                string stateName;
                if (usersession.User.Settings.SingleScanSettings == (int) SingleScanSettings.AutoPrint)
                {
                    usersession.User.Settings.SingleScanSettings = (int) SingleScanSettings.Scan;
                    stateName = "OFF";
                }
                else
                {
                    usersession.User.Settings.SingleScanSettings = (int) SingleScanSettings.AutoPrint;
                    stateName = "ON";
                }

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
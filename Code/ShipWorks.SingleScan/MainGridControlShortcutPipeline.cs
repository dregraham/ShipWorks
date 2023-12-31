﻿using System;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Settings;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Pipeline to clear or focus quick search text box based on shortcut messages
    /// </summary>
    public class MainGridControlShortcutPipeline : IMainGridControlPipeline
    {
        private IDisposable subscription;
        private readonly IMessenger messenger;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IMessageHelper messageHelper;
        private readonly ISchedulerProvider schedulerProvider;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridControlShortcutPipeline(IMessenger messenger,
            ICurrentUserSettings currentUserSettings,
            IMessageHelper messageHelper,
            ISchedulerProvider schedulerProvider)
        {
            this.messageHelper = messageHelper;
            this.schedulerProvider = schedulerProvider;
            this.currentUserSettings = currentUserSettings;
            this.messenger = messenger;
        }

        /// <summary>
        /// Subscribe to QuickSearch shortcut messages 
        /// </summary>
        public IDisposable Register(IMainGridControl mainGridControl)
        {
            subscription = messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ClearQuickSearch) ||
                            m.AppliesTo(KeyboardShortcutCommand.FocusQuickSearch))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Where(_ => currentUserSettings.GetUIMode() == UIMode.Batch)
                .Subscribe(message => HandleQuickSearchMessage(message, mainGridControl));

            return subscription;
        }

        /// <summary>
        /// Handle QuickSearch Message
        /// </summary>
        private void HandleQuickSearchMessage(ShortcutMessage shortcutMessage, IMainGridControl mainGridControl)
        {
            UpdateQuickSearchTextbox(shortcutMessage, mainGridControl);
            ShowShortcutIndicator(shortcutMessage);
        }

        /// <summary>
        /// Show shortcut indicator
        /// </summary>
        private void ShowShortcutIndicator(ShortcutMessage shortcutMessage)
        {
            if (currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator))
            {
                string actionName = EnumHelper.GetDescription(shortcutMessage.Shortcut.Action);

                if (shortcutMessage.Trigger == ShortcutTriggerType.Hotkey)
                {
                    messageHelper.ShowKeyboardPopup($"{shortcutMessage.Value}: {actionName}");
                }
                else
                {
                    messageHelper.ShowBarcodePopup($"Barcode: {actionName}");
                }
            }
        }

        /// <summary>
        /// Clears or focuses quick search textbox
        /// </summary>
        private void UpdateQuickSearchTextbox(ShortcutMessage shortcutMessage, IMainGridControl mainGridControl)
        {
            if (shortcutMessage.AppliesTo(KeyboardShortcutCommand.ClearQuickSearch))
            {
                mainGridControl.ClearSearch();
            }
            else
            {
                messenger.Send(new FocusQuickSearchMessage(this));
            }
        }
    }
}

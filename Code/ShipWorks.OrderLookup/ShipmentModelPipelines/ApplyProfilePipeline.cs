using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for applying a profile via shortcut or barcode
    /// </summary>
    public class ApplyProfilePipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyProfilePipeline(
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            ICurrentUserSettings currentUserSettings,
            IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            this.currentUserSettings = currentUserSettings;
            this.schedulerProvider = schedulerProvider;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model) =>
            messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ApplyProfile))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Select(x => model.ApplyProfile(x.Shortcut.RelatedObjectID.Value))
                .Subscribe();

        ///// <summary>
        ///// Show shortcut indicator
        ///// </summary>
        //private void ShowShortcutIndicator(ShortcutMessage shortcutMessage)
        //{
        //    if (currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.ShortcutIndicator))
        //    {
        //        string actionName = EnumHelper.GetDescription(shortcutMessage.Shortcut.Action);

        //        if (shortcutMessage.Trigger == ShortcutTriggerType.Hotkey)
        //        {
        //            messageHelper.ShowKeyboardPopup($"{shortcutMessage.Value}: {actionName}");
        //        }
        //        else
        //        {
        //            messageHelper.ShowBarcodePopup($"Barcode: {actionName}");
        //        }
        //    }
        //}
    }
}

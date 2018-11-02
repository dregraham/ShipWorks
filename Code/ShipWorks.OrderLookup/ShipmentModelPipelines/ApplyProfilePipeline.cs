using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Shipping.Services;
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
        private readonly IShippingProfileRepository profileRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplyProfilePipeline(
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            ICurrentUserSettings currentUserSettings,
            IShippingProfileRepository profileRepository,
            IMessageHelper messageHelper)
        {
            this.profileRepository = profileRepository;
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
                .Select(x => profileRepository.Get(x.Shortcut.RelatedObjectID.Value))
                .Select(model.ApplyProfile)
                .Subscribe();
    }
}

using System;
using System.Linq;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Users;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for clearing the search box
    /// </summary>
    public class ClearSearchPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IShortcutManager shortcutManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClearSearchPipeline(
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            IShortcutManager shortcutManager)
        {
            this.schedulerProvider = schedulerProvider;
            this.shortcutManager = shortcutManager;
            this.messenger = messenger;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        public IDisposable Register(IOrderLookupShipmentModel model) =>
            messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ClearQuickSearch))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Where(_ => model.CanAcceptFocus())
                .Do(_ => model.Unload())
                .Do(shortcutManager.ShowShortcutIndicator)
                .Subscribe();
    }
}

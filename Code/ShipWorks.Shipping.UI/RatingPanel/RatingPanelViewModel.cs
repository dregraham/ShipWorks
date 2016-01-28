using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// View model for getting rates
    /// </summary>
    public partial class RatingPanelViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDisposable subscriptions;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor just for tests
        /// </summary>
        protected RatingPanelViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messenger"></param>
        public RatingPanelViewModel(IMessenger messenger, ISchedulerProvider schedulerProvider) : this()
        {
            this.messenger = messenger;

            subscriptions = new CompositeDisposable(
                messenger.OfType<RatesRetrievingMessage>()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(_ => IsLoading = true),
                messenger.OfType<RatesRetrievingMessage>()
                    .CombineLatest(messenger.OfType<RatesRetrievedMessage>(), (x, y) => new { RetrievingHash = x.RatingHash, Message = y })
                    .Where(x => x.RetrievingHash == x.Message.RatingHash)
                    .Select(x => x.Message)
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(LoadRates)
            );
        }

        /// <summary>
        /// Load the rates contained in the given message
        /// </summary>
        public void LoadRates(RatesRetrievedMessage message)
        {
            Rates = message.RateGroup.Rates.Select(x => new RateResultDisplay(x)).ToArray();

            ShowDuties = Rates.Any(x => !string.IsNullOrEmpty(x.Duties));
            ShowTaxes = Rates.Any(x => !string.IsNullOrEmpty(x.Taxes));
            ShowShipping = Rates.Any(x => !string.IsNullOrEmpty(x.Shipping));

            IsLoading = false;
        }

        /// <summary>
        /// Dispose any held resources
        /// </summary>
        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}

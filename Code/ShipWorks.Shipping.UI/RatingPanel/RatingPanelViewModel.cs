using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using Autofac.Features.Indexed;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
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
        private RateResultDisplay selectedRate;

        /// <summary>
        /// Constructor just for tests
        /// </summary>
        protected RatingPanelViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Currently selected rate
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RateResultDisplay SelectedRate
        {
            get { return selectedRate; }
            set { handler.Set(nameof(SelectedRate), ref selectedRate, value); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messenger"></param>
        public RatingPanelViewModel(IMessenger messenger, ISchedulerProvider schedulerProvider,
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceLookup) : this()
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
                    .Subscribe(LoadRates),
                messenger.OfType<ShipmentChangedMessage>()
                    .Where(x => x.ChangedField == "ServiceType")
                    .Select(x => Rates.FirstOrDefault(rate => rate.AppliesToService(ratingServiceLookup[x.ShipmentAdapter.ShipmentTypeCode], x.ShipmentAdapter)))
                    .Where(x => x != null)
                    .Subscribe(x => SelectedRate = x)
            );
        }

        /// <summary>
        /// Load the rates contained in the given message
        /// </summary>
        public void LoadRates(RatesRetrievedMessage message)
        {
            if (message.Results.Success)
            {
                Rates = message.Results.Value.Rates.Select(x => new RateResultDisplay(x)).ToArray();
                ShowEmptyMessage = false;
                EmptyMessage = string.Empty;
            }
            else
            {
                Rates = Enumerable.Empty<RateResultDisplay>();
                EmptyMessage = message.Results.Message;
                ShowEmptyMessage = true;
            }

            Footnotes = message.Results.Value.FootnoteFactories
                .Select(x => x.CreateViewModel(message.ShipmentAdapter))
                .ToList();
            ShowFootnotes = Footnotes.Any();

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

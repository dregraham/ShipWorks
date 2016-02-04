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
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

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
        private RateResult selectedRate;
        private readonly IIndex<ShipmentTypeCode, IRatingService> ratingServiceLookup;

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
        public RateResult SelectedRate
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
            this.ratingServiceLookup = ratingServiceLookup;

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
                    .Subscribe(x => SelectRate(x.ShipmentAdapter)),
                handler.Where(x => nameof(SelectedRate).Equals(x, StringComparison.Ordinal))
                    .Where(_ => SelectedRate != null)
                    .Subscribe(_ => messenger.Send(new SelectedRateChangedMessage(this, SelectedRate)))
            );
        }

        /// <summary>
        /// Load the rates contained in the given message
        /// </summary>
        public void LoadRates(RatesRetrievedMessage message)
        {
            if (message.Results.Success)
            {
                Rates = message.Results.Value.Rates.ToArray();
                ShowEmptyMessage = false;
                EmptyMessage = string.Empty;
            }
            else
            {
                Rates = Enumerable.Empty<RateResult>();
                EmptyMessage = message.Results.Message;
                ShowEmptyMessage = true;
            }

            Footnotes = message.Results.Value?.FootnoteFactories
                .Select(x => x.CreateViewModel(message.ShipmentAdapter))
                .ToList() ?? Enumerable.Empty<object>();
            ShowFootnotes = Footnotes.Any();

            ShowDuties = Rates.Any(x => x.Duties.HasValue);
            ShowTaxes = Rates.Any(x => x.Taxes.HasValue);
            ShowShipping = Rates.Any(x => x.Shipping.HasValue);

            SelectRate(message.ShipmentAdapter);

            IsLoading = false;
        }

        /// <summary>
        /// Set the selected rate for the shipment
        /// </summary>
        private void SelectRate(ICarrierShipmentAdapter shipmentAdapter)
        {
            if (shipmentAdapter == null)
            {
                SelectedRate = null;
                return;
            }

            SelectedRate = Rates.FirstOrDefault(rate =>
                ratingServiceLookup[shipmentAdapter.ShipmentTypeCode].IsRateSelectedByShipment(rate, shipmentAdapter));
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

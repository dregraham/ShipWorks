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
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
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

            // When a RatesRetrievingMessage comes in, we want to wait for it's completed 
            // RatesRetrievedMessage before calling LoadRates.  The following code allows us to do that.
            // See the Switch example at http://download.microsoft.com/download/C/5/D/C5D669F9-01DF-4FAF-BBA9-29C096C462DB/Rx%20HOL%20.NET.pdf
            // for more info.  
            IObservable<RatesRetrievedMessage> mergedMessages = 
                (from rateRetrievingMsg in messenger.OfType<RatesRetrievingMessage>()
                 select messenger.OfType<RatesRetrievedMessage>()
                                 .Where(rateRetrivedMsg => rateRetrievingMsg.RatingHash == rateRetrivedMsg.RatingHash)
                ).Switch();
                      
            subscriptions = new CompositeDisposable(
                messenger.OfType<RatesRetrievingMessage>()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(_ => ShowSpinner()),
                mergedMessages.Throttle(TimeSpan.FromMilliseconds(250))
                    .Select(x => x)
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
        /// Blank out items that can show so that the user can't see them
        /// while rates are being retrieved, and show the spinner.
        /// </summary>
        private void ShowSpinner()
        {
            Rates = Enumerable.Empty<RateResult>();
            Footnotes = Enumerable.Empty<object>();
            ShowEmptyMessage = false;
            IsLoading = true;
        }

        /// <summary>
        /// Load the rates contained in the given message
        /// </summary>
        public void LoadRates(RatesRetrievedMessage message)
        {
            if (message.Success)
            {
                Rates = message.RateGroup.Rates.ToArray();
                ShowEmptyMessage = false;
                EmptyMessage = string.Empty;
            }
            else
            {
                Rates = Enumerable.Empty<RateResult>();
                EmptyMessage = message.ErrorMessage;
                ShowEmptyMessage = true;
            }

            Footnotes = message.RateGroup.FootnoteFactories
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
            if (shipmentAdapter == null || Rates == null)
            {
                SelectedRate = null;
                return;
            }

            SelectedRate = Rates.FirstOrDefault(rate => shipmentAdapter.DoesRateMatchSelectedService(rate));
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

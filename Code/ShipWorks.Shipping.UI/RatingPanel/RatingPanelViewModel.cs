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

            // When a RatesRetrievingMessage comes in, we want to wait for it's completed
            // RatesRetrievedMessage before calling LoadRates.  The following code allows us to do that.
            // See the Switch example at http://download.microsoft.com/download/C/5/D/C5D669F9-01DF-4FAF-BBA9-29C096C462DB/Rx%20HOL%20.NET.pdf
            // for more info.
            //IObservable<RatesRetrievedMessage> mergedMessages =
            //    (from rateRetrievingMsg in messenger.OfType<RatesRetrievingMessage>()
            //     select messenger.OfType<RatesRetrievedMessage>()
            //                     .Where(rateRetrivedMsg => rateRetrievingMsg.RatingHash == rateRetrivedMsg.RatingHash)
            //    ).Switch();

            //messenger.OfType<RatesRetrievingMessage>()
            //    .Select(GetMatchingRatesRetrievedMessage)
            //    .Switch()
            //    .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
            //    .Select(x => x)
            //    .ObserveOn(schedulerProvider.Dispatcher)
            //    .Subscribe(LoadRates);


            subscriptions = new CompositeDisposable(
                messenger.OfType<RatesRetrievingMessage>()
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(_ => ShowSpinner()),
                messenger.OfType<RatesRetrievingMessage>()
                    .Select(GetMatchingRatesRetrievedMessage)
                    .Switch()
                    .Throttle(TimeSpan.FromMilliseconds(250), schedulerProvider.Default)
                    .Select(x => x)
                    .ObserveOn(schedulerProvider.Dispatcher)
                    .Subscribe(LoadRates),
                messenger.OfType<ShipmentChangedMessage>()
                    .Where(x => x.ChangedField == "ServiceType")
                    .Subscribe(x => SelectRate(x.ShipmentAdapter)),
                messenger.OfType<RatesNotSupportedMessage>()
                    .Subscribe(HandleRatesNotSupportedMessage),
                handler.Where(x => nameof(SelectedRate).Equals(x, StringComparison.Ordinal))
                    .Where(_ => SelectedRate != null)
                    .Subscribe(_ => messenger.Send(new SelectedRateChangedMessage(this, SelectedRate)))
            );
        }

        /// <summary>
        /// Get rates retrieved messages that match the rates retrieving message
        /// </summary>
        private IObservable<RatesRetrievedMessage> GetMatchingRatesRetrievedMessage(RatesRetrievingMessage rateRetrievingMsg)
        {
            return messenger.OfType<RatesRetrievedMessage>()
               .Where(rateRetrivedMsg => rateRetrievingMsg.RatingHash == rateRetrivedMsg.RatingHash);
        }

        /// <summary>
        /// Handle a rates not supported message.
        /// </summary>
        private void HandleRatesNotSupportedMessage(RatesNotSupportedMessage message)
        {
            Footnotes = Enumerable.Empty<object>();
            Rates = Enumerable.Empty<RateResult>();
            EmptyMessage = message.Message;
            ShowEmptyMessage = true;
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
                // Do not show rates if the order has more than 1 shipment
                if (message?.ShipmentAdapter?.Shipment?.Order?.Shipments?.Count > 1)
                {
                    Footnotes = Enumerable.Empty<object>();
                    Rates = Enumerable.Empty<RateResult>();
                    EmptyMessage = "Unable to get rates for orders with multiple shipments.";
                    ShowEmptyMessage = true;
                }
                else if (message?.ShipmentAdapter?.ShipmentTypeCode == ShipmentTypeCode.Amazon ||
                         message?.ShipmentAdapter?.ShipmentTypeCode == ShipmentTypeCode.BestRate)
                {
                    Footnotes = Enumerable.Empty<object>();
                    Rates = Enumerable.Empty<RateResult>();
                    EmptyMessage = "Please use Ship Orders to get rates for this carrier.";
                    ShowEmptyMessage = true;
                }
                else
                {
                    Rates = message.RateGroup.Rates.ToArray();
                    ShowEmptyMessage = false;
                    EmptyMessage = string.Empty;

                    Footnotes = message.RateGroup.FootnoteFactories
                        .Select(x => x.CreateViewModel(message.ShipmentAdapter))
                        .ToList() ?? Enumerable.Empty<object>();
                    ShowFootnotes = Footnotes.Any();
                }
            }
            else
            {
                Rates = Enumerable.Empty<RateResult>();
                EmptyMessage = message.ErrorMessage;
                ShowEmptyMessage = true;
            }

            // If we didn't get any footnotes, and no error message, and not rates,
            // show the no rates message.
            if (!ShowFootnotes && !Rates.Any() && !ShowEmptyMessage)
            {
                ShowEmptyMessage = true;
                EmptyMessage = "No rates are available for the shipment.";
            }

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

            RateResult rate = Rates.FirstOrDefault(rateToTest => shipmentAdapter.DoesRateMatchSelectedService(rateToTest));

            // If the rate isn't null and isn't selectable, see if it has a kid
            if (!rate?.Selectable == true)
            {
                rate = shipmentAdapter.GetChildRateForRate(rate, Rates);
            }

            SelectedRate = rate;
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

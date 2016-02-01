using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Discounted;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.NotQualified;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.Carriers.Amazon;

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
                .Concat(new[] {
                    (IRateFootnoteFactory) new AmazonNotLinkedFootnoteFactory(ShipmentTypeCode.Usps),
                    new AmazonCarrierTermsAndConditionsNotAcceptedFootnoteFactory(new [] {"Foo", "Bar" }),
                    new InformationFootnoteFactory("This is a test message..."),
                    new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new Exception("This is the exception message")),
                    new ExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new InvalidPackageDimensionsException("This is the exception message")),
                    new UspsRatePromotionFootnoteFactory(ShipmentTypeCode.Other, new ShipmentEntity(), true),
                    new UspsRatePromotionFootnoteFactory(ShipmentTypeCode.Other, new ShipmentEntity(), false),
                    new UspsRateNotQualifiedFootnoteFactory(ShipmentTypeCode.Other),
                    new UspsRateDiscountedFootnoteFactory(ShipmentTypeCode.Other, new List<RateResult>(), new List<RateResult>()),
                    new Express1PromotionRateFootnoteFactory(ShipmentTypeCode.Other, null),
                    new Express1NotQualifiedRateFootnoteFactory(ShipmentTypeCode.Other),
                    new Express1DiscountedRateFootnoteFactory(ShipmentTypeCode.Other, new List<RateResult>(), new List<RateResult>()),
                    new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.Other),
                    new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new [] {
                        new BrokerException(new ShippingException("Error"), BrokerExceptionSeverityLevel.Error, new OtherShipmentType())
                    }),
                    new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new [] {
                        new BrokerException(new ShippingException("Information"), BrokerExceptionSeverityLevel.Information, new OtherShipmentType())
                    }),
                    new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.Other, new [] {
                        new BrokerException(new ShippingException("Warning"), BrokerExceptionSeverityLevel.Warning, new OtherShipmentType())
                    })
                })
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Core.Messaging.Messages.Shipping;
using Interapptive.Shared.Collections;
using ShipWorks.Data;
using System.Threading.Tasks;
using ShipWorks.Core.Common.Threading;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// View model for getting rates
    /// </summary>
    public class RatingPanelViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly IDisposable subscriptions;
        private readonly IMessenger messenger;

        private readonly IShippingManager shippingManager;
        private readonly IShipmentTypeFactory shipmentTypeFactory;

        private ShipmentEntity selectedShipment;
        private readonly bool consolidatePostalRates;
        private bool actionLinkVisible;
        private bool showAllRates;
        private bool showSpinner;
        private RateResult selectedRateResult;
        private RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
        private string errorMessage = string.Empty;
        private CancellationTokenSource cancelRatesToken;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messenger"></param>
        public RatingPanelViewModel(IMessenger messenger, IShippingManager shippingManager, IShipmentTypeFactory shipmentTypeFactory)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            this.messenger = messenger;
            this.shippingManager = shippingManager;
            this.shipmentTypeFactory = shipmentTypeFactory;

            consolidatePostalRates = true;
            ActionLinkVisible = false;
            ShowAllRates = true;

            subscriptions = new CompositeDisposable(
                messenger.AsObservable<UspsAutomaticExpeditedChangedMessage>().Subscribe(x => HandleUspsAutomaticExpeditedChangedMessage(x)),
                messenger.AsObservable<ShipmentChangedMessage>().Subscribe(HandleShipmentChangedMessage),
                messenger.AsObservable<OrderSelectionChangingMessage>()
                    .CombineLatest(messenger.AsObservable<OrderSelectionChangedMessage>(), (x, y) => new { OrderIdList = x.OrderIdList, Message = y })
                    .Where(x => x.OrderIdList.Intersect(x.Message.LoadedOrderSelection.Select(y => y.Order.OrderID)).Any())
                    .Select(x => x.Message)
                    .Subscribe(RefreshSelectedShipments)
            );
        }

        /// <summary>
        /// Gets/Sets whether to show the action link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ActionLinkVisible
        {
            get { return actionLinkVisible; }
            set
            {
                handler.Set(nameof(ActionLinkVisible), ref actionLinkVisible, value);
            }
        }

        /// <summary>
        /// Gets/Sets whether to show all rates
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowAllRates
        {
            get { return showAllRates; }
            set
            {
                handler.Set(nameof(ShowAllRates), ref showAllRates, value);
            }
        }

        /// <summary>
        /// Gets/Sets the rate group 
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RateGroup RateGroup
        {
            get { return rateGroup; }
            set
            {
                handler.Set(nameof(RateGroup), ref rateGroup, value);
            }
        }

        /// <summary>
        /// Gets/Sets any error message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                handler.Set(nameof(ErrorMessage), ref errorMessage, value);
            }
        }

        /// <summary>
        /// Gets/Sets whether to show the spinner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShowSpinner
        {
            get { return showSpinner; }
            set
            {
                handler.Set(nameof(ShowSpinner), ref showSpinner, value);
            }
        }

        /// <summary>
        /// Gets/Sets whether to show the spinner
        /// </summary>
        [Obfuscation(Exclude = true)]
        public RateResult SelectedRateResult
        {
            get { return selectedRateResult; }
            set
            {
                handler.Set(nameof(SelectedRateResult), ref selectedRateResult, value);

                messenger.Send(new SelectedRateChangedMessage(this, selectedRateResult));
            }
        }

        /// <summary>
        /// Store for the shipment
        /// </summary>
        public StoreEntity Store { get; set; }

        /// <summary>
        /// Forces rates to be refreshed by re-fetching the rates from the shipping provider.
        /// </summary>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
        public void RefreshRates(bool ignoreCache)
        {
            CancellationToken token = ResetCancellationTokenSource();

            ShipmentEntity shipment = selectedShipment;
            if (shipment == null)
            {
                ErrorMessage = "No shipments are selected.";
                return;
            }

            if (shipment.Processed)
            {
                ErrorMessage = "The shipment has already been processed.";
                return;
            }

            ShipmentType shipmentType = shipmentTypeFactory.Get(shipment.ShipmentTypeCode);

            if (!shipmentType.SupportsGetRates)
            {
                RateGroup rateGroupWithNoRatesFooter = new RateGroup(new List<RateResult>());

                rateGroupWithNoRatesFooter.AddFootnoteFactory(new InformationFootnoteFactory("Select another provider to get rates."));

                RateGroup = rateGroupWithNoRatesFooter;
            }
            else
            {
                // We need to fetch the rates from the provider
                FetchRates(shipment, ignoreCache, token);
            }
        }

        /// <summary>
        /// Load the shipment from the given order
        /// </summary>
        public void RefreshSelectedShipments(OrderSelectionChangedMessage orderMessage)
        {
            int orders = orderMessage.LoadedOrderSelection.HasMoreOrLessThanCount(1);
            if (orders != 0)
            {
                return;
            }

            ShipmentEntity shipment = GetLoadedShipmentResult(orderMessage.LoadedOrderSelection.Single());
            if (shipment != null)
            {
                selectedShipment = EntityUtility.CloneEntity(shipment, true);
                RefreshRates(false);
            }
            else
            {
                ShowSpinner = false;
            }
        }

        /// <summary>
        /// Sets the LoadedShipmentResult based on orderSelectionLoaded
        /// </summary>
        private ShipmentEntity GetLoadedShipmentResult(OrderSelectionLoaded loadedSelection)
        {
            if (loadedSelection.Exception != null)
            {
                ErrorMessage = "An error occurred while retrieving rates.";
                return null;
            }

            int moreOrLessThanOne = loadedSelection.Shipments.HasMoreOrLessThanCount(1);

            if (moreOrLessThanOne > 0)
            {
                ErrorMessage = "Multiple shipments selected.";
                return null;
            }

            if (moreOrLessThanOne < 0)
            {
                ErrorMessage = "No shipments are selected.";
                return null;
            }

            return loadedSelection.Shipments.Single();
        }

        /// <summary>
        /// Called when the shipment values have changed. We need to refresh the
        /// shipment data/rates being displayed to accurately.
        /// </summary>
        private void HandleShipmentChangedMessage(ShipmentChangedMessage message)
        {
            CancellationToken cancellationToken = ResetCancellationTokenSource();

            // Refresh the shipment data and then the rates
            ShipmentEntity shipment = message.Shipment;

            FetchRates(shipment, false, cancellationToken);
        }

        /// <summary>
        /// Called when the shipping settings for using USPS has changed. We need to refresh the
        /// shipment data/rates being displayed to accurately reflect that the shipment type has changed to USPS.
        /// </summary>
        private Task HandleUspsAutomaticExpeditedChangedMessage(UspsAutomaticExpeditedChangedMessage message)
        {
            CancellationToken cancellationToken = ResetCancellationTokenSource();

            ShipmentEntity shipment = selectedShipment;

            if (shipment == null)
            {
                return TaskUtility.CompletedTask;
            }

            // Refresh the shipment data and then the rates
            shippingManager.RefreshShipment(shipment);

            return FetchRates(shipment, false, cancellationToken);
        }

        /// <summary>
        /// Fetches the rates from the shipment type and 
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="ignoreCache">Should the cached rates be ignored?</param>
        private async Task FetchRates(ShipmentEntity shipment, bool ignoreCache, CancellationToken cancellationToken)
        {
            ShipmentRateGroup panelRateGroup = new ShipmentRateGroup(new RateGroup(new List<RateResult>()), shipment);

            RateGroup rates = null;
            ShipmentType shipmentType = null;
            try
            {
                using (Observable.Timer(TimeSpan.FromMilliseconds(75)).Subscribe(_ => ShowSpinner = true))
                {
                    // Fetch the rates and add them to the cache
                    shipmentType = PrepareShipmentAndGetShipmentType(shipment);
                    if (!shipmentType.SupportsGetRates)
                    {
                        throw new ShippingException("Rating not supported.");
                    }

                    // We want to ignore the cache primarily when changes come from the rate control, since only the promotion
                    // footer raises the event and we want to include Express1 rates in that case
                    if (ignoreCache)
                    {
                        shippingManager.RemoveShipmentFromRatesCache(shipment);
                    }

                    rates = await shippingManager.GetRatesAsync(shipment, shipmentType, cancellationToken);

                    panelRateGroup = new ShipmentRateGroup(rates, shipment);
                }
            }
            catch (InvalidRateGroupShippingException ex)
            {
                panelRateGroup = new ShipmentRateGroup(ex.InvalidRates, shipment);
                ErrorMessage = ex.Message;
            }
            catch (FedExAddressValidationException ex)
            {
                panelRateGroup = new ShipmentRateGroup(new InvalidRateGroup(new FedExShipmentType(), ex), shipment);
                ErrorMessage = ex.Message;
            }
            catch (ShippingException ex)
            {
                // While the rate group should be cached, there was a situation where getting credentials from Tango caused an exception,
                // which happened before exception handling kicked in.  So calling GetRates again and relying on cache was causing a crash.
                if (rates == null)
                {
                    rates = new RateGroup(new List<RateResult>());
                    ShipmentType exceptionShipmentType = shipmentType ?? shipmentTypeFactory.Get(ShipmentTypeCode.None);
                    rates.AddFootnoteFactory(new ExceptionsRateFootnoteFactory(exceptionShipmentType, ex));
                }

                panelRateGroup = new ShipmentRateGroup(rates, shipment);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            RateGroup = panelRateGroup;
            ShowSpinner = false;
        }

        /// <summary>
        /// Prepares the shipment for get rates and gets shipment type.
        /// This handles consolidating of postal rates if required.
        /// </summary>
        private ShipmentType PrepareShipmentAndGetShipmentType(ShipmentEntity shipment)
        {
            ShipmentType shipmentType;
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;

            // Only change this to best rate for non-USPS postal types
            if (consolidatePostalRates &&
                PostalUtility.IsPostalShipmentType(shipmentTypeCode) &&
                !PostalUtility.IsPostalSetup() &&
                shipmentTypeCode != ShipmentTypeCode.Usps &&
                shipmentTypeCode != ShipmentTypeCode.Express1Endicia &&
                shipmentTypeCode != ShipmentTypeCode.Express1Usps)
            {
                shipmentType = new BestRateShipmentType(new BestRateShippingBrokerFactory(new List<IShippingBrokerFilter> { new PostalCounterBrokerFilter(), new PostalOnlyBrokerFilter() }));

                shipment.ShipmentType = (int)ShipmentTypeCode.BestRate;
                shippingManager.EnsureShipmentLoaded(shipment);

                shipment.BestRate.DimsProfileID = shipment.Postal.DimsProfileID;
                shipment.BestRate.DimsLength = shipment.Postal.DimsLength;
                shipment.BestRate.DimsWidth = shipment.Postal.DimsWidth;
                shipment.BestRate.DimsHeight = shipment.Postal.DimsHeight;
                shipment.BestRate.DimsWeight = shipment.Postal.DimsWeight;
                shipment.BestRate.DimsAddWeight = shipment.Postal.DimsAddWeight;
                shipment.BestRate.ServiceLevel = (int)ServiceLevelType.Anytime;
                shipment.BestRate.InsuranceValue = shipment.Postal.InsuranceValue;
            }
            else
            {
                shipmentType = shipmentTypeFactory.Get(shipment.ShipmentTypeCode);
            }

            return shipmentType;
        }

        /// <summary>
        /// Cancel any existing actions and get a new token
        /// </summary>
        private CancellationToken ResetCancellationTokenSource()
        {
            cancelRatesToken?.Cancel();
            cancelRatesToken?.Dispose();
            cancelRatesToken = new CancellationTokenSource();
            return cancelRatesToken.Token;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                subscriptions.Dispose();
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);

            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.UI.RatingPanel
{
    /// <summary>
    /// View model for getting rates
    /// </summary>
    public partial class RatingPanelViewModel : IDisposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected Func<ISecurityContext> securityContextRetriever;
        protected readonly PropertyChangedHandler handler;

        private readonly IDisposable subscriptions;
        private RateResult selectedRate;
        private long? orderID;

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
        public RatingPanelViewModel(IMessenger messenger, IEnumerable<IRatingPanelGlobalPipeline> globalPipelines, Func<ISecurityContext> securityContextRetriever) : this()
        {
            this.securityContextRetriever = securityContextRetriever;

            subscriptions = new CompositeDisposable(
                globalPipelines.Select(x => x.Register(this)).Concat(new[] {
                handler.Where(x => nameof(SelectedRate).Equals(x, StringComparison.Ordinal))
                    .Where(_ => SelectedRate != null)
                    .Subscribe(_ => messenger.Send(new SelectedRateChangedMessage(this, SelectedRate)))
                })
            );
        }

        /// <summary>
        /// Currently selected rate
        /// </summary>
        [Obfuscation(Exclude = true)]
        public virtual RateResult SelectedRate
        {
            get { return selectedRate; }
            set { handler.Set(nameof(SelectedRate), ref selectedRate, value); }
        }

        /// <summary>
        /// Check to see if the user has permission to select a rate (selecting a
        /// rate will update the shipment, which some users may not be allowed to do).
        /// </summary>
        private bool UserHasPermissionToSelectRate => orderID.HasValue && securityContextRetriever().HasPermission(PermissionType.ShipmentsCreateEditProcess, orderID);

        /// <summary>
        /// Handle a rates not supported message.
        /// </summary>
        public virtual void HandleRatesNotSupportedMessage(RatesNotSupportedMessage message)
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
        public virtual void ShowSpinner()
        {
            IsLoading = true;
            ShowEmptyMessage = false;
            ShowFootnotes = false;
            AllowSelection = false;
        }

        /// <summary>
        /// Load the rates contained in the given message
        /// </summary>
        public virtual void LoadRates(RatesRetrievedMessage message)
        {
            if (message.Success)
            {
                SetRateResults(message.RateGroup.Rates, string.Empty,
                    message.RateGroup.FootnoteFactories.Select(x => x.CreateViewModel(message.ShipmentAdapter)));

                if (message.ShipmentAdapter.Shipment.ReturnShipment || message.ShipmentAdapter.Shipment.IncludeReturn)
                {
                    var returnFootnoteFactory = new InformationFootnoteFactory("Rates reflect the service charge only and do not include additional fees for returns.");
                    Footnotes = Footnotes.Concat(new[] { returnFootnoteFactory.CreateViewModel(message.ShipmentAdapter) });
                    ShowFootnotes = true;
                }
            }
            else
            {
                SetRateResults(Enumerable.Empty<RateResult>(), message.ErrorMessage,
                    message.RateGroup.FootnoteFactories.Select(x => x.CreateViewModel(message.ShipmentAdapter)));
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
            AllowSelection = UserHasPermissionToSelectRate;
        }

        /// <summary>
        /// Sets the rate result fields that will be displayed
        /// </summary>
        public virtual void SetRateResults(IEnumerable<RateResult> rateResults, string message,
            IEnumerable<object> footnoteViewModels)
        {
            Rates = rateResults;
            EmptyMessage = message;

            Footnotes = footnoteViewModels.ToList() ?? Enumerable.Empty<object>();

            ShowFootnotes = Footnotes.Any();
            ShowEmptyMessage = !string.IsNullOrEmpty(EmptyMessage);
        }

        /// <summary>
        /// Set the selected rate for the shipment
        /// </summary>
        public virtual void SelectRate(ICarrierShipmentAdapter shipmentAdapter)
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

            orderID = shipmentAdapter.Shipment.OrderID;
            SelectedRate = rate;
        }

        /// <summary>
        /// Dispose any held resources
        /// </summary>
        public virtual void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}

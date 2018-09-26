using System;
using System.ComponentModel;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Users.Security;

namespace ShipWorks.OrderLookup.Controls.Rating
{
    /// <summary>
    /// View model for the RatingPanelControl for use with Order lookup mode
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Rates)]
    public class OrderLookupRatingPanelViewModel : RatingPanelViewModel
    {
        private readonly IOrderLookupMessageBus messageBus;
        private readonly IOrderLookupRatingService ratingService;
        private readonly IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup;
        private RateResult selectedRate;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupRatingPanelViewModel(IOrderLookupMessageBus messageBus, IOrderLookupRatingService ratingService,
                                               IIndex<ShipmentTypeCode, IRateHashingService> rateHashingServiceLookup,
                                               Func<ISecurityContext> securityContextRetriever)
        {
            this.messageBus = messageBus;
            this.ratingService = ratingService;
            this.rateHashingServiceLookup = rateHashingServiceLookup;

            messageBus.PropertyChanged += MessageBusPropertyChanged;
            this.securityContextRetriever = securityContextRetriever;
        }

        /// <summary>
        /// The currently selected rate
        /// </summary>
        public override RateResult SelectedRate
        {
            get => selectedRate;
            set
            {
                selectedRate = value;
                messageBus.ShipmentAdapter.SelectServiceFromRate(selectedRate);
                
                handler.Set(nameof(SelectedRate), ref selectedRate, value); 
                handler.RaisePropertyChanged(nameof(messageBus));
            }
        }

        /// <summary>
        /// Update when the order changes
        /// </summary>
        private void MessageBusPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Reload rates if the order or any fields that affect rating change
            if ((e.PropertyName == "Order" && messageBus.Order != null) ||
                rateHashingServiceLookup[messageBus.ShipmentAdapter.Shipment.ShipmentTypeCode]
                    .IsRatingField(e.PropertyName))
            {
                GenericResult<RateGroup> rates = ratingService.GetRates(messageBus.ShipmentAdapter.Shipment);
                
                LoadRates(new RatesRetrievedMessage(this, ratingService.LatestRateHash, rates, messageBus.ShipmentAdapter));
            }
        }
    }
}
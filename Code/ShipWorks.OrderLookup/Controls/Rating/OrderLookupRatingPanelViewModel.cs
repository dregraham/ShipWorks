using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations;
using ShipWorks.Users.Security;

namespace ShipWorks.OrderLookup.Controls.Rating
{
    /// <summary>
    /// View model for the RatingPanelControl for use with Order lookup mode
    /// </summary>
    [KeyedComponent(typeof(INotifyPropertyChanged), OrderLookupPanels.Rates)]
    public class OrderLookupRatingPanelViewModel : RatingPanelViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupRatingPanelViewModel(IOrderLookupShipmentModel shipmentModel,
            IMessenger messenger,
            IEnumerable<IRatingPanelGlobalPipeline> globalPipelines,
            Func<ISecurityContext> securityContextRetriever)  : base(messenger, globalPipelines, securityContextRetriever)
        {
            ShipmentModel = shipmentModel;
            shipmentModel.PropertyChanged += OnShipmentModelPropertyChanged;
        }

        /// <summary>
        /// ShipmentModel Property Changed
        /// </summary>
        private void OnShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PostalShipmentFields.Service.Name && ShipmentModel.ShipmentAdapter != null)
            {
                SelectRate(ShipmentModel.ShipmentAdapter);
            }
        }
        
        [Obfuscation(Exclude = true)]
        public IOrderLookupShipmentModel ShipmentModel { get; }

        /// <summary>
        /// The currently selected rate
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override RateResult SelectedRate
        {
            get => base.SelectedRate;
            set
            {
                if (base.SelectedRate == value)
                {
                    return;
                }

                base.SelectedRate = value;
                ShipmentModel.ShipmentAdapter?.SelectServiceFromRate(value);
            }
        }
    }
}
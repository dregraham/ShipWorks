using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.UI.RatingPanel;
using ShipWorks.Shipping.UI.RatingPanel.ObservableRegistrations;
using ShipWorks.UI;
using ShipWorks.Users.Security;

namespace ShipWorks.OrderLookup.Controls.Rating
{
    /// <summary>
    /// View model for the RatingPanelControl for use with Order lookup mode
    /// </summary>
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.Amazon)]
    [WpfView(typeof(GenericRatingControl))]
    public class GenericRatingViewModel : RatingPanelViewModel, IRatingViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericRatingViewModel(IOrderLookupShipmentModel shipmentModel,
            IMessenger messenger,
            IEnumerable<IRatingPanelGlobalPipeline> globalPipelines,
            Func<ISecurityContext> securityContextRetriever) : base(messenger, globalPipelines, securityContextRetriever)
        {
            ShipmentModel = shipmentModel;
            ShipmentModel.PropertyChanged += OnShipmentModelPropertyChanged;
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

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation]
        public bool Expanded { get; set; } = true;

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation]
        public string Title => "Rates";

        /// <summary>
        /// Title of the section
        /// </summary>
        [Obfuscation]
        public bool Visible => true;

        /// <summary>
        /// Shipment model
        /// </summary>
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

                if (SelectedRate != null)
                {
                    ShipmentModel.TotalCost = SelectedRate.AmountOrDefault;
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            ShipmentModel.PropertyChanged -= OnShipmentModelPropertyChanged;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.OrderLookup.FieldManager;
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
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.AmazonSFP)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.AmazonSWA)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.BestRate)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.Endicia)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.FedEx)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.Usps)]
    [KeyedComponent(typeof(IRatingViewModel), ShipmentTypeCode.DhlEcommerce)]
    [WpfView(typeof(GenericRatingControl))]
    public class GenericRatingViewModel : RatingPanelViewModel, IRatingViewModel, IOrderLookupViewModel
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
        /// Field layout repository
        /// </summary>
        public IOrderLookupFieldLayoutProvider FieldLayoutProvider => ShipmentModel.FieldLayoutProvider;

        /// <summary>
        /// Panel ID
        /// </summary>
        public SectionLayoutIDs PanelID => SectionLayoutIDs.Rates;

        /// <summary>
        /// ShipmentModel Property Changed
        /// </summary>
        private void OnShipmentModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == PostalShipmentFields.Service.Name || e.PropertyName == AmazonSFPShipmentFields.ShippingServiceID.Name) && ShipmentModel.ShipmentAdapter != null)
            {
                SelectRate(ShipmentModel.ShipmentAdapter);
            }
            if (e.PropertyName == nameof(ShipmentModel.SelectedOrder))
            {
                handler.RaisePropertyChanged(ShipmentFields.Processed.Name);
            }
        }

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
                    if (ShipmentModel.ShipmentAdapter.ShipmentTypeCode == ShipmentTypeCode.BestRate)
                    {
                        base.SelectedRate = Rates.FirstOrDefault();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    base.SelectedRate = value;
                }

                ShipmentModel.ShipmentAdapter?.SelectServiceFromRate(value);

                ShipmentModel.TotalCost = SelectedRate != null ? SelectedRate.AmountOrDefault : 0;
                ShipmentModel.SelectedRate = SelectedRate;
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
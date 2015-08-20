using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Service options editor for the "AmazonServiceControl" shipment type
    /// </summary>
    public partial class AmazonServiceControl : ServiceControlBase
    {
        private readonly AmazonServiceViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        /// <param name="viewModel">The view model for this control.</param>
        public AmazonServiceControl(RateControl rateControl, AmazonServiceViewModel viewModel) 
            : base(ShipmentTypeCode.Amazon, rateControl)
        {
            this.viewModel = viewModel;
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            originControl.Initialize(ShipmentTypeCode.Amazon);
        }

        /// <summary>
        /// Load the shipment entity data into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            List<ShipmentEntity> shipmentsAsList = shipments.ToList();
            base.LoadShipments(shipmentsAsList, enableEditing, enableShippingAddress);

            // Load the view model and bind it to the UI controls
            viewModel.Load(shipmentsAsList);
            CreateUiBindings();

            originControl.LoadShipments(shipmentsAsList);

            UpdateInsuranceDisplay();

            UpdateSectionDescription();
        }

        /// <summary>
        /// Add ui control bindings to the view model
        /// </summary>
        private void CreateUiBindings()
        {
            carrier.DataBindings.Clear();
            carrier.DataBindings.Add("Text", viewModel, "Carrier");
            carrier.DataBindings.Add("MultiValued", viewModel, "CarrierIsMultiValued");

            service.DataBindings.Clear();
            service.DataBindings.Add("Text", viewModel, "Service");
            service.DataBindings.Add("MultiValued", viewModel, "ServiceIsMultiValued");

            tracking.DataBindings.Clear();
            tracking.DataBindings.Add("Text", viewModel, "Tracking");
            tracking.DataBindings.Add("MultiValued", viewModel, "TrackingIsMultiValued");

            cost.DataBindings.Clear();
            cost.DataBindings.Add("Amount", viewModel, "Cost");
            cost.DataBindings.Add("MultiValued", viewModel, "CostIsMultiValued");

            shipDate.DataBindings.Clear();
            shipDate.DataBindings.Add("Text", viewModel, "ShipDate");
            shipDate.DataBindings.Add("MultiValued", viewModel, "ShipDateIsMultiValued");

            weight.DataBindings.Clear();
            weight.DataBindings.Add("Weight", viewModel, "ContentWeight");
            weight.DataBindings.Add("MultiValued", viewModel, "ContentWeightIsMultiValued");
        }

        /// <summary>
        /// Update the insurance display for the given shipments
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            viewModel.Load(LoadedShipments);

            weight.DataBindings.Clear();
            weight.DataBindings.Add("Weight", viewModel, "ContentWeight");
            weight.DataBindings.Add("MultiValued", viewModel, "ContentWeightIsMultiValued");
        }

        /// <summary>
        /// Save the content of the control to the entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            originControl.SaveToEntities();

            viewModel.Save(LoadedShipments);

            insuranceControl.SaveToInsuranceChoices();

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Origin data has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            sectionFrom.ExtraText = originControl.OriginDescription;
        }

        /// <summary>
        /// Important parts of the shipment details have changed
        /// </summary>
        private void OnShipmentDetailsChanged(object sender, EventArgs e)
        {
            UpdateSectionDescription();
        }

        /// <summary>
        /// Update the description of the section
        /// </summary>
        private void UpdateSectionDescription()
        {
            string text;

            if (service.MultiValued || carrier.MultiValued)
            {
                text = "(Multiple)";
            }
            else
            {
                text = carrier.Text;

                if (service.Text.Length > 0)
                {
                    if (text.Length > 0)
                    {
                        text += ", ";
                    }

                    text += service.Text;
                }
            }

            sectionShipment.ExtraText = text;
        }

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }
    }
}

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
using ShipWorks.Shipping.Carriers.Amazon.Enums;
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
            base.Initialize();

            EnumHelper.BindComboBox<AmazonDeliveryExperienceType>(deliveryConfirmation);

            originControl.Initialize(ShipmentTypeCode.Amazon);
            dimensionsControl.Initialize();
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

            LoadDimensions(shipmentsAsList);

            originControl.LoadShipments(shipmentsAsList);

            UpdateInsuranceDisplay();

            UpdateSectionDescription();
        }

        private void LoadDimensions(List<ShipmentEntity> shipments)
        {
            List<DimensionsAdapter> dimensions = new List<DimensionsAdapter>();

            shipments.ForEach(s => dimensions.Add(new DimensionsAdapter(s.Amazon)));

            //Load the dimensions
            dimensionsControl.LoadDimensions(dimensions);
        }

        /// <summary>
        /// Add ui control bindings to the view model
        /// </summary>
        private void CreateUiBindings()
        {
            deliveryConfirmation.DataBindings.Clear();
            deliveryConfirmation.DataBindings.Add("SelectedValue", viewModel, "DeliveryConfirmation");
            deliveryConfirmation.DataBindings.Add("MultiValued", viewModel, "DeliveryConfirmationIsMultiValued");

            carrierWillPickUp.DataBindings.Clear();
            carrierWillPickUp.DataBindings.Add("Checked", viewModel, "CarrierWillPickUp");
            carrierWillPickUp.DataBindings.Add("CheckState", viewModel, "CarrierWillPickUpCheckState");
            
            mustArriveByDate.DataBindings.Clear();
            mustArriveByDate.DataBindings.Add("Text", viewModel, "MustArriveByDate");
            mustArriveByDate.DataBindings.Add("MultiValued", viewModel, "MustArriveByDateIsMultiValued");

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
            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

            viewModel.Load(LoadedShipments);

            weight.DataBindings.Clear();
            weight.DataBindings.Add("Weight", viewModel, "ContentWeight");
            weight.DataBindings.Add("MultiValued", viewModel, "ContentWeightIsMultiValued");

            // Start the dimensions control listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;
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

            //Save dimensions
            dimensionsControl.SaveToEntities();

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
            string text = string.Empty;

            //if (service.MultiValued || carrier.MultiValued)
            //{
            //    text = "(Multiple)";
            //}
            //else
            //{
            //    text = carrier.Text;

            //    if (service.Text.Length > 0)
            //    {
            //        if (text.Length > 0)
            //        {
            //            text += ", ";
            //        }

            //        text += service.Text;
            //    }
            //}

            sectionShipment.ExtraText = text;
        }

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Service options editor for the "AmazonServiceControl" shipment type
    /// </summary>
    public partial class AmazonServiceControl : ServiceControlBase
    {
        private readonly AmazonServiceViewModel viewModel;
        private readonly AmazonShipmentType amazonShipmentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        /// <param name="viewModel">The view model for this control.</param>
        /// <param name="amazonShipmentType">AmazonShipmentType</param>
        public AmazonServiceControl(RateControl rateControl, AmazonServiceViewModel viewModel, AmazonShipmentType amazonShipmentType) 
            : base(ShipmentTypeCode.Amazon, rateControl)
        {
            this.viewModel = viewModel;
            this.amazonShipmentType = amazonShipmentType;
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
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();
            viewModel.PropertyChanged -= OnViewModelPropertyChanged;

            List<ShipmentEntity> shipmentsAsList = shipments.ToList();
            base.LoadShipments(shipmentsAsList, enableEditing, enableShippingAddress);

            // Load the view model and bind it to the UI controls
            viewModel.Load(shipmentsAsList);
            CreateUiBindings();

            LoadDimensions(shipmentsAsList);

            originControl.LoadShipments(shipmentsAsList);

            UpdateInsuranceDisplay();

            UpdateSectionDescription();

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }
        /// <summary>
        /// Loads available dimensions for shipments
        /// </summary>
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
            dimensionsControl.DimensionsChanged += DimensionsControl_DimensionsChanged;


            deliveryConfirmation.DataBindings.Clear();
            deliveryConfirmation.DataBindings.Add(nameof(deliveryConfirmation.SelectedValue), viewModel.DeliveryExperience, nameof(viewModel.DeliveryExperience.PropertyValue), false, DataSourceUpdateMode.OnPropertyChanged);
            deliveryConfirmation.DataBindings.Add(nameof(deliveryConfirmation.MultiValued), viewModel.DeliveryExperience, nameof(viewModel.DeliveryExperience.IsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);

            carrierWillPickUp.DataBindings.Clear();
            carrierWillPickUp.DataBindings.Add(nameof(carrierWillPickUp.Checked), viewModel, nameof(viewModel.CarrierWillPickUp), false, DataSourceUpdateMode.OnPropertyChanged);
            carrierWillPickUp.DataBindings.Add(nameof(carrierWillPickUp.CheckState), viewModel, nameof(viewModel.CarrierWillPickUpCheckState), false, DataSourceUpdateMode.OnPropertyChanged);

            sendDeliverBy.DataBindings.Clear();
            sendDeliverBy.DataBindings.Add(nameof(sendDeliverBy.Checked), viewModel, nameof(viewModel.SendDeliverBy), false, DataSourceUpdateMode.OnPropertyChanged);
            sendDeliverBy.DataBindings.Add(nameof(sendDeliverBy.CheckState), viewModel, nameof(viewModel.SendDeliverByCheckState), false, DataSourceUpdateMode.OnPropertyChanged);

            mustArriveByDate.DataBindings.Clear();
            mustArriveByDate.DataBindings.Add(nameof(mustArriveByDate.Text), viewModel, nameof(viewModel.DateMustArriveBy), false, DataSourceUpdateMode.OnPropertyChanged);

            weight.DataBindings.Clear();
            weight.DataBindings.Add(nameof(weight.Weight), viewModel, nameof(viewModel.ContentWeight), false, DataSourceUpdateMode.OnPropertyChanged);
            weight.DataBindings.Add(nameof(weight.MultiValued), viewModel, nameof(viewModel.ContentWeightIsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);

            service.DataBindings.Clear();
            service.DataBindings.Add(nameof(service.DataSource), viewModel, nameof(viewModel.ServicesAvailable), false, DataSourceUpdateMode.OnPropertyChanged);
            service.DataBindings.Add(nameof(service.SelectedItem), viewModel, nameof(viewModel.ShippingService), false, DataSourceUpdateMode.OnPropertyChanged);
            service.DataBindings.Add(nameof(service.MultiValued), viewModel, nameof(viewModel.ServiceIsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);
            service.SelectedValueChanged += OnServiceSelectedValueChanged;
        }

        private void DimensionsControl_DimensionsChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Handles the service drop down selection changed so that we can update the rate control
        /// </summary>
        private void OnServiceSelectedValueChanged(object sender, EventArgs e)
        {
            AmazonRateTag newValue = (AmazonRateTag)service.SelectedItem;

            if (newValue?.ShippingServiceId != viewModel?.ShippingService?.ShippingServiceId)
            {
                RateResult rateResult = RateControl.RateGroup.Rates.FirstOrDefault(r => ((AmazonRateTag)r.Tag).ShippingServiceId == newValue?.ShippingServiceId);
                RateControl.SelectRate(rateResult);
            }
        }

        /// <summary>
        /// Handle the view model property changed event
        /// </summary>
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (service.IsHandleCreated)
            {
                if (e.PropertyName == nameof(viewModel.ServicesAvailable))
                {
                    service.Invoke((MethodInvoker) delegate
                    {
                        AmazonRateTag previousValue = viewModel.ShippingService;
                        service.BindDataSourceAndPreserveSelection(viewModel.ServicesAvailable);
                        service.SelectedValue = previousValue.ShippingServiceId;
                    });
                    return;
                }
            }

            if (e.PropertyName == nameof(viewModel.ShippingService))
            {
                this.Invoke((MethodInvoker)delegate
                {
                    RaiseShipmentServiceChanged();
                    UpdateSectionDescription();
                });
                return;
            }

            if (amazonShipmentType.RatingFields.FieldsContainName(e.PropertyName))
            {
                RaiseRateCriteriaChanged();
            }
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
            weight.DataBindings.Add(nameof(weight.Weight), viewModel, nameof(viewModel.ContentWeight), false, DataSourceUpdateMode.OnPropertyChanged);
            weight.DataBindings.Add(nameof(weight.MultiValued), viewModel, nameof(viewModel.ContentWeightIsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);

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
        /// Update the description of the section
        /// </summary>
        private void UpdateSectionDescription()
        {
            if (service.MultiValued)
            {
                sectionShipment.ExtraText = "(Multiple Services)";
            }
            else
            {
                sectionShipment.ExtraText = service.Text;
            }
        }

        /// <summary>
        /// Handle rate selection from the grid
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            base.OnRateSelected(sender, e);

            AmazonRateTag rateTag = e.Rate?.Tag as AmazonRateTag;
            if (rateTag == null)
            {
                return;
            }

            viewModel.SelectRate(rateTag);
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                // Update the selected rate in the rate control to coincide with the service change
                AmazonRateTag selectedRateTag = service.SelectedItem as AmazonRateTag;

                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    AmazonRateTag rateTag = r.Tag as AmazonRateTag;
                    if (string.IsNullOrWhiteSpace(rateTag?.Description))
                    {
                        return false;
                    }

                    return rateTag.ShippingServiceId == selectedRateTag?.ShippingServiceId;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
        }
    }
}

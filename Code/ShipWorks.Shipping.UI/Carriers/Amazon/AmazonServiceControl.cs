﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Editing;
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
        private readonly AmazonRateHashingService rateHashingService;
        private IDisposable propertyChangedSubscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        /// <param name="viewModel">The view model for this control.</param>
        /// <param name="rateHashingService"></param>
        public AmazonServiceControl(RateControl rateControl, AmazonServiceViewModel viewModel, AmazonRateHashingService rateHashingService)
            : base(ShipmentTypeCode.Amazon, rateControl)
        {
            this.viewModel = viewModel;
            this.rateHashingService = rateHashingService;
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            EnumHelper.BindComboBox<AmazonDeliveryExperienceType>(deliveryConfirmation);

            originControl.Initialize(ShipmentTypeCode.Amazon);
            dimensionsControl.Initialize();
            shipDate.Value = DateTime.Now;

            originControl.OriginChanged += (s, e) => RaiseRateCriteriaChanged();
            dimensionsControl.DimensionsChanged += OnDimensionsChanged;
            dimensionsControl.DimensionsChanged += OnShipSenseFieldChanged;
            weight.WeightChanged += OnWeightChanged;
            weight.WeightChanged += OnShipSenseFieldChanged;
            shipDate.ValueChanged += OnShipDateChanged;
            
            weight.ConfigureTelemetryEntityCounts = telemetryEvent =>
            {
                telemetryEvent.AddMetric(WeightControl.ShipmentQuantityTelemetryKey, LoadedShipments?.Count ?? 0);
                telemetryEvent.AddMetric(WeightControl.PackageQuantityTelemetryKey, 1);
            };
        }

        /// <summary>
        /// Force the weight binding on weight change
        /// </summary>
        /// <remarks>
        /// When code changes the weight, we need to force the binding to write its value. This isn't necessary when running
        /// ShipWorks through Visual Studio, but is necessary when running an obfuscated build. This would imply that there
        /// is an obfuscation issue, but I can't quite tell what it is.
        /// 
        /// UPDATE: 7/6/2017 Mirza Mulaosmanovic
        /// Only write value when count is one because it was writing the multi value text to the view model which gets 
        /// converted to zero and sets the weight of all of the selected shipments to zero
        /// </remarks>
        private void OnWeightChanged(object sender, EventArgs e)
        {
            if (LoadedShipments.IsCountEqualTo(1))
            {
                foreach (Binding binding in weight.DataBindings.OfType<Binding>().Where(x => x.PropertyName == nameof(weight.Weight)))
                {
                    binding.WriteValue();
                }
            }
        }

        /// <summary>
        /// Called when [ship date changed].
        /// </summary>
        private void OnShipDateChanged(object sender, EventArgs e)
        {
            foreach (Binding binding in shipDate.DataBindings.OfType<Binding>().Where(x => x.PropertyName == nameof(shipDate.Value)))
            {
                binding.WriteValue();
            }

            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Load the shipment entity data into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();
            propertyChangedSubscriptions?.Dispose();

            List<ShipmentEntity> shipmentsAsList = shipments.ToList();
            base.LoadShipments(shipmentsAsList, enableEditing, enableShippingAddress);

            // Load the view model and bind it to the UI controls
            viewModel.Load(shipmentsAsList);
            CreateUiBindings();

            LoadDimensions(shipmentsAsList);

            originControl.LoadShipments(shipmentsAsList);

            UpdateInsuranceDisplay();

            UpdateSectionDescription();

            propertyChangedSubscriptions = SubscribeToPropertyChangedEvent();
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
            deliveryConfirmation.DataBindings.Clear();
            deliveryConfirmation.DataBindings.Add(nameof(deliveryConfirmation.SelectedValue),
                viewModel.DeliveryExperience, nameof(viewModel.DeliveryExperience.PropertyValue),
                true, DataSourceUpdateMode.OnPropertyChanged);
            deliveryConfirmation.DataBindings.Add(nameof(deliveryConfirmation.MultiValued),
                viewModel.DeliveryExperience, nameof(viewModel.DeliveryExperience.IsMultiValued),
                false, DataSourceUpdateMode.OnPropertyChanged);

            shipDate.DataBindings.Clear();
            shipDate.DataBindings.Add(nameof(shipDate.Value), viewModel, nameof(viewModel.ShipDate), false, DataSourceUpdateMode.OnPropertyChanged);
            shipDate.DataBindings.Add(nameof(shipDate.MultiValued), viewModel, nameof(viewModel.ShipDateIsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);

            weight.DataBindings.Clear();
            weight.DataBindings.Add(nameof(weight.Weight), viewModel, nameof(viewModel.ContentWeight), false, DataSourceUpdateMode.OnPropertyChanged);
            weight.DataBindings.Add(nameof(weight.MultiValued), viewModel, nameof(viewModel.ContentWeightIsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);

            service.SelectedValueChanged -= OnServiceSelectedValueChanged;
            service.DataBindings.Clear();
            service.DataBindings.Add(nameof(service.DataSource), viewModel, nameof(viewModel.ServicesAvailable), false, DataSourceUpdateMode.OnPropertyChanged);
            service.DataBindings.Add(nameof(service.SelectedItem), viewModel, nameof(viewModel.ShippingService), false, DataSourceUpdateMode.OnPropertyChanged);
            service.DataBindings.Add(nameof(service.MultiValued), viewModel, nameof(viewModel.ServiceIsMultiValued), false, DataSourceUpdateMode.OnPropertyChanged);
            service.SelectedValueChanged += OnServiceSelectedValueChanged;
        }

        /// <summary>
        /// Handle when dimensions have changed
        /// </summary>
        private void OnDimensionsChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Handles the service drop down selection changed so that we can update the rate control
        /// </summary>
        private void OnServiceSelectedValueChanged(object sender, EventArgs e)
        {
            AmazonRateTag newValue = service.SelectedItem as AmazonRateTag;

            if (newValue?.ShippingServiceId != viewModel?.ShippingService?.ShippingServiceId)
            {
                RateResult rateResult = RateControl.RateGroup.Rates.FirstOrDefault(MatchingAmazonServiceRate(newValue));
                RateControl.SelectRate(rateResult);
            }
        }

        /// <summary>
        /// Get a method that will match amazon rates against the given service
        /// </summary>
        private Func<RateResult, bool> MatchingAmazonServiceRate(AmazonRateTag newValue)
        {
            return rate =>
            {
                AmazonRateTag amazonTag = rate.Tag as AmazonRateTag;
                return amazonTag?.ShippingServiceId == newValue?.ShippingServiceId;
            };
        }

        /// <summary>
        /// Subscribe to all the property changed events
        /// </summary>
        private IDisposable SubscribeToPropertyChangedEvent()
        {
            IObservable<string> events = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    x => viewModel.PropertyChanged += x,
                    x => viewModel.PropertyChanged -= x)
                .Select(x => x.EventArgs.PropertyName);

            return new CompositeDisposable(
                events.Where(x => x == nameof(viewModel.ServicesAvailable)).Subscribe(OnServicesAvailableChanged),
                events.Where(x => x == nameof(viewModel.ShippingService)).Subscribe(OnShippingServiceChanged),
                events.Where(rateHashingService.RatingFields.FieldsContainName).Subscribe(OnRatingFieldChanged)
            );
        }

        /// <summary>
        /// The available services have changed
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnServicesAvailableChanged(string propertyName)
        {
            AmazonRateTag previousValue = viewModel.ShippingService;
            service.BindDataSourceAndPreserveSelection(viewModel.ServicesAvailable);
            service.SelectedValue = previousValue?.ShippingServiceId ?? string.Empty;
            service.Enabled = viewModel.ServicesAvailable.Count > 1;
        }

        /// <summary>
        /// The selected service has changed
        /// </summary>
        private void OnShippingServiceChanged(string propertyName)
        {
            RaiseShipmentServiceChanged();
            UpdateSectionDescription();
            UpdateInsuranceDisplay();
        }

        /// <summary>
        /// A rating field has changed
        /// </summary>
        private void OnRatingFieldChanged(string propertyName) => RaiseRateCriteriaChanged();

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
            sectionShipment.ExtraText = service.MultiValued ? "(Multiple Services)" : service.Text;
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
                RateControl.SelectRate(RateControl.RateGroup.Rates.FirstOrDefault());
            }
        }

        /// <summary>
        /// Pre select a rate
        /// </summary>
        public override void PreSelectRate(RateSelectedEventArgs args) =>
            viewModel.ShippingService = args.Rate.Tag as AmazonRateTag;

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e) => RaiseShipSenseFieldChanged();
    }
}

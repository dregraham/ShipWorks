using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
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
        private readonly IMessenger messenger;
        private List<AmazonServiceTypeEntity> allServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        /// <param name="serviceTypeRepository">Repository of Amazon service types</param>
        public AmazonServiceControl(RateControl rateControl, IAmazonServiceTypeRepository serviceTypeRepository, IMessenger messenger)
            : base(ShipmentTypeCode.Amazon, rateControl)
        {
            this.messenger = messenger;
            InitializeComponent();
            allServices = serviceTypeRepository.Get();

            this.messenger.OfType<RatesRetrievedMessage>()
                .Subscribe(x =>
                {
                    if (x.Success)
                    {
                        UpdateServiceTypes(new List<ShipmentEntity> {x.ShipmentAdapter.Shipment});
                    }
                });
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        protected override void Initialize()
        {
            EnumHelper.BindComboBox<AmazonDeliveryExperienceType>(deliveryConfirmation);

            InitializeServiceComboBox();

            originControl.Initialize(ShipmentTypeCode.Amazon);
            dimensionsControl.Initialize();
            shipDate.Value = DateTime.Now;

            originControl.OriginChanged += (s, e) => RaiseRateCriteriaChanged();
            dimensionsControl.DimensionsChanged += (s, e) => RaiseRateCriteriaChanged();
            weight.WeightChanged += (s, e) => RaiseRateCriteriaChanged();
            shipDate.ValueChanged += (s, e) => RaiseRateCriteriaChanged();
            deliveryConfirmation.SelectedValueChanged += (s, e) => RaiseRateCriteriaChanged();

            dimensionsControl.DimensionsChanged += (s, e) => RaiseShipSenseFieldChanged();
            weight.WeightChanged += (s, e) => RaiseShipSenseFieldChanged();

            service.SelectedValueChanged += OnServiceChanged;

            weight.ConfigureTelemetryEntityCounts = telemetryEvent =>
            {
                telemetryEvent.AddMetric(WeightControl.ShipmentQuantityTelemetryKey, LoadedShipments?.Count ?? 0);
                telemetryEvent.AddMetric(WeightControl.PackageQuantityTelemetryKey, 1);
            };
        }


        /// <summary>
        /// Load the shipment entity data into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            originControl.DestinationChanged -= OnOriginDestinationChanged;

            List<ShipmentEntity> shipmentsAsList = shipments.ToList();
            base.LoadShipments(shipmentsAsList, enableEditing, enableShippingAddress);

            originControl.DestinationChanged += OnOriginDestinationChanged;

            LoadShipmentDetails();
            UpdateInsuranceDisplay();
            UpdateSectionDescription();
            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Load shipment details
        /// </summary>
        private void LoadShipmentDetails()
        {
            // Load the origin
            originControl.LoadShipments(LoadedShipments);

            List<DimensionsAdapter> dimensions = new List<DimensionsAdapter>();

            // Update the service types
            service.SelectedValueChanged -= OnServiceChanged;
            UpdateServiceTypes(LoadedShipments);

            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    AmazonServiceTypeEntity serviceType = allServices.FirstOrDefault(s => s.ApiValue == shipment.Amazon.ShippingServiceID);
                    if (serviceType != null)
                    {
                        service.ApplyMultiText(serviceType.Description);
                    }

                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    dimensions.Add(new DimensionsAdapter(shipment.Amazon));

                    deliveryConfirmation.ApplyMultiValue((AmazonDeliveryExperienceType) shipment.Amazon.DeliveryExperience);
                }
            }

            service.SelectedValueChanged += OnServiceChanged;

            //Load the dimensions
            dimensionsControl.LoadDimensions(dimensions);
        }

        /// <summary>
        /// Update the available choices for services
        /// </summary>
        private void UpdateServiceTypes(List<ShipmentEntity> shipments)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                Dictionary<int, string> availableServices = lifetimeScope.ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.Amazon)
                    .BuildServiceTypeDictionary(shipments);

                service.BindDataSourceAndPreserveSelection(allServices.Where(s => availableServices.ContainsKey(s.AmazonServiceTypeID)).ToList());
            }
        }

        /// <summary>
        /// Handles the service drop down selection changed so that we can update the rate control
        /// </summary>
        private void OnServiceChanged(object sender, EventArgs e)
        {
            SyncSelectedRate();
            UpdateSectionDescription();
        }

        /// <summary>
        /// Update the insurance display for the given shipments
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.InsuranceOptionsChanged -= OnRateCriteriaChanged;
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
            insuranceControl.InsuranceOptionsChanged += OnRateCriteriaChanged;
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            // Stop the dimensions control from listening to weight changes
            dimensionsControl.ShipmentWeightBox = null;

            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                }
            }

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

            // Save the origin
            originControl.SaveToEntities();

            //Save insurance info
            insuranceControl.SaveToInsuranceChoices();

            //Save dimensions
            dimensionsControl.SaveToEntities();

            // Save the other fields
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                service.ReadMultiValue(v => shipment.Amazon.ShippingServiceID = v.ToString());
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);

                deliveryConfirmation.ReadMultiValue(v => shipment.Amazon.DeliveryExperience = (int) v);

                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
            }

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
            sectionShipment.ExtraText = service.MultiValued ? "(Multiple Services)" : (service.SelectedItem as AmazonServiceTypeEntity).Description;
        }

        /// <summary>
        /// Handle rate selection from the grid
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;
            AmazonRateTag rateTag = e.Rate?.Tag as AmazonRateTag;

            if (rateTag == null)
            {
                return;
            }

            AmazonServiceTypeEntity selectedService = allServices.FirstOrDefault(s => s.ApiValue == rateTag.ShippingServiceId);

            service.SelectedItem = selectedService;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                // Update the selected rate in the rate control to coincide with the service change
                string apiValue = service.SelectedValue as string;

                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    AmazonRateTag rateTag = r.Tag as AmazonRateTag;
                    if (string.IsNullOrWhiteSpace(rateTag?.Description))
                    {
                        return false;
                    }

                    return rateTag.ShippingServiceId == apiValue;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.SelectRate(RateControl.RateGroup.Rates.FirstOrDefault());
            }
        }

        /// <summary>
        /// Initializes the service ComboBox.
        /// </summary>
        private void InitializeServiceComboBox()
        {
            service.DataSource = null;
            service.DisplayMember = "Description";
            service.ValueMember = "ApiValue";
            service.DataSource = allServices;
        }
    }
}

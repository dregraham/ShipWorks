using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateServiceControl(ShipmentTypeCode shipmentTypeCode)
            : base (shipmentTypeCode)
        {
            InitializeComponent();

            rateControl.ReloadRatesRequired += OnReloadRatesRequired;
        }


        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.BestRate);
            dimensionsControl.Initialize();

            EnumHelper.BindComboBox<TransitTimeRange>(transitDays);
        }

        /// <summary>
        /// Load the data for the list of shipments into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();

            RecipientDestinationChanged -= OnRecipientDestinationChanged;

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            RecipientDestinationChanged += OnRecipientDestinationChanged;

            LoadShipmentDetails();

            UpdateInsuranceDisplay();

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Load shipment details
        /// </summary>
        private void LoadShipmentDetails()
        {
            // Load the origin
            originControl.LoadShipments(LoadedShipments);

            List<DimensionsAdapter> dimensions = new List<DimensionsAdapter>();

            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    shipDate.ApplyMultiDate(shipment.ShipDate);

                    transitDays.ApplyMultiValue((TransitTimeRange) shipment.BestRate.TransitDays);

                    dimensions.Add(new DimensionsAdapter(shipment.BestRate));
                }
            }

            //Load the dimensions
            dimensionsControl.LoadDimensions(dimensions);
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            //Save dimensions
            dimensionsControl.SaveToEntities();

            // Save the other fields
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);
                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
                transitDays.ReadMultiValue(v => shipment.BestRate.TransitDays = (int)v);
            }

            ResumeRateCriteriaChangeEvent();
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

            // Start the dimensiosn control listening to weight changes
            dimensionsControl.ShipmentWeightBox = weight;
        }

        /// <summary>
        /// Called whent the recipient country has changed.  We may have to switch from an international to domestic UI
        /// </summary>
        private void OnRecipientDestinationChanged(object sender, EventArgs e)
        {
            SaveToShipments();
            LoadShipmentDetails();
        }

        /// <summary>
        /// Some aspect of the shipment that affects rates has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        private void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnOriginChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

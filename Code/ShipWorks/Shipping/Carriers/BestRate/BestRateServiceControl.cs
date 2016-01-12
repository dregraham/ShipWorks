using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BestRateServiceControl : ServiceControlBase
    {
        private RateResult cachedRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateServiceControl"/> class.
        /// </summary>
        public BestRateServiceControl(ShipmentTypeCode shipmentTypeCode, RateControl rateControl)
            : base (shipmentTypeCode, rateControl)
        {
            InitializeComponent();
            RateControl.ShowAllRates = false;
            RateControl.ActionLinkVisible = true;

            sectionLabelOptions.Visible = false;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.BestRate);
            dimensionsControl.Initialize();

            EnumHelper.BindComboBox<ServiceLevelType>(serviceLevel);
        }

        /// <summary>
        /// Load the data for the list of shipments into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            RecipientDestinationChanged -= OnRecipientDestinationChanged;
            originControl.DestinationChanged -= OnOriginDestinationChanged;

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            RecipientDestinationChanged += OnRecipientDestinationChanged;
            originControl.DestinationChanged += OnOriginDestinationChanged;
            
            LoadShipmentDetails();
            UpdateInsuranceDisplay();

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

            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    weight.ApplyMultiWeight(shipment.ContentWeight);
                    dimensions.Add(new DimensionsAdapter(shipment.BestRate));

                    serviceLevel.ApplyMultiValue((ServiceLevelType) shipment.BestRate.ServiceLevel);
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
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            //Save dimensions
            dimensionsControl.SaveToEntities();

            //Save insurance info
            insuranceControl.SaveToInsuranceChoices();

            // Save the other fields
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);
                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
                serviceLevel.ReadMultiValue(v => shipment.BestRate.ServiceLevel = (int)v);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
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
        /// Called when the recipient country has changed.  We may have to switch from an international to domestic UI
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

        /// <summary>
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }
        
        /// <summary>
        /// Event raised when the origin address has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            sectionFrom.ExtraText = originControl.OriginDescription;

            OnRateCriteriaChanged(sender, e);
        }

        /// <summary>
        /// Update the insurance rate display
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.LoadInsuranceChoices(
                LoadedShipments.Select(
                    shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
        }

        /// <summary>
        /// Service Level Changed
        /// </summary>
        private void OnServiceLevelChanged(object sender, EventArgs e)
        {
            OnRateCriteriaChanged(sender, e);
        }


        /// <summary>
        /// Called when the configure rate is clicked
        /// </summary>
        public override void OnConfigureRateClick(object sender, RateSelectedEventArgs e)
        {
            BestRateShipmentType.ApplySelectedShipmentRate(LoadedShipments[0], e.Rate);

            // Don't raise event if it was just the 'More...' link:
            BestRateResultTag bestRateResultTag = e.Rate.Tag as BestRateResultTag;
            if (bestRateResultTag != null && !bestRateResultTag.IsRealRate)
            {
                return;
            }

            RaiseShipmentTypeChanged();
        }

        /// <summary>
        /// Event raised when Rate is selected in rate control.
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            // Due to some weird behavior with the show more link and the previous 
            // selection not being cleared out when a new row was selected in the grid, 
            // we need to compare the currently selected rate with what the service 
            // control thinks is the selected rate. If they're different we have to 
            // explicitly call the SelectRate method on the rate control again. This will
            // fire another RateSelected event, but this time the e.Rate and the 
            // cachedRate will be the same.
            if (e.Rate != cachedRate)
            {
                cachedRate = e.Rate;
                RateControl.SelectRate(e.Rate);
            }
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (LoadedShipments.Count > 1 || serviceLevel.MultiValued)
            {
                RateControl.ClearSelection();
                cachedRate = null;
            }
            else
            {
                // Always select the first rate since all the rates already take the service 
                // level and the other fields into account when compiling the list of rates
                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault();
                
                cachedRate = matchingRate;
                RateControl.SelectRate(matchingRate);
            }
        }
    }
}

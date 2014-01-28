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
        private readonly BestRateShipmentType bestRateShipment;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateServiceControl(ShipmentTypeCode shipmentTypeCode)
            : base (shipmentTypeCode)
        {
            InitializeComponent();
            
            bestRateShipment = new BestRateShipmentType();
            bestRateShipment.SignUpForProviderAccountCompleted += OnAccountSignUp;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize()
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
        /// Event raised when Rate is selected in rate control.
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            bestRateShipment.ApplySelectedShipmentRate(LoadedShipments[0], e.Rate);
            
            RaiseShipmentTypeChanged();
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
        /// Called when an account has been signed up via an entry in the rates grid.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAccountSignUp(object sender, EventArgs e)
        {
            // TODO: rateControl.ClearRates(string.Empty);
            RaiseRatesCleared();
        }
    }
}

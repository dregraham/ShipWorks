using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    /// <summary>
    /// UserControl for editing the Asendia service settings
    /// </summary>
    [KeyedComponent(typeof(ServiceControlBase), ShipmentTypeCode.Asendia)]
    public partial class AsendiaServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsendiaServiceControl"/> class.
        /// </summary>
        public AsendiaServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.Asendia, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the combo boxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.Asendia);

            dimensionsControl.Initialize();

            LoadAccounts();

            weight.ConfigureTelemetryEntityCounts = telemetryEvent =>
            {
                telemetryEvent.AddMetric(WeightControl.ShipmentQuantityTelemetryKey, LoadedShipments?.Count ?? 0);
                telemetryEvent.AddMetric(WeightControl.PackageQuantityTelemetryKey, 1);
            };

            cutoffDateDisplay.ShipmentType = ShipmentTypeCode;
        }

        /// <summary>
        /// Load the list of Asendia accounts
        /// </summary>
        public override void LoadAccounts()
        {
            asendiaAccount.SelectedIndexChanged -= OnChangeAccount;
            asendiaAccount.DisplayMember = "Key";
            asendiaAccount.ValueMember = "Value";

            if (AsendiaAccountManager.Accounts.Count > 0)
            {
                asendiaAccount.DataSource = AsendiaAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.AsendiaAccountID)).ToList();
                asendiaAccount.Enabled = true;
            }
            else
            {
                asendiaAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                asendiaAccount.Enabled = false;
            }

            asendiaAccount.SelectedIndexChanged += OnChangeAccount;
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendShipSenseFieldChangeEvent();

            RecipientDestinationChanged -= OnRecipientDestinationChanged;
            originControl.DestinationChanged -= OnOriginDestinationChanged;

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            RecipientDestinationChanged += OnRecipientDestinationChanged;
            originControl.DestinationChanged += OnOriginDestinationChanged;

            LoadShipmentDetails();
            UpdateInsuranceDisplay();
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
            UpdateServiceTypes(LoadedShipments);
            
            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    asendiaAccount.ApplyMultiValue(shipment.Asendia.AsendiaAccountID);

                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    // Sets service type only if it is available
                    AsendiaServiceType asendiaServiceType = shipment.Asendia.Service;
                    if (((IEnumerable<EnumEntry<AsendiaServiceType>>) service.DataSource).Any(x => x.Value == asendiaServiceType))
                    {
                        service.ApplyMultiValue(asendiaServiceType);
                    }

                    nonMachinable.ApplyMultiCheck(shipment.Asendia.NonMachinable);
                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    dimensions.Add(new DimensionsAdapter(shipment.Asendia));
                }
            }

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
                Dictionary<int, string> availableServices = lifetimeScope
                    .ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.Asendia)
                    .BuildServiceTypeDictionary(shipments);

                service.BindToEnumAndPreserveSelection<AsendiaServiceType>(x => availableServices.ContainsKey((int) x));
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (asendiaAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                AsendiaAccountEntity account = asendiaAccount.SelectedIndex >= 0 ?
                    AsendiaAccountManager.GetAccount((long) asendiaAccount.SelectedValue) :
                    null;

                if (account != null)
                {
                    text += account.Description;
                }
                else
                {
                    text += "(None)";
                }
            }

            sectionFrom.ExtraText = text + ", " + originControl.OriginDescription;
        }

        /// <summary>
        /// The selected Asendia account has changed
        /// </summary>
        private void OnChangeAccount(object sender, EventArgs e)
        {
            if (asendiaAccount.SelectedValue != null)
            {
                long accountID = (long) asendiaAccount.SelectedValue;
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.Asendia.AsendiaAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        [NDependIgnoreLongMethod]
        public override void SaveToShipments()
        {
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
                asendiaAccount.ReadMultiValue(v => shipment.Asendia.AsendiaAccountID = (long) v);

                service.ReadMultiValue(v => shipment.Asendia.Service = (AsendiaServiceType) v);
                nonMachinable.ReadMultiCheck(c => shipment.Asendia.NonMachinable = c);
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);

                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
            }

            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Update the insurance rate display
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
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
        /// Some aspect of the shipment that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
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
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public override void FlushChanges()
        {
            base.FlushChanges();

            dimensionsControl.FlushChanges();
            weight.FlushChanges();
        }

        /// <summary>
        /// Should the specified label format be included in the list of available formats
        /// </summary>
        protected override bool ShouldIncludeLabelFormatInList(ThermalLanguage format)
        {
            return format != ThermalLanguage.EPL;
        }
    }
}
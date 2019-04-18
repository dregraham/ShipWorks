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
using ShipWorks.Shipping.Carriers.Amazon.SWA;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SWA
{
    /// <summary>
    /// UserControl for editing the AmazonSWA service settings
    /// </summary>
    [KeyedComponent(typeof(ServiceControlBase), ShipmentTypeCode.AmazonSWA)]
    public partial class AmazonSWAServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSWAServiceControl"/> class.
        /// </summary>
        public AmazonSWAServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.AmazonSWA, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the combo boxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.AmazonSWA);

            dimensionsControl.Initialize();

            LoadAccounts();

            dimensionsControl.DimensionsChanged += (s, a) => RaiseShipSenseFieldChanged();
            weight.WeightChanged += (s, a) => RaiseShipSenseFieldChanged();
            ShipSenseFieldChanged += (s, a) => SaveToShipments();

            weight.ConfigureTelemetryEntityCounts = telemetryEvent =>
            {
                telemetryEvent.AddMetric(WeightControl.ShipmentQuantityTelemetryKey, LoadedShipments?.Count ?? 0);
                telemetryEvent.AddMetric(WeightControl.PackageQuantityTelemetryKey, 1);
            };

            cutoffDateDisplay.ShipmentType = ShipmentTypeCode;
        }

        /// <summary>
        /// A rate has been selected
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            AmazonSWAServiceType selectedValue = EnumHelper.GetEnumList<AmazonSWAServiceType>()
                .FirstOrDefault(f => f.ApiValue == e.Rate.Days).Value;

            service.SelectedValue = selectedValue;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// Load the list of AmazonSWA accounts
        /// </summary>
        public override void LoadAccounts()
        {
            AmazonSWAAccount.SelectedIndexChanged -= OnChangeAccount;
            AmazonSWAAccount.DisplayMember = "Key";
            AmazonSWAAccount.ValueMember = "Value";

            if (AmazonSWAAccountManager.Accounts.Count > 0)
            {
                AmazonSWAAccount.DataSource = AmazonSWAAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.AmazonSWAAccountID)).ToList();
                AmazonSWAAccount.Enabled = true;
            }
            else
            {
                AmazonSWAAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                AmazonSWAAccount.Enabled = false;
            }

            AmazonSWAAccount.SelectedIndexChanged += OnChangeAccount;
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
                    AmazonSWAAccount.ApplyMultiValue(shipment.AmazonSWA.AmazonSWAAccountID);

                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    // Sets service type only if it is available
                    AmazonSWAServiceType AmazonSWAServiceType = (AmazonSWAServiceType) shipment.AmazonSWA.Service;
                    if (((IEnumerable<EnumEntry<AmazonSWAServiceType>>) service.DataSource).Any(x => x.Value == AmazonSWAServiceType))
                    {
                        service.ApplyMultiValue(AmazonSWAServiceType);
                    }

                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    dimensions.Add(new DimensionsAdapter(shipment.AmazonSWA));
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
                    .ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.AmazonSWA)
                    .BuildServiceTypeDictionary(shipments);

                service.BindToEnumAndPreserveSelection<AmazonSWAServiceType>(x => availableServices.ContainsKey((int) x));
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (AmazonSWAAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                AmazonSWAAccountEntity account = AmazonSWAAccount.SelectedIndex >= 0 ?
                    AmazonSWAAccountManager.GetAccount((long) AmazonSWAAccount.SelectedValue) :
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
        /// The selected AmazonSWA account has changed
        /// </summary>
        private void OnChangeAccount(object sender, EventArgs e)
        {
            if (AmazonSWAAccount.SelectedValue != null)
            {
                long accountID = (long) AmazonSWAAccount.SelectedValue;
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.AmazonSWA.AmazonSWAAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// Save the values in the control to the specified entities
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();

            // Save the origin
            originControl.SaveToEntities();

            //Save dimensions
            dimensionsControl.SaveToEntities();

            // Save the other fields
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                AmazonSWAAccount.ReadMultiValue(v => shipment.AmazonSWA.AmazonSWAAccountID = (long) v);

                service.ReadMultiValue(v => shipment.AmazonSWA.Service =  (int) v);
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);

                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
            }

            ResumeShipSenseFieldChangeEvent();
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
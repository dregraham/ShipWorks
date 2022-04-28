using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// UserControl for editing the DHL eCommerce service settings
    /// </summary>
    [KeyedComponent(typeof(ServiceControlBase), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceServiceControl : ServiceControlBase
    {
        private readonly IDhlEcommerceAccountRepository accountRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceServiceControl(RateControl rateControl, IDhlEcommerceAccountRepository accountRepo)
            : base(ShipmentTypeCode.DhlEcommerce, rateControl)
        {
            InitializeComponent();
            this.accountRepo = accountRepo;
        }

        /// <summary>
        /// Initialize the combo boxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.DhlEcommerce);

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
        /// Load the list of DHL eCommerce accounts
        /// </summary>
        public override void LoadAccounts()
        {
            dhlEcommerceAccount.SelectedIndexChanged -= OnChangeAccount;
            dhlEcommerceAccount.DisplayMember = "Key";
            dhlEcommerceAccount.ValueMember = "Value";

            if (accountRepo.Accounts.Count() > 0)
            {
                dhlEcommerceAccount.DataSource = accountRepo.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.DhlEcommerceAccountID)).ToList();
                dhlEcommerceAccount.Enabled = true;
            }
            else
            {
                dhlEcommerceAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                dhlEcommerceAccount.Enabled = false;
            }

            dhlEcommerceAccount.SelectedIndexChanged += OnChangeAccount;
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

            packageType.BindToEnumAndPreserveSelection<DhlEcommercePackagingType>(x => true);

            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    dhlEcommerceAccount.ApplyMultiValue(shipment.DhlEcommerce.DhlEcommerceAccountID);

                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    // Sets service type only if it is available
                    var serviceType = (DhlEcommerceServiceType) shipment.DhlEcommerce.Service;
                    if (((IEnumerable<EnumEntry<DhlEcommerceServiceType>>) service.DataSource).Any(x => x.Value == serviceType))
                    {
                        service.ApplyMultiValue(serviceType);
                    }

                    packageType.ApplyMultiValue((DhlEcommercePackagingType) shipment.DhlEcommerce.PackagingType);
                    saturdayDelivery.ApplyMultiCheck(shipment.DhlEcommerce.SaturdayDelivery);
                    deliveryDutyPaid.ApplyMultiCheck(shipment.DhlEcommerce.DeliveredDutyPaid);
                    nonMachinable.ApplyMultiCheck(shipment.DhlEcommerce.NonMachinable);
                    residentialDelivery.ApplyMultiCheck(shipment.DhlEcommerce.ResidentialDelivery);
                    reference1.ApplyMultiText(shipment.DhlEcommerce.Reference1);
                    shipDate.ApplyMultiDate(shipment.ShipDate.ToLocalTime());
                    dimensions.Add(new DimensionsAdapter(shipment.DhlEcommerce));
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
                    .ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.DhlEcommerce)
                    .BuildServiceTypeDictionary(shipments);

                service.BindToEnumAndPreserveSelection<DhlEcommerceServiceType>(x => availableServices.ContainsKey((int) x));
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (dhlEcommerceAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                var account = dhlEcommerceAccount.SelectedIndex >= 0 ?
                    accountRepo.GetAccount((long) dhlEcommerceAccount.SelectedValue) :
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
        /// The selected DHL eCommerce account has changed
        /// </summary>
        private void OnChangeAccount(object sender, EventArgs e)
        {
            if (dhlEcommerceAccount.SelectedValue != null)
            {
                long accountID = (long) dhlEcommerceAccount.SelectedValue;
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.DhlEcommerce.DhlEcommerceAccountID = accountID;
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

            //Save insurance info
            insuranceControl.SaveToInsuranceChoices();

            //Save dimensions
            dimensionsControl.SaveToEntities();

            // Save the other fields
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                dhlEcommerceAccount.ReadMultiValue(v => shipment.DhlEcommerce.DhlEcommerceAccountID = (long) v);

                service.ReadMultiValue(v => shipment.DhlEcommerce.Service = (int) v);
                packageType.ReadMultiValue(v => shipment.DhlEcommerce.PackagingType = (int) v);
                saturdayDelivery.ReadMultiCheck(c => shipment.DhlEcommerce.SaturdayDelivery = c);
                deliveryDutyPaid.ReadMultiCheck(c => shipment.DhlEcommerce.DeliveredDutyPaid = c);
                nonMachinable.ReadMultiCheck(c => shipment.DhlEcommerce.NonMachinable = c);
                residentialDelivery.ReadMultiCheck(c => shipment.DhlEcommerce.ResidentialDelivery = c);
                reference1.ReadMultiText(t => shipment.DhlEcommerce.Reference1 = t);
                shipDate.ReadMultiDate(v => shipment.ShipDate = v.Date.ToUniversalTime());

                weight.ReadMultiWeight(v => shipment.ContentWeight = v);

                shipment.ResidentialDetermination = (int) (residentialDelivery.Checked ? ResidentialDeterminationType.Residential : ResidentialDeterminationType.CommercialIfCompany);
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
            return format != ThermalLanguage.EPL && format != ThermalLanguage.ZPL;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.ApplicationCore;
using Autofac;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// UserControl for editing the OnTrac service settings
    /// </summary>
    public partial class OnTracServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public OnTracServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.OnTrac, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the combo boxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.OnTrac);

            dimensionsControl.Initialize();

            EnumHelper.BindComboBox<OnTracCodType>(codPaymentType);
            EnumHelper.BindComboBox<OnTracPackagingType>(packagingType);

            LoadAccounts();
        }

        /// <summary>
        /// Load the list of OnTrac accounts
        /// </summary>
        public override void LoadAccounts()
        {
            onTracAccount.SelectedIndexChanged -= OnChangeAccount;
            onTracAccount.DisplayMember = "Key";
            onTracAccount.ValueMember = "Value";

            if (OnTracAccountManager.Accounts.Count > 0)
            {
                onTracAccount.DataSource = OnTracAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.OnTracAccountID)).ToList();
                onTracAccount.Enabled = true;
            }
            else
            {
                onTracAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                onTracAccount.Enabled = false;
            }

            onTracAccount.SelectedIndexChanged += OnChangeAccount;
        }

        /// <summary>
        /// Load the shipment data into the ui
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

            // Update the service types
            service.SelectedValueChanged -= OnServiceChanged;
            UpdateServiceTypes(LoadedShipments);
            UpdatePackageTypes(LoadedShipments);

            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    onTracAccount.ApplyMultiValue(shipment.OnTrac.OnTracAccountID);

                    weight.ApplyMultiWeight(shipment.ContentWeight);

                    packagingType.ApplyMultiValue((OnTracPackagingType) shipment.OnTrac.PackagingType);

                    codEnabled.ApplyMultiCheck(shipment.OnTrac.IsCod);
                    codAmount.ApplyMultiAmount(shipment.OnTrac.CodAmount);
                    codPaymentType.ApplyMultiValue((OnTracCodType) shipment.OnTrac.CodType);

                    referenceNumber.ApplyMultiText(shipment.OnTrac.Reference1);
                    referenceNumber2.ApplyMultiText(shipment.OnTrac.Reference2);
                    instructions.ApplyMultiText(shipment.OnTrac.Instructions);

                    // Sets service type only if it is available
                    var onTracServiceType = (OnTracServiceType) shipment.OnTrac.Service;
                    if (((EnumList<OnTracServiceType>) service.DataSource).Any(x => x.Value == onTracServiceType))
                    {
                        service.ApplyMultiValue(onTracServiceType);
                    }

                    shipDate.ApplyMultiDate(shipment.ShipDate);
                    saturdayDelivery.ApplyMultiCheck(shipment.OnTrac.SaturdayDelivery);
                    signatureRequired.ApplyMultiCheck(shipment.OnTrac.SignatureRequired);

                    dimensions.Add(new DimensionsAdapter(shipment.OnTrac));
                }
            }

            service.SelectedValueChanged += OnServiceChanged;

            //Load the dimensions
            dimensionsControl.LoadDimensions(dimensions);
        }

        /// <summary>
        /// Update the available choices for packages
        /// </summary>
        private void UpdatePackageTypes(IEnumerable<ShipmentEntity> shipments)
        {
            Dictionary<int, string> availablePackages = (new OnTracShipmentType()).BuildPackageTypeDictionary(shipments.ToList());

            packagingType.BindToEnumAndPreserveSelection<OnTracPackagingType>(p => availablePackages.ContainsKey((int) p));
        }

        /// <summary>
        /// Update the available choices for services
        /// </summary>
        private void UpdateServiceTypes(List<ShipmentEntity> shipments)
        {
            // Are there any international shipments? - This is outside of the next loop because allInternational is needed for UpdateServiceTypes and service
            // type needs to be updated for the loop to loop correctly
            bool anyDomestic = shipments.Any(shipment => Geography.GetCountryCode(shipment.ShipPerson.CountryCode) == "US");

            if (anyDomestic)
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    Dictionary<int, string> availableServices = lifetimeScope.ResolveKeyed<IShipmentServicesBuilder>(ShipmentTypeCode.OnTrac)
                        .BuildServiceTypeDictionary(shipments);

                    service.BindToEnumAndPreserveSelection<OnTracServiceType>(x => availableServices.ContainsKey((int)x));
                }
            }
            else
            {
                EnumHelper.BindComboBox<OnTracServiceType>(service, serviceType => serviceType == OnTracServiceType.None);
            }

            // Disable it if its "None"
            service.Enabled = anyDomestic;
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (onTracAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                OnTracAccountEntity account = onTracAccount.SelectedIndex >= 0 ?
                    OnTracAccountManager.GetAccount((long) onTracAccount.SelectedValue) :
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
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// The selected OnTrac account has changed
        /// </summary>
        private void OnChangeAccount(object sender, EventArgs e)
        {
            if (onTracAccount.SelectedValue != null)
            {
                long accountID = (long) onTracAccount.SelectedValue;
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.OnTrac.OnTracAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
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

            //Save insurance info
            insuranceControl.SaveToInsuranceChoices();

            //Save dimensions
            dimensionsControl.SaveToEntities();

            // Save the other fields
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                onTracAccount.ReadMultiValue(v => shipment.OnTrac.OnTracAccountID = (long) v);

                codAmount.ReadMultiAmount(v => shipment.OnTrac.CodAmount = v);
                codEnabled.ReadMultiCheck(v => shipment.OnTrac.IsCod = v);
                codPaymentType.ReadMultiValue(v => shipment.OnTrac.CodType = (int) v);

                referenceNumber.ReadMultiText(v => shipment.OnTrac.Reference1 = v);
                referenceNumber2.ReadMultiText(v => shipment.OnTrac.Reference2 = v);
                instructions.ReadMultiText(v => shipment.OnTrac.Instructions = v);

                service.ReadMultiValue(v => shipment.OnTrac.Service = (int) v);
                shipDate.ReadMultiDate(v => shipment.ShipDate = v);
                saturdayDelivery.ReadMultiCheck(v => shipment.OnTrac.SaturdayDelivery = v);
                signatureRequired.ReadMultiCheck(v => shipment.OnTrac.SignatureRequired = v);

                weight.ReadMultiWeight(v => shipment.ContentWeight = v);
                packagingType.ReadMultiValue(v => shipment.OnTrac.PackagingType = (int) v);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// A rate has been selected.
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;
            OnTracServiceType servicetype = (OnTracServiceType)e.Rate.OriginalTag;

            service.SelectedValue = servicetype;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// Update the insurance rate display
        /// </summary>
        public override void UpdateInsuranceDisplay()
        {
            insuranceControl.InsuranceOptionsChanged -= OnRateCriteriaChanged;
            insuranceControl.LoadInsuranceChoices(LoadedShipments.Select(shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));

            insuranceControl.InsuranceOptionsChanged += OnRateCriteriaChanged;
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
        /// Called when the service type has changed.
        /// </summary>
        private void OnServiceChanged(object sender, EventArgs e)
        {
            SyncSelectedRate();
        }

        /// <summary>
        /// Synchronizes the selected rate in the rate control.
        /// </summary>
        public override void SyncSelectedRate()
        {
            if (!service.MultiValued && service.SelectedValue != null)
            {
                // Update the selected rate in the rate control to coincide with the service change
                OnTracServiceType serviceType = (OnTracServiceType)service.SelectedValue;
                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    if (r.Tag == null || r.ShipmentType != ShipmentTypeCode.OnTrac)
                    {
                        return false;
                    }

                    return (OnTracServiceType)r.OriginalTag == serviceType;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
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
    }
}
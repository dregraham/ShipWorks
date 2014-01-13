﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// UserControl for editing the OnTrac service settings
    /// </summary>
    public partial class OnTracServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracServiceControl()
            : base(ShipmentTypeCode.OnTrac)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialie the comboboxes
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.OnTrac);

            dimensionsControl.Initialize();

            EnumHelper.BindComboBox<OnTracCodType>(codPaymentType);
            EnumHelper.BindComboBox<OnTracPackagingType>(packagingType);

            LoadOnTracAccounts();
        }

        /// <summary>
        /// Load the list of OnTrac accounts
        /// </summary>
        private void LoadOnTracAccounts()
        {
            onTracAccount.SelectedIndexChanged -= OnChangeAccount;
            onTracAccount.DisplayMember = "Key";
            onTracAccount.ValueMember = "Value";

            if (OnTracAccountManager.Accounts.Count > 0)
            {
                onTracAccount.DataSource =
                    OnTracAccountManager.Accounts.Select(
                        s => new KeyValuePair<string, long>(s.Description, s.OnTracAccountID))
                                        .ToList();
                onTracAccount.Enabled = true;
            }
            else
            {
                onTracAccount.DataSource = new List<KeyValuePair<string, long>>
                                           {
                                               new KeyValuePair<string, long>("(No accounts)", 0)
                                           };
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

            base.RecipientDestinationChanged -= OnRecipientDestinationChanged;

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);
            
            base.RecipientDestinationChanged += OnRecipientDestinationChanged;

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

            // Are there any international shipments? - This is outside of the next loop because allInternational is needed for UpdateServiceTypes and service
            // type needs to be updated for the loop to loop correctly
            bool allInternational = LoadedShipments.Any(shipment => !PostalUtility.IsDomesticCountry(shipment.ShipCountryCode));

            // Update the service types
            UpdateServiceTypes(!allInternational);

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

                    // Sets service type only if it is avalable
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
            
            //Load the dimensions
            dimensionsControl.LoadDimensions(dimensions);
        }

        /// <summary>
        /// Update the available choices for services
        /// </summary>
        /// <param name="anyDomestic">
        /// If any Domestic, enable service. If the previous value was set and is currently available,
        /// set it.
        /// </param>
        private void UpdateServiceTypes(bool anyDomestic)
        {
            bool previousMulti = service.MultiValued;
            object previousValue = service.SelectedValue;

            if (anyDomestic)
            {
                EnumHelper.BindComboBox<OnTracServiceType>(service, serviceType => serviceType != OnTracServiceType.None);

                // Set back the previous value
                if (previousMulti)
                {
                    service.MultiValued = true;
                }
                else if (previousValue != null)
                {
                    service.SelectedValue = previousValue;

                    if (service.SelectedIndex == -1)
                    {
                        service.SelectedIndex = 0;
                    }
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
                OnTracAccountEntity account = onTracAccount.SelectedIndex >= 0
                    ? OnTracAccountManager.GetAccount((long) onTracAccount.SelectedValue)
                    : null;

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
        }

        /// <summary>
        /// A rate has been selected.
        /// </summary>
        private void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;

            OnTracServiceType servicetype = (OnTracServiceType)e.Rate.Tag;

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
            insuranceControl.LoadInsuranceChoices(
                LoadedShipments.Select(
                    shipment => ShipmentTypeManager.GetType(shipment).GetParcelDetail(shipment, 0).Insurance));
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
    }
}
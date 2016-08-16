using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// Service Control for iParcel
    /// </summary>
    public partial class iParcelServiceControl : ServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelServiceControl" /> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public iParcelServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.iParcel, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the combo boxes
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.iParcel);

            LoadAccounts();

            packageControl.Initialize();
        }

        /// <summary>
        /// Load the list of IParcel accounts
        /// </summary>
        public override void LoadAccounts()
        {
            iParcelAccount.DisplayMember = "Key";
            iParcelAccount.ValueMember = "Value";

            if (iParcelAccountManager.Accounts.Count > 0)
            {
                iParcelAccount.DataSource = iParcelAccountManager.Accounts.Select(s => new KeyValuePair<string, long>(s.Description, s.IParcelAccountID)).ToList();
                iParcelAccount.Enabled = true;
            }
            else
            {
                iParcelAccount.DataSource = new List<KeyValuePair<string, long>>
                {
                    new KeyValuePair<string, long>("(No accounts)", 0)
                };

                iParcelAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load the data for the list of shipments into the control
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            // The base will disable if editing is not enabled, but due to the packaging selection, we need to customize how it works
            sectionShipment.ContentPanel.Enabled = true;

            // Manually disable all shipment panel controls, except the packaging control.  They still need to be able to switch packages
            // even after processing
            foreach (Control control in sectionShipment.ContentPanel.Controls)
            {
                if (control == packageControl)
                {
                    continue;
                }

                control.Enabled = enableEditing;
            }

            // Load the origin
            originControl.LoadShipments(shipments);

            iParcelAccount.SelectedIndexChanged -= OnChangeIParcelAccount;

            // Load the shipment details
            LoadShipmentDetails();

            iParcelAccount.SelectedIndexChanged += OnChangeIParcelAccount;

            // Load the package information
            packageControl.LoadShipments(LoadedShipments, enableEditing);

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Should the specified label format be included in the list of available formats
        /// </summary>
        protected override bool ShouldIncludeLabelFormatInList(ThermalLanguage format)
        {
            return format != ThermalLanguage.ZPL;
        }

        /// <summary>
        /// Loads the shipment details.
        /// </summary>
        private void LoadShipmentDetails()
        {
            BindServiceDropdown();

            // Determine if all shipments will have the same destination service types
            using (new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    iParcelAccount.ApplyMultiValue(shipment.IParcel.IParcelAccountID);
                    service.ApplyMultiValue((iParcelServiceType) shipment.IParcel.Service);

                    referenceCustomer.ApplyMultiText(shipment.IParcel.Reference);

                    emailTrack.ApplyMultiCheck(shipment.IParcel.TrackByEmail);

                    isDeliveryDutyPaid.ApplyMultiCheck(shipment.IParcel.IsDeliveryDutyPaid);
                }
            }
        }

        /// <summary>
        /// Binds the service drop down list based on service types that have been excluded.
        /// </summary>
        private void BindServiceDropdown()
        {
            service.DisplayMember = "Key";
            service.ValueMember = "Value";

            iParcelShipmentType shipmentType = (iParcelShipmentType) ShipmentTypeManager.GetType(ShipmentTypeCode.iParcel);
            List<iParcelServiceType> availableServiceTypes = shipmentType.GetAvailableServiceTypes().Select(s => (iParcelServiceType) s).ToList();

            // Always include the service that the shipments are currently configured with
            IEnumerable<iParcelServiceType> loadedServices = LoadedShipments.Select(s => (iParcelServiceType) s.IParcel.Service).Distinct();
            availableServiceTypes = availableServiceTypes.Union(loadedServices).ToList();

            service.DataSource = ActiveEnumerationBindingSource.Create<iParcelServiceType>
                (
                    availableServiceTypes.Select(s => new KeyValuePair<string, iParcelServiceType>(EnumHelper.GetDescription(s), s)).ToList()
                );
        }

        /// <summary>
        /// The selected IParcel account has changed
        /// </summary>
        private void OnChangeIParcelAccount(object sender, EventArgs e)
        {
            if (iParcelAccount.SelectedValue != null)
            {
                long accountID = (long) iParcelAccount.SelectedValue;
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    shipment.IParcel.IParcelAccountID = accountID;
                }

                originControl.NotifySelectedAccountChanged();
            }
        }

        /// <summary>
        /// Save the current content of the service control to the given shipments.
        /// </summary>
        public override void SaveToShipments()
        {
            SuspendRateCriteriaChangeEvent();
            SuspendShipSenseFieldChangeEvent();

            base.SaveToShipments();
            originControl.SaveToEntities();

            // Save the packages
            packageControl.SaveToEntities();

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                shipment.ContentWeight = shipment.IParcel.Packages.Sum(p => p.Weight);
                iParcelAccount.ReadMultiValue(v => shipment.IParcel.IParcelAccountID = (long) v);

                service.ReadMultiValue(v =>
                {
                    if (v != null)
                    {
                        shipment.IParcel.Service = (int) v;
                    }
                });

                referenceCustomer.ReadMultiText(t => shipment.IParcel.Reference = t);
                emailTrack.ReadMultiCheck(c => shipment.IParcel.TrackByEmail = c);
                isDeliveryDutyPaid.ReadMultiCheck(c => shipment.IParcel.IsDeliveryDutyPaid = c);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Refresh the weight box with the latest weight information from the loaded shipments
        /// </summary>
        public override void RefreshContentWeight()
        {
            // We need to save the package stuff so we know what weights we are dealing with
            packageControl.SaveToEntities();
            bool changes = false;

            // Go through each shipment
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                changes |= iParcelShipmentType.RedistributeContentWeight(shipment);
            }

            // If there were changes we have to reload the package ui
            if (changes)
            {
                packageControl.LoadShipments(LoadedShipments, base.EnableEditing);
            }
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (iParcelAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                IParcelAccountEntity account = iParcelAccount.SelectedIndex >= 0 ? iParcelAccountManager.GetAccount((long) iParcelAccount.SelectedValue) : null;
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
            OnRateCriteriaChanged(sender, e);
        }

        /// <summary>
        /// One of the values that affects rates has changed
        /// </summary>
        private void OnRateCriteriaChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// A rate has been selected
        /// </summary>
        public override void OnRateSelected(object sender, RateSelectedEventArgs e)
        {
            int oldIndex = service.SelectedIndex;
            iParcelRateSelection rate = e.Rate.OriginalTag as iParcelRateSelection;

            service.SelectedValue = rate.ServiceType;
            if (service.SelectedIndex == -1 && oldIndex != -1)
            {
                service.SelectedIndex = oldIndex;
            }
        }

        /// <summary>
        /// One of the values that affects ShipSense has changed
        /// </summary>
        private void OnShipSenseFieldChanged(object sender, EventArgs e)
        {
            RaiseShipSenseFieldChanged();
        }

        /// <summary>
        /// Called when [package control resize].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnPackageControlSizeChanged(object sender, EventArgs e)
        {
            sectionShipment.Height = (sectionShipment.Height - sectionShipment.ContentPanel.Height) + packageControl.Bottom + 4;
        }

        /// <summary>
        /// Called when the selected service is changed.
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
            if (!service.MultiValued)
            {
                iParcelServiceType serviceType = (iParcelServiceType) service.SelectedValue;

                // Update the selected rate in the rate control to coincide with the service change
                RateResult matchingRate = RateControl.RateGroup.Rates.FirstOrDefault(r =>
                {
                    if (r.Tag == null || r.ShipmentType != ShipmentTypeCode.iParcel)
                    {
                        return false;
                    }

                    return ((iParcelRateSelection) r.OriginalTag).ServiceType == serviceType;
                });

                RateControl.SelectRate(matchingRate);
            }
            else
            {
                RateControl.ClearSelection();
            }
        }

        /// <summary>
        /// Flush any in-progress changes before saving
        /// </summary>
        /// <remarks>This should cause weight controls to finish, etc.</remarks>
        public override void FlushChanges()
        {
            base.FlushChanges();

            packageControl?.FlushChanges();
        }
    }
}

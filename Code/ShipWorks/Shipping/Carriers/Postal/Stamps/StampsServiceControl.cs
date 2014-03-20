﻿using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Service control for Stamps.com settings
    /// </summary>
    public partial class StampsServiceControl : PostalServiceControlBase
    {
        readonly bool isExpress1;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public StampsServiceControl(RateControl rateControl)
            : this(ShipmentTypeCode.Stamps, false, rateControl) 
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        protected StampsServiceControl(ShipmentTypeCode shipmentTypeCode, bool isExpress1, RateControl rateControl)
            : base(shipmentTypeCode, rateControl)
        {
            this.isExpress1 = isExpress1;

            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode);

            linkManageStampsAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            LoadAccounts();
        }

        /// <summary>
        /// Load the list of stamps.com accounts
        /// </summary>
        public override void LoadAccounts()
        {
            stampsAccount.DisplayMember = "Key";
            stampsAccount.ValueMember = "Value";

            var accounts = StampsAccountManager.GetAccounts(isExpress1, false);

            if (accounts.Count > 0)
            {
                stampsAccount.DataSource = accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.StampsAccountID)).ToList();
                stampsAccount.Enabled = true;
            }
            else
            {
                stampsAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                stampsAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Load the shipment data into the ui
        /// </summary>
        public override void LoadShipments(IEnumerable<ShipmentEntity> shipments, bool enableEditing, bool enableShippingAddress)
        {

            if (shipments == null)
            {
                throw new ArgumentNullException("shipments");
            }

            SuspendRateCriteriaChangeEvent();

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            // Load the origin
            originControl.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    stampsAccount.ApplyMultiValue(shipment.Postal.Stamps.StampsAccountID);
                    requireFullAddressValidation.ApplyMultiCheck(shipment.Postal.Stamps.RequireFullAddressValidation);
                    hidePostage.ApplyMultiCheck(shipment.Postal.Stamps.HidePostage);
                    memo.ApplyMultiText(shipment.Postal.Stamps.Memo);
                }
            }
            ResumeRateCriteriaChangeEvent();
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

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                stampsAccount.ReadMultiValue(v => shipment.Postal.Stamps.StampsAccountID = (long) v);
                requireFullAddressValidation.ReadMultiCheck(c => shipment.Postal.Stamps.RequireFullAddressValidation = c);
                hidePostage.ReadMultiCheck(c => shipment.Postal.Stamps.HidePostage = c);
                memo.ReadMultiText(t => shipment.Postal.Stamps.Memo = t);
            }

            ResumeRateCriteriaChangeEvent();
        }

        /// <summary>
        /// Selected origin has changed
        /// </summary>
        private void OnOriginChanged(object sender, EventArgs e)
        {
            string text = "Account: ";

            if (stampsAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                StampsAccountEntity account = stampsAccount.SelectedIndex >= 0 ? StampsAccountManager.GetAccount((long) stampsAccount.SelectedValue) : null;
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
        /// Open the window for managing the available stamps.com accounts
        /// </summary>
        private void OnManageStampsAccounts(object sender, EventArgs e)
        {
            using (StampsAccountManagerDlg dlg = new StampsAccountManagerDlg(isExpress1))
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = stampsAccount.MultiValued;
            long oldAccount = multiValue ? -1 : (long) stampsAccount.SelectedValue;

            stampsAccount.SelectedValueChanged -= this.OnOriginChanged;

            LoadAccounts();

            if (multiValue)
            {
                stampsAccount.MultiValued = true;
            }
            else
            {
                if (stampsAccount.SelectedValue == null || oldAccount != (long) stampsAccount.SelectedValue)
                {
                    stampsAccount.SelectedValue = oldAccount;
                }

                if (stampsAccount.SelectedValue == null)
                {
                    stampsAccount.SelectedIndex = 0;
                }
            }

            stampsAccount.SelectedValueChanged += this.OnOriginChanged;

            OnOriginChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the shipment options based on the configured shipment values
        /// </summary>
        /// <param name="postalPackagingType"></param>
        protected override void UpdateAvailableShipmentOptions(PostalPackagingType? postalPackagingType)
        {
            base.UpdateAvailableShipmentOptions(postalPackagingType);

            // Stamps API doesn't allow hidden postage on envelopes
            bool disableHiddenPostage = postalPackagingType == PostalPackagingType.Envelope;
            
            // Disable the hide postage option should for first class envelopes
            hidePostage.Enabled = !disableHiddenPostage;
        }
    }
}

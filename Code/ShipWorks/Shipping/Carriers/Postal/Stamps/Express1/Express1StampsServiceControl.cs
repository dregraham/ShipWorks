﻿using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Service control for Stamps.com settings
    /// </summary>
    public partial class Express1StampsServiceControl : PostalServiceControlBase
    {
        private const UspsResellerType StampsResellerType = UspsResellerType.Express1;

        /// <summary>
        /// Initializes a new <see cref="Express1StampsServiceControl"/> instance.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public Express1StampsServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.Express1Stamps, rateControl)
        {
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

            var accounts = UspsAccountManager.GetAccounts(StampsResellerType, false);

            if (accounts.Count > 0)
            {
                stampsAccount.DataSource = accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.UspsAccountID)).ToList();
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
            SuspendShipSenseFieldChangeEvent();

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            // Load the origin
            originControl.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    stampsAccount.ApplyMultiValue(shipment.Postal.Usps.UspsAccountID);
                    requireFullAddressValidation.ApplyMultiCheck(shipment.Postal.Usps.RequireFullAddressValidation);
                    memo.ApplyMultiText(shipment.Postal.Usps.Memo);
                }
            }
            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
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

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                stampsAccount.ReadMultiValue(v => shipment.Postal.Usps.UspsAccountID = (long) v);
                requireFullAddressValidation.ReadMultiCheck(c => shipment.Postal.Usps.RequireFullAddressValidation = c);
                memo.ReadMultiText(t => shipment.Postal.Usps.Memo = t);
            }

            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
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
                UspsAccountEntity account = stampsAccount.SelectedIndex >= 0 ? UspsAccountManager.GetAccount((long) stampsAccount.SelectedValue) : null;
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
            using (UspsAccountManagerDlg dlg = new UspsAccountManagerDlg(StampsResellerType))
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = stampsAccount.MultiValued;
            long oldAccount = multiValue ? -1 : (long) stampsAccount.SelectedValue;

            stampsAccount.SelectedValueChanged -= OnOriginChanged;

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

            stampsAccount.SelectedValueChanged += OnOriginChanged;

            OnOriginChanged(null, EventArgs.Empty);
        }
    }
}

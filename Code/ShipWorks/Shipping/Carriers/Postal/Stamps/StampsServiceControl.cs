using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using Interapptive.Shared.Utility;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Service control for Stamps.com settings
    /// </summary>
    public partial class StampsServiceControl : PostalServiceControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StampsServiceControl()
            : base(ShipmentTypeCode.Stamps)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.Stamps);

            
            linkManageStampsAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            LoadStampsAccounts();
        }

        /// <summary>
        /// Load the list of stamps.com accounts
        /// </summary>
        private void LoadStampsAccounts()
        {
            stampsAccount.DisplayMember = "Key";
            stampsAccount.ValueMember = "Value";

            if (StampsAccountManager.StampsAccounts.Count > 0)
            {
                stampsAccount.DataSource = StampsAccountManager.StampsAccounts.Select(a => new KeyValuePair<string, long>(a.Username, a.StampsAccountID)).ToList();
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

            base.LoadShipments(shipments, enableEditing, enableShippingAddress);

            // Load the origin
            originControl.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in shipments)
                {
                    stampsAccount.ApplyMultiValue(shipment.Postal.Stamps.StampsAccountID);
                    requireFullAddressValidation.ApplyMultiCheck(shipment.Postal.Stamps.RequireFullAddressValidation);
                    hidePostage.ApplyMultiCheck(shipment.Postal.Stamps.HidePostage);
                    memo.ApplyMultiText(shipment.Postal.Stamps.Memo);
                }
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
                    text += account.Username;
                }
                else
                {
                    text += "(None)";
                }
            }

            sectionFrom.ExtraText = text + ", " + originControl.OriginDescription;
        }

        /// <summary>
        /// Open the window for managing the available stamps.com accounts
        /// </summary>
        private void OnManageStampsAccounts(object sender, EventArgs e)
        {
            using (StampsAccountManagerDlg dlg = new StampsAccountManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = stampsAccount.MultiValued;
            long oldAccount = multiValue ? -1 : (long) stampsAccount.SelectedValue;

            stampsAccount.SelectedValueChanged -= this.OnOriginChanged;

            LoadStampsAccounts();

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

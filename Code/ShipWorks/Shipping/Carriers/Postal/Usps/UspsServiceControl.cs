using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Shipping.Editing;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Settings.Origin;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Service control for USPS settings
    /// </summary>
    public partial class UspsServiceControl : PostalServiceControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsServiceControl"/> class.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public UspsServiceControl(RateControl rateControl)
            : this(ShipmentTypeCode.Stamps, rateControl) 
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsServiceControl(ShipmentTypeCode shipmentTypeCode, RateControl rateControl)
            : base(shipmentTypeCode, rateControl)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the comboboxes
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            originControl.Initialize(ShipmentTypeCode.Usps);

            linkManageStampsAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            LoadAccounts();
        }

        /// <summary>
        /// Rate shopping selction has changed
        /// </summary>
        private void OnRateShopChanged(object sender, EventArgs e)
        {
            stampsAccount.SelectedValueChanged -= OnOriginChanged;
            SaveToShipments();
            
            LoadAccounts();

            using (MultiValueScope scope = new MultiValueScope())
            {
                LoadAccountValue(scope);
            }

            stampsAccount.SelectedValueChanged += OnOriginChanged;
            stampsAccount.Enabled = !rateShop.Checked && rateShop.CheckState != CheckState.Checked;
            
            UpdateFromSectionText();
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Load the list of USPS accounts
        /// </summary>
        public override void LoadAccounts()
        {
            stampsAccount.DisplayMember = "Key";
            stampsAccount.ValueMember = "Value";

            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(UspsResellerType.StampsExpedited, false);

            if (accounts.Count > 0)
            {
                List<KeyValuePair<string, long>> stampsAccounts = accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.UspsAccountID)).ToList();

                if (rateShop.CheckState == CheckState.Indeterminate || rateShop.CheckState == CheckState.Checked && accounts.Count > 1)
                {
                    stampsAccounts.Insert(0, new KeyValuePair<string, long>("Rate Shopping", 0));
                }

                stampsAccount.DataSource = stampsAccounts;
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
                    rateShop.ApplyMultiCheck(shipment.Postal.Usps.RateShop);
                    requireFullAddressValidation.ApplyMultiCheck(shipment.Postal.Usps.RequireFullAddressValidation);
                    hidePostage.ApplyMultiCheck(shipment.Postal.Usps.HidePostage);
                    memo.ApplyMultiText(shipment.Postal.Usps.Memo);
                    LoadAccountValue(scope);
                }
            }
            
            ResumeRateCriteriaChangeEvent();
            ResumeShipSenseFieldChangeEvent();
        }

        /// <summary>
        /// Loads the account value from the loaded shipments.
        /// </summary>
        /// <param name="scope">A MultiValueScope is required since the account value is being applied to a MultiValueComboBox.</param>
        private void LoadAccountValue(MultiValueScope scope)
        {
            if (scope == null)
            {
                throw new InvalidOperationException("MultiValueScope cannot be null.");
            }

            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                if (!shipment.Processed && shipment.Postal.Usps.RateShop)
                {
                    stampsAccount.ApplyMultiValue(0);
                }
                else
                {
                    stampsAccount.ApplyMultiValue(shipment.Postal.Usps.UspsAccountID);
                }
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

            // Save the 
            foreach (ShipmentEntity shipment in LoadedShipments)
            {
                rateShop.ReadMultiCheck(c => shipment.Postal.Usps.RateShop = c);
                stampsAccount.ReadMultiValue(v => shipment.Postal.Usps.UspsAccountID = (long)v == 0 ? shipment.Postal.Usps.UspsAccountID : (long)v);
                requireFullAddressValidation.ReadMultiCheck(c => shipment.Postal.Usps.RequireFullAddressValidation = c);
                hidePostage.ReadMultiCheck(c => shipment.Postal.Usps.HidePostage = c);
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
            UpdateFromSectionText();

            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Update the from section's text
        /// </summary>
        private void UpdateFromSectionText()
        {
            string text = "Account: ";

            if (stampsAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                UspsAccountEntity account = stampsAccount.SelectedIndex >= 0 ? UspsAccountManager.GetAccount((long)stampsAccount.SelectedValue) : null;
                if (account != null && ((stampsAccount.Enabled) || (rateShop.Checked && LoadedShipments.First().Processed)))
                {
                    text += account.Description;
                }
                else if (rateShop.Checked && !stampsAccount.Enabled)
                {
                    text += "(Rate Shopping)";
                }
                else
                {
                    text += "(None)";
                }
            }

            sectionFrom.ExtraText = text + ", " + originControl.OriginDescription;
        }

        /// <summary>
        /// Open the window for managing the available USPS accounts
        /// </summary>
        private void OnManageStampsAccounts(object sender, EventArgs e)
        {
            using (UspsAccountManagerDlg dlg = new UspsAccountManagerDlg(UspsResellerType.StampsExpedited))
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = stampsAccount.MultiValued;
            long oldAccount = multiValue ? -1 : (long)stampsAccount.SelectedValue;

            stampsAccount.SelectedValueChanged -= this.OnOriginChanged;

            LoadAccounts();

            if (multiValue)
            {
                stampsAccount.MultiValued = true;
            }
            else
            {
                if (stampsAccount.SelectedValue == null || oldAccount != (long)stampsAccount.SelectedValue)
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

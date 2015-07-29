using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Service control for USPS settings
    /// </summary>
    public partial class UspsServiceControl : PostalServiceControlBase
    {
        /// <summary>
        /// Constructor that is needed for the designer to work
        /// </summary>
        protected UspsServiceControl()
        {
            if (!DesignModeDetector.IsDesignerHosted())
            {
                throw new InvalidOperationException("The default constructor for UspsServiceControl should only be used by the VS designer");
            }

            InitializeComponent();
        }

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

            linkManageUspsAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            LoadAccounts();
        }

        /// <summary>
        /// Rate shopping selction has changed
        /// </summary>
        private void OnRateShopChanged(object sender, EventArgs e)
        {
            uspsAccount.SelectedValueChanged -= OnOriginChanged;
            SaveToShipments();
            
            LoadAccounts();

            using (MultiValueScope scope = new MultiValueScope())
            {
                LoadAccountValue(scope);
            }

            uspsAccount.SelectedValueChanged += OnOriginChanged;
            uspsAccount.Enabled = !rateShop.Checked && rateShop.CheckState != CheckState.Checked;
            
            UpdateFromSectionText();
            RaiseRateCriteriaChanged();
        }

        /// <summary>
        /// Load the list of USPS accounts
        /// </summary>
        public override void LoadAccounts()
        {
            uspsAccount.DisplayMember = "Key";
            uspsAccount.ValueMember = "Value";

            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(UspsResellerType.None, false);

            if (accounts.Count > 0)
            {
                List<KeyValuePair<string, long>> uspsAccounts = accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.UspsAccountID)).ToList();

                if (rateShop.CheckState == CheckState.Indeterminate || rateShop.CheckState == CheckState.Checked && accounts.Count > 1)
                {
                    uspsAccounts.Insert(0, new KeyValuePair<string, long>("Rate Shopping", 0));
                }

                uspsAccount.DataSource = uspsAccounts;
                uspsAccount.Enabled = true;
            }
            else
            {
                uspsAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                uspsAccount.Enabled = false;
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

            originControl.DestinationChanged -= OnOriginDestinationChanged;
            base.LoadShipments(shipments, enableEditing, enableShippingAddress);
            originControl.DestinationChanged += OnOriginDestinationChanged;

            // Load the origin
            originControl.LoadShipments(shipments);

            using (MultiValueScope scope = new MultiValueScope())
            {
                foreach (ShipmentEntity shipment in LoadedShipments)
                {
                    rateShop.ApplyMultiCheck(shipment.Postal.Usps.RateShop);
                    requireFullAddressValidation.ApplyMultiCheck(shipment.Postal.Usps.RequireFullAddressValidation);
                    hidePostage.ApplyMultiCheck(shipment.Postal.Usps.HidePostage);
                    memo1.ApplyMultiText(shipment.Postal.Memo1);
                    memo2.ApplyMultiText(shipment.Postal.Memo2);
                    memo3.ApplyMultiText(shipment.Postal.Memo3);
                    noPostage.ApplyMultiCheck(shipment.Postal.NoPostage);
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
                    uspsAccount.ApplyMultiValue(0);
                }
                else
                {
                    uspsAccount.ApplyMultiValue(shipment.Postal.Usps.UspsAccountID);
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
                uspsAccount.ReadMultiValue(v => shipment.Postal.Usps.UspsAccountID = (long)v == 0 ? shipment.Postal.Usps.UspsAccountID : (long)v);
                requireFullAddressValidation.ReadMultiCheck(c => shipment.Postal.Usps.RequireFullAddressValidation = c);
                hidePostage.ReadMultiCheck(c => shipment.Postal.Usps.HidePostage = c);
                memo1.ReadMultiText(t => shipment.Postal.Memo1 = t);
                memo2.ReadMultiText(t => shipment.Postal.Memo2 = t);
                memo3.ReadMultiText(t => shipment.Postal.Memo3 = t);
                noPostage.ReadMultiCheck(x => shipment.Postal.NoPostage = x);
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

            if (uspsAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                UspsAccountEntity account = uspsAccount.SelectedIndex >= 0 ? UspsAccountManager.GetAccount((long)uspsAccount.SelectedValue) : null;
                if (account != null && (uspsAccount.Enabled || LoadedShipments.First().Processed))
                {
                    text += account.Description;
                }
                else if (rateShop.Checked && !uspsAccount.Enabled)
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
        private void OnManageUspsAccounts(object sender, EventArgs e)
        {
            using (UspsAccountManagerDlg dlg = new UspsAccountManagerDlg(UspsResellerType.None))
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = uspsAccount.MultiValued;
            long oldAccount = multiValue ? -1 : (long)uspsAccount.SelectedValue;

            uspsAccount.SelectedValueChanged -= OnOriginChanged;

            LoadAccounts();

            if (multiValue)
            {
                uspsAccount.MultiValued = true;
            }
            else
            {
                if (uspsAccount.SelectedValue == null || oldAccount != (long)uspsAccount.SelectedValue)
                {
                    uspsAccount.SelectedValue = oldAccount;
                }

                if (uspsAccount.SelectedValue == null)
                {
                    uspsAccount.SelectedIndex = 0;
                }
            }

            uspsAccount.SelectedValueChanged += OnOriginChanged;

            OnOriginChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the shipment options based on the configured shipment values
        /// </summary>
        /// <param name="postalPackagingType"></param>
        protected override void UpdateAvailableShipmentOptions(PostalPackagingType? postalPackagingType)
        {
            base.UpdateAvailableShipmentOptions(postalPackagingType);

            // USPS API doesn't allow hidden postage on envelopes
            bool disableHiddenPostage = postalPackagingType == PostalPackagingType.Envelope;

            // Disable the hide postage option should for first class envelopes
            hidePostage.Enabled = !disableHiddenPostage;
        }

        /// <summary>
        /// Handle NoPostage checkbox changing
        /// </summary>
        private void OnNoPostageChanged(object sender, EventArgs e)
        {
            RaiseRateCriteriaChanged();
        }
    }
}

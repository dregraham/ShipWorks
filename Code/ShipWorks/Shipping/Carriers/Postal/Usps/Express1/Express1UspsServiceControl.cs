using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Service control for Expres1 Usps settings
    /// </summary>
    public partial class Express1UspsServiceControl : PostalServiceControlBase
    {
        private const UspsResellerType ResellerType = UspsResellerType.Express1;

        /// <summary>
        /// Initializes a new <see cref="Express1UspsServiceControl"/> instance.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        public Express1UspsServiceControl(RateControl rateControl)
            : base(ShipmentTypeCode.Express1Usps, rateControl)
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

            linkManageUspsAccounts.Visible = UserSession.Security.HasPermission(PermissionType.ShipmentsManageSettings);
            LoadAccounts();
        }

        /// <summary>
        /// Load the list of Express1 Usps accounts
        /// </summary>
        public override void LoadAccounts()
        {
            uspsAccount.DisplayMember = "Key";
            uspsAccount.ValueMember = "Value";

            var accounts = UspsAccountManager.GetAccounts(ResellerType, false);

            if (accounts.Count > 0)
            {
                uspsAccount.DataSource = accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.UspsAccountID)).ToList();
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
                    uspsAccount.ApplyMultiValue(shipment.Postal.Usps.UspsAccountID);
                    requireFullAddressValidation.ApplyMultiCheck(shipment.Postal.Usps.RequireFullAddressValidation);
                    memo1.ApplyMultiText(shipment.Postal.Memo1);
                    memo2.ApplyMultiText(shipment.Postal.Memo2);
                    memo3.ApplyMultiText(shipment.Postal.Memo3);
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
                uspsAccount.ReadMultiValue(v => shipment.Postal.Usps.UspsAccountID = (long) v);
                requireFullAddressValidation.ReadMultiCheck(c => shipment.Postal.Usps.RequireFullAddressValidation = c);
                memo1.ReadMultiText(t => shipment.Postal.Memo1 = t);
                memo2.ReadMultiText(t => shipment.Postal.Memo2 = t);
                memo3.ReadMultiText(t => shipment.Postal.Memo3 = t);
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

            if (uspsAccount.MultiValued)
            {
                text += "(Multiple)";
            }
            else
            {
                UspsAccountEntity account = uspsAccount.SelectedIndex >= 0 ? UspsAccountManager.GetAccount((long) uspsAccount.SelectedValue) : null;
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
        /// Open the window for managing the available Usps accounts
        /// </summary>
        private void OnManageUspsAccounts(object sender, EventArgs e)
        {
            using (UspsAccountManagerDlg dlg = new UspsAccountManagerDlg(ResellerType))
            {
                dlg.ShowDialog(this);
            }

            bool multiValue = uspsAccount.MultiValued;
            long oldAccount = multiValue ? -1 : (long) uspsAccount.SelectedValue;

            uspsAccount.SelectedValueChanged -= OnOriginChanged;

            LoadAccounts();

            if (multiValue)
            {
                uspsAccount.MultiValued = true;
            }
            else
            {
                if (uspsAccount.SelectedValue == null || oldAccount != (long) uspsAccount.SelectedValue)
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
    }
}

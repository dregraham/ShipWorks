using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// UserControl for editing Stamps.com profiles
    /// </summary>
    public partial class StampsProfileControl : PostalProfileControlBase
    {
        private ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Need this for the designer...
        /// </summary>
        public StampsProfileControl() : this(ShipmentTypeCode.Stamps)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StampsProfileControl(ShipmentTypeCode shipmentTypeCode)
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPage);

            this.shipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Load the given profile into the control
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            LoadStampsAccounts();

            StampsProfileEntity stampsProfile = profile.Postal.Stamps;

            AddValueMapping(stampsProfile, StampsProfileFields.StampsAccountID, stateAccount, stampsAccount, labelAccount);
            AddValueMapping(stampsProfile, StampsProfileFields.HidePostage, stateStealth, hidePostage, labelStealth);
            AddValueMapping(stampsProfile, StampsProfileFields.RequireFullAddressValidation, validationState, requireFullAddressValidation, labelValidation);
            AddValueMapping(stampsProfile, StampsProfileFields.Memo, stateMemo, memo, labelMemo);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);
        }
        
        /// <summary>
        /// Load the list of stamps.com accounts
        /// </summary>
        private void LoadStampsAccounts()
        {
            stampsAccount.DisplayMember = "Key";
            stampsAccount.ValueMember = "Value";

            StampsResellerType stampsResellerType = PostalUtility.GetStampsResellerType(shipmentTypeCode);
            List<StampsAccountEntity> accounts = StampsAccountManager.GetAccounts(stampsResellerType);
            if (accounts.Any())
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
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// UserControl for editing USPS profiles
    /// </summary>
    public partial class Express1UspsProfileControl : PostalProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPage);
        }

        /// <summary>
        /// Load the given profile into the control
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            LoadUspsAccounts();

            UspsProfileEntity uspsProfile = profile.Postal.Usps;

            AddValueMapping(uspsProfile, UspsProfileFields.UspsAccountID, stateAccount, uspsAccount, labelAccount);

            AddValueMapping(uspsProfile, UspsProfileFields.HidePostage, stateStealth, hidePostage, labelStealth);
            AddValueMapping(uspsProfile, UspsProfileFields.RequireFullAddressValidation, validationState, requireFullAddressValidation, labelValidation);
            AddValueMapping(uspsProfile, PostalProfileFields.Memo1, stateMemo, memo, labelMemo);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);
        }
        
        /// <summary>
        /// Load the list of USPS accounts
        /// </summary>
        private void LoadUspsAccounts()
        {
            uspsAccount.DisplayMember = "Key";
            uspsAccount.ValueMember = "Value";

            UspsResellerType uspsResellerType = PostalUtility.GetUspsResellerType(ShipmentTypeCode.Express1Usps);
            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(uspsResellerType);
            if (accounts.Any())
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
    }
}

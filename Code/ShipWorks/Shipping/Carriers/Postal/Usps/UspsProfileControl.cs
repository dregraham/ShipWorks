﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Profiles;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// UserControl for editing USPS profiles
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.Usps)]
    public partial class UspsProfileControl : PostalProfileControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsProfileControl()
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

            if (UspsUtility.IsStampsInsuranceActive)
            {
                insuranceControl.UseInsuranceBoxLabel = UspsUtility.StampsInsuranceDisplayName;
            }

            LoadUspsAccounts();

            UspsProfileEntity uspsProfile = profile.Postal.Usps;

            AddValueMapping(uspsProfile, UspsProfileFields.UspsAccountID, stateAccount, uspsAccount, labelAccount);
            AddValueMapping(uspsProfile, UspsProfileFields.RateShop, stateRateShop, rateShop, labelRateShop);

            AddValueMapping(uspsProfile, UspsProfileFields.HidePostage, stateStealth, hidePostage, labelStealth);
            AddValueMapping(profile.Postal, PostalProfileFields.NoPostage, stateNoPostage, noPostage, labelNoPostage);
            AddValueMapping(uspsProfile, UspsProfileFields.RequireFullAddressValidation, validationState, requireFullAddressValidation, labelValidation);

            AddValueMapping(profile.Postal, PostalProfileFields.Memo1, stateMemo1, memo1, labelMemo1);
            AddValueMapping(profile.Postal, PostalProfileFields.Memo2, stateMemo2, memo2, labelMemo2);
            AddValueMapping(profile.Postal, PostalProfileFields.Memo3, stateMemo3, memo3, labelMemo3);

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

            UspsResellerType uspsResellerType = PostalUtility.GetUspsResellerType(ShipmentTypeCode.Usps);

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

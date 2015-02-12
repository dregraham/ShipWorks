using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// UserControl for editing USPS profiles
    /// </summary>
    public partial class UspsProfileControl : PostalProfileControlBase
    {
        private ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Need this for the designer...
        /// </summary>
        public UspsProfileControl() : this(ShipmentTypeCode.Usps)
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UspsProfileControl(ShipmentTypeCode shipmentTypeCode)
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

            LoadUspsAccounts();

            UspsProfileEntity uspsProfile = profile.Postal.Usps;

            AddValueMapping(uspsProfile, UspsProfileFields.UspsAccountID, stateAccount, uspsAccount, labelAccount);
            AddValueMapping(uspsProfile, UspsProfileFields.RateShop, stateRateShop, rateShop, labelRateShop);

            AddValueMapping(uspsProfile, UspsProfileFields.HidePostage, stateStealth, hidePostage, labelStealth);
            AddValueMapping(uspsProfile, UspsProfileFields.RequireFullAddressValidation, validationState, requireFullAddressValidation, labelValidation);
            AddValueMapping(uspsProfile, UspsProfileFields.Memo, stateMemo, memo, labelMemo);

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

            StampsResellerType uspsResellerType = PostalUtility.GetStampsResellerType(shipmentTypeCode);
            List<UspsAccountEntity> accounts = StampsAccountManager.GetAccounts(uspsResellerType);
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

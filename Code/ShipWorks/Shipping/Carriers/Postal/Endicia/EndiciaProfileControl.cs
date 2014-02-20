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
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// UserControl for editing Endicia profiles
    /// </summary>
    public partial class EndiciaProfileControl : PostalProfileControlBase
    {
        // the reseller sub-type this profile configures
        EndiciaReseller endiciaReseller = EndiciaReseller.None;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaProfileControl(EndiciaReseller endiciaReseller)
        {
            InitializeComponent();

            this.endiciaReseller = endiciaReseller;
        }

        /// <summary>
        /// Load the given profile into the control
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            if (EndiciaUtility.IsEndiciaInsuranceActive)
            {
                insuranceControl.UseInsuranceBoxLabel = "Endicia Insurance";
            }

            LoadEndiciaAccounts();

            EnumHelper.BindComboBox<PostalSortType>(sortType);
            EnumHelper.BindComboBox<PostalEntryFacility>(entryFacility);

            EndiciaProfileEntity endiciaProfile = profile.Postal.Endicia;

            AddValueMapping(endiciaProfile, EndiciaProfileFields.EndiciaAccountID, stateAccount, endiciaAccount, labelAccount);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.StealthPostage, stateStealth, hidePostage, labelStealth);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.NoPostage, stateNoPostage, noPostage, labelNoPostage);

            AddValueMapping(endiciaProfile, EndiciaProfileFields.RubberStamp1, stateRubberStamp1, rubberStamp1, labelRubberStamp1);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.RubberStamp2, stateRubberStamp2, rubberStamp2, labelRubberStamp2);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.RubberStamp3, stateRubberStamp3, rubberStamp3, labelRubberStamp3);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.ReferenceID, stateReferenceID, referenceID, labelReferenceID);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.ScanBasedReturn, scanBasedPaymentState, scanBasedPayment);

            AddValueMapping(profile.Postal, PostalProfileFields.SortType, stateSortType, sortType, labelSortType);
            AddValueMapping(profile.Postal, PostalProfileFields.EntryFacility, stateEntryFacility, entryFacility, labelEntryFacility);
        }

        /// <summary>
        /// Load the list of Endicia accounts
        /// </summary>
        private void LoadEndiciaAccounts()
        {
            endiciaAccount.DisplayMember = "Key";
            endiciaAccount.ValueMember = "Value";

            if (EndiciaAccountManager.GetAccounts(endiciaReseller).Count > 0)
            {
                endiciaAccount.DataSource = EndiciaAccountManager.GetAccounts(endiciaReseller).Select(a => new KeyValuePair<string, long>(a.Description, a.EndiciaAccountID)).ToList();
                endiciaAccount.Enabled = true;
            }
            else
            {
                endiciaAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                endiciaAccount.Enabled = false;
            }
        }
    }
}

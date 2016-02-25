using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

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

            ResizeGroupBoxes(tabPage);

            this.endiciaReseller = endiciaReseller;

            if (!IsScanBasedReturnsEnabled())
            {
                scanBasedPaymentState.Visible = false;
                scanBasedPayment.Visible = false;
                groupReturns.Height -= scanBasedPayment.Bottom - returnShipment.Bottom;
            }
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
            AddValueMapping(profile.Postal, PostalProfileFields.NoPostage, stateNoPostage, noPostage, labelNoPostage);

            AddValueMapping(profile.Postal, PostalProfileFields.Memo1, stateRubberStamp1, rubberStamp1, labelRubberStamp1);
            AddValueMapping(profile.Postal, PostalProfileFields.Memo2, stateRubberStamp2, rubberStamp2, labelRubberStamp2);
            AddValueMapping(profile.Postal, PostalProfileFields.Memo3, stateRubberStamp3, rubberStamp3, labelRubberStamp3);
            AddValueMapping(endiciaProfile, EndiciaProfileFields.ReferenceID, stateReferenceID, referenceID, labelReferenceID);

            if (IsScanBasedReturnsEnabled())
            {
                AddValueMapping(endiciaProfile, EndiciaProfileFields.ScanBasedReturn, scanBasedPaymentState, scanBasedPayment);
            }

            AddValueMapping(profile.Postal, PostalProfileFields.SortType, stateSortType, sortType, labelSortType);
            AddValueMapping(profile.Postal, PostalProfileFields.EntryFacility, stateEntryFacility, entryFacility, labelEntryFacility);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat);
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

        /// <summary>
        /// Determines if endicia scan-based returns are enabled.
        /// </summary>
        private static bool IsScanBasedReturnsEnabled()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return
                    lifetimeScope.Resolve<ILicenseService>().CheckRestriction(EditionFeature.EndiciaScanBasedReturns, null) ==
                    EditionRestrictionLevel.None;
            }
        }
    }
}

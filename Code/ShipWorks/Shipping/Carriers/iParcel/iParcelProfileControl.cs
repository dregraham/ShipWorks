using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// i-parce Profile Control
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class iParcelProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelProfileControl" /> class.
        /// </summary>
        public iParcelProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);
            ResizeGroupBoxes(tabPagePackages);

            packagesCount.Items.Clear();

            for (int i = 1; i <= 5; i++)
            {
                packagesCount.Items.Add(i);
            }

            // Use the i-parcel specific factory for populating the suggested tokens
            skuAndQuantity.TokenSuggestionFactory = new iParcelTokenSuggestionFactory();

            // i-parcel does not support ZPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.ZPL);
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        /// <param name="profile"></param>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            IParcelProfileEntity iParcelProfile = profile.IParcel;

            if (ShippingSettings.Fetch().IParcelInsuranceProvider == (int) InsuranceProvider.Carrier)
            {
                insuranceControl.UseInsuranceBoxLabel = "i-parcel Declared Value";
                insuranceControl.InsuredValueLabel = "Declared value:";
            }

            LoadIParcelAccounts();

            EnumHelper.BindComboBox<iParcelServiceType>(service);

            //From
            AddValueMapping(iParcelProfile, IParcelProfileFields.IParcelAccountID, accountState, iParcelAccount, labelAccount);

            //Service
            AddValueMapping(iParcelProfile, IParcelProfileFields.Service, serviceState, service, labelService);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);

            //Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            //Reference
            AddValueMapping(iParcelProfile, IParcelProfileFields.Reference, referenceState, referenceNumber, labelReference);

            //Options
            AddValueMapping(iParcelProfile, IParcelProfileFields.TrackByEmail, emailTrackState, emailTrack, labelEmailTrack);
            AddValueMapping(iParcelProfile, IParcelProfileFields.IsDeliveryDutyPaid, dutyDeliveryPaidState, dutyDeliveryPaid, labelDuty);
            AddValueMapping(iParcelProfile, IParcelProfileFields.SkuAndQuantities, skuAndQuantityState, skuAndQuantity, labelSkuAndQuantity);


            packagesState.Checked = iParcelProfile.Packages.Count > 0;
            packagesCount.SelectedIndex = packagesState.Checked ? iParcelProfile.Packages.Count - 1 : -1;
            packagesCount.Enabled = packagesState.Checked;

            LoadPackageEditingUI();

            packagesState.CheckedChanged += new EventHandler(OnChangePackagesChecked);
            packagesCount.SelectedIndexChanged += new EventHandler(OnChangePackagesCount);
        }

        /// <summary>
        /// Loads the i-parcel accounts.
        /// </summary>
        private void LoadIParcelAccounts()
        {
            iParcelAccount.DisplayMember = "Key";
            iParcelAccount.ValueMember = "Value";

            if (iParcelAccountManager.Accounts.Count > 0)
            {
                iParcelAccount.DataSource = iParcelAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.IParcelAccountID)).ToList();
                iParcelAccount.Enabled = true;
            }
            else
            {
                iParcelAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                iParcelAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            foreach (iParcelProfilePackageControl control in panelPackageControls.Controls)
            {
                // If its visible it means ite being edited
                if (control.Visible)
                {
                    control.SaveToEntity();
                }
            }
        }

        /// <summary>
        /// Changing if packages are enabled
        /// </summary>
        void OnChangePackagesChecked(object sender, EventArgs e)
        {
            if (packagesState.Checked)
            {
                packagesCount.Enabled = true;
                packagesCount.SelectedIndex = 0;
            }
            else
            {
                packagesCount.Enabled = false;
                packagesCount.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Changing the count of profile packages
        /// </summary>
        void OnChangePackagesCount(object sender, EventArgs e)
        {
            int count;

            if (packagesCount.SelectedIndex == -1)
            {
                count = 0;
            }
            else
            {
                count = packagesCount.SelectedIndex + 1;
            }

            // Go through each package that already exists
            foreach (IParcelProfilePackageEntity package in Profile.IParcel.Packages)
            {
                // If we need more live packages, mark this one as alive
                if (count > 0)
                {
                    if (package.Fields.State == EntityState.Deleted)
                    {
                        package.Fields.State = package.IsNew ? EntityState.New : EntityState.Fetched;
                    }
                }
                // Otherwise mark this one as deleted
                else
                {
                    package.Fields.State = EntityState.Deleted;
                }

                count--;
            }

            // While we still need to create more, create more
            for (int i = 0; i < count; i++)
            {
                IParcelProfilePackageEntity package = new IParcelProfilePackageEntity();
                Profile.IParcel.Packages.Add(package);
            }

            LoadPackageEditingUI();
        }

        /// <summary>
        /// Load the UI for editing all the package profile controls
        /// </summary>
        private void LoadPackageEditingUI()
        {
            // Get all the not marked for deleted packages
            List<IParcelProfilePackageEntity> packages = Profile.IParcel.Packages.Where(p => p.Fields.State != EntityState.Deleted).ToList();

            int index = 0;
            Control lastControl = null;

            // Ensure each one has a UI control
            foreach (IParcelProfilePackageEntity package in packages)
            {
                iParcelProfilePackageControl control;

                // If there is a control for it already, it should match up with this package
                if (panelPackageControls.Controls.Count > index)
                {
                    control = (iParcelProfilePackageControl)panelPackageControls.Controls[index];
                    control.Visible = true;

                    Debug.Assert(control.ProfilePackage == package);
                }
                else
                {
                    control = new iParcelProfilePackageControl();

                    int top = 0;
                    if (panelPackageControls.Controls.Count > 0)
                    {
                        top = panelPackageControls.Controls[panelPackageControls.Controls.Count - 1].Bottom + 4;
                    }

                    control.Width = panelPackageControls.Width;
                    control.Top = top;
                    control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

                    panelPackageControls.Controls.Add(control);
                    control.LoadProfilePackage(package);
                }

                lastControl = control;
                index++;
            }

            // Make all the ones we don't need not visible
            for (int i = packages.Count; i < panelPackageControls.Controls.Count; i++)
            {
                panelPackageControls.Controls[i].Visible = false;
            }

            panelPackageControls.Height = lastControl == null ? 0 : lastControl.Bottom + 4;
        }

        /// <summary>
        /// Cancel any changes that have not yet been committed
        /// </summary>
        public override void CancelChanges()
        {
            base.CancelChanges();

            // Go through the list of packages
            foreach (IParcelProfilePackageEntity package in Profile.IParcel.Packages.ToList())
            {
                // If its new, then we created it, and we gots to get rid of it
                if (package.IsNew)
                {
                    Profile.IParcel.Packages.Remove(package);
                }
                // If its marked as deleted, we have to restore it
                else if (package.Fields.State == EntityState.Deleted)
                {
                    package.Fields.State = EntityState.Fetched;
                }
            }
        }
    }
}

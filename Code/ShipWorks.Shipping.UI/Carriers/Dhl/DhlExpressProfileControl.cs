﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express Profile Control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressProfileControl : ShippingProfileControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DhlExpressProfileControl" /> class.
        /// </summary>
        public DhlExpressProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);
            ResizeGroupBoxes(tabPagePackages);

            packagesCount.Items.Clear();

            for (int i = 1; i <= 5; i++)
            {
                packagesCount.Items.Add(i);
            }

            // ShipEngine does not support EPL
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        /// <param name="profile"></param>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            DhlExpressProfileEntity DhlExpressProfile = profile.DhlExpress;

            LoadDhlExpressAccounts();

            EnumHelper.BindComboBox<DhlExpressServiceType>(service);
            EnumHelper.BindComboBox<ShipEngineContentsType>(contents);
            EnumHelper.BindComboBox<ShipEngineNonDeliveryType>(nonDelivery);

            //From
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.DhlExpressAccountID, accountState, dhlExpressAccount, labelAccount);

            //Service
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.Service, serviceState, service, labelService);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            //Options
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.SaturdayDelivery, saturdayState, saturdayDelivery, labelSaturday);
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.DeliveryDutyPaid, dutyDeliveryPaidState, dutyDeliveryPaid, labelDuty);
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.NonMachinable, nonMachinableState, nonMachinable, labelNonMachinable);

            //Customs
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.Contents, contentsState, contents, labelContents);
            AddValueMapping(DhlExpressProfile, DhlExpressProfileFields.NonDelivery, nonDeliveryState, nonDelivery, labelNonDelivery);

            packagesState.Checked = profile.PackageProfile.Count > 0;
            packagesCount.SelectedIndex = packagesState.Checked ? profile.PackageProfile.Count - 1 : -1;
            packagesCount.Enabled = packagesState.Checked;

            LoadPackageEditingUI();

            packagesState.CheckedChanged += new EventHandler(OnChangePackagesChecked);
            packagesCount.SelectedIndexChanged += new EventHandler(OnChangePackagesCount);
        }

        /// <summary>
        /// Loads the Dhl Express accounts.
        /// </summary>
        private void LoadDhlExpressAccounts()
        {
            dhlExpressAccount.DisplayMember = "Key";
            dhlExpressAccount.ValueMember = "Value";

            if (DhlExpressAccountManager.Accounts.Count > 0)
            {
                dhlExpressAccount.DataSource = DhlExpressAccountManager.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.DhlExpressAccountID)).ToList();
                dhlExpressAccount.Enabled = true;
            }
            else
            {
                dhlExpressAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                dhlExpressAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            foreach (DhlExpressProfilePackageControl control in panelPackageControls.Controls)
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
            foreach (PackageProfileEntity package in Profile.PackageProfile)
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
                PackageProfileEntity package = new PackageProfileEntity();
                Profile.PackageProfile.Add(package);
            }

            LoadPackageEditingUI();
        }

        /// <summary>
        /// Load the UI for editing all the package profile controls
        /// </summary>
        [SuppressMessage("SonarQube", "S1698:Consider using 'Equals' if value comparison was intended",
            Justification = "This is used in an Assert.Debug and is existing code")]
        private void LoadPackageEditingUI()
        {
            // Get all the not marked for deleted packages
            List<PackageProfileEntity> packages = Profile.PackageProfile.Where(p => p.Fields.State != EntityState.Deleted).ToList();

            int index = 0;
            Control lastControl = null;

            // Ensure each one has a UI control
            foreach (PackageProfileEntity package in packages)
            {
                DhlExpressProfilePackageControl control;

                // If there is a control for it already, it should match up with this package
                if (panelPackageControls.Controls.Count > index)
                {
                    control = (DhlExpressProfilePackageControl) panelPackageControls.Controls[index];
                    control.Visible = true;

                    Debug.Assert(control.ProfilePackage == package);
                }
                else
                {
                    control = new DhlExpressProfilePackageControl();

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
            foreach (PackageProfileEntity package in Profile.PackageProfile.ToList())
            {
                // If its new, then we created it, and we gots to get rid of it
                if (package.IsNew)
                {
                    Profile.PackageProfile.Remove(package);
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

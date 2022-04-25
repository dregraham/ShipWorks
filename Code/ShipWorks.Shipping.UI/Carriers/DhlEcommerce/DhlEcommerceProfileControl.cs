﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// DHL eCommerce Profile Control
    /// </summary>
    [KeyedComponent(typeof(ShippingProfileControlBase), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceProfileControl : ShippingProfileControlBase
    {
        private readonly IDhlEcommerceAccountRepository accountRepo;

        /// <summary>
        /// Constructor for Visual Studio Designer
        /// </summary>
        public DhlEcommerceProfileControl()
        {
            InitializeComponent();

            ResizeGroupBoxes(tabPageSettings);
            ResizeGroupBoxes(tabPagePackages);

            packagesCount.Items.Clear();

            for (int i = 1; i <= 25; i++)
            {
                packagesCount.Items.Add(i);
            }

            // ShipEngine only support Standard for DHL eCommerce
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.EPL);
            requestedLabelFormat.ExcludeFormats(ThermalLanguage.ZPL);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceProfileControl(IDhlEcommerceAccountRepository accountRepo) : this()
        {
            this.accountRepo = accountRepo;
        }

        /// <summary>
        /// Load the given profile
        /// </summary>
        public override void LoadProfile(ShippingProfileEntity profile)
        {
            base.LoadProfile(profile);

            var dhlEcommerceProfile = profile.DhlEcommerce;

            LoadDhlEcommerceAccounts();

            EnumHelper.BindComboBox<DhlExpressServiceType>(service);
            EnumHelper.BindComboBox<ShipEngineContentsType>(contents);
            EnumHelper.BindComboBox<ShipEngineNonDeliveryType>(nonDelivery);
            EnumHelper.BindComboBox<TaxIdType>(taxIdType);

            customsTinIssuingAuthority.DisplayMember = "Key";
            customsTinIssuingAuthority.ValueMember = "Value";
            customsTinIssuingAuthority.DataSource = Geography.Countries.Select(n => new KeyValuePair<string, string>(n, Geography.GetCountryCode(n))).ToList();


            //From
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.DhlEcommerceAccountID, accountState, dhlEcommerceAccount, labelAccount);

            //Service
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.Service, serviceState, service, labelService);

            // Labels
            AddValueMapping(profile, ShippingProfileFields.RequestedLabelFormat, requestedLabelFormatState, requestedLabelFormat, labelThermalNote);

            // Insurance
            AddValueMapping(profile, ShippingProfileFields.Insurance, insuranceState, insuranceControl);

            //Options
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.SaturdayDelivery, saturdayState, saturdayDelivery, labelSaturday);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.DeliveryDutyPaid, dutyDeliveryPaidState, dutyDeliveryPaid, labelDuty);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.NonMachinable, nonMachinableState, nonMachinable, labelNonMachinable);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.ResidentialDelivery, resDeliveryState, resDelivery, labelResDelivery);

            //Customs
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.Contents, contentsState, contents, labelContents);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.NonDelivery, nonDeliveryState, nonDelivery, labelNonDelivery);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.CustomsRecipientTin, customsRecipientTinState, customsRecipientTin, labelCustomsRecipientTin);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.CustomsTaxIdType, taxIdTypeState, taxIdType, labelTaxIdType);
            AddValueMapping(dhlEcommerceProfile, DhlEcommerceProfileFields.CustomsTinIssuingAuthority, customsTinIssuingAuthorityState, customsTinIssuingAuthority, labelCustomsTinIssuingAuthority);

            packagesState.Checked = profile.Packages.Count > 0;
            packagesCount.SelectedIndex = packagesState.Checked ? profile.Packages.Count - 1 : -1;
            packagesCount.Enabled = packagesState.Checked;

            LoadPackageEditingUI();

            packagesState.CheckedChanged += new EventHandler(OnChangePackagesChecked);
            packagesCount.SelectedIndexChanged += new EventHandler(OnChangePackagesCount);
        }

        /// <summary>
        /// Loads the DHL eCommerce accounts.
        /// </summary>
        private void LoadDhlEcommerceAccounts()
        {
            dhlEcommerceAccount.DisplayMember = "Key";
            dhlEcommerceAccount.ValueMember = "Value";

            if (accountRepo.Accounts.Count() > 0)
            {
                dhlEcommerceAccount.DataSource = accountRepo.Accounts.Select(a => new KeyValuePair<string, long>(a.Description, a.DhlEcommerceAccountID)).ToList();
                dhlEcommerceAccount.Enabled = true;
            }
            else
            {
                dhlEcommerceAccount.DataSource = new List<KeyValuePair<string, long>> { new KeyValuePair<string, long>("(No accounts)", 0) };
                dhlEcommerceAccount.Enabled = false;
            }
        }

        /// <summary>
        /// Save all the package profile stuff to their entities
        /// </summary>
        public override void SaveToEntity()
        {
            base.SaveToEntity();

            foreach (DhlEcommerceProfilePackageControl control in panelPackageControls.Controls)
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
            foreach (PackageProfileEntity package in Profile.Packages)
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
                Profile.Packages.Add(package);
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
            List<PackageProfileEntity> packages = Profile.Packages.Where(p => p.Fields.State != EntityState.Deleted).ToList();

            int index = 0;
            Control lastControl = null;

            // Ensure each one has a UI control
            foreach (PackageProfileEntity package in packages)
            {
                DhlEcommerceProfilePackageControl control;

                // If there is a control for it already, it should match up with this package
                if (panelPackageControls.Controls.Count > index)
                {
                    control = (DhlEcommerceProfilePackageControl) panelPackageControls.Controls[index];
                    control.Visible = true;

                    Debug.Assert(control.ProfilePackage == package);
                }
                else
                {
                    control = new DhlEcommerceProfilePackageControl();

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
            foreach (PackageProfileEntity package in Profile.Packages.ToList())
            {
                // If its new, then we created it, and we gots to get rid of it
                if (package.IsNew)
                {
                    Profile.Packages.Remove(package);
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
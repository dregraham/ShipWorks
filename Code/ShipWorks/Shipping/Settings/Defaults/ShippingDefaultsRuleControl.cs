using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using log4net;
using Autofac;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Shipping.Settings.Defaults
{
    /// <summary>
    /// UserControl for displaying a profile rule for initial shipment settings
    /// </summary>
    public partial class ShippingDefaultsRuleControl : UserControl
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingDefaultsRuleControl));
        private const long NoFilterSelectedID = long.MinValue;

        ShippingDefaultsRuleEntity rule;
        int position = 0;

        /// <summary>
        /// User has clicked the delete button on the rule line
        /// </summary>
        public event EventHandler DeleteClicked;

        /// <summary>
        /// User has clicked to move the rule up
        /// </summary>
        public event EventHandler MoveUpClicked;

        /// <summary>
        /// User has clicked to move the rule down
        /// </summary>
        public event EventHandler MoveDownClicked;

        /// <summary>
        /// User wants to manage profiles
        /// </summary>
        public event EventHandler MangeProfilesClicked;

        private long originalFilterID;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDefaultsRuleControl()
        {
            InitializeComponent();

            toolStipDelete.Renderer = new NoBorderToolStripRenderer();
            originalFilterID = NoFilterSelectedID;
        }

        /// <summary>
        /// The rule that the control has been initialized with
        /// </summary>
        public ShippingDefaultsRuleEntity Rule
        {
            get { return rule; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is filter disabled.
        /// </summary>
        public bool IsFilterDisabled
        {
            get
            {
                return filterCombo.IsSelectedFilterDisabled;
            }
        }

        /// <summary> 
        /// Gets a value indicating whether this the filter being used for this rule has changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the filter filter changed; otherwise, <c>false</c>.
        /// </value>
        public bool HasFilterChanged
        {
            get { return originalFilterID != filterCombo.SelectedFilterNode.FilterID; }
        }

        /// <summary>
        /// Initialize the control to work with the settings of the given rule
        /// </summary>
        public void Initialize(ShippingDefaultsRuleEntity rule)
        {
            this.rule = rule;

            filterCombo.LoadLayouts(FilterTarget.Orders);

            filterCombo.SelectedFilterNodeID = rule.FilterNodeID;
            
            if (filterCombo.SelectedFilterNode == null)
            {
                filterCombo.SelectFirstNode();
            }
            else
            {
                originalFilterID = filterCombo.SelectedFilterNode.FilterID;
            }

            UpdateProfileDisplay(ShippingProfileManager.GetProfile(rule.ShippingProfileID));
        }

        /// <summary>
        /// Update the displayed profile text
        /// </summary>
        private void UpdateProfileDisplay(ShippingProfileEntity profile)
        {
            linkProfile.Text = profile != null ? profile.Name : "(none)";
            linkProfile.Tag = profile != null ? profile.ShippingProfileID : 0;
        }

        /// <summary>
        /// Displayed index of the rule
        /// </summary>
        public void UpdatePosition(int index, int total)
        {
            labelIndex.Text = string.Format("{0}.", index);

            buttonMoveUp.Enabled = (index > 1);
            buttonMoveDown.Enabled = (index < total);

            position = index;

            UpdateProfileDisplay(ShippingProfileManager.GetProfile((long) linkProfile.Tag));
        }

        /// <summary>
        /// User clicked the delete button
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (DeleteClicked != null)
            {
                DeleteClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Move this rule down
        /// </summary>
        private void OnMoveDown(object sender, EventArgs e)
        {
            if (MoveDownClicked != null)
            {
                MoveDownClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Move this rule up
        /// </summary>
        private void OnMoveUp(object sender, EventArgs e)
        {
            if (MoveUpClicked != null)
            {
                MoveUpClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Clicked the profile link
        /// </summary>
        private void OnClickProfileLink(object sender, EventArgs e)
        {
            ContextMenuStrip menuStrip = CreateProfilesMenu();

            menuStrip.Show(linkProfile.Parent.PointToScreen(new Point(linkProfile.Left, linkProfile.Bottom)));
        }

        /// <summary>
        /// Create the profiles menu
        /// </summary>
        private ContextMenuStrip CreateProfilesMenu()
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ShippingProfileEntity selected = ShippingProfileManager.GetProfile((long) linkProfile.Tag);

            if (selected != null)
            {
                ToolStripMenuItem editProfile = new ToolStripMenuItem("Edit");
                editProfile.Tag = selected;
                editProfile.Click += new EventHandler(OnEditProfile);
                menuStrip.Items.Add(editProfile);

                menuStrip.Items.Add(new ToolStripSeparator());
            }

            ToolStripMenuItem selectMenu = new ToolStripMenuItem("Select");
            menuStrip.Items.Add(selectMenu);

            foreach (ShippingProfileEntity profile in ShippingProfileManager.Profiles)
            {
                if (profile.ShipmentType == rule.ShipmentType && !profile.ShipmentTypePrimary)
                {
                    ToolStripMenuItem menuItem = new ToolStripMenuItem(profile.Name);
                    menuItem.Tag = profile;
                    menuItem.Click += new EventHandler(OnChooseProfile);
                    selectMenu.DropDownItems.Add(menuItem);
                }
            }

            if (selectMenu.DropDownItems.Count == 0)
            {
                selectMenu.DropDownItems.Add(new ToolStripMenuItem("(none)") { Enabled = false });
            }

            menuStrip.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem manageProfiles = new ToolStripMenuItem("Manage Profiles...");
            manageProfiles.Click += new EventHandler(OnManageProfiles);
            menuStrip.Items.Add(manageProfiles);

            return menuStrip;
        }

        /// <summary>
        /// Edit the selected profile
        /// </summary>
        void OnEditProfile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            ShippingProfileEntity profile = (ShippingProfileEntity) menuItem.Tag;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                using (ShippingProfileEditorDlg dlg = new ShippingProfileEditorDlg(profile, lifetimeScope))
                {
                    dlg.ShowDialog(this);
                }
            }

            UpdateProfileDisplay(profile);
        }

        /// <summary>
        /// set the profile associated with the menu item raising the event
        /// </summary>
        void OnChooseProfile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            ShippingProfileEntity profile = (ShippingProfileEntity) menuItem.Tag;

            UpdateProfileDisplay(profile);
        }

        /// <summary>
        /// Manage profiles
        /// </summary>
        void OnManageProfiles(object sender, EventArgs e)
        {
            if (MangeProfilesClicked != null)
            {
                MangeProfilesClicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Save the settings for the rule line
        /// </summary>
        public void SaveSettings()
        {
            rule.FilterNodeID = filterCombo.SelectedFilterNodeID;
            rule.ShippingProfileID = (long) linkProfile.Tag;
            rule.Position = position;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                try
                {
                    ShippingDefaultsRuleManager.SaveRule(rule, adapter);
                }
                catch (ORMConcurrencyException ex)
                {
                    log.Error("Saving output group", ex);

                    // Skip this error.  It means that a group we are trying to save couldn't be saved b\c it was
                    // deleted somewhere else.  If that happens, for these settings, we just let it go.
                    ShippingDefaultsRuleManager.CheckForChangesNeeded();
                }

                adapter.Commit();
            }

            // Sync up the original filter ID with the saved filter ID for instances where 
            // this control's remains in memory and the Initialize method is not called 
            // prior to showing the control. This will prevent a false positive in the 
            // HasFilterChanged property.
            originalFilterID = filterCombo.SelectedFilterNode != null ? filterCombo.SelectedFilterNode.FilterID : NoFilterSelectedID;
        }
    }
}

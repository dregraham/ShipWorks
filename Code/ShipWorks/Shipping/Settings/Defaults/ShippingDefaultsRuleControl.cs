using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.Settings.Defaults
{
    /// <summary>
    /// UserControl for displaying a profile rule for initial shipment settings
    /// </summary>
    public partial class ShippingDefaultsRuleControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShippingDefaultsRuleControl));
        private const long NoFilterSelectedID = long.MinValue;
        private const long NoneShippingProfileID = 0;

        private int position;

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
        private long originalFilterNodeID;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDefaultsRuleControl()
        {
            InitializeComponent();

            filterCombo.AllowMyFilters = false;

            toolStipDelete.Renderer = new NoBorderToolStripRenderer();
            originalFilterID = NoFilterSelectedID;
            originalFilterNodeID = NoneShippingProfileID;
        }

        /// <summary>
        /// The rule that the control has been initialized with
        /// </summary>
        public ShippingDefaultsRuleEntity Rule { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is filter disabled.
        /// </summary>
        public bool IsFilterDisabled => filterCombo.IsSelectedFilterDisabled;

        /// <summary>
        /// Gets a value indicating whether this instance is a my filter.
        /// </summary>
        public bool IsMyFilter => filterCombo.IsSelectedFilterMyFilter;

        /// <summary>
        /// Gets the selected filter name
        /// </summary>
        public string SelectedFilterName => filterCombo.SelectedFilterNode?.Filter?.Name;

        /// <summary>
        /// Gets a value indicating whether this the filter being used for this rule has changed.
        /// </summary>
        /// <value>
        /// <c>true</c> if the filter changed; otherwise, <c>false</c>.
        /// </value>
        public bool HasFilterChanged => originalFilterID != filterCombo.SelectedFilterNode.FilterID;

        /// <summary>
        /// Initialize the control to work with the settings of the given rule
        /// </summary>
        public void Initialize(ShippingDefaultsRuleEntity rule)
        {
            Rule = rule;
            originalFilterNodeID = Rule.FilterNodeID;

            filterCombo.AllowMyFilters = Rule.FilterNodeID != 0 && FilterHelper.IsMyFilter(Rule.FilterNodeID);

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
        private void UpdateProfileDisplay(IShippingProfileEntity profile)
        {
            linkProfile.Text = profile?.Name ?? "(none)";
            linkProfile.Tag = profile?.ShippingProfileID ?? NoneShippingProfileID;
        }

        /// <summary>
        /// Displayed index of the rule
        /// </summary>
        public void UpdatePosition(int index, int total)
        {
            labelIndex.Text = string.Format("{0}.", index);

            buttonMoveUp.Enabled = index > 1;
            buttonMoveDown.Enabled = index < total;

            position = index;

            UpdateProfileDisplay(ShippingProfileManager.GetProfile((long) linkProfile.Tag));
        }

        /// <summary>
        /// User clicked the delete button
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Move this rule down
        /// </summary>
        private void OnMoveDown(object sender, EventArgs e)
        {
            MoveDownClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Move this rule up
        /// </summary>
        private void OnMoveUp(object sender, EventArgs e)
        {
            MoveUpClicked?.Invoke(this, EventArgs.Empty);
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
            IShippingProfileEntity selected = ShippingProfileManager.GetProfile((long) linkProfile.Tag);

            if (selected != null)
            {
                ToolStripMenuItem editProfile = new ToolStripMenuItem("Edit");
                editProfile.Tag = selected;
                editProfile.Click += OnEditProfile;
                menuStrip.Items.Add(editProfile);

                menuStrip.Items.Add(new ToolStripSeparator());
            }

            menuStrip.Items.Add(CreateSelectProfileMenu());
            menuStrip.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem manageProfiles = new ToolStripMenuItem("Manage Profiles...");
            manageProfiles.Click += OnManageProfiles;
            menuStrip.Items.Add(manageProfiles);

            return menuStrip;
        }

        /// <summary>
        /// Creates the select profile menu
        /// </summary>
        private ToolStripMenuItem CreateSelectProfileMenu()
        {
            ToolStripMenuItem selectMenu = new ToolStripMenuItem("Select");

            List<IShippingProfileEntity> applicableProfiles =
                ShippingProfileManager.GetProfilesFor(Rule.ShipmentTypeCode, false).ToList();

            if (applicableProfiles.Any())
            {
                // Global profiles
                List<IShippingProfileEntity> globalProfiles = applicableProfiles
                                                              .Where(p => p.ShipmentType == null)
                                                              .OrderBy(p => p.Name).ToList();

                globalProfiles.ForEach(p => AddProfileToMenu(p, selectMenu));

                // Carrier Profiles
                List<IShippingProfileEntity> carrierProfiles = applicableProfiles
                                                               .Where(p => p.ShipmentType == Rule.ShipmentTypeCode)
                                                               .OrderBy(p => p.Name).ToList();

                if (globalProfiles.Any() && carrierProfiles.Any())
                {
                    selectMenu.DropDownItems.Add(new ToolStripSeparator());
                }

                if (carrierProfiles.Any())
                {
                    ToolStripLabel carrierLabel = new ToolStripLabel(EnumHelper.GetDescription(Rule.ShipmentTypeCode))
                    {
                        Font = new Font(new FontFamily("Tahoma"), 6.5f, FontStyle.Bold),
                        Margin = new Padding(-4, 2, 2, 2),
                        Enabled = false
                    };
                    selectMenu.DropDownItems.Add(carrierLabel);
                    carrierProfiles.ForEach(p => AddProfileToMenu(p, selectMenu));

                    selectMenu.DropDown.PerformLayout();
                }
            }
            else
            {
                selectMenu.DropDownItems.Add(new ToolStripMenuItem("(none)") { Enabled = false });
            }

            return selectMenu;
        }

        /// <summary>
        /// Adds given profile to the given menu
        /// </summary>
        private void AddProfileToMenu(IShippingProfileEntity profile, ToolStripMenuItem menu)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem(profile.Name);
            menuItem.Tag = profile;
            menuItem.Click += OnChooseProfile;
            menu.DropDown.Items.Add(menuItem);
        }

        /// <summary>
        /// Edit the selected profile
        /// </summary>
        private void OnEditProfile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            IShippingProfileEntity profile = (IShippingProfileEntity) menuItem.Tag;

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                IShippingProfileService shippingProfileService = lifetimeScope.Resolve<IShippingProfileService>();

                ShippingProfileEditorDlg profileEditor = lifetimeScope.Resolve<ShippingProfileEditorDlg>(
                    new TypedParameter(typeof(IEditableShippingProfile), shippingProfileService.GetEditable(profile.ShippingProfileID))
                );
                profileEditor.ShowDialog(this);
            }

            UpdateProfileDisplay(profile);
        }

        /// <summary>
        /// set the profile associated with the menu item raising the event
        /// </summary>
        private void OnChooseProfile(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;
            IShippingProfileEntity profile = (IShippingProfileEntity) menuItem.Tag;

            UpdateProfileDisplay(profile);
        }

        /// <summary>
        /// Manage profiles
        /// </summary>
        private void OnManageProfiles(object sender, EventArgs e)
        {
            MangeProfilesClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return a list of validation errors
        /// </summary>
        public string ValidationErrors
        {
            get
            {
                if (IsMyFilter)
                {
                    return $"Rule at position {position} is using a My Filter. Shipping Rules can no longer use a My Filter. Any changes to this rule will not be saved.";
                }

                if (FilterHelper.IsMyFilter(originalFilterNodeID))
                {
                    return $"Rule at position {position} was using a My Filter that is not available to this user account. " +
                           $"Shipping Rules can no longer use a My filter.  " +
                           $"This rule has been updated to use a filter that is not a My Filter and the profile will be changed to \"none\".";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Save the settings for the rule line
        /// </summary>
        public void SaveSettings()
        {
            bool isOriginalFilterNodeAMyFilter = originalFilterNodeID != 0 && FilterHelper.IsMyFilter(originalFilterNodeID);

            // If the selected filter node is not a my filter and the original was not a my filter, just carry on as usual.
            if (!FilterHelper.IsMyFilter(filterCombo.SelectedFilterNode) && !isOriginalFilterNodeAMyFilter)
            {
                SaveRule((long) linkProfile.Tag);
            }

            // The selected filter is a my filter, so it must by this user's filter (we don't show other user's my filters).
            // In this case we want to tell the user we will not be saving this shipping rule because a my filter
            // is still selected.
            if (IsMyFilter)
            {
                return;
            }

            // If the original filter node id was a my filter, set the profile to the "none" profile and save
            if (isOriginalFilterNodeAMyFilter)
            {
                SaveRule(NoneShippingProfileID);
            }
        }

        /// <summary>
        /// Save the shipping rule with the given shipping profile id
        /// </summary>
        private void SaveRule(long shippingProfileID)
        {
            Rule.FilterNodeID = filterCombo.SelectedFilterNodeID;
            Rule.ShippingProfileID = shippingProfileID;

            Rule.Position = position;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                try
                {
                    ShippingDefaultsRuleManager.SaveRule(Rule, adapter);
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
            originalFilterID = filterCombo.SelectedFilterNode?.FilterID ?? NoFilterSelectedID;

            originalFilterNodeID = Rule.FilterNodeID;
        }
    }
}

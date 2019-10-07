using System;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Grid;
using ShipWorks.UI.Controls;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Option page for the "Personal" section
    /// </summary>
    public partial class SettingsPagePersonal : SettingsPageBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsPagePersonal()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<FilterInitialSortType>(filterInitialSort);
            EnumHelper.BindComboBox<WeightDisplayFormat>(comboWeightFormat);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (UserSession.IsLoggedOn)
            {
                UserSettingsEntity settings = UserSession.User.Settings;

                colorScheme.SelectedIndex = (int) ShipWorksDisplay.ColorScheme;
                systemTray.Checked = ShipWorksDisplay.HideInSystemTray;

                minimizeRibbon.Checked = settings.MinimizeRibbon;
                showQatBelowRibbon.Checked = settings.ShowQAToolbarBelowRibbon;

                radioInitialFilterRecent.Checked = settings.FilterInitialUseLastActive;
                radioInitialFilterAlways.Checked = !settings.FilterInitialUseLastActive;

                filterComboBox.LoadLayouts(FilterTarget.Orders, FilterTarget.Customers);

                // Find the node we need in the layout
                FilterNodeEntity filterNode = FilterLayoutContext.Current.FindNode(settings.FilterInitialSpecified);
                if (filterNode == null || filterNode.Filter.IsSavedSearch)
                {
                    filterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
                }

                // Set the selected node
                filterComboBox.SelectedFilterNode = filterNode;

                // Set selected sort option
                filterInitialSort.SelectedValue = (FilterInitialSortType) settings.FilterInitialSortType;

                comboWeightFormat.SelectedValue = (WeightDisplayFormat) settings.ShippingWeightFormat;
            }
            else
            {
                Controls.Clear();

                Label label = new Label();
                label.Text = "You are not logged on.";
                label.Location = sectionDisplay.Location;
                label.AutoSize = true;
                label.Font = new Font(Font, FontStyle.Bold);
                Controls.Add(label);
            }
        }

        /// <summary>
        /// Save the changes made
        /// </summary>
        public override void Save()
        {
            if (UserSession.IsLoggedOn)
            {
                UserSettingsEntity settings = UserSession.User.Settings;

                ShipWorksDisplay.ColorScheme = (ColorScheme) colorScheme.SelectedIndex;
                ShipWorksDisplay.HideInSystemTray = systemTray.Checked;

                settings.MinimizeRibbon = minimizeRibbon.Checked;
                settings.ShowQAToolbarBelowRibbon = showQatBelowRibbon.Checked;

                settings.FilterInitialUseLastActive = radioInitialFilterRecent.Checked;
                settings.FilterInitialSortType = (int) filterInitialSort.SelectedValue;

                settings.FilterInitialSpecified = filterComboBox.SelectedFilterNode.FilterNodeID;

                settings.ShippingWeightFormat = (int) comboWeightFormat.SelectedValue;

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(settings);
                }
            }
        }

        /// <summary>
        /// Changing which method of initial filter selection to use
        /// </summary>
        private void OnChangeInitialFilterSelection(object sender, EventArgs e)
        {
            filterComboBox.Enabled = radioInitialFilterAlways.Checked;
        }
    }
}

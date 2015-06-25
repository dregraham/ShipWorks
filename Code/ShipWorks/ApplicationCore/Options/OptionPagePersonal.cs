using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Users;
using ShipWorks.Filters;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.UI.Controls;
using ShipWorks.Filters.Grid;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Option page for the "Personal" section
    /// </summary>
    public partial class OptionPagePersonal : OptionPageBase
    {
        ShipWorksOptionsData data;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPagePersonal(ShipWorksOptionsData data)
        {
            InitializeComponent();

            this.data = data;

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

                minimizeRibbon.Checked = data.MinimizeRibbon;
                showQatBelowRibbon.Checked = data.ShowQatBelowRibbon;

                radioInitialFilterRecent.Checked = settings.FilterInitialUseLastActive;
                radioInitialFilterAlways.Checked = !settings.FilterInitialUseLastActive;

                filterComboBox.LoadLayouts(FilterTarget.Orders, FilterTarget.Customers);

                // Find the node we need in the layout
                FilterNodeEntity filterNode = FilterLayoutContext.Current.FindNode(settings.FilterInitialSpecified);
                if (filterNode == null)
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

                data.MinimizeRibbon = minimizeRibbon.Checked;
                data.ShowQatBelowRibbon = showQatBelowRibbon.Checked;

                settings.FilterInitialUseLastActive = radioInitialFilterRecent.Checked;
                settings.FilterInitialSortType = (int) filterInitialSort.SelectedValue;

                settings.FilterInitialSpecified = filterComboBox.SelectedFilterNode.FilterNodeID;

                settings.ShippingWeightFormat = (int) comboWeightFormat.SelectedValue;
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

using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Users;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.UI.Controls;
using ShipWorks.Filters.Grid;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Option page for the "Personal" section
    /// </summary>
    public partial class OptionPagePersonal : OptionPageBase
    {
        private readonly ShipWorksOptionsData data;
        private readonly IWin32Window owner;
        private readonly IScannerConfigurationRepository scannerRepo;

        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPagePersonal(ShipWorksOptionsData data, IWin32Window owner, ILifetimeScope scope)
        {
            InitializeComponent();

            this.data = data;
            this.owner = owner;

            scannerRepo = scope.Resolve<IScannerConfigurationRepository>();

            unregisterScannerButton.Enabled = !string.IsNullOrWhiteSpace(scannerRepo.GetScannerName());

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

                // Load single scan settings and update ui
                singleScan.Checked = (SingleScanSettings) settings.SingleScanSettings != SingleScanSettings.Disabled;
                autoPrint.Checked = (SingleScanSettings) settings.SingleScanSettings == SingleScanSettings.AutoPrint;
                UpdateSingleScanSettingsUI();
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

                if (autoPrint.Checked)
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.AutoPrint;
                }
                else if (singleScan.Checked)
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.Scan;
                }
                else
                {
                    settings.SingleScanSettings = (int) SingleScanSettings.Disabled;
                }
            }
        }

        /// <summary>
        /// Update auto print checkbox state based on single scan setting
        /// </summary>
        private void UpdateSingleScanSettingsUI()
        {
            // Only allow auto print to be checked when single scan is enabled
            if (!singleScan.Checked)
            {
                autoPrint.Checked = false;
                autoPrint.Enabled = false;
                registerScannerLabel.Visible = false;
            }
            else
            {
                autoPrint.Enabled = true;
                registerScannerLabel.Visible = string.IsNullOrWhiteSpace(scannerRepo.GetScannerName());
            }

            unregisterScannerButton.Enabled = !string.IsNullOrWhiteSpace(scannerRepo.GetScannerName());
        }

        /// <summary>
        /// Update UI when single scan is enabled
        /// </summary>
        private void OnChangeSingleScanSettings(object sender, EventArgs e) => UpdateSingleScanSettingsUI();

        /// <summary>
        /// Changing which method of initial filter selection to use
        /// </summary>
        private void OnChangeInitialFilterSelection(object sender, EventArgs e)
        {
            filterComboBox.Enabled = radioInitialFilterAlways.Checked;
        }

        /// <summary>
        /// Called when [click register scanner].
        /// </summary>
        private void OnClickRegisterScanner(object sender, EventArgs e)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                Form findScanner = scope.ResolveNamed<Form>("ScannerRegistrationDialog");
                try
                {
                    findScanner.ShowDialog(owner);
                }
                catch (TargetInvocationException ex)
                {
                    MessageHelper.ShowError(owner, ex.InnerException.Message);
                }

                UpdateSingleScanSettingsUI();
            }
        }

        /// <summary>
        /// Called when [click unregister scanner].
        /// </summary>
        private void OnClickUnregisterScanner(object sender, EventArgs e)
        {
            scannerRepo.ClearScannerName();
            UpdateSingleScanSettingsUI();
        }
    }
}

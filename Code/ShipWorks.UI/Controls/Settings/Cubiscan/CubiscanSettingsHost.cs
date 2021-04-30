using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Settings;

namespace ShipWorks.UI.Controls.Settings.Cubiscan
{
    [Component(RegistrationType.SpecificService, Service = typeof(ICubiscanSettingsPage))]
    public partial class CubiscanSettingsHost : SettingsPageBase, ICubiscanSettingsPage
    {
        private readonly Func<IWin32Window, ICubiscanSettingsViewModel> settingsViewModelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CubiscanSettingsHost(Func<IWin32Window, ICubiscanSettingsViewModel> settingsViewModelFactory)
        {
            this.settingsViewModelFactory = settingsViewModelFactory;

            InitializeComponent();
        }

        /// <summary>
        /// When the control loads, set and load the view model
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Visible = false;

            var apiSettingsControl = new CubiscanSettingsControl();
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.Child = apiSettingsControl;
            Controls.Add(elementHost);

            var settingsViewModel = settingsViewModelFactory(this);
            apiSettingsControl.DataContext = settingsViewModel;
            settingsViewModel.Load();

            Visible = true;
        }
    }
}
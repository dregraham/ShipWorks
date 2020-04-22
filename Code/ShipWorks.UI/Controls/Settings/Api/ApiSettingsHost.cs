using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Api;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.ApplicationCore.Settings.Api;

namespace ShipWorks.UI.Controls.Settings.Api
{
    /// <summary>
    /// Winforms host for the WPF ApiSettingsControl
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IApiSettingsPage))]
    public partial class ApiSettingsHost : SettingsPageBase, IApiSettingsPage
    {
        private readonly ApiSettingsViewModel viewModel;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiSettingsHost(ApiSettingsViewModel viewModel, IMessageHelper messageHelper)
        {
            this.viewModel = viewModel;
            this.messageHelper = messageHelper;
            InitializeComponent();
        }

        /// <summary>
        /// Whether or not the api settings page is currently in the process of saving
        /// </summary>
        public bool IsSaving => viewModel.Status == ApiStatus.Updating;

        /// <summary>
        /// When the control loads, set and load the view model
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Visible = false;

            ApiSettingsControl apiSettingsControl = new ApiSettingsControl();
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.Child = apiSettingsControl;
            Controls.Add(elementHost);

            apiSettingsControl.DataContext = viewModel;
            viewModel.Load();

            Visible = true;
        }
    }
}

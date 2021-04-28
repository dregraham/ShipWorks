using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Settings.Api;
using ShipWorks.ApplicationCore.Settings.Warehouse;
using ShipWorks.Core.Messaging;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Form for displaying the settings ShipWorks provides
    /// </summary>
    public partial class ShipWorksSettings : Form
    {
        // Maps list item name to the settings page that should be displayed for it
        private Dictionary<string, ISettingsPage> settingsPages = new Dictionary<string, ISettingsPage>();
        private readonly IMessenger messenger;
        private readonly IApiSettingsPage apiSettingsPage;

        /// <summary>
        /// Indicates if warehouse is allowed
        /// </summary>
        public static bool IsWarehouseAllowed
        {
            get
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
                    EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                    // If warehouse is not allowed, return false
                    return restrictionLevel == EditionRestrictionLevel.None;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksSettings(ILifetimeScope scope)
        {
            InitializeComponent();

            // We disable scanning when opening the settings dialog. Once the dialog closes we will start it
            // again if we need to with any new settings.
            messenger = scope.Resolve<IMessenger>();
            messenger.Send(new DisableSingleScanInputFilterMessage(this));

            settingsPages["My Settings"] = InitializeSettingsPage(new SettingsPagePersonal(scope));

            settingsPages["Logging"] = InitializeSettingsPage(new SettingsPageLogging());
            settingsPages["Scan-to-Ship"] = InitializeSettingsPage(new SettingsPageScanToShip(this, scope));

            if (IsWarehouseAllowed)
            {
                settingsPages["Warehouse"] = InitializeSettingsPage(scope.Resolve<IWarehouseSettingsViewModel>());
            }

            if (UserSession.IsLoggedOn && UserSession.User.IsAdmin)
            {
                settingsPages["Advanced"] = InitializeSettingsPage(new SettingsPageAdvanced());
                apiSettingsPage = scope.Resolve<IApiSettingsPage>();
                settingsPages["API"] = InitializeSettingsPage(apiSettingsPage);
            }

            if (InterapptiveOnly.IsInterapptiveUser || InterapptiveOnly.MagicKeysDown)
            {
                settingsPages["Internal"] = InitializeSettingsPage(new SettingsPageInterapptive());
            }

            menuList.Items.Clear();

            foreach (string pageName in settingsPages.Keys)
            {
                menuList.Items.Add(pageName);
            }

            menuList.SelectedIndex = 0;
        }

        /// <summary>
        /// Initialize the settings page for use in the control
        /// </summary>
        private ISettingsPage InitializeSettingsPage(ISettingsPage settingsPage)
        {
            settingsPage.Control.Visible = false;
            settingsPage.Control.Parent = sectionContainer;
            settingsPage.Control.Dock = DockStyle.Fill;

            return settingsPage;
        }

        /// <summary>
        /// Current settings page is changing
        /// </summary>
        private void OnChangeSettingsPage(object sender, EventArgs e)
        {
            if (apiSettingsPage != null && apiSettingsPage.Control.Visible && apiSettingsPage.IsSaving == true)
            {
                MessageHelper.ShowError(this, "ShipWorks is currently updating API settings. Please wait.");
                return;
            }

            string pageName = menuList.Items[menuList.SelectedIndex].ToString();
            ISettingsPage page = settingsPages[pageName];

            foreach (Control control in sectionContainer.Controls)
            {
                bool isActivePage = (control == page.Control);

                control.Visible = isActivePage;

                // This marks that its been loaded
                if (isActivePage)
                {
                    control.Tag = new object();
                }
            }
        }

        /// <summary>
        /// Committing the changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            var loadedSettingsPages = settingsPages
                .Select(x => x.Value)
                .Where(x => x?.Control.Tag != null);

            // Save all settings pages
            foreach (ISettingsPage settingsPage in loadedSettingsPages)
            {
                settingsPage.Save();
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// When the form is trying to close itself, check to make sure we aren't in the middle of saving.
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (apiSettingsPage?.IsSaving == true)
            {
                MessageHelper.ShowError(this, "ShipWorks is currently updating API settings. Please wait.");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Start SingleScan if appropriate.
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            messenger.Send(new EnableSingleScanInputFilterMessage(this));
        }

        /// <summary>
        /// Draw the menu list item
        /// </summary>
        private void MenuListDrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString((string) menuList.Items[e.Index], e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        /// <summary>
        /// Measures the menu list item
        /// </summary>
        private void MenuListMeasureItem(object sender, MeasureItemEventArgs e)
        {
            string text = (string) menuList.Items[e.Index];
            Graphics graphics = Graphics.FromHwnd(this.Handle);
            SizeF size = graphics.MeasureString(text, this.Font);
            float dif = size.Width - menuList.Width;

            int menuItemHeight = 26;
            while (dif > 0)
            {
                dif -= menuList.Width;
                menuItemHeight += 10;
            }

            e.ItemHeight = menuItemHeight;
        }
    }
}

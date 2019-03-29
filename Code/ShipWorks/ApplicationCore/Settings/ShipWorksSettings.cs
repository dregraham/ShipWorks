using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Core.Messaging;
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
        private Dictionary<string, UserControl> settingsPages = new Dictionary<string, UserControl>();
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksSettings(ShipWorksSettingsData data, ILifetimeScope scope)
        {
            InitializeComponent();

            // We disable scanning when opening the settings dialog. Once the dialog closes we will start it
            // again if we need to with any new settings.
            messenger = scope.Resolve<IMessenger>();
            messenger.Send(new DisableSingleScanInputFilterMessage(this));

            settingsPages["My Settings"] = InitializeSettingsPage(new SettingsPagePersonal(data));

            settingsPages["Logging"] = InitializeSettingsPage(new SettingsPageLogging());
            settingsPages["Keyboard && Barcode Shortcuts"] = InitializeSettingsPage(new SettingsPageShortcuts(this, scope));

            if (UserSession.IsLoggedOn && UserSession.User.IsAdmin)
            {
                settingsPages["Advanced"] = InitializeSettingsPage(new SettingsPageAdvanced());
            }

            if (InterapptiveOnly.IsInterapptiveUser || InterapptiveOnly.MagicKeysDown)
            {
                settingsPages["Interapptive"] = InitializeSettingsPage(new SettingsPageInterapptive());
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
        private UserControl InitializeSettingsPage(UserControl settingsPage)
        {
            settingsPage.Visible = false;
            settingsPage.Parent = sectionContainer;
            settingsPage.Dock = DockStyle.Fill;
            settingsPage.AutoScroll = true;

            return settingsPage;
        }

        /// <summary>
        /// Current settings page is changing
        /// </summary>
        private void OnChangeSettingsPage(object sender, EventArgs e)
        {
            string pageName = menuList.Items[menuList.SelectedIndex].ToString();
            Control page = settingsPages[pageName];

            foreach (Control control in sectionContainer.Controls)
            {
                bool isActivePage = (control == page);

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
            // Save all settings pages
            foreach (Control control in sectionContainer.Controls)
            {
                SettingsPageBase pageBase = control as SettingsPageBase;
                if (pageBase != null && control.Tag != null)
                {
                    pageBase.Save();
                }
            }

            DialogResult = DialogResult.OK;
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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Form for displaying the options ShipWorks provides
    /// </summary>
    public partial class ShipWorksOptions : Form
    {
        // Maps list item name to the option page that should be displayed for it
        Dictionary<string, UserControl> optionPages = new Dictionary<string, UserControl>();
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksOptions(ShipWorksOptionsData data, ILifetimeScope scope)
        {
            InitializeComponent();

            // We disable scanning when opening the options dialog. Once the dialog closes we will start it
            // again if we need to with any new settings.
            messenger = scope.Resolve<IMessenger>();
            messenger.Send(new DisableSingleScanInputFilterMessage(this));

            optionPages["My Settings"] = InitializeOptionPage(new OptionPagePersonal(data));

            optionPages["Logging"] = InitializeOptionPage(new OptionPageLogging());
            optionPages["Keyboard and Barcode Shortcuts"] = InitializeOptionPage(new OptionPageShortcuts(this, scope));

            if (UserSession.IsLoggedOn && UserSession.User.IsAdmin)
            {
                optionPages["Advanced"] = InitializeOptionPage(new OptionPageAdvanced());
            }

            if (InterapptiveOnly.IsInterapptiveUser || InterapptiveOnly.MagicKeysDown)
            {
                optionPages["Interapptive"] = InitializeOptionPage(new OptionPageInterapptive());
            }

            menuList.Items.Clear();

            foreach (string pageName in optionPages.Keys)
            {
                menuList.Items.Add(pageName);
            }

            menuList.SelectedIndex = 0;
        }

        /// <summary>
        /// Initialize the option page for use in the control
        /// </summary>
        private UserControl InitializeOptionPage(UserControl optionPage)
        {
            optionPage.Visible = false;
            optionPage.Parent = sectionContainer;
            optionPage.Dock = DockStyle.Fill;
            optionPage.AutoScroll = true;

            return optionPage;
        }

        /// <summary>
        /// Current option page is changing
        /// </summary>
        private void OnChangeOptionPage(object sender, EventArgs e)
        {
            string pageName = menuList.Items[menuList.SelectedIndex].ToString();
            Control page = optionPages[pageName];

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
            // Save all option pages
            foreach (Control control in sectionContainer.Controls)
            {
                OptionPageBase pageBase = control as OptionPageBase;
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
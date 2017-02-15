using System;
using System.Collections.Generic;
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

            optionPages["My Settings"] = InitializeOptionPage(new OptionPagePersonal(data, this, scope));
            optionPages["Logging"] = InitializeOptionPage(new OptionPageLogging());

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
    }
}
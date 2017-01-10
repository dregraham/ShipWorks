using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Divelements.SandRibbon.Rendering;
using Divelements.SandRibbon;
using System.Reflection;
using System.Diagnostics;
using Autofac;
using ShipWorks.UI;
using ShipWorks.Users;
using Interapptive.Shared.UI;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Form for displaying the options ShipWorks provides
    /// </summary>
    public partial class ShipWorksOptions : Form
    {
        // Maps list item name to the option page that should be displayed for it
        Dictionary<string, UserControl> optionPages = new Dictionary<string, UserControl>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksOptions(ShipWorksOptionsData data, ILifetimeScope scope)
        {
            InitializeComponent();

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
    }
}
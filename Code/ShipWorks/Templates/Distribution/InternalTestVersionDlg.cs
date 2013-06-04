using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using Interapptive.Shared.Win32;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Templates.Distribution
{
    /// <summary>
    /// Only displayed to interapptive developers to enter the version number to pretend for ShipWorks for testing
    /// </summary>
    public partial class InternalTestVersionDlg : Form
    {
        string swVersionKey = "TemplateDistributionShipWorksVersionOverride";

        /// <summary>
        /// Constructor
        /// </summary>
        public InternalTestVersionDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializatin
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            installedVersion.Text = SystemData.Fetch().TemplateVersion;
            swVersion.Text = InterapptiveOnly.Registry.GetValue(swVersionKey, "3.0.0");
        }

        /// <summary>
        /// The version of ShipWorks to pretend we are
        /// </summary>
        public Version ShipWorksVersion
        {
            get { return new Version(swVersion.Text); }
        }

        /// <summary>
        /// The installed template version to pretend we have
        /// </summary>
        public Version TemplateVersion
        {
            get { return new Version(installedVersion.Text); }
        }

        /// <summary>
        /// Ensure the given text box version number has 4 full digits
        /// </summary>
        private void EnsureFourDigits(TextBox versionBox)
        {
            for (int i = Math.Max(0, 3 - versionBox.Text.ToCharArray().Count(c => c == '.')); i > 0; i--)
            {
                versionBox.Text += ".0";
            }
        }
        
        /// <summary>
        /// Closing window
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            try
            {
                new Version(installedVersion.Text);
                new Version(swVersion.Text);

                InterapptiveOnly.Registry.SetValue(swVersionKey, swVersion.Text);

                EnsureFourDigits(installedVersion);
                EnsureFourDigits(swVersion);

                DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageHelper.ShowMessage(this, "Enter valid version numbers.");
            }
        }
    }
}

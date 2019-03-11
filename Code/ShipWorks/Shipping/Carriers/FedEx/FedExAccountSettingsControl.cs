using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.Linq;
using ShipWorks.Shipping.Carriers.Api;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing the settings of FedEx account
    /// </summary>
    public partial class FedExAccountSettingsControl : UserControl
    {
        private const string filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the settings from the account into the control
        /// </summary>
        public void LoadAccount(FedExAccountEntity account)
        {
            signatureAuth.Text = account.SignatureRelease;

            XElement root = XElement.Parse(account.SmartPostHubList);

            var hubs = root.Descendants("HubID");
            if (hubs.Count() > 0)
            {
                // The first is the default
                hubID.Text = (string) hubs.First();

                // Additional hubs
                additionalHubs.Lines = hubs.Skip(1).Select(e => (string) e).ToArray();
            }
        }

        /// <summary>
        /// Save the settings from the account into the control
        /// </summary>
        public void SaveToAccount(FedExAccountEntity account)
        {
            account.SignatureRelease = signatureAuth.Text;

            XElement root = new XElement("Root");

            if (hubID.Text.Trim().Length > 0)
            {
                if (!Regex.IsMatch(hubID.Text.Trim(), @"^[0-9]{4}$"))
                {
                    throw new CarrierException("Please enter a Hub ID of 4 numbers with no alpha characters.");
                }
                
                root.Add(new XElement("HubID", hubID.Text.Trim()));
            }

            foreach (string hubLine in additionalHubs.Lines.Select(l => l.Trim()).Where(l => l.Length > 0))
            {
                if (!Regex.IsMatch(hubLine.Trim(), @"^[0-9]{4}$"))
                {
                    throw new CarrierException("Please enter a Hub ID of 4 numbers with no alpha characters.");
                }
                root.Add(new XElement("HubID", hubLine.Trim()));
            }

            account.SmartPostHubList = root.ToString();
        }

        /// <summary>
        /// Browse for a letterhead image
        /// </summary>
        private void OnBrowseLetterhead(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filter;
            using (dlg)
            {
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        /// <summary>
        /// Browse for a signature image
        /// </summary>
        private void OnBrowseSignature(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filter;
            using (dlg)
            {
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pictureBox2.Image = new Bitmap(dlg.FileName);
                }
            }
        }
    }
}

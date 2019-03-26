using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing the settings of FedEx account
    /// </summary>
    public partial class FedExAccountSettingsControl : UserControl
    {
        private readonly long maxImageByteSize = 50000;
        private readonly int maxImageWidth = 700;
        private readonly int maxImageHeight = 50;

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

            if (account.Letterhead.Length > 0)
            {
                letterheadPreview.Image = account.Letterhead.Base64StringToImage();
            }

            if (account.Signature.Length > 0)
            {
                signaturePreview.Image = account.Signature.Base64StringToImage();
            }
        }

        /// <summary>
        /// Save the settings from the account into the control
        /// </summary>
        public void SaveToAccount(FedExAccountEntity account)
        {
            var letterheadFilename = openFileDialogLetterhead.FileName;
            var signatureFilename = openFileDialogSignature.FileName;

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

            try
            {
                if (letterheadFilename.Length > 0)
                {
                    var bitmapImageLetterhead = new Bitmap(letterheadFilename);
                    account.Letterhead = bitmapImageLetterhead.ImageToBase64String(letterheadPreview.Image.RawFormat);
                }

                if (signatureFilename.Length > 0)
                {
                    var bitmapImageSignature = new Bitmap(signatureFilename);
                    account.Signature = bitmapImageSignature.ImageToBase64String(signaturePreview.Image.RawFormat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Browse for a letterhead image
        /// </summary>
        private void OnBrowseLetterhead(object sender, EventArgs e)
        {
            if (openFileDialogLetterhead.ShowDialog(this) == DialogResult.OK)
            {
                if (openFileDialogLetterhead.FileName.ValidateImageSize(maxImageByteSize))
                {
                    if (openFileDialogLetterhead.FileName.ValidateImageDimensions(maxImageWidth, maxImageHeight))
                    {
                        letterheadPreview.Image = new Bitmap(openFileDialogLetterhead.FileName);
                    }
                    else
                    {
                        MessageHelper.ShowMessage(this, "The selected image exceeds the max dimensions of 700x50.");
                    }
                }
                else
                {
                    MessageHelper.ShowMessage(this, "The selected image is too large.");
                }
            }
        }

        /// <summary>
        /// Browse for a signature image
        /// </summary>
        private void OnBrowseSignature(object sender, EventArgs e)
        {
            if (openFileDialogSignature.ShowDialog(this) == DialogResult.OK)
            {
                if (openFileDialogSignature.FileName.ValidateImageSize(maxImageByteSize))
                {
                    if (openFileDialogSignature.FileName.ValidateImageDimensions(maxImageWidth, maxImageHeight))
                    {
                        signaturePreview.Image = new Bitmap(openFileDialogSignature.FileName);
                    }
                    else
                    {
                        MessageHelper.ShowMessage(this, "The selected image exceeds the max dimensions of 700x50.");
                    }
                }
                else
                {
                    MessageHelper.ShowMessage(this, "The selected image is too large.");
                }
            }
        }

        private void OnLearnMore(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://support.shipworks.com/hc/en-us/articles/360025471432", null);
        }
    }
}

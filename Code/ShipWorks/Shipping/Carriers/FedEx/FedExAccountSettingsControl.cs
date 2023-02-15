using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// User control for editing the settings of FedEx account
    /// </summary>
    public partial class FedExAccountSettingsControl : UserControl
    {
        private const int MaxImageWidth = 700;
        private const int MaxImageHeight = 50;
        private string letterheadString;
        private string signatureString;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExAccountSettingsControl()
        {
            InitializeComponent();
            EnumHelper.BindComboBox<FedExSmartPostHub>(hubID);
        }

        /// <summary>
        /// Load the settings from the account into the control
        /// </summary>
        public void LoadAccount(FedExAccountEntity account)
        {
            signatureAuth.Text = account.SignatureRelease;

            using (MultiValueScope scope = new MultiValueScope())
            {
                hubID.ApplyMultiValue((FedExSmartPostHub) account.SmartPostHub);
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
            account.SignatureRelease = signatureAuth.Text;
            
            hubID.ReadMultiValue(v => account.SmartPostHub = (int) v);

            try
            {
                if (letterheadString != null && letterheadString != account.Letterhead)
                {
                    account.Letterhead = letterheadString;
                }

                if (signatureString != null && signatureString != account.Signature)
                {
                    account.Signature = signatureString;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// Browse for a letterhead image
        /// </summary>
        private void OnBrowseLetterhead(object sender, EventArgs e)
        {
            if (openFileDialogLetterhead.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var image = Image.FromFile(openFileDialogLetterhead.FileName);

                    if (image.Size.Width <= MaxImageWidth && image.Size.Height <= MaxImageHeight)
                    {
                        letterheadString = image.ImageToBase64String(image.RawFormat);
                        letterheadPreview.Image = image;
                    }
                    else
                    {
                        MessageHelper.ShowError(this,
                            "The selected image exceeds the max resolution of 700 pixels wide by 50 pixels long");
                    }
                }
                catch (Exception)
                {
                    MessageHelper.ShowError(this, "The selected image is invalid");
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
                try
                {
                    var image = Image.FromFile(openFileDialogSignature.FileName);

                    if (image.Size.Width <= MaxImageWidth && image.Size.Height <= MaxImageHeight)
                    {
                        signatureString = image.ImageToBase64String(image.RawFormat);
                        signaturePreview.Image = image;
                    }
                    else
                    {
                        MessageHelper.ShowError(this,
                            "The selected image exceeds the max resolution of 700 pixels wide by 50 pixels long");
                    }
                }
                catch (Exception)
                {
                    MessageHelper.ShowError(this, "The selected image is invalid");
                }
            }
        }

        private void OnLearnMore(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://support.shipworks.com/hc/en-us/articles/360025471432", null);
        }
    }
}

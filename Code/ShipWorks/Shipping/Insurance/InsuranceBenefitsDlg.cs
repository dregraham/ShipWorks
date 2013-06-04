using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Window for displaying promotional info about shipworks insurance
    /// </summary>
    public partial class InsuranceBenefitsDlg : Form
    {
        InsuranceCost cost;
        bool allowEnableShipWorks;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceBenefitsDlg(InsuranceCost cost, bool allowEnableShipWorks = false)
        {
            InitializeComponent();

            this.cost = cost;
            this.allowEnableShipWorks = allowEnableShipWorks;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            var images = InsuranceUtility.LoadInfoBannerImages();

            if (images != null)
            {
                Image headerImage = images.Item1;
                Image footerImage = images.Item2;

                Width = Math.Max(headerImage.Width, footerImage.Width);

                pictureBoxHeader.Image = headerImage;
                pictureBoxHeader.Height = headerImage.Height;

                enableInsurance.Top = pictureBoxHeader.Bottom;
                labelInsurance.Top = pictureBoxHeader.Bottom;

                pictureBoxFooter.Image = footerImage;
                pictureBoxFooter.Height = footerImage.Height;

                panelBottom.Top = enableInsurance.Bottom + 2;
                panelBottom.Height = footerImage.Height + 35;

                ClientSize = new Size(ClientSize.Width, panelBottom.Bottom);
            }

            if (cost == null)
            {
                enableInsurance.Visible = false;
                labelInsurance.Visible = false;

                Height -= 28;
                panelBottom.Top -= 28;
            }
            else
            {
                string text = string.Format("Insure this package for only ${0:0.00} with ShipWorks Insurance", cost.ShipWorks);

                if (cost.ShipWorks < cost.Carrier)
                {
                    text += string.Format(" and save ${0:0.00}.", cost.Carrier - cost.ShipWorks);
                }
                else
                {
                    text += ".";
                }

                enableInsurance.Text = text;
                labelInsurance.Text = text;

                enableInsurance.Visible = allowEnableShipWorks;
                labelInsurance.Visible = !allowEnableShipWorks;
            }
        }

        /// <summary>
        /// Click the rates and policies link
        /// </summary>
        private void OnClickRatesPolicies(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/insurance", this);
        }

        /// <summary>
        /// Indicates if the user chose to turn on ShipWorks insurance
        /// </summary>
        public bool ShipWorksInsuranceEnabled
        {
            get { return enableInsurance.Checked; }
        }
    }
}

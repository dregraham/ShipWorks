using System;
using System.Drawing;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Window for displaying promotional info about shipworks insurance
    /// </summary>
    public partial class InsuranceBenefitsDlg : Form
    {
        private readonly InsuranceCost cost;
        private readonly bool allowEnableShipWorks;

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
            ClientSize = new Size(ClientSize.Width, panelBottom.Bottom);

            if (cost == null)
            {
                enableInsurance.Visible = false;
                labelInsurance.Visible = false;
            }
            else
            {
                string text = $"Insure this package for only ${cost.ShipWorks:0.00} with ShipWorks Insurance";

                if (cost.ShipWorks < cost.Carrier)
                {
                    text += $" and save ${cost.Carrier - cost.ShipWorks:0.00}.";
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
            WebHelper.OpenUrl("https://www.shipworks.com/insurance", this);
        }

        /// <summary>
        /// Indicates if the user chose to turn on ShipWorks insurance
        /// </summary>
        public bool ShipWorksInsuranceEnabled => enableInsurance.Checked;
    }
}

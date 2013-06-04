using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// User control for allowing selection of an insurance provider
    /// </summary>
    public partial class InsuranceProviderChooser : UserControl
    {
        string carrierProviderName = "Carrier";
        InsuranceProvider initialProvider = InsuranceProvider.ShipWorks;

        /// <summary>
        /// The selected provider has changed
        /// </summary>
        public event EventHandler ProviderChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceProviderChooser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            comboProvider.Items.Add("ShipWorks Insurance");
            comboProvider.Items.Add(CarrierProviderName);

            InsuranceProvider = initialProvider;

            carrierMessage.Top = linkShipWorks.Top;
        }

        /// <summary>
        /// The selected provider has changed
        /// </summary>
        private void OnChangeProvider(object sender, EventArgs e)
        {
            linkShipWorks.Visible = comboProvider.SelectedIndex == 0;
            carrierMessage.Visible = comboProvider.SelectedIndex == 1;

            if (ProviderChanged != null)
            {
                ProviderChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Clicking the link to get more information on ShipWorks insurance
        /// </summary>
        private void OnLinkShipWorks(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (InsuranceBenefitsDlg dlg = new InsuranceBenefitsDlg(null))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Get or set the selected insurance provider
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InsuranceProvider InsuranceProvider
        {
            get
            {
                return comboProvider.Items.Count == 0 ? 
                    initialProvider : 
                    (InsuranceProvider) comboProvider.SelectedIndex + 1;
            }
            set
            {
                if (comboProvider.Items.Count == 0)
                {
                    initialProvider = value;
                }
                else
                {
                    int provider = (int) value;

                    if (provider < 1 || provider > 2)
                    {
                        provider = (int) InsuranceProvider.ShipWorks;
                    }

                    comboProvider.SelectedIndex = provider - 1;
                }
            }
        }

        /// <summary>
        /// The name of the insurance provider of the carrier
        /// </summary>
        public string CarrierProviderName
        {
            get
            {
                return carrierProviderName;
            }
            set
            {
                carrierProviderName = value;

                if (comboProvider.Items.Count > 0)
                {
                    comboProvider.Items[1] = CarrierProviderName;
                }
            }
        }

        /// <summary>
        /// The info message to display next to the box when carrier insurance is selected
        /// </summary>
        public string CarrierMessage
        {
            get
            {
                return carrierMessage.Text;
            }
            set
            {
                carrierMessage.Text = value;
            }
        }
    }
}

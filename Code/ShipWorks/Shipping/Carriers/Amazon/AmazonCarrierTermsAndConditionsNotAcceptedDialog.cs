using System.Collections.Generic;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Dialog box that displays a list of carriers for which the terms and condtions have not been accepted
    /// </summary>
    public partial class AmazonCarrierTermsAndConditionsNotAcceptedDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCarrierTermsAndConditionsNotAcceptedDialog"/> class.
        /// </summary>
        /// <param name="carrierNames">The carrier names.</param>
        public AmazonCarrierTermsAndConditionsNotAcceptedDialog(IEnumerable<string> carrierNames)
        {
            InitializeComponent();

            // Property used for unit testing.
            CarrierNames = carrierNames;

            carriersLabel.Text = $"{string.Join("\n", CarrierNames)}";

            carriersLabel.Top = carrierNamesMessageLabel.Bottom + 5;

            howToFixMessageLabel.Top = carriersLabel.Bottom + 5;

            okButton.Top = howToFixMessageLabel.Bottom + 5;

            infoPictureBox.Top = ((carrierNamesMessageLabel.Bottom + howToFixMessageLabel.Top) / 2) - infoPictureBox.Height / 2;

            Height = howToFixMessageLabel.Bottom + 60;
        }

        /// <summary>
        /// Gets the carrier names.
        /// </summary>
        /// <value>
        /// The carrier names.
        /// </value>
        public IEnumerable<string> CarrierNames { private set; get; }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}

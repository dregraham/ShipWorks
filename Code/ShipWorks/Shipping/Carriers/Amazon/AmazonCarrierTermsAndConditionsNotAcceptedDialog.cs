using System.Collections.Generic;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public partial class AmazonCarrierTermsAndConditionsNotAcceptedDialog : Form
    {
        public AmazonCarrierTermsAndConditionsNotAcceptedDialog(List<string> carrierNames)
        {
            InitializeComponent();

            // Property used for unit testing.
            CarrierNames = carrierNames;

            carriersLabel.Text = $"{string.Join("\n", CarrierNames)}";

            howToFixMessageLabel.Top = carriersLabel.Bottom + 5;
        }

        public List<string> CarrierNames { private set; get; }

    }
}

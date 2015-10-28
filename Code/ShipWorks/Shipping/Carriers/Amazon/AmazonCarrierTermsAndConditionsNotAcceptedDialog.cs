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
        public AmazonCarrierTermsAndConditionsNotAcceptedDialog(List<string> carrierNames)
        {
            InitializeComponent();

            // Property used for unit testing.
            CarrierNames = carrierNames;

            carriersLabel.Text = $"{string.Join("\n", CarrierNames)}";

            howToFixMessageLabel.Top = carriersLabel.Bottom + 5;
        }

        /// <summary>
        /// Gets the carrier names.
        /// </summary>
        /// <value>
        /// The carrier names.
        /// </value>
        public List<string> CarrierNames { private set; get; }
    }
}

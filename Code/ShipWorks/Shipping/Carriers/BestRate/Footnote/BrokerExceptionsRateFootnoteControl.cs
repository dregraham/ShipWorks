using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    public partial class BrokerExceptionsRateFootnoteControl : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerExceptionsRateFootnoteControl" /> class.
        /// </summary>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        public BrokerExceptionsRateFootnoteControl(IEnumerable<BrokerException> brokerExceptions,
            BrokerExceptionSeverityLevel severityLevel)
        {
            InitializeComponent();

            // Assuming that the brokerExceptions passed in have been properly sorted by now.
            BrokerExceptions = brokerExceptions;
            pictureBox.Image = EnumHelper.GetImage(severityLevel);
        }

        /// <summary>
        /// Gets the broker exceptions.
        /// </summary>
        public IEnumerable<BrokerException> BrokerExceptions { get; private set; }

        /// <summary>
        /// Called when the "More info" link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnMoreInfoClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (BestRateMissingRatesDialog dialog = new BestRateMissingRatesDialog(BrokerExceptions))
            {
                dialog.ShowDialog(this);
            }
        }
    }
}

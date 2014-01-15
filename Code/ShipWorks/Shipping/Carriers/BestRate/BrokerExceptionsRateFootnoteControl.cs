using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Interapptive.Shared.Utility;

using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BrokerExceptionsRateFootnoteControl : RateFootnoteControl
    {
        private readonly IEnumerable<BrokerException> brokerExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerExceptionsRateFootnoteControl" /> class.
        /// </summary>
        /// <param name="brokerExceptions">The broker exceptions.</param>
        public BrokerExceptionsRateFootnoteControl(IEnumerable<BrokerException> brokerExceptions)
        {
            InitializeComponent();

            this.brokerExceptions = brokerExceptions;

            // This assumes that the brokerExceptions passed in have been properly sorted by now.
            pictureBox.Image = EnumHelper.GetImage(brokerExceptions.FirstOrDefault().SeverityLevel);
        }

        /// <summary>
        /// Called when the "More info" link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnMoreInfoClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (BestRateErrorDialog dialog = new BestRateErrorDialog(brokerExceptions))
            {
                dialog.ShowDialog(this);
            }
        }
    }
}

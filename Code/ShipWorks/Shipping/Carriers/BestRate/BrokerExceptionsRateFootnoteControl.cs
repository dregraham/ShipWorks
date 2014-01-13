using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class BrokerExceptionsRateFootnoteControl : RateFootnoteControl
    {
        private readonly IEnumerable<ShippingException> shippingExceptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerExceptionsRateFootnoteControl"/> class.
        /// </summary>
        /// <param name="shippingExceptions">The shipping exceptions.</param>
        public BrokerExceptionsRateFootnoteControl(IEnumerable<ShippingException> shippingExceptions)
        {
            InitializeComponent();
            
            this.shippingExceptions = shippingExceptions;
        }

        /// <summary>
        /// Called when the "More info" link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnMoreInfoClick(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("ShipWorks encountered errors while getting rates:{0}{0}", Environment.NewLine);

            foreach (ShippingException shippingException in shippingExceptions)
            {
                message.AppendFormat("- {0}{1}{1}", shippingException.Message, Environment.NewLine);
            }

            MessageHelper.ShowMessage(this, message.ToString());
        }
    }
}

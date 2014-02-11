using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Shows a general exception while getting rates.
    /// </summary>
    public partial class ExceptionsRateFootnoteControl : RateFootnoteControl
    {
        private readonly string errorMessageText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteControl"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public ExceptionsRateFootnoteControl(string errorMessage)
        {
            errorMessageText = errorMessage;

            InitializeComponent();
        }

        /// <summary>
        /// Called when [click exceptions link].
        /// </summary>
        private void OnClickExceptionsLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageHelper.ShowInformation(this, errorMessageText);
        }
    }
}

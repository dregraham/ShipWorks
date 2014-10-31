using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Editing.Rating
{
    /// <summary>
    /// Shows a general exception while getting rates.
    /// </summary>
    public partial class ExceptionsRateFootnoteControl : RateFootnoteControl
    {
        private readonly Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteControl" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ExceptionsRateFootnoteControl(Exception exception)
        {
            this.exception = exception;

            InitializeComponent();
        }

        /// <summary>
        /// Called when [click exceptions link].
        /// </summary>
        private void OnClickExceptionsLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShippingException shippingException = exception as ShippingException;

            if (shippingException != null && !string.IsNullOrWhiteSpace(shippingException.HelpLink))
            {
                using (ExceptionsRateFootnoteDlg dialog = new ExceptionsRateFootnoteDlg(shippingException.Message, shippingException.HelpLink))
                {
                    dialog.ShowDialog(this);
                }
            }
            else
            {
                // Just show a standard message box
                MessageHelper.ShowInformation(this, exception.Message);
            }
        }
    }
}

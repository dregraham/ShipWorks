using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// EventHandler for the ProvideEmailSendDate event
    /// </summary>
    public delegate void EmailSendDateDelayProviderEventHandler(object sender, EmailSendDateDelayProviderEventArgs e);

    /// <summary>
    /// EventArgs for the ProvideEmailSendDate event
    /// </summary>
    public class EmailSendDateDelayProviderEventArgs : EventArgs
    {
        List<TemplateInput> input;
        DateTime? delayUntilDate;

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSendDateDelayProviderEventArgs(List<TemplateInput> input)
        {
            this.input = input;
        }

        /// <summary>
        /// The data input into the template
        /// </summary>
        public List<TemplateInput> TemplateInput
        {
            get { return input; }
        }

        /// <summary>
        /// Get or set the DateTime that email sending will be delayed until.  If null, there will be no delay.
        /// </summary>
        public DateTime? DelayUntilDate
        {
            get { return delayUntilDate; }
            set { delayUntilDate = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ShipWorks.Email;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// Event delegate for the EmailGenerationCompleted event
    /// </summary>
    public delegate void EmailGenerationCompletedEventHandler(object sender, EmailGenerationCompletedEventArgs e);

    /// <summary>
    /// EventArgs for the EmailGenerationCompleted event
    /// </summary>
    public class EmailGenerationCompletedEventArgs : AsyncCompletedEventArgs
    {
        // The successfully generated email messages
        List<EmailOutboundEntity> emailsGenerated;

        // Indicates if any messages were not sent due to permissions problems
        int securityDenials;

        /// <summary>
        /// Constrctor
        /// </summary>
        public EmailGenerationCompletedEventArgs(List<EmailOutboundEntity> emailsGenerated, int securityDenials, Exception error, bool canceled, object userState)
            : base(error, canceled, userState)
        {
            this.emailsGenerated = emailsGenerated;
            this.securityDenials = securityDenials;
        }

        /// <summary>
        /// The emails successfully generated
        /// </summary>
        public List<EmailOutboundEntity> EmailsGenerated
        {
            get { return emailsGenerated; }
        }

        /// <summary>
        /// Indicates if any messages could not be sent due to security permission problems
        /// </summary>
        public int SecurityDenials
        {
            get { return securityDenials; }
        }
    }
}

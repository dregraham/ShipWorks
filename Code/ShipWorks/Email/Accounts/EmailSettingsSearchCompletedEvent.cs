using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Event delegate for the EmailSettingsSearchCompleted event
    /// </summary>
    public delegate void EmailSettingsSearchCompletedEventHandler(object sender, EmailSettingsSearchCompletedEventArgs e);

    /// <summary>
    /// EventArgs for the EmailSettingsSearchCompleted event
    /// </summary>
    public class EmailSettingsSearchCompletedEventArgs : AsyncCompletedEventArgs
    {
        EmailSettingsSearchResult result;

        /// <summary>
        /// Constrctor
        /// </summary>
        public EmailSettingsSearchCompletedEventArgs(EmailSettingsSearchResult result, Exception error, bool canceled, object userState)
            : base(error, canceled, userState)
        {
            this.result = result;
        }

        /// <summary>
        /// The result of the settings search
        /// </summary>
        public EmailSettingsSearchResult SearchResult
        {
            get { return result; }
        }
    }
}

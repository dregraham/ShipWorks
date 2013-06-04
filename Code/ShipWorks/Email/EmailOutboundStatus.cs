using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Interapptive.Shared.Utility;

namespace ShipWorks.Email
{
    /// <summary>
    /// Status of an outgoing email message
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EmailOutboundStatus
    {
        /// <summary>
        /// The message is ready to send, but no attempt has been made yet to send it.
        /// </summary>
        [Description("Unsent")]
        [ImageResource("mail2")]
        Ready = 0,

        /// <summary>
        /// The message has been successfully sent.
        /// </summary>
        [Description("Sent")]
        [ImageResource("mail_ok")]
        Sent = 1,

        /// <summary>
        /// The last attempt at sending the message failed.
        /// </summary>
        [Description("Error")]
        [ImageResource("mail_error")]
        Failed = 2
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Specifies how a token will be used in the context of ShipWorks
    /// </summary>
    public enum TokenUsage
    {
        /// <summary>
        /// Generic list of suggested tokens.
        /// </summary>
        Generic,

        /// <summary>
        /// The token is being used as a shipment reference number or Endicia rubber stamp.
        /// </summary>
        ShippingReference,

        /// <summary>
        /// The token is being used file or folder name.
        /// </summary>
        FileName,

        /// <summary>
        /// The token is being used in the subject of an email.
        /// </summary>
        EmailSubject,

        /// <summary>
        /// The token is being used as an email address.
        /// </summary>
        EmailAddress,

        /// <summary>
        /// The token is being used to set status of orders
        /// </summary>
        OrderStatus
    }
}

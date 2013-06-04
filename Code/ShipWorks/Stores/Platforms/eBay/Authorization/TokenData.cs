using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization
{
    /// <summary>
    /// A data transport object for an eBay token.
    /// </summary>
    public class TokenData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenData"/> class.
        /// </summary>
        public TokenData()
        {
            UserId = string.Empty;
            Key = string.Empty;
            ExpirationDate = DateTime.MinValue;
        }

        /// <summary>
        /// Gets or sets the user ID.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime ExpirationDate { get; set; }
    }
}

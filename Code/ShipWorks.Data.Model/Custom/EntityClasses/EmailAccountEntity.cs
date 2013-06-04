using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension for the EmailAccountEntity
    /// </summary>
    public partial class EmailAccountEntity
    {
        /// <summary>
        /// Get a friendly name used to represent this account to a user
        /// </summary>
        public string FriendlyName
        {
            get
            {
                return string.Format("{0} <{1}>", DisplayName, EmailAddress);
            }
        }
    }
}

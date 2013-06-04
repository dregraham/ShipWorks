using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class for builtin EndiciaAccountEntity
    /// </summary>
    public partial class EndiciaAccountEntity
    {
        /// <summary>
        /// Indicates if this account was migrated from ShipWorks2, and has not yet been updated for the new API
        /// </summary>
        public bool IsDazzleMigrationPending
        {
            get { return string.IsNullOrEmpty(AccountNumber) && string.IsNullOrEmpty(SignupConfirmation); }
        }
    }
}

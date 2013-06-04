using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class for builtin FedExAccountEntity
    /// </summary>
    public partial class FedExAccountEntity
    {
        /// <summary>
        /// Indicates if this account was migrated from ShipWorks2, and has not yet been updated for the new API
        /// </summary>
        public bool Is2xMigrationPending
        {
            get { return string.IsNullOrEmpty(MeterNumber); }
        }
    }
}

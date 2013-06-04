using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Provides nested error display information
    /// </summary>
    public abstract class EntityGridRowErrorProvider
    {
        /// <summary>
        /// Get the error message for the given row.  Return null to indicate no error.
        /// </summary>
        public abstract string GetError(EntityBase2 entity);
    }
}

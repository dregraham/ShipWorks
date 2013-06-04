using System;
using System.Collections.Generic;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Transactions;
using System.Diagnostics;

namespace ShipWorks.Data.Adapter
{
    public partial class DataAccessAdapter
    {
        /// <summary>
        /// Get the persistence information for the given field.
        /// </summary>
        public static IFieldPersistenceInfo GetPersistenceInfo(IEntityField2 field)
        {
            using (TransactionScope suppress = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (DataAccessAdapter adapter = new DataAccessAdapter())
                {
                    return adapter.GetFieldPersistenceInfo(field);
                }
            }
        }
    }
}

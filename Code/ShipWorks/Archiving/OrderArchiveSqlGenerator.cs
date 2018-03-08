using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Class used for generating SQL for order archiving
    /// </summary>
    public class OrderArchiveSqlGenerator : IOrderArchiveSqlGenerator
    {
        /// <summary>
        /// Generate and return SQL for copying a database
        /// </summary>
        public string CopyDatabaseSql()
        {
            return ResourceUtility.ReadString("ShipWorks.Archiving.CopyDatabase.sql");
        }

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        public string ArchiveOrderDataSql(DateTime maxOrderDate)
        {
            return string.Format(ResourceUtility.ReadString("ShipWorks.Archiving.ArchiveOrderData.sql"), maxOrderDate.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}

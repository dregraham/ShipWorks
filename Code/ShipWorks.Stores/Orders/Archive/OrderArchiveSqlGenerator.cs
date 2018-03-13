using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Class used for generating SQL for order archiving
    /// </summary>
    [Component]
    public class OrderArchiveSqlGenerator : IOrderArchiveSqlGenerator
    {
        /// <summary>
        /// Generate and return SQL for copying a database
        /// </summary>
        public string CopyDatabaseSql()
        {
            return ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.CopyDatabase.sql");
        }

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        public string ArchiveOrderDataSql(DateTime maxOrderDate)
        {
            return string.Format(ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.ArchiveOrderData.sql"),
                maxOrderDate.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}

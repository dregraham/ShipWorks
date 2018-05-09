using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// ArchvieManagerViewModel for design mode
    /// </summary>
    public class DesignModeArchiveManagerViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeArchiveManagerViewModel()
        {
            Archives = new DesignModeArchiveManagerArchive[] {
                (new DateTime(2018, 1, 1, 12, 30, 0), new DateTime(2018, 1, 31, 12, 30, 0), 12),
                (new DateTime(2018, 2, 1, 12, 30, 0), new DateTime(2018, 2, 28, 12, 30, 0), 123),
                (new DateTime(2018, 3, 1, 12, 30, 0), new DateTime(2018, 3, 30, 12, 30, 0), 1212),
                (new DateTime(2018, 4, 1, 12, 30, 0), new DateTime(2018, 4, 30, 12, 30, 0), 12235423),
                (new DateTime(2018, 5, 1, 12, 30, 0), new DateTime(2018, 5, 30, 12, 30, 0), 1232)
            };
        }

        /// <summary>
        /// Selected cutoff date for order archiving
        /// </summary>
        public IEnumerable<DesignModeArchiveManagerArchive> Archives { get; set; }
    }

    public class DesignModeArchiveManagerArchive
    {
        public DateTime FirstOrderDate { get; set; }
        public DateTime LastOrderDate { get; set; }
        public long OrderCount { get; set; }

        public static implicit operator DesignModeArchiveManagerArchive((DateTime oldestOrder, DateTime newestOrder, long orderCount) value) =>
            new DesignModeArchiveManagerArchive { FirstOrderDate = value.oldestOrder, LastOrderDate = value.newestOrder, OrderCount = value.orderCount };
    }
}

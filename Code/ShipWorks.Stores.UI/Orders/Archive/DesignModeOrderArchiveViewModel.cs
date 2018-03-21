using System;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// OrderArchiveViewModel for design mode
    /// </summary>
    public class DesignModeOrderArchiveViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeOrderArchiveViewModel()
        {
            ArchiveDate = DateTime.Now;
            OrderCounts = 1_234_567;
        }

        /// <summary>
        /// Selected cutoff date for order archiving
        /// </summary>
        public DateTime ArchiveDate { get; set; }

        /// <summary>
        /// Are order counts being loaded
        /// </summary>
        public bool IsLoadingCounts { get; set; }

        /// <summary>
        /// Count of orders that will be archived
        /// </summary>
        public long OrderCounts { get; set; }
    }
}

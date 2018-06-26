using System;

namespace ShipWorks.Stores.UI.Orders.Archive
{
    /// <summary>
    /// ScheduleArchiveViewModel for design mode
    /// </summary>
    public class DesignModeScheduleArchiveViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeScheduleArchiveViewModel()
        {
            NumberOfDaysToKeep = 90;
            Enabled = true;
            DayOfWeek = DayOfWeek.Sunday;
        }

        /// <summary>
        /// Selected number of days of order data to keep
        /// </summary>
        public int NumberOfDaysToKeep { get; set; }

        /// <summary>
        /// Is auto archiving enabled?
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Day of week for the archive to be executed
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }
    }
}

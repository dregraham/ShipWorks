using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Base for all item types represented in the dashboard
    /// </summary>
    public abstract class DashboardItem
    {
        DashboardBar dashboardBar;

        /// <summary>
        /// Constructor
        /// </summary>
        protected DashboardItem()
        {

        }

        /// <summary>
        /// Initialize the item with given bar that it will display its informaiton in
        /// </summary>
        public virtual void Initialize(DashboardBar dashboardBar)
        {
            this.dashboardBar = dashboardBar;
        }

        /// <summary>
        /// Gives the item a chance to do custom processing on dismissal.
        /// </summary>
        public virtual void Dismiss()
        {

        }

        /// <summary>
        /// The dashboard bar that the item renders itself to
        /// </summary>
        public DashboardBar DashboardBar
        {
            get { return dashboardBar; }
        }
    }
}

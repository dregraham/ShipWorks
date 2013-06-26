using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.WindowsServices;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    public class DashboardServiceStoppedItem : DashboardItem
    {
        private readonly ShipWorksStatusDialog statusDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardServiceStoppedItem"/> class.
        /// </summary>
        /// <param name="statusDialog">The status dialog.</param>
        public DashboardServiceStoppedItem(ShipWorksStatusDialog statusDialog)
        {
            this.statusDialog = statusDialog;
        }

        /// <summary>
        /// Initialize the item with given bar that it will display its informaiton in
        /// </summary>
        /// <param name="dashboardBar">The UI element associated with ths dashboard item.</param>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);
            dashboardBar.CanUserDismiss = false;
        }
    }
}

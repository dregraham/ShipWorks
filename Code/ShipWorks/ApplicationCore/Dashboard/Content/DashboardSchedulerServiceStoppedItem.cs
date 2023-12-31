﻿using ShipWorks.ApplicationCore.Services.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    public class DashboardSchedulerServiceStoppedItem : DashboardItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardSchedulerServiceStoppedItem" /> class.
        /// </summary>
        public DashboardSchedulerServiceStoppedItem()
        { }

        /// <summary>
        /// Initialize the item with given bar that it will display its information in
        /// </summary>
        /// <param name="dashboardBar">The UI element associated with this dashboard item.</param>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);
            
            DashboardBar.CanUserDismiss = false;

            DashboardBar.Image = Resources.gear_stop_16;
            DashboardBar.PrimaryText = "Actions";

            DashboardBar.SecondaryText = "A required ShipWorks action scheduler is not running.";

            List<DashboardAction> dashboardActions = new List<DashboardAction> { new DashboardActionMethod("[link]More info[/link]", OnMoreInfo) };
            DashboardBar.ApplyActions(dashboardActions);
        }

        /// <summary>
        /// Called when the [More Info] link of the DashboardBar is clicked to show the service status dialog.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="userState">State of the user.</param>
        private void OnMoreInfo(Control owner, object userState)
        {
          using (ServiceStatusDialog dialog = new ServiceStatusDialog())
          {
              dialog.ShowDialog(owner);
          }
        }
    }
}

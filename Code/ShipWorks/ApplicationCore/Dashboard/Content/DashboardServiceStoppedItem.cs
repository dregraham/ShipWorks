using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    public class DashboardServiceStoppedItem : DashboardItem
    {
        private readonly List<WindowsServiceEntity> schedulerEntities;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardServiceStoppedItem" /> class.
        /// </summary>
        /// <param name="schedulers">The schedulers that are not running.</param>
        public DashboardServiceStoppedItem(IEnumerable<WindowsServiceEntity> schedulers)
        {
            schedulerEntities = new List<WindowsServiceEntity>(schedulers);
        }

        /// <summary>
        /// Initialize the item with given bar that it will display its information in
        /// </summary>
        /// <param name="dashboardBar">The UI element associated with ths dashboard item.</param>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);
            
            DashboardBar.CanUserDismiss = false;

            DashboardBar.Image = Resources.gear_stop_16;
            DashboardBar.PrimaryText = "Schedulers";

            bool usePlural = schedulerEntities.Count > 1;
            DashboardBar.SecondaryText = string.Format("There {0} {1} ShipWorks scheduling service{2} not running.", usePlural ? "are" : "is", schedulerEntities.Count, usePlural ? "s" : string.Empty);

            List<DashboardAction> dashboardActions = new List<DashboardAction> { new DashboardActionMethod("[link]More Info[/link]", OnMoreInfo) };
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

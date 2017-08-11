using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Properties;
using System.Reflection;
using Autofac;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// An item in the dashboard that shows that there is a new version of ShipWorks available
    /// </summary>
    class DashboardOnlineVersionItem : DashboardItem
    {
        ShipWorksOnlineVersion online;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardOnlineVersionItem(ShipWorksOnlineVersion online)
        {
            this.online = online;
        }

        /// <summary>
        /// The version of ShipWorks the dashboard item is displaying
        /// </summary>
        public ShipWorksOnlineVersion OnlineVersion
        {
            get { return online; }
            set
            {
                online = value;

                UpdateVersionDisplay();
            }
        }

        /// <summary>
        /// Set the dashboard bar that the item will display its content in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            dashboardBar.Image = Resources.box_software;
            dashboardBar.SecondaryText = "is now available.";
            
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                if (lifetimeScope.Resolve<IUserSession>().User.IsAdmin)
                {
                    DashboardBar.ApplyActions(new List<DashboardAction>
                    {
                        new DashboardActionUrl("[link]Download now [/link] or see", online.DownloadUrl),
                        new DashboardActionUrl("[link]what's new[/link].", online.WhatsNewUrl)
                    });
                }
            }
            
            UpdateVersionDisplay();
        }

        /// <summary>
        /// Update the version displayed in the bar
        /// </summary>
        private void UpdateVersionDisplay()
        {
            DashboardBar.PrimaryText = "ShipWorks " + online.Version.ToString();
        }

        /// <summary>
        /// The dashboard item is being dismissed
        /// </summary>
        public override void Dismiss()
        {
            base.Dismiss();

            // Signoff on having seen this version
            ShipWorksOnlineVersionChecker.Signoff(online.Version);
        }
    }
}

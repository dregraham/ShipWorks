﻿using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Properties;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Dashboard item representing a notifcation to the user 
    /// that they can now sign up for a one balance accout
    /// </summary>
    public class DashboardOneBalancePromoItem : DashboardItem
    {
        /// <summary>
        /// Initialization
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            DashboardBar.Image = Resources.information16;
            DashboardBar.PrimaryText = "Save up to 66% on shipping through UPS from ShipWorks.";
            DashboardBar.SecondaryText = string.Empty;
            DashboardBar.ApplyActions(new List<DashboardAction> { new DashboardActionMethod("Click here to [link]learn more.[/link]", OnViewHelpArticle) });

            DashboardBar.CanUserDismiss = true;
        }

        /// <summary>
        /// Open the default web browser to the help article
        /// </summary>
        private void OnViewHelpArticle(Control owner, object userState)
        {
            System.Diagnostics.Process.Start("https://support.shipworks.com/hc/en-us/articles/360040449091");
        }
    }
}

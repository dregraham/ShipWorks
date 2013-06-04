using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ShipWorks.Properties;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a dashboard item for displaying informational messages from ShipWorks
    /// </summary>
    class DashboardLocalMessageItem : DashboardMessageItem
    {
        string identifier;
        DashboardMessageImageType imageType;
        string primaryText;
        string secondaryText;
        DashboardAction[] actions;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardLocalMessageItem(string identifier, DashboardMessageImageType imageType, string primaryText, string secondaryText, params DashboardAction[] actions)
            : base(DateTime.UtcNow)
        {
            this.identifier = identifier;
            this.imageType = imageType;
            this.primaryText = primaryText;
            this.secondaryText = secondaryText;
            this.actions = actions;
        }

        /// <summary>
        /// Sets the bar that the item will be displayed in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            dashboardBar.Image = GetImage(imageType);
            dashboardBar.PrimaryText = primaryText;
            dashboardBar.SecondaryText = FormatSecondaryText(secondaryText);
            dashboardBar.ApplyActions(actions);
        }

        /// <summary>
        /// The identifier of the local message
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
        }
    }
}

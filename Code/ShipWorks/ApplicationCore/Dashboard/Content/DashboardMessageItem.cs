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
    /// Base class for information messages displayed in ShipWorks.  Can be local messages or from the server.
    /// </summary>
    abstract class DashboardMessageItem : DashboardItem
    {
        DateTime timestamp;

        /// <summary>
        /// Constructor
        /// </summary>
        protected DashboardMessageItem(DateTime timestamp)
        {
            this.timestamp = timestamp;
        }

        /// <summary>
        /// The date\time the message was issued
        /// </summary>
        public DateTime Timestamp
        {
            get { return timestamp; }
        }

        /// <summary>
        /// Should the item try to use a friendly date/time for when the item was added.
        /// </summary>
        public bool UseFriendlyDateTime { get; set; } = true;

        /// <summary>
        /// Format the secondary text to display to include the timestamp
        /// </summary>
        protected string FormatSecondaryText(string text)
        {
            DateTime local = timestamp.ToLocalTime();
            string timeText;

            if (UseFriendlyDateTime && local.Date == DateTime.Now.Date)
            {
                timeText = string.Format("Today {0:t}", local);
            }
            else if (UseFriendlyDateTime && local.Date == DateTime.Now.AddDays(-1).Date)
            {
                timeText = string.Format("Yesterday {0:t}", local);
            }
            else
            {
                timeText = local.ToString("g");
            }

            return string.Format("({0}) {1}", timeText, text);
        }

        /// <summary>
        /// Determine the appropriate image to use
        /// </summary>
        protected Image GetImage(DashboardMessageImageType imageType)
        {
            switch (imageType)
            {
                case DashboardMessageImageType.Information:
                    return Resources.information16;

                case DashboardMessageImageType.Success:
                    return Resources.check16;

                case DashboardMessageImageType.Error:
                    return Resources.error16;

                case DashboardMessageImageType.Warning:
                    return Resources.warning16;

                case DashboardMessageImageType.Exclamation:
                    return Resources.exclamation16;

                case DashboardMessageImageType.Canceled:
                    return Resources.cancel16;

                case DashboardMessageImageType.LightBulb:
                    return Resources.lightbulb16;
            }

            return Resources.information16;
        }
    }
}

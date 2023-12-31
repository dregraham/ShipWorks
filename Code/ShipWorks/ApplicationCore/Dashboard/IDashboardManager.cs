﻿using ShipWorks.ApplicationCore.Dashboard.Content;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Dashboard manager
    /// </summary>
    public interface IDashboardManager
    {
        /// <summary>
        /// Show a local dashboard message
        /// </summary>
        void ShowLocalMessage(string identifier, DashboardLocalMessageDetails options, params DashboardAction[] actions);
    }
}

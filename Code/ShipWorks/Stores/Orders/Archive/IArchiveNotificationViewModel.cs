﻿using System.Windows.Forms.Integration;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// View Model for the Archive Notification View Model
    /// </summary>
    public interface IArchiveNotificationViewModel
    {
        /// <summary>
        /// Show the archive notification
        /// </summary>
        void Show(ElementHost host);
    }
}
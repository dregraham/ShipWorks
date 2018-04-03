using System;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// View Model for the Archive Notification View Model
    /// </summary>
    [Component]
    public class ArchiveNotificationViewModel : IArchiveNotificationViewModel
    {
        readonly Func<IArchiveNotification> createControl;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveNotificationViewModel(Func<IArchiveNotification> createControl)
        {
            this.createControl = createControl;
        }

        /// <summary>
        /// Show the archive notification
        /// </summary>
        public void Show(ElementHost host)
        {
            createControl().AddTo(host, this);
        }
    }
}
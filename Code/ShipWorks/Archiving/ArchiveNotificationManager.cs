using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Manage the archive notification
    /// </summary>
    [Component]
    public class ArchiveNotificationManager : IArchiveNotificationManager
    {
        private readonly Panel container;
        private readonly IConfigurationData configurationData;
        private readonly Func<IArchiveNotificationViewModel> createViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchiveNotificationManager(Panel container, IConfigurationData configurationData, Func<IArchiveNotificationViewModel> createViewModel)
        {
            this.createViewModel = createViewModel;
            this.container = container;
            this.configurationData = configurationData;
        }

        /// <summary>
        /// Show the archive notification banner, if necessary
        /// </summary>
        public void ShowIfNecessary()
        {
            if (container.InvokeRequired)
            {
                container.Invoke((Action) ShowIfNecessary);
                return;
            }

            if (configurationData.IsArchive())
            {
                AddArchiveNotificationControlIfNecessary();
                container.Visible = true;
            }
            else
            {
                container.Visible = false;
            }
        }

        /// <summary>
        /// Refresh the UI
        /// </summary>
        public void RefreshUI()
        {
            if (container.Visible)
            {
                // This seems to force a complete redraw of the whole control hierarchy, 
                // while invalidating the control (and it's WPF child) did not.
                container.Visible = false;
                container.Visible = true;
            }
        }

        /// <summary>
        /// Reset the notification
        /// </summary>
        public void Reset()
        {
            container.Visible = false;
        }

        /// <summary>
        /// Add the archive notification control to the panel, if necessary
        /// </summary>
        private void AddArchiveNotificationControlIfNecessary()
        {
            if (container.Controls.OfType<Control>().Any())
            {
                return;
            }

            var host = new ElementHost
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            createViewModel().Show(host);
            container.Controls.Add(host);
        }
    }
}

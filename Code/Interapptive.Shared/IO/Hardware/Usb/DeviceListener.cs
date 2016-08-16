using System;
using System.Management;

namespace Interapptive.Shared.Usb
{
    /// <summary>
    /// Listen for device attached and configuration changed events
    /// </summary>
    public class DeviceListener : IDisposable
    {
        // Listen for device added (2) and device configuration changed (1) events
        private const string eventSelectionQuery = "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 1";
        private readonly ManagementEventWatcher deviceWatcher;

        /// <summary>
        /// Create an instance of the DeviceListener class
        /// </summary>
        public DeviceListener()
        {
            deviceWatcher = new ManagementEventWatcher();
            deviceWatcher.EventArrived += OnDeviceWatcherEventArrived;
            deviceWatcher.Query = new WqlEventQuery(eventSelectionQuery);
        }

        /// <summary>
        /// A device was attached or configured
        /// </summary>
        public event EventHandler DeviceChanged;

        /// <summary>
        /// A device was added or configuration was changed
        /// </summary>
        void OnDeviceWatcherEventArrived(object sender, EventArrivedEventArgs e)
        {
            if (sender == deviceWatcher)
            {
                DeviceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Start the device watcher
        /// </summary>
        internal void Start()
        {
            deviceWatcher.Start();
        }

        /// <summary>
        /// Dispose the instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the instance
        /// </summary>
        /// <param name="disposing">Is the object already being disposed?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (deviceWatcher != null)
            {
                // Disposing does not stop watching for events, so we need to manually do that
                deviceWatcher.Stop();
                deviceWatcher.EventArrived -= OnDeviceWatcherEventArrived;
                deviceWatcher.Dispose();
            }
        }
    }
}

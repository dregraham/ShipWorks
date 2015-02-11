using System;
using System.Threading;
using Interapptive.Shared.Usb;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Utility class for reading the weight from an external scale
    /// </summary>
    public static class ScaleReader
    {
        static readonly ScaleReadResult notFoundResult = new ScaleReadResult(ScaleReadStatus.NotFound, "Could not find a compatible scale, the scale is in motion, or the scale is being used by another application.");

        static ScaleUsbReader usbReader;
        static ScaleSerialPortReader serialReader;

        static volatile ScaleReadResult lastResult = notFoundResult;

        static object threadLock = new object();
        private static readonly DeviceListener DeviceListener = new DeviceListener();

        /// <summary>
        /// Static constructor
        /// </summary>
        static ScaleReader()
        {
            DeviceListener.DeviceChanged += OnDeviceListenerDeviceChanged;
            SetUsbScale();
        }

        /// <summary>
        /// Attempt to set up a usb scale after a device was attached or configuration was changed
        /// </summary>
        private static void OnDeviceListenerDeviceChanged(object sender, EventArgs e)
        {
            SetUsbScale();
        }

        /// <summary>
        /// Attempt to get a reference to a usb scale
        /// </summary>
        private static void SetUsbScale()
        {
            // Don't bother setting up the scale if we already have one
            if (usbReader != null)
            {
                return;
            }
            
            lock (threadLock)
            {
                // If the scale was already set up, just bail
                if (usbReader != null)
                {
                    return;
                }

                // Try and get a reading from the usb scale
                usbReader = new ScaleUsbReader();
                ScaleReadResult usbResult = usbReader.ReadScale();

                // Success, return result now
                if (usbResult.Status == ScaleReadStatus.NotFound)
                {
                    // Didn't work, clear it
                    usbReader = null;
                }
            }
        }

        /// <summary>
        /// Read from any attached scale that can be found
        /// </summary>
        public static ScaleReadResult ReadScale()
        {
            return ReadScale(false);
        }

        /// <summary>
        /// Read from any attached scale that can be found
        /// </summary>
        public static ScaleReadResult ReadScale(bool isPolling)
        {
            // If we can't take the lock don't worry about it - we'll just immediately return whatever the most recent result was.   This prevents oodles of threads and UI from trying to 
            // read all at the same time, and also prevents blocking (which was happening, and causing hangs
            if (Monitor.TryEnter(threadLock))
            {
                try
                {
                    lastResult = InternalReadScale(isPolling);
                }
                finally
                {
                    Monitor.Exit(threadLock);
                }
            }

            return lastResult;
        }

        /// <summary>
        /// The internal function that does the actual scale reading
        /// </summary>
        private static ScaleReadResult InternalReadScale(bool isPolling)
        {
            if (isPolling && serialReader != null)
            {
                return new ScaleReadResult(ScaleReadStatus.NotFound, "Serial scales are not read during polling.");
            }

            ScaleReadResult existingResult = ReadExisting();
            if (existingResult.Status != ScaleReadStatus.NotFound)
            {
                return existingResult;
            }
            else
            {
                // Reading from the existing didn't work, clear them
                usbReader = null;
                serialReader = null;
            }

            // Try usb first
            if (usbReader != null)
            {
                ScaleReadResult usbResult = usbReader.ReadScale();

                // Success, return result now
                if (usbResult.Status != ScaleReadStatus.NotFound)
                {
                    return usbResult;
                }
                
                // Didn't work, clear it
                usbReader = null;
            }
            
            // Read serial - but not as a background polling
            serialReader = new ScaleSerialPortReader();
            ScaleReadResult serialResult = (!isPolling) ? serialReader.ReadScale() : new ScaleReadResult(ScaleReadStatus.NotFound, "Serial scales are not read during polling.");

            // Success, return result now
            if (serialResult.Status != ScaleReadStatus.NotFound)
            {
                return serialResult;
            }
            else
            {
                // Didn't work, clear it
                serialReader = null;
            }

            // If we get here, we did not find a valid scale from which to read the weight
            return notFoundResult;
        }

        /// <summary>
        /// Read from an existing previously succesfull scale
        /// </summary>
        private static ScaleReadResult ReadExisting()
        {
            // Previously successful USB
            if (usbReader != null)
            {
                return usbReader.ReadScale();
            }

            // Previously successful serial
            if (serialReader != null)
            {
                return serialReader.ReadScale();
            }

            return new ScaleReadResult(ScaleReadStatus.NotFound, "");
        }
    }
}

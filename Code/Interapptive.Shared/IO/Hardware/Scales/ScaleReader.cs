using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Usb;
using log4net;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Utility class for reading the weight from an external scale
    /// </summary>
    public static class ScaleReader
    {
        const int MaxScaleConnectionAttempts = 20;

        static readonly ILog log = LogManager.GetLogger(typeof(ScaleReader));

        static readonly ScaleReadResult unknownNotFoundResult = ScaleReadResult.NotFound(string.Empty);
        static readonly ScaleReadResult serialScalesNotPolledResult =
            ScaleReadResult.NotFound("Serial scales are not read during polling.");
        static readonly ScaleReadResult notFoundResult = ScaleReadResult.NotFound(
            "Could not find a compatible scale, the scale is in motion, or the scale is being used by another application.");

        static ScaleUsbReader usbReader;
        static ScaleSerialPortReader serialReader;

        static ScaleReadResult lastResult = notFoundResult;

        static object threadLock = new object();
        static readonly DeviceListener DeviceListener = new DeviceListener();

        public static IObservable<ScaleReadResult> ReadEvents { get; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static ScaleReader()
        {
            DeviceListener.DeviceChanged += OnDeviceListenerDeviceChanged;

            TaskEx.Run(() => SetUsbScale());

            ReadEvents = Observable.Interval(TimeSpan.FromMilliseconds(250))
                .Select(_ => ReadScale(true))
                .Publish().RefCount();
        }

        /// <summary>
        /// Attempt to set up a usb scale after a device was attached or configuration was changed
        /// </summary>
        private static void OnDeviceListenerDeviceChanged(object sender, EventArgs e) => SetUsbScale();

        /// <summary>
        /// Attempt to get a reference to a usb scale
        /// </summary>
        private static void SetUsbScale()
        {
            log.InfoFormat("Device attached or changed");

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

                int currentAttempt = 0;
                const int attemptDelayInMilliseconds = 200;

                // Try to connect to the scale for a period of time, as some scales (and computers) take
                // longer to connect than others.
                while (usbReader == null && currentAttempt < MaxScaleConnectionAttempts)
                {
                    currentAttempt++;

                    // Try and get a reading from the usb scale
                    usbReader = new ScaleUsbReader();
                    ScaleReadResult usbResult = usbReader.ReadScale();

                    // Success, return result now
                    if (usbResult.Status == ScaleReadStatus.NotFound)
                    {
                        // Didn't work, clear it and wait to try again
                        usbReader = null;
                        Thread.Sleep(attemptDelayInMilliseconds * currentAttempt);
                    }
                }

                log.DebugFormat("Scale {0} after {1} attempts",
                    usbReader == null ? "could not be attached" : "attached successfully",
                    currentAttempt);
            }
        }

        /// <summary>
        /// Read from any attached scale that can be found
        /// </summary>
        public static Task<ScaleReadResult> ReadScale() => TaskEx.Run(() => ReadScale(false));

        /// <summary>
        /// Read from any attached scale that can be found
        /// </summary>
        private static ScaleReadResult ReadScale(bool isPolling)
        {
            // If we can't take the lock don't worry about it - we'll just immediately return whatever the most recent result was.
            // This prevents oodles of threads and UI from trying to read all at the same time,
            // and also prevents blocking (which was happening, and causing hangs
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
                return serialScalesNotPolledResult;
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
            ScaleReadResult serialResult = (!isPolling) ?
                serialReader.ReadScale() :
                serialScalesNotPolledResult;

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
        /// Read from an existing previously successful scale
        /// </summary>
        private static ScaleReadResult ReadExisting()
        {
            return usbReader?.ReadScale() ??
                serialReader?.ReadScale() ??
                unknownNotFoundResult;
        }
    }
}

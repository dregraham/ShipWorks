﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Usb;
using Interapptive.Shared.Win32;
using log4net;

namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Utility class for reading the weight from an external scale
    /// </summary>
    public static class ScaleReader
    {
        
        private const int MaxScaleConnectionAttempts = 20;
        
        // Override default delay by setting HKEY_CURRENT_USER\SOFTWARE\Interapptive\ShipWorks\Options\ScaleReadDelayOverrideMilliseconds
        private const string DefaultScaleReadDelayMs = "250";
        private const string ScaleReadDelayOverrideRegistryName = "ScaleReadDelayOverrideMilliseconds";
        private const string RegistryKey = @"Software\Interapptive\ShipWorks\Options";
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(ScaleReader));

        private static readonly ScaleReadResult UnknownNotFoundResult = ScaleReadResult.NotFound(string.Empty);
        private static readonly ScaleReadResult SerialScalesNotPolledResult =
            ScaleReadResult.NotFound("Serial scales are not read during polling.");
        private static readonly ScaleReadResult NotFoundResult = ScaleReadResult.NotFound(
            "Could not find a compatible scale, the scale is in motion, or the scale is being used by another application.");

        private static ScaleUsbReader usbReader;
        private static ScaleSerialPortReader serialReader;
        private static ScaleCubiscanReader cubiscanReader;

        private static readonly object ThreadLock = new object();
        static readonly DeviceListener DeviceListener = new DeviceListener();
        private static ICubiscanConfigurationManager CubiscanConfigurationManager;
        
        private static IObserver<bool> scaleObserver;
        
        public static IObservable<ScaleReadResult> ReadEvents { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        static ScaleReader()
        {
            DeviceListener.DeviceChanged += OnDeviceListenerDeviceChanged;

            Task.Run(() =>
            {
                SetUsbScale();
                DeviceListener.Start();
            });
            
            TimeSpan scaleReadDelay = GetScaleReadDelay();
            
            ReadEvents = Observable.Create<bool>(x =>
            {
                scaleObserver = x;
                x.OnNext(true);
                return Disposable.Create(() => scaleObserver = null);
            })
            .Delay(scaleReadDelay)
            .Select(_ => ReadScale(true))
            .Do(_ => scaleObserver?.OnNext(true))
            .Publish().RefCount();
        }

        /// <summary>
        /// Get the time delay - allows override from registry
        /// </summary>
        private static TimeSpan GetScaleReadDelay()
        {
            var registry = new RegistryHelper(RegistryKey);
            var delayInMilliseconds = registry.GetValue(ScaleReadDelayOverrideRegistryName, DefaultScaleReadDelayMs);
            if (!int.TryParse(delayInMilliseconds, out int parsedDelay))
            {
                // This will crash ShipWorks. I'm ok with this. The only way this could happen is if the
                // Registry is set to a unparsable int.
                throw new Exception($"Couldn't parse Scale Delay value {delayInMilliseconds}. Set at HKEY_CURRENT_USER\\{RegistryKey}");
            }

            return TimeSpan.FromMilliseconds(parsedDelay);
        }

        /// <summary>
        /// Call Initialize to explicitly trigger static constructor
        /// </summary>
        public static void Initialize(ICubiscanConfigurationManager cubiscanConfigurationManager)
        {
            Log.Info("ScaleReader.Initialize called to explicitly trigger static constructor");
            CubiscanConfigurationManager = cubiscanConfigurationManager;
            cubiscanReader = new ScaleCubiscanReader();
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
            Log.InfoFormat("Device attached or changed");

            // Don't bother setting up the scale if we already have one
            if (usbReader != null || CubiscanConfigurationManager.GetConfiguration().IsConfigured)
            {
                return;
            }

            lock (ThreadLock)
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

                Log.DebugFormat("Scale {0} after {1} attempts",
                    usbReader == null ? "could not be attached" : "attached successfully",
                    currentAttempt);
            }
        }

        /// <summary>
        /// Read from any attached scale that can be found
        /// </summary>
        public static Task<ScaleReadResult> ReadScale() => Task.Run(() => ReadScale(false));

        /// <summary>
        /// Read from any attached scale that can be found
        /// </summary>
        private static ScaleReadResult ReadScale(bool isPolling)
        {
            // We can lock here because even if there were a deadlock, the UI won't hang since this is being called
            // in a task. If it were to lock indefinitely, we couldn't use the scale anyway
            lock (ThreadLock)
            {
                ScaleReadResult result = InternalReadScale(isPolling);

                if (result.Status == ScaleReadStatus.Success && result.Weight < 0)
                {
                    return ScaleReadResult.ReadError($"Weight measured less than zero ({result.Weight} lbs).", result.ScaleType);
                }

                return result;
            }
        }

        /// <summary>
        /// The internal function that does the actual scale reading
        /// </summary>
        private static ScaleReadResult InternalReadScale(bool isPolling)
        {
            var cubiscanConfiguration = CubiscanConfigurationManager.GetConfiguration();
            if (cubiscanConfiguration.IsConfigured)
            {
                return cubiscanReader.ReadScale(cubiscanConfiguration);
            }
            
            // We may be in a situation where both scales are plugged in, so we should still poll the USB scale
            if (isPolling && serialReader != null && usbReader == null)
            {
                return SerialScalesNotPolledResult;
            }

            ScaleReadResult existingResult = ReadExistingUsbOrSerial();
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
                SerialScalesNotPolledResult;

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
            return NotFoundResult;
        }

        /// <summary>
        /// Read from an existing previously successful scale
        /// </summary>
        private static ScaleReadResult ReadExistingUsbOrSerial()
        {
            return
                usbReader?.ReadScale() ??
                serialReader?.ReadScale() ??
                UnknownNotFoundResult;
        }
    }
}

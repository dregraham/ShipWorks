﻿using System;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Listens for a barcode scanner to register
    /// </summary>
    public class ScannerRegistrationListener : IScannerRegistrationListener
    {
        private readonly IScannerMessageFilterFactory scannerMessageFilterFactory;
        private IScannerMessageFilter findScannerMessageFilter;
        private readonly IUser32Devices user32Devices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerRegistrationListener"/> class.
        /// </summary>
        public ScannerRegistrationListener(IUser32Devices user32Devices,
            IScannerMessageFilterFactory scannerMessageFilterFactory)
        {
            this.user32Devices = user32Devices;
            this.scannerMessageFilterFactory = scannerMessageFilterFactory;
        }

        /// <summary>
        /// Start listening for a barcode scanner to register
        /// </summary>
        public void Start()
        {
            findScannerMessageFilter = scannerMessageFilterFactory.CreateScannerRegistrationMessageFilter();
            findScannerMessageFilter.Enable();
            
            user32Devices.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = RawInputDeviceConstants.Keyboard.UsagePage,
                Usage = RawInputDeviceConstants.Keyboard.Usage,
                Flags = (int) (RawInputDeviceNotificationFlags.Default | RawInputDeviceNotificationFlags.DeviceNotify),
                TargetHandle = (IntPtr) null
            });
        }

        /// <summary>
        /// Stop listening for a barcode scanner to register
        /// </summary>
        public void Stop()
        {
            findScannerMessageFilter.Disable();
            
            user32Devices.RegisterRawInputDevice(new RawInputDevice
            {
                UsagePage = RawInputDeviceConstants.Keyboard.UsagePage,
                Usage = RawInputDeviceConstants.Keyboard.Usage,
                Flags = (int) RawInputDeviceNotificationFlags.RemoveDevice,
                TargetHandle = (IntPtr) null
            });
        }
    }
}
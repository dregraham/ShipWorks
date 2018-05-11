﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Interop;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Common;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Message filter used by scanner service
    /// </summary>
    [Component(RegistrationType.Self)]
    public class RegisteredScannerInputHandler : IScannerMessageFilter
    {
        private readonly IUser32Input user32Input;
        private readonly IScannerIdentifier scannerIdentifier;
        private readonly IScanBuffer scanBuffer;
        private readonly IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar;
        private readonly ILog log;

        private readonly HashSet<VirtualKeys> pressedKeys = new HashSet<VirtualKeys>();
        private bool shouldBlock;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegisteredScannerInputHandler(IScannerIdentifier scannerIdentifier, IUser32Input user32Input,
            IScanBuffer scanBuffer, IWindowsMessageFilterRegistrar windowsMessageFilterRegistrar, Func<Type, ILog> getLogger)
        {
            this.scannerIdentifier = scannerIdentifier;
            this.user32Input = user32Input;
            this.scanBuffer = scanBuffer;
            this.windowsMessageFilterRegistrar = windowsMessageFilterRegistrar;
            log = getLogger(GetType());
        }

        /// <summary>
        /// Filter messages for the scanner
        /// </summary>
        public bool PreFilterMessage(ref Message message) =>
            FilterMessage(message.Msg, message.LParam, message.WParam);

        /// <summary>
        /// Enable RegisteredScannerInputHandler
        /// </summary>
        public void Disable()
        {
            windowsMessageFilterRegistrar.RemoveMessageFilter(this);
            ComponentDispatcher.ThreadFilterMessage -= OnThreadFilterMessage;
        }

        /// <summary>
        /// Disable Monitoring of scanner input
        /// </summary>
        public void Enable()
        {
            windowsMessageFilterRegistrar.AddMessageFilter(this);
            ComponentDispatcher.ThreadFilterMessage += OnThreadFilterMessage;
        }

        /// <summary>
        /// Event handler for 
        /// </summary>
        private void OnThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            handled = FilterMessage(msg.message, msg.lParam, msg.wParam);
        }

        /// <summary>
        /// Takes the components of a message and returns true if the message should be filtered out.
        /// </summary>
        /// <returns></returns>
        private bool FilterMessage(int message, IntPtr lParam, IntPtr wParam)
        {
            switch ((WindowsMessage) message)
            {
                case WindowsMessage.INPUT_DEVICE_CHANGE:
                    HandleDeviceChange(lParam, wParam);
                    return false;
                case WindowsMessage.INPUT:
                    return HandleInput(lParam);
                case WindowsMessage.KEYFIRST:
                case WindowsMessage.KEYUP:
                case WindowsMessage.CHAR:
                case WindowsMessage.DEADCHAR:
                case WindowsMessage.SYSKEYDOWN:
                case WindowsMessage.SYSKEYUP:
                case WindowsMessage.SYSCHAR:
                case WindowsMessage.SYSDEADCHAR:
                case WindowsMessage.KEYLAST:
                    return shouldBlock && pressedKeys.Contains((VirtualKeys) wParam);
            }

            return false;
        }

        /// <summary>
        /// Handle raw input message
        /// </summary>
        private bool HandleInput(IntPtr messageHandle)
        {
            GenericResult<RawInput> result = user32Input.GetRawInputData(messageHandle, RawInputCommand.Input);

            if (!scannerIdentifier.IsRegisteredScanner(result.Value.Header.DeviceHandle))
            {
                return false;
            }

            if (!result.Success)
            {
                log.Error(result.Message);
                return false;
            }

            shouldBlock = ProcessRawInputData(result.Value);
            return true;
        }

        /// <summary>
        /// Process the raw input data from a scanner
        /// </summary>
        private bool ProcessRawInputData(RawInput input)
        {
            if (input.Data.Keyboard.Message == WindowsMessage.KEYFIRST)
            {
                pressedKeys.Add(input.Data.Keyboard.VirtualKey);
                string text = GetCharacter(input.Data.Keyboard.VirtualKey);
                scanBuffer.Append(input.Header.DeviceHandle, text);
            }
            else
            {
                pressedKeys.Remove(input.Data.Keyboard.VirtualKey);
            }

            return pressedKeys.Any();
        }

        /// <summary>
        /// Handle a device change
        /// </summary>
        private void HandleDeviceChange(IntPtr deviceHandle, IntPtr changeType)
        {
            if (changeType.ToInt32() == 1)
            {
                scannerIdentifier.HandleDeviceAdded(deviceHandle);
                return;
            }

            scannerIdentifier.HandleDeviceRemoved(deviceHandle);
        }

        /// <summary>
        /// Get a character for the given key
        /// </summary>
        private string GetCharacter(VirtualKeys virtualKey)
        {
            return user32Input.GetCharactersFromKeys(virtualKey, pressedKeys.Contains(VirtualKeys.Shift), pressedKeys.Contains(VirtualKeys.Control), false);
        }
    }
}

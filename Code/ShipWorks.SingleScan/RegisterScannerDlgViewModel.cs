using System;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.SingleScan
{
    [Component]
    public class RegisterScannerDlgViewModel : IRegisterScannerDlgViewModel, IDisposable
    {
        private readonly IScannerService scannerService;
        private readonly IScannerIdentifier scannerIdentifier;
        private string dots = "...";
        private IntPtr deviceHandle;
        private IDisposable scanSubcription;


        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterScannerDlgViewModel"/> class.
        /// </summary>
        public RegisterScannerDlgViewModel(IScannerService scannerService, IMessenger messenger, IScannerIdentifier scannerIdentifier)
        {
            this.scannerService = scannerService;
            this.scannerIdentifier = scannerIdentifier;
            SaveScannerCommand = new RelayCommand(SaveScanner);

            scanSubcription = messenger.OfType<ScanMessage>()
                 .Subscribe(ScanDetected);

            scannerService.BeginFindScanner();
        }

        /// <summary>
        /// Detects a scan event and saves the result and device handle
        /// </summary>
        /// <param name="scanMessage">The scan message.</param>
        private void ScanDetected(ScanMessage scanMessage)
        {
            ScanResult = scanMessage.ScannedText;
            deviceHandle = scanMessage.DeviceHandle;
        }

        /// <summary>
        /// Message to displaying indicating we are waiting for a scan
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string WaitingMessage => "Waiting for scan" + dots;

        /// <summary>
        /// Gets a value indicating whether a scan result has been found.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ResultFound => !string.IsNullOrWhiteSpace(ScanResult);

        /// <summary>
        /// The scan result.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ScanResult { get; set; }

        /// <summary>
        /// The window loaded command.
        /// </summary>
        public ICommand WindowLoadedCommand { get; set; }

        /// <summary>
        /// Gets or sets the save scanner command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SaveScannerCommand { get; set; }

        /// <summary>
        /// Saves the scanner.
        /// </summary>
        private void SaveScanner()
        {
            scannerIdentifier.Save(deviceHandle);
        }

        /// <summary>
        /// Ends scanner search
        /// </summary>
        public void Dispose()
        {
            scannerService.EndFindScanner();
            scanSubcription.Dispose();
        }
    }
}
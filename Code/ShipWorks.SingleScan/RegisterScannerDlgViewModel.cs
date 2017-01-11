using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.SingleScan
{
    [Component]
    public class RegisterScannerDlgViewModel : IRegisterScannerDlgViewModel, IDisposable, INotifyPropertyChanged
    {
        private readonly IScannerService scannerService;
        private readonly IScannerIdentifier scannerIdentifier;
        private IntPtr deviceHandle;
        private readonly IDisposable scanSubscription;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private string waitingMessage = "Waiting for scan";
        private string scanResult;
        private bool resultFound;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterScannerDlgViewModel"/> class.
        /// </summary>
        public RegisterScannerDlgViewModel(IScannerService scannerService, IMessenger messenger, IScannerIdentifier scannerIdentifier)
        {
            this.scannerService = scannerService;
            this.scannerIdentifier = scannerIdentifier;
            SaveScannerCommand = new RelayCommand(SaveScanner);
            CancelCommand = new RelayCommand(Cancel);
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            scanSubscription = messenger.OfType<ScanMessage>()
                 .Subscribe(ScanDetected);

            scannerService.BeginFindScanner();
        }

        /// <summary>
        /// Gets or sets the action to close the parent dialog.
        /// </summary>
        public Action<DialogResult> CloseDialog { get; set; }

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
        public string WaitingMessage
        {
            get { return waitingMessage; }
            set
            {
                handler.Set(nameof(WaitingMessage), ref waitingMessage, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether a scan result has been found.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ResultFound
        {
            get { return resultFound; }
            set
            {
                handler.Set(nameof(ResultFound), ref resultFound, value);
            }
        }

        /// <summary>
        /// The scan result.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ScanResult
        {
            get { return scanResult; }
            set
            {
                handler.Set(nameof(ScanResult), ref scanResult, value);
                ResultFound = true;
            }
        }

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
        /// Gets or sets the save scanner command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// Saves the scanner.
        /// </summary>
        private void SaveScanner()
        {
            scannerIdentifier.Save(deviceHandle);
            CloseDialog?.Invoke(DialogResult.OK);
        }

        /// <summary>
        /// Executes the close action
        /// </summary>
        private void Cancel() => CloseDialog?.Invoke(DialogResult.Cancel);

        /// <summary>
        /// Ends scanner search
        /// </summary>
        public void Dispose()
        {
            scannerService.EndFindScanner();
            scanSubscription.Dispose();
        }
    }
}
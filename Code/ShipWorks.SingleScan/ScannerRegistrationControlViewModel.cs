using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;
using ShipWorks.Core.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// ViewModel to support ScannerRegistrationControl - used to register a scanner
    /// </summary>
    [Component]
    public class ScannerRegistrationControlViewModel : IScannerRegistrationControlViewModel, IDisposable, INotifyPropertyChanged
    {
        private readonly IScannerRegistrationListener scannerRegistrationListener;
        private readonly IScannerIdentifier scannerIdentifier;
        private readonly IMessageHelper messageHelper;
        private IntPtr deviceHandle;
        private readonly IDisposable scanSubscription;
        private readonly PropertyChangedHandler handler;
        private string scanResult;
        private bool resultFound;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerRegistrationControlViewModel(IScannerRegistrationListener scannerRegistrationListener, IMessenger messenger, IScannerIdentifier scannerIdentifier, IMessageHelper messageHelper)
        {
            this.scannerRegistrationListener = scannerRegistrationListener;
            this.scannerIdentifier = scannerIdentifier;
            this.messageHelper = messageHelper;
            SaveScannerCommand = new RelayCommand(SaveScanner);
            CancelCommand = new RelayCommand(Close);
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            scanSubscription = messenger.OfType<ScanMessage>()
                 .Subscribe(ScanDetected);

            scannerRegistrationListener.Start();
        }

        /// <summary>
        /// Gets or sets the action to close the parent dialog.
        /// </summary>
        public Action CloseDialog { get; set; }

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
        /// Ends scanner search
        /// </summary>
        public void Dispose()
        {
            scannerRegistrationListener.Stop();
            scanSubscription.Dispose();
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
        /// Saves the scanner.
        /// </summary>
        private void SaveScanner()
        {
            GenericResult<string> result = scannerIdentifier.Save(deviceHandle);

            if (!result.Success)
            {
                messageHelper.ShowError(result.Message);
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Executes the close action
        /// </summary>
        private void Close() => CloseDialog?.Invoke();
    }
}
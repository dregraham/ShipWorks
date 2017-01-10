using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    [Component]
    public class RegisterScannerDlgViewModel : IRegisterScannerDlgViewModel
    {
        private readonly IScannerService scannerService;
        private readonly IScannerConfigurationRepository scannerConfigRepo;
        private string dots = "...";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterScannerDlgViewModel"/> class.
        /// </summary>
        public RegisterScannerDlgViewModel(IScannerService scannerService, IScannerConfigurationRepository scannerConfigRepo)
        {
            this.scannerService = scannerService;
            this.scannerConfigRepo = scannerConfigRepo;
            SaveScannerCommand = new RelayCommand(SaveScanner);
            WindowLoadedCommand = new RelayCommand(OnWindowLoaded);
        }

        /// <summary>
        /// Called when [window loaded].
        /// </summary>
        private void OnWindowLoaded()
        {

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

        }
    }
}
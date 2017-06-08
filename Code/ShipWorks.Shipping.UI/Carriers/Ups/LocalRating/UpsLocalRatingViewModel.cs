using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using log4net;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    /// <summary>
    /// ViewModel to allow users to set up LocalRating
    /// </summary>
    [Component]
    public class UpsLocalRatingViewModel : IUpsLocalRatingViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public const string SampleRatesFileResourceName = "ShipWorks.Shipping.UI.Carriers.Ups.LocalRating.UpsLocalRatesSample.xlsx";
        public const string SampleZoneFileResourceName = "ShipWorks.Shipping.UI.Carriers.Ups.LocalRating.UpsZonesSample.xlsx";
        public const string ExistingRateFileResourceName = "ShipWorks.Shipping.UI.Carriers.Ups.LocalRating.UpsLocalRates.xlsx";
        public const string ExistingZoneFileResourceName = "ShipWorks.Shipping.UI.Carriers.Ups.LocalRating.UpsZones.xlsx";
        public const string NoRatesUploadedMessage = "Rates have not been uploaded for this account";
        public const string NoZonesUploadedMessage = "Zones have not been uploaded";
        private const string Extension = ".xlsx";
        private const string Filter = "Excel File (*.xlsx)|*.xlsx";
        private const string DefaultRateFileName = "UpsLocalRatesSample.xlsx";
        private const string DefaultZoneFileName = "UpsZonesSample.xlsx";
        private const string WarningMessage =
            "Local rates is an experimental feature and for rating purposes only. It does not affect billing. Please ensure the rates uploaded match the rates on your UPS account.\n\n" +
            "Note: All previously uploaded rates will be overwritten with the new rates.";

        private readonly IUpsLocalRateTable rateTable;
        private readonly IFileDialogFactory fileDialogFactory;
        private readonly IMessageHelper messageHelper;
        private readonly IUpsLocalRateValidator rateValidator;
        private readonly ILog log;
        protected readonly PropertyChangedHandler handler;

        private bool localRatingEnabled;
        private string rateStatusMessage;
        private string uploadMessage;
        private UpsAccountEntity upsAccount;
        private bool errorUploading;
        private bool isUploading;
        private string zoneStatusMessage;
        private string spinnerText;

        // Action to call when busy uploading a file
        private Action<bool> isBusy;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingViewModel"/> class.
        /// </summary>
        public UpsLocalRatingViewModel(IUpsLocalRateTable rateTable, 
            IFileDialogFactory fileDialogFactory, 
            IMessageHelper messageHelper, 
            Func<Type, ILog> logFactory, 
            IUpsLocalRateValidator rateValidator)
        {
            this.rateTable = rateTable;
            this.fileDialogFactory = fileDialogFactory;
            DownloadSampleRateFileCommand = new RelayCommand(DownloadSampleRateFile);
            DownloadSampleZoneFileCommand = new RelayCommand(DownloadSampleZoneFile);
            UploadRatingFileCommand = new RelayCommand(CallUploadRatingFile);
            UploadZoneFileCommand = new RelayCommand(CallUploadZoneFile);

            this.messageHelper = messageHelper;
            this.rateValidator = rateValidator;
            log = logFactory(GetType());
            
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Command to download the sample rate file
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DownloadSampleRateFileCommand { get; }

        /// <summary>
        /// Command to download the sample zone file
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DownloadSampleZoneFileCommand { get; }

        /// <summary>
        /// Command to upload the modified rate file
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UploadRatingFileCommand { get; }

        /// <summary>
        /// Gets the upload rating file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UploadZoneFileCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [local rating enabled].
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool LocalRatingEnabled
        {
            get { return localRatingEnabled; }
            set { handler.Set(nameof(LocalRatingEnabled), ref localRatingEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string RateStatusMessage
        {
            get { return rateStatusMessage; }
            set { handler.Set(nameof(RateStatusMessage), ref rateStatusMessage, value); }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ZoneStatusMessage
        {
            get { return zoneStatusMessage; }
            set { handler.Set(nameof(ZoneStatusMessage), ref zoneStatusMessage, value); }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string UploadMessage
        {
            get { return uploadMessage; }
            set { handler.Set(nameof(UploadMessage), ref uploadMessage, value); }
        }

        /// <summary>
        /// Whether or not we are in the process of validating the rates.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsUploading
        {
            get { return isUploading; }
            set { handler.Set(nameof(IsUploading), ref isUploading, value); }
        }

        /// <summary>
        /// Whether or not there was an error validating the rate file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ErrorUploading
        {
            get { return errorUploading; }
            set { handler.Set(nameof(ErrorUploading), ref errorUploading, value); }
        }

        /// <summary>
        /// Text for spinner to display
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string SpinnerText
        {
            get { return spinnerText; }
            set { handler.Set(nameof(SpinnerText), ref spinnerText, value); }
        }

        /// <summary>
        /// Loads the UpsAccount information to the view model
        /// </summary>
        public void Load(UpsAccountEntity account, Action<bool> isBusy)
        {
            try
            {
                this.isBusy = isBusy;
                LocalRatingEnabled = account.LocalRatingEnabled;
                rateTable.Load(account);
                upsAccount = account;
                UpdateMessages();
            }
            catch (UpsLocalRatingException e)
            {
                UploadMessage = e.Message;
            }
        }

        /// <summary>
        /// Saves view model information to the UpsAccount
        /// </summary>
        /// <returns>true when successful or false when save fails</returns>
        public bool Save()
        {
            upsAccount.LocalRatingEnabled = LocalRatingEnabled;

            if (LocalRatingEnabled && upsAccount.UpsRateTableID == null)
            {
                ErrorUploading = true;
                UploadMessage = "Please upload your rate table to enable local rating";
                upsAccount.LocalRatingEnabled = false;
                return false;
            }

            if (LocalRatingEnabled && !rateTable.ZoneUploadDate.HasValue)
            {
                ErrorUploading = true;
                UploadMessage = "Please upload your zones to enable local rating";
                upsAccount.LocalRatingEnabled = false;
                return false;
            }

            if (IsUploading)
            {
                messageHelper.ShowError("Please wait until the current action has finished before closing the UPS account window");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Downloads the sample rate file.
        /// </summary>
        private void DownloadSampleRateFile()
        {
            DownloadFile(SampleRatesFileResourceName, DefaultRateFileName);
        }

        /// <summary>
        /// Downloads the sample zone file.
        /// </summary>
        private void DownloadSampleZoneFile()
        {
            DownloadFile(SampleZoneFileResourceName, DefaultZoneFileName);
        }

        /// <summary>
        /// Downloads the given file
        /// </summary>
        /// <param name="resourceName">Resource name for the file</param>
        /// <param name="defaultFileName">File name for the</param>
        private void DownloadFile(string resourceName, string defaultFileName)
        {
            ISaveFileDialog fileDialog = fileDialogFactory.CreateSaveFileDialog();
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;
            fileDialog.DefaultFileName = defaultFileName;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                SaveFile(fileDialog, resourceName);

                fileDialog.ShowFile();
            }
            catch (ShipWorksSaveFileDialogException e)
            {
                messageHelper.ShowError(e.Message);
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="fileDialog">The file dialog.</param>
        /// <param name="resourceName">Resource name for the file</param>
        private void SaveFile(ISaveFileDialog fileDialog, string resourceName)
        {
            Assembly shippingAssembly = Assembly.GetAssembly(GetType());
            using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(resourceName))
            {
                using (Stream selectedFileStream = fileDialog.CreateFileStream())
                {
                    resourceStream.CopyTo(selectedFileStream);
                }
            }
        }

        /// <summary>
        /// Uploads the rating File.
        /// </summary>
        /// <remarks>
        /// The reason we don't call this directly is because we need to be able to test UploadRatingFile().
        /// ICommand can't take a method that returns Task and we can't await a void method.
        /// </remarks>
        private async void CallUploadRatingFile()
        {
            await UploadRatingFile();
        }

        /// <summary>
        /// Uploads the rating file.
        /// </summary>
        protected async Task UploadRatingFile()
        {
            messageHelper.ShowWarning(WarningMessage);

            IOpenFileDialog fileDialog = fileDialogFactory.CreateOpenFileDialog();
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ErrorUploading = false;
                    UploadMessage = string.Empty;
                    SpinnerText = "Uploading rates";
                    IsUploading = true;
                    isBusy(true);

                    await Task.Run(() =>
                    {
                        using (Stream fileStream = fileDialog.CreateFileStream())
                        {
                            rateTable.LoadRates(fileStream);
                        }

                        rateTable.SaveRates(upsAccount);

                        if (ValidateRates())
                        {
                            UpdateMessages();
                            UploadMessage = "Local rates have been uploaded successfully";
                            log.Info("Successfully uploaded rate table");
                        }
                    });
                }
                catch (Exception e) when (e is UpsLocalRatingException || e is ShipWorksOpenFileDialogException)
                {
                    ErrorUploading = true;
                    UploadMessage =
                        $"Local rates failed to upload:\n\n{e.Message}\n\nFile: {fileDialog.SelectedFileName}\n\nPlease review and try uploading your local rates again.";
                    log.Error($"Error uploading rate table: {e.Message}", e);
                }
                finally
                {
                    IsUploading = false;
                    isBusy(false);
                }
            }
        }

        /// <summary>
        /// Uploads the zone file.
        /// </summary>
        /// <remarks>
        /// The reason we don't call this directly is because we need to be able to test UploadZoneFile().
        /// ICommand can't take a method that returns Task and we can't await a void method.
        /// </remarks>
        private async void CallUploadZoneFile()
        {
            await UploadZoneFile();
        }

        /// <summary>
        /// Uploads the zone file.
        /// </summary>
        protected async Task UploadZoneFile()
        {
            IOpenFileDialog fileDialog = fileDialogFactory.CreateOpenFileDialog();
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ErrorUploading = false;
                    UploadMessage = string.Empty;
                    SpinnerText = "Uploading zones";
                    IsUploading = true;
                    isBusy(true);

                    await Task.Run(() =>
                    {
                        using (Stream fileStream = fileDialog.CreateFileStream())
                        {
                            rateTable.LoadZones(fileStream);
                        }

                        rateTable.SaveZones();

                        if (ValidateRates())
                        {
                            UpdateMessages();
                            UploadMessage = "Zones have been uploaded successfully";
                            log.Info("Successfully uploaded zone file");
                        }
                    });
                }
                catch (Exception e) when (e is UpsLocalRatingException || e is ShipWorksOpenFileDialogException)
                {
                    ErrorUploading = true;
                    UploadMessage =
                        $"Zones failed to upload:\n\n{e.Message}\n\nFile: {fileDialog.SelectedFileName}\n\nPlease review and try uploading your zones again.";
                    log.Error($"Error uploading zones: {e.Message}", e);
                }
                finally
                {
                    IsUploading = false;
                    isBusy(false);
                }
            }
        }

        /// <summary>
        /// Validate the rates using the rate validator
        /// update status message with failure 
        /// </summary>
        /// <returns>true if the rates are valid</returns>
        private bool ValidateRates()
        {
            if (rateTable.RateUploadDate.HasValue && rateTable.ZoneUploadDate.HasValue)
            {
                ILocalRateValidationResult validationResult = rateValidator.ValidateRecentShipments(upsAccount);

                if (validationResult.RateDiscrepancies.Any())
                {
                    UploadMessage =
                        "Warning: ShipWorks has detected one or more discrepancies with your current Local Rate File compared to your previously shipped orders. Please review your Local Rate File.\n\n" +
                        validationResult.GetUserFriendlyMessage();
                    ErrorUploading = true;
                    log.Error(validationResult.GetUserFriendlyMessage());
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Sets the status message.
        /// </summary>
        private void UpdateMessages()
        {
            RateStatusMessage = rateTable.RateUploadDate.HasValue ?
                $"Last Upload: {rateTable.RateUploadDate.Value.ToLocalTime():g}" :
                NoRatesUploadedMessage;

            ZoneStatusMessage = rateTable.ZoneUploadDate.HasValue ? 
                $"Last Upload: {rateTable.ZoneUploadDate.Value.ToLocalTime():g}" :
                NoZonesUploadedMessage;
        }
    }
}

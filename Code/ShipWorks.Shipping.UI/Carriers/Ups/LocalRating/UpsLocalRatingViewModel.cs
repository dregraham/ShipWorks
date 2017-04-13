using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
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

        public const string SampleFileResourceName = "ShipWorks.Shipping.UI.Carriers.Ups.LocalRating.UpsLocalRatesSample.xlsx";
        private const string Extension = ".xlsx";
        private const string Filter = "Excel File (*.xlsx)|*.xlsx";
        private const string DefaultFileName = "UpsLocalRatesSample.xlsx";
        private const string WarningMessage =
            "Local rates is an experimental feature and for rating purposes only. It does not affect billing. Please ensure the rates uploaded match the rates on your UPS account.\n\n" +
            "Note: All previously uploaded rates will be overwritten with the new rates.";

        private readonly IUpsLocalRateTable rateTable;
        private readonly Func<ISaveFileDialog> saveFileDialogFactory;
        private readonly Func<IOpenFileDialog> openFileDialogFactory;
        private readonly IMessageHelper messageHelper;
        private readonly ILog log;
        protected readonly PropertyChangedHandler handler;

        private bool localRatingEnabled;
        private string statusMessage;
        private string validationMessage;
        private UpsAccountEntity upsAccount;
        private bool errorValidatingRates;
        private bool validatingRates;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingViewModel"/> class.
        /// </summary>
        public UpsLocalRatingViewModel(IUpsLocalRateTable rateTable, Func<ISaveFileDialog> saveFileDialogFactory, Func<IOpenFileDialog> openFileDialogFactory, IMessageHelper messageHelper, Func<Type, ILog> logFactory)
        {
            this.rateTable = rateTable;
            this.saveFileDialogFactory = saveFileDialogFactory;
            this.openFileDialogFactory = openFileDialogFactory;
            DownloadSampleFileCommand = new RelayCommand(DownloadSampleFile);
            UploadRatingFileCommand = new RelayCommand(CallUploadRatingFile);

            this.messageHelper = messageHelper;
            log = logFactory(GetType());

            handler = new PropertyChangedHandler(this, () => PropertyChanged);
        }

        /// <summary>
        /// Command to download the sample file
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DownloadSampleFileCommand { get; }

        /// <summary>
        /// Gets the upload rating file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UploadRatingFileCommand { get; }

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
        public string StatusMessage
        {
            get { return statusMessage; }
            set { handler.Set(nameof(StatusMessage), ref statusMessage, value); }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ValidationMessage
        {
            get { return validationMessage; }
            set { handler.Set(nameof(ValidationMessage), ref validationMessage, value); }
        }

        /// <summary>
        /// Whether or not there was an error validating the rate file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ValidatingRates
        {
            get { return validatingRates; }
            set { handler.Set(nameof(ValidatingRates), ref validatingRates, value); }
        }

        /// <summary>
        /// Whether or not there was an error validating the rate file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ErrorValidatingRates
        {
            get { return errorValidatingRates; }
            set { handler.Set(nameof(ErrorValidatingRates), ref errorValidatingRates, value); }
        }

        /// <summary>
        /// Loads the UpsAccount information to the view model
        /// </summary>
        public void Load(UpsAccountEntity account)
        {
            try
            {
                LocalRatingEnabled = account.LocalRatingEnabled;
                rateTable.Load(account);
                upsAccount = account;
                SetStatusMessage();
            }
            catch (UpsLocalRatingException e)
            {
                ValidationMessage = e.Message;
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
                ValidationMessage = "Please upload your rate table to enable local rating";
                upsAccount.LocalRatingEnabled = false;
                return false;
            }

            if (ValidatingRates)
            {
                messageHelper.ShowError("Please wait until the rate table has finished uploading before closing the UPS account window");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Downloads the sample file.
        /// </summary>
        private void DownloadSampleFile()
        {
            ISaveFileDialog fileDialog = saveFileDialogFactory();
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;
            fileDialog.DefaultFileName = DefaultFileName;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                SaveFile(fileDialog);

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
        private void SaveFile(ISaveFileDialog fileDialog)
        {
            Assembly shippingAssembly = Assembly.GetAssembly(GetType());
            using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(SampleFileResourceName))
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

            IOpenFileDialog fileDialog = openFileDialogFactory();
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ErrorValidatingRates = false;
                    ValidationMessage = string.Empty;
                    ValidatingRates = true;
                    await Task.Run(() =>
                    {
                        using (Stream fileStream = fileDialog.CreateFileStream())
                        {
                            rateTable.Load(fileStream);
                            rateTable.Save(upsAccount);

                            SetStatusMessage();
                            ValidationMessage = "Local rates have been uploaded successfully";
                            log.Info("Successfully uploaded rate table");
                            ValidatingRates = false;
                        }
                    });
                }
                catch (Exception e) when (e is UpsLocalRatingException || e is ShipWorksOpenFileDialogException)
                {
                    ValidatingRates = false;
                    ErrorValidatingRates = true;
                    ValidationMessage =
                        $"Local rates failed to upload:\n\n{e.Message}\n\nPlease review and try uploading your local rates again.";
                    log.Error($"Error uploading rate table: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Sets the status message.
        /// </summary>
        private void SetStatusMessage()
        {
            StatusMessage = upsAccount.UpsRateTableID == null ?
                "There is no rate table associated with the selected account" :
                $"Last Upload: {rateTable.UploadDate.Value.ToLocalTime():g}";
        }
    }
}

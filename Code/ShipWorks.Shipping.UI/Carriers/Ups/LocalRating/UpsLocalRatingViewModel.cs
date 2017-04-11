﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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
        protected readonly PropertyChangedHandler Handler;

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
            UploadRatingFileCommand = new RelayCommand(UploadRatingFile);

            this.messageHelper = messageHelper;
            log = logFactory(GetType());


            Handler = new PropertyChangedHandler(this, () => PropertyChanged);
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
            set { Handler.Set(nameof(LocalRatingEnabled), ref localRatingEnabled, value); }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string StatusMessage
        {
            get { return statusMessage; }
            set { Handler.Set(nameof(StatusMessage), ref statusMessage, value); }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ValidationMessage
        {
            get { return validationMessage; }
            set { Handler.Set(nameof(ValidationMessage), ref validationMessage, value); }
        }

        /// <summary>
        /// Whether or not there was an error validating the rate file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ValidatingRates
        {
            get { return validatingRates; }
            set { Handler.Set(nameof(ValidatingRates), ref validatingRates, value); }
        }

        /// <summary>
        /// Whether or not there was an error validating the rate file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ErrorValidatingRates
        {
            get { return errorValidatingRates; }
            set { Handler.Set(nameof(ErrorValidatingRates), ref errorValidatingRates, value); }
        }

        /// <summary>
        /// Loads the UpsAccount information to the view model
        /// </summary>
        public void Load(UpsAccountEntity account)
        {
            LocalRatingEnabled = account.LocalRatingEnabled;

            upsAccount = account;
            SetStatusMessage();
        }

        /// <summary>
        /// Saves view model information to the UpsAccount
        /// </summary>
        /// <returns>true when successful or false when save fails</returns>
        public bool Save()
        {
            upsAccount.LocalRatingEnabled = LocalRatingEnabled;

            if (LocalRatingEnabled && upsAccount.UpsRateTable == null)
            {
                ValidationMessage = "Please upload your rate table to enable local rating";
                upsAccount.LocalRatingEnabled = false;
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

            Assembly shippingAssembly = Assembly.GetAssembly(GetType());

            using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(SampleFileResourceName))
            {
                using (Stream selectedFileStream = fileDialog.CreateFileStream())
                {
                    resourceStream.CopyTo(selectedFileStream);
                }
            }

            fileDialog.ShowFile();
        }

        /// <summary>
        /// Uploads the rating File.
        /// </summary>
        private void UploadRatingFile()
        {
            messageHelper.ShowWarning(WarningMessage);

            IOpenFileDialog fileDialog = openFileDialogFactory();
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ValidatingRates = true;
                    Stream fileStream = fileDialog.CreateFileStream();
                    rateTable.Load(fileStream);
                    rateTable.Save(upsAccount);

                    SetStatusMessage();
                    ValidationMessage = "Local rates have been uploaded successfully";
                    log.Info("Successfully uploaded rate table");
                    ValidatingRates = false;
                }
                catch (UpsLocalRatingException e)
                {
                    ValidatingRates = false;
                    ErrorValidatingRates = true;
                    ValidationMessage = $"Local rates failed to upload:\n\n{e.Message}\n\nPlease review and try uploading your local rates again.";
                    log.Error($"Error uploading rate table: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Sets the status message.
        /// </summary>
        private void SetStatusMessage()
        {
            StatusMessage = upsAccount.UpsRateTable?.UploadDate == null ?
                "There is no rate table associated with the selected account" :
                $"Last Upload: {upsAccount.UpsRateTable.UploadDate:g}";
        }
    }
}

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    /// <summary>
    /// ViewModel to allow users to set up LocalRating
    /// </summary>
    [Component]
    public class UpsLocalRatingViewModel : IUpsLocalRatingViewModel
    {
        private readonly Func<ISaveFileDialog> fileDialogFactory;
        public const string SampleFileResourceName = "ShipWorks.Shipping.UI.Carriers.Ups.LocalRating.UpsLocalRatesSample.xlsx";
        private const string Extension = ".xlsx";
        private const string Filter = "Excel File (*.xlsx)|*.xlsx";
        private const string DefaultFileName = "UpsLocalRatesSample.xlsx";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingViewModel"/> class.
        /// </summary>
        public UpsLocalRatingViewModel(Func<ISaveFileDialog> fileDialogFactory)
        {
            this.fileDialogFactory = fileDialogFactory;
            DownloadSampleFileCommand = new RelayCommand(DownloadSampleFile);
            UploadRatingFileCommand = new RelayCommand(UploadRatingFile, () => LocalRatingEnabled);
        }

        /// <summary>
        /// Command to download the sample file
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DownloadSampleFileCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [local rating enabled].
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool LocalRatingEnabled { get; set; }

        /// <summary>
        /// Gets the upload rating file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UploadRatingFileCommand { get; }

        /// <summary>
        /// Loads the UpsAccount information to the view model
        /// </summary>
        public void Load(UpsAccountEntity upsAccount)
        {
            LocalRatingEnabled = upsAccount.LocalRatingEnabled;
        }

        /// <summary>
        /// Saves view model information to the UpsAccount
        /// </summary>
        public void Save(UpsAccountEntity upsAccount)
        {
            upsAccount.LocalRatingEnabled = LocalRatingEnabled;
        }

        /// <summary>
        /// Downloads the sample file.
        /// </summary>
        private void DownloadSampleFile()
        {
            ISaveFileDialog fileDialog = fileDialogFactory();
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
            throw new NotImplementedException();
        }
    }
}

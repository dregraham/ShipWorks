using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    /// <summary>
    /// ViewModel to allow users to set up LocalRating
    /// </summary>
    [Component]
    public class UpsLocalRatingViewModel : IUpsLocalRatingViewModel
    {
        private readonly IIndex<FileDialogType, IFileDialog> fileDialogFactory;
        private const string SampleFileResourceName = "ShipWorks.Shipping.UpsLocalRatesSample.xlsx";
        private const string Extension = ".xlsx";
        private const string Filter = "Excel File (*.xlsx)|*.xlsx";
        private const string DefaultFileName = "UpsLocalRatesSample.xlsx";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingViewModel"/> class.
        /// </summary>
        public UpsLocalRatingViewModel(IIndex<FileDialogType, IFileDialog> fileDialogFactory)
        {
            this.fileDialogFactory = fileDialogFactory;
            DownloadSampleFile = new RelayCommand(DownloadSampleFileCommand);
            UploadRatingFile = new RelayCommand(UploadRatingFileCommand, () => LocalRatingEnabled);
        }

        /// <summary>
        /// Command to download the sample file
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand DownloadSampleFile { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [local rating enabled].
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool LocalRatingEnabled { get; set; }


        /// <summary>
        /// Gets the upload rating file.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand UploadRatingFile { get; }

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
        public bool Save(UpsAccountEntity upsAccount)
        {
            upsAccount.LocalRatingEnabled = LocalRatingEnabled;

            return true;
        }

        /// <summary>
        /// Downloads the sample file.
        /// </summary>
        private void DownloadSampleFileCommand()
        {
            IFileDialog fileDialog = fileDialogFactory[FileDialogType.Save];
            fileDialog.DefaultExt = Extension;
            fileDialog.Filter = Filter;
            fileDialog.DefaultFileName = DefaultFileName;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Assembly shippingAssembly = Assembly.GetAssembly(typeof(UpsLabelService));

            using (Stream resourceStream = shippingAssembly.GetManifestResourceStream(SampleFileResourceName))
            using (Stream selectedFileStream = fileDialog.CreateFileStream())
            {
                resourceStream.CopyTo(selectedFileStream);
                resourceStream.Close();
            }

            System.Diagnostics.Process.Start(fileDialog.SelectedFileName);
        }

        /// <summary>
        /// Uploads the rating File.
        /// </summary>
        private void UploadRatingFileCommand()
        {
            throw new NotImplementedException();
        }
    }
}

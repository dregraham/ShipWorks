using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsLocalRatingViewModel"/> class.
        /// </summary>
        public UpsLocalRatingViewModel()
        {
            DownloadSampleFile = new RelayCommand(DownloadSampleFileAccount);
            UploadRatingFile = new RelayCommand(UploadRatingFileCommand);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves view model information to the UpsAccount
        /// </summary>
        public bool Save(UpsAccountEntity upsAccount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uploads the rating File.
        /// </summary>
        private void UploadRatingFileCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Downloads the sample file account.
        /// </summary>
        private void DownloadSampleFileAccount()
        {
            
        }
    }
}

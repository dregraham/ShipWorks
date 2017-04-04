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
    [Component]
    public class UpsLocalRatingViewModel : IUpsLocalRatingViewModel
    {
        public UpsLocalRatingViewModel()
        {
            DownloadSampleFile = new RelayCommand(DownloadSampleFileAccount);
        }

        public void Load(UpsAccountEntity upsAccount)
        {
            throw new NotImplementedException();
        }

        public bool Save(UpsAccountEntity upsAccount)
        {
            throw new NotImplementedException();
        }

        [Obfuscation(Exclude = true)]
        public ICommand DownloadSampleFile { get; }

        private void DownloadSampleFileAccount()
        {
            
        }
    }
}

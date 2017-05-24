using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    [Component]
    public class UpsLocalRatingExceptionFootnoteViewModel : IUpsLocalRatingExceptionFootnoteViewModel
    {
        private readonly IWin32Window owner;

        public UpsLocalRatingExceptionFootnoteViewModel(IWin32Window owner)
        {
            this.owner = owner;
            OnClickMoreInfo = new RelayCommand(DisplayErrorMessage);
        }

        public RelayCommand OnClickMoreInfo { get; set; }

        public string ErrorMessage { get; set; }

        private void DisplayErrorMessage()
        {
            MessageHelper.ShowError(owner, ErrorMessage);
        }
    }
}
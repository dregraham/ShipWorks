using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    public class UpsPromoFootnoteViewModel : IUpsPromoFootnoteViewModel
    {
        private readonly IWin32Window owner;

        public UpsPromoFootnoteViewModel(IWin32Window owner)
        {
            this.owner = owner;
            ActivatePromo = new RelayCommand(ActivatePromoAction);
        }

        /// <summary>
        /// Command to show the shipping settings
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ActivatePromo { get; }

        /// <summary>
        /// The UPS promo.
        /// </summary>
        public IUpsPromo UpsPromo { get; set; }

        /// <summary>
        /// Bring up the UpsPromoDlg
        /// </summary>
        private void ActivatePromoAction()
        {
            using (UpsPromoDlg dlg = new UpsPromoDlg(UpsPromo))
            {
                dlg.ShowDialog(owner);
            }

            RateCache.Instance.Clear();
        }
    }
}
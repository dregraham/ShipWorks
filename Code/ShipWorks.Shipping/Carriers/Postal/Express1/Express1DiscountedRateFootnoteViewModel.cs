using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Express1 discounted rate footnote view model
    /// </summary>
    public class Express1DiscountedRateFootnoteViewModel : IExpress1DiscountedRateFootnoteViewModel
    {
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner"></param>
        public Express1DiscountedRateFootnoteViewModel(IWin32Window owner)
        {
            this.owner = owner;
            ViewSavings = new RelayCommand(ViewSavingsAction);
        }

        /// <summary>
        /// List of discounted rates
        /// </summary>
        public List<RateResult> DiscountedRates { get; set; }

        /// <summary>
        /// List or original rates
        /// </summary>
        public List<RateResult> OriginalRates { get; set; }

        /// <summary>
        /// View available savings
        /// </summary>
        public ICommand ViewSavings { get; }

        /// <summary>
        /// View the detailed Endicia vs. Express1 savings
        /// </summary>
        private void ViewSavingsAction()
        {
            using (Express1ActualSavingsDlg dlg = new Express1ActualSavingsDlg(OriginalRates, DiscountedRates))
            {
                dlg.ShowDialog(owner);
            }
        }
    }
}

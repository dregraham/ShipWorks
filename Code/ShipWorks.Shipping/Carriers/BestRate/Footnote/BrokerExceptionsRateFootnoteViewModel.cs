using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Footnote to display broker exceptions
    /// </summary>
    public class BrokerExceptionsRateFootnoteViewModel : IBrokerExceptionsRateFootnoteViewModel
    {
        private readonly IWin32Window owner;

        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerExceptionsRateFootnoteViewModel(IWin32Window owner)
        {
            this.owner = owner;
            ShowExceptions = new RelayCommand(ShowExceptionsAction);
        }

        /// <summary>
        /// List of exceptions to display
        /// </summary>
        public IEnumerable<BrokerException> BrokerExceptions { get; set; }

        /// <summary>
        /// Severity level of the exceptions
        /// </summary>
        public BrokerExceptionSeverityLevel SeverityLevel { get; set; }

        /// <summary>
        /// Show Exceptions command
        /// </summary>
        public ICommand ShowExceptions { get; }

        /// <summary>
        /// Called when the "More info" link is clicked.
        /// </summary>
        private void ShowExceptionsAction()
        {
            using (BestRateMissingRatesDialog dialog = new BestRateMissingRatesDialog(BrokerExceptions))
            {
                dialog.ShowDialog(owner);
            }
        }
    }
}

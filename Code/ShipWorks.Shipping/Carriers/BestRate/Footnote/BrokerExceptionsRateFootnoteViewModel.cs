using System.Collections.Generic;
using System.Reflection;
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
        [Obfuscation(Exclude = true)]
        public IEnumerable<BrokerException> BrokerExceptions { get; set; }

        /// <summary>
        /// Severity level of the exceptions
        /// </summary>
        [Obfuscation(Exclude = true)]
        public BrokerExceptionSeverityLevel SeverityLevel { get; set; }

        /// <summary>
        /// Show Exceptions command
        /// </summary>
        [Obfuscation(Exclude = true)]
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

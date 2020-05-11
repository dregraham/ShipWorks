using System;
using System.Windows.Input;

namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interface for view models that need to show a setup wizard
    /// </summary>
    public interface IOneBalanceShowSetupWizardViewModel
    {
        /// <summary>
        /// RelayCommand for Showing the setup dialog
        /// </summary>
        ICommand ShowSetupWizardCommand { get; }

        /// <summary>
        /// Raised when setup is complete
        /// </summary>
        event EventHandler SetupComplete;

        /// <summary>
        /// Refreshes the account dependent attributes of the controls 
        /// </summary>
        void Refresh();
    }
}
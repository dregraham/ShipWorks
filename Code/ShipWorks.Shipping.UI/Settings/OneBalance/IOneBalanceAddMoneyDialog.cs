namespace ShipWorks.Shipping.UI.Settings.OneBalance
{
    /// <summary>
    /// Interface for the One Balance Add Money Dialog
    /// </summary>
    public interface IOneBalanceAddMoneyDialog
    {
        /// <summary>
        /// Initializes the Add Money Dialog. This should already 
        /// exist in the Window base class
        /// </summary>
        void InitializeComponent();

        /// <summary>
        /// Shows the dialog to the user. This should already 
        /// exist in the Window base class
        /// </summary>
        bool? ShowDialog();
    }
}
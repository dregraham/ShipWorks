using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Interface that represents a factory for creating the AutoPrintConfirmationDlg
    /// </summary>
    public interface IAutoPrintConfirmationDlgFactory
    {
        /// <summary>
        /// Create an AutoPrintConfirmationDlg with the given display text
        /// </summary>
        /// <param name="scanMessageText">The scan message text that the dialog is listening for to confirm and close</param>
        /// <param name="messagingText">The messaging text to display on the IForm</param>
        IForm Create(string scanMessageText, MessagingText messagingText);
    }
}
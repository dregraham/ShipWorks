using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Services
{
    public interface IAutoPrintConfirmationDlgFactory
    {
        /// <summary>
        /// Create an AutoPrintConfirmationDlg with the given display text
        /// </summary>
        /// <param name="scanMessageText">The scan message that the dialog is listening for to confirm and close</param>
        /// <param name="displayText">The text that is displayed to the user</param>
        IDialog Create(string scanMessageText, string displayText);
    }
}
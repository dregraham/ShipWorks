using System.Windows.Forms;
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
        /// <param name="scanMessageText">The scan message that the dialog is listening for to confirm and close</param>
        /// <param name="title">The title of the dialog</param>
        /// <param name="displayText">The text that is displayed to the user</param>
        /// <param name="continueText">The text that is displayed to the user in the contine button</param>
        IForm Create(string scanMessageText, string title, string displayText, string continueText);
    }
}
using System;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Display messages to the user without taking a dependency on the UI
    /// </summary>
    public interface IMessageHelper
    {
        /// <summary>
        /// Show an error
        /// </summary>
        void ShowError(string message);

        /// <summary>
        /// Show an error
        /// </summary>
        void ShowError(IWin32Window owner, string message);

        /// <summary>
        /// Show an information message
        /// </summary>
        void ShowInformation(string message);

        /// <summary>
        /// Show a new progress dialog
        /// </summary>
        IDisposable ShowProgressDialog(string title, string description);

        /// <summary>
        /// Show a yes/no question with the given text
        /// </summary>
        DialogResult ShowQuestion(string text);

        /// <summary>
        /// Show a dialog and get the results
        /// </summary>
        /// <returns></returns>
        DialogResult ShowDialog(Func<Form> createDialog);
		
        /// <summary>
        /// Show an information message, takes an owner
        /// </summary>
        void ShowInformation(IWin32Window owner, string message);

        /// <summary>
        /// Show a question message box.  
        /// </summary>
        DialogResult ShowQuestion(MessageBoxIcon icon, MessageBoxButtons buttons, string message);
    }
}
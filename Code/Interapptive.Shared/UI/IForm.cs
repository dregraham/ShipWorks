using System;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Interface for a basic UI form
    /// </summary>
    public interface IForm : IDisposable
    {
        /// <summary>
        /// Show the dialog
        /// </summary>
        DialogResult ShowDialog(IWin32Window control);
    }
}

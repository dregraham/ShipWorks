using System;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    public interface IFormsDialog : IDisposable
    {
        /// <summary>
        /// Shows the Winforms Dialog
        /// </summary>
        DialogResult ShowDialog(IWin32Window owner);
    }
}
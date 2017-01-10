using System.ComponentModel;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Interface for finding/using the MainForm
    /// </summary>
    public interface IMainForm : ISynchronizeInvoke, IWin32Window
    {
    }
}

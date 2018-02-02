using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Window state saver for Windows Forms
    /// </summary>
    public interface IFormsWindowStateSaver : IWindowStateSaver
    {
        /// <summary>
        /// Adds the SplitContainer to the elements being remembered
        /// </summary>
        IFormsWindowStateSaver ManageSplitter(SplitContainer splitContainer);

        /// <summary>
        /// Adds the SplitContainer to the elements being remembered
        /// </summary>
        IFormsWindowStateSaver ManageSplitter(SplitContainer splitContainer, string name);
    }
}
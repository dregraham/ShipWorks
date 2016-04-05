using System.Windows;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for displaying WPF Dialogs
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// The dialogs data context
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();

        /// <summary>
        /// Dialog result
        /// </summary>
        /// <returns></returns>
        bool? DialogResult { get; set; }

        /// <summary>
        /// The dialogs owner
        /// </summary>
        Window Owner { get; set; }

        /// <summary>
        /// Starting location of the window
        /// </summary>
        WindowStartupLocation WindowStartupLocation { get; set; }

        /// <summary>
        /// Topmost window or not
        /// </summary>
        bool Topmost { get; set; }

        /// <summary>
        /// Dialog window height.
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Dialog window width.
        /// </summary>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the load owner.
        /// </summary>
        void LoadOwner(IWin32Window owner);
    }
}


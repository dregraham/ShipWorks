using System.Windows;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// DO NOT IMPLEMENT THIS INTERFACE DIRECTLY. USE INTEROPWINDOW INSTEAD!
    /// Interface for displaying WPF Dialogs. 
    /// </summary>
    /// <remarks>
    /// This interface was originally used to create wpf dialogs with winforms owners. This eventaully evolved into the
    /// InteropWindow class. PLEASE IMPLEMENT THAT INSTEAD. OTHERWISE YOU WILL NOT GET THE PROPER FUNCTIONALITY.
    /// </remarks>
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
        /// Loads the owner
        /// </summary>
        void LoadOwner(IWin32Window owner);

        /// <summary>
        /// Closes the dialog
        /// </summary>
        void Close();
    }
}


using System.Windows;

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
    }
}


using System.IO;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Interface to allow a user to Save a file
    /// </summary>
    public interface ISaveFileDialog
    {
        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the 
        /// "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        /// <example>
        /// Text File (*.txt)|*.txt
        /// </example>
        string Filter { set; }
        
        /// <summary>
        /// Gets or sets the default file extension.
        /// </summary>
        /// <example>
        /// .txt
        /// </example>
        string DefaultExt { set; }
        
        /// <summary>
        /// Gets or sets the default file name used to initialize the file dialog box.
        /// </summary>
        string DefaultFileName { set; }

        /// <summary>
        /// Gets the name of the selected file.
        /// </summary>
        string SelectedFileName { get; }

        /// <summary>
        /// Shows the file Dialog box
        /// </summary>
        DialogResult ShowDialog();
        
        /// <summary>
        /// Gets a stream of the file
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="System.UnauthorizedAccessException" />
        Stream CreateFileStream();
    }
}

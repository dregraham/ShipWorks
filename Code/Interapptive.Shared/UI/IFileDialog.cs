using System.IO;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Interface to get a file name from the user
    /// </summary>
    public interface IFileDialog
    {
        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the 
        /// "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        string Filter { set; }


        /// <summary>
        /// Gets or sets the default file extension.
        /// </summary>
        string DefaultExt { set; }


        /// <summary>
        /// Gets or sets the default file name used to initialize the file dialog box.
        /// </summary>
        string DefaultFileName { set; }

        /// <summary>
        /// Shows the file Dialog box
        /// </summary>
        DialogResult ShowDialog();


        /// <summary>
        /// Gets a stream of the file
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException" />
        Stream CreateFileStream();
    }
}

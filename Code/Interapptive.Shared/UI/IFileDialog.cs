using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Interface to get a file from the user
    /// </summary>
    public interface IFileDialog
    {
        /// <summary>
        /// Shows the open file Dialog box
        /// </summary>
        /// <remarks>Sets FileName property if dialog result is OK</remarks>
        /// <param name="filter">
        /// The file name filter string, which determines the choices that appear in the "Files of type" box in the dialog box.
        /// </param>
        /// <returns>DialogResult</returns>
        DialogResult ShowOpenFile(string filter);

        /// <summary>
        /// Shows the save file dialog box.
        /// </summary>
        /// <remarks>Sets FileName property if dialog result is OK</remarks>
        /// <param name="filter"> 
        /// The file name filter string, which determines the choices that appear in the "Save as file type" box in the dialog box.
        /// </param>
        /// <param name="defaultExtension">The default file extension.</param>
        /// <param name="initialFileName">
        /// A string containing the file name selected in the file dialog box.
        /// </param>
        /// <returns>DialogResult</returns>
        DialogResult ShowSaveFile(string filter, string defaultExtension, string initialFileName);

        /// <summary>
        /// Gets the name of the file - Empty string if not set.
        /// </summary>
        string FileName { get; }
    }
}

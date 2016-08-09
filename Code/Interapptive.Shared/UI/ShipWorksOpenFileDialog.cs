using System;
using System.IO;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Get a file from the user
    /// </summary>
    public class ShipWorksOpenFileDialog : IFileDialog
    {
        private readonly Control owner;
        private string selectedFileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksOpenFileDialog"/> class.
        /// </summary>
        public ShipWorksOpenFileDialog(Func<Control> ownerFunc)
        {
            owner = ownerFunc();
        }

        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the
        /// "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        public string Filter { private get; set; }

        /// <summary>
        /// Gets or sets the default file extension.
        /// </summary>
        public string DefaultExt { private get; set; }

        /// <summary>
        /// Gets or sets the default file name used to initialize the file dialog box.
        /// </summary>
        public string DefaultFileName { private get; set; }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <returns></returns>
        public DialogResult ShowDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = Filter;
                openFileDialog.DefaultExt = DefaultExt;
                openFileDialog.FileName = DefaultFileName;

                DialogResult dialogResult = openFileDialog.ShowDialog(owner);

                if (dialogResult == DialogResult.OK)
                {
                    selectedFileName = openFileDialog.FileName;
                }

                return dialogResult;
            }
        }

        /// <summary>
        /// Gets the file with the name chosen by the user from ShowOpenFile or ShowSaveFile.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException" />
        public Stream CreateFileStream()
        {
            return string.IsNullOrEmpty(selectedFileName) ? null : File.OpenRead(selectedFileName);
        }
    }
}
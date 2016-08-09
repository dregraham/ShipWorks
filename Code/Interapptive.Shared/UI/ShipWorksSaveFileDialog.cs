using System;
using System.IO;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Gets a file from the user
    /// </summary>
    public class ShipWorksSaveFileDialog : IFileDialog
    {
        private readonly Control owner;
        private string selectedFileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipWorksSaveFileDialog"/> class.
        /// </summary>
        public ShipWorksSaveFileDialog(Func<Control> ownerFunc)
        {
            owner = ownerFunc();
        }

        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that appear in the
        /// "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Gets or sets the default file extension.
        /// </summary>
        public string DefaultExt { get; set; }

        /// <summary>
        /// Gets or sets the default file name used to initialize the file dialog box.
        /// </summary>
        public string DefaultFileName { private get; set; }

        /// <summary>
        /// Shows the save file Dialog box
        /// </summary>
        public DialogResult ShowDialog()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = Filter;
                saveFileDialog.AddExtension = true;
                saveFileDialog.DefaultExt = DefaultExt;
                saveFileDialog.FileName = DefaultFileName;

                DialogResult dialogResult = saveFileDialog.ShowDialog(owner);

                if (dialogResult == DialogResult.OK)
                {
                    selectedFileName = saveFileDialog.FileName;
                }

                return dialogResult;
            }
        }

        /// <summary>
        /// Gets a stream of the file
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public Stream CreateFileStream()
        {
            return string.IsNullOrEmpty(selectedFileName) ? null : File.Open(selectedFileName, FileMode.Create);
        }
    }
}

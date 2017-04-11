using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Interapptive.Shared.UI
{
    /// <summary>
    /// Gets a file from the user
    /// </summary>
    public class ShipWorksSaveFileDialog : ISaveFileDialog
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
            try
            {
                return string.IsNullOrEmpty(selectedFileName) ? null : File.Open(selectedFileName, FileMode.Create);
            }
            catch (Exception e)
            {
                string message = $"An error occurred saving the file:{Environment.NewLine}{Environment.NewLine}" +
                                 $"{e.Message}";
                throw new ShipWorksSaveFileDialogException(message, e);
            }
        }

        /// <summary>
        /// Shows the file.
        /// </summary>
        public void ShowFile()
        {
            try
            {
                Process.Start(selectedFileName);
            }
            catch (Exception e)
            {
                string message = $"An error occurred opening the file after it was saved:{Environment.NewLine}{Environment.NewLine}" +
                                 $"{e.Message}";
                throw new ShipWorksSaveFileDialogException(message, e);
            }
        }
    }
}

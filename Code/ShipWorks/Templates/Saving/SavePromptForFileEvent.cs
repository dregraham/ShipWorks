using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace ShipWorks.Templates.Saving
{
    /// <summary>
    /// EventHandler for the PromptForFile event
    /// </summary>
    public delegate void SavePromptForFileEventHandler(object sender, SavePromptForFileEventArgs e);

    /// <summary>
    /// EventArgs for the PromptForFile event
    /// </summary>
    public class SavePromptForFileEventArgs : CancelEventArgs
    {
        string name;
        string folder;

        SaveFileNamePart part;
        string fileFilter;

        object userState;
        string resultName;

        /// <summary>
        /// Constructor
        /// </summary>
        public SavePromptForFileEventArgs(string name, string folder, SaveFileNamePart part, string fileFilter, object userState)
        {
            if (part == SaveFileNamePart.Name)
            {
                throw new InvalidOperationException("Cannot request just the name. Has to be the full name, or just the folder.");
            }

            this.name = name;
            this.folder = folder;

            this.part = part;
            this.fileFilter = fileFilter;

            this.userState = userState;
        }

        /// <summary>
        /// The initial name to use to fill in the file selection dialog.  Will be either a full filename or a folder name
        /// depending on the value of PartRequested.
        /// </summary>
        public string InitialName
        {
            get
            {
                if (part == SaveFileNamePart.Folder)
                {
                    return folder;
                }
                else
                {
                    return Path.Combine(folder, name);
                }
            }
        }

        /// <summary>
        /// The file name part that the user should be prompted for
        /// </summary>
        public SaveFileNamePart PartRequested
        {
            get { return part; }
        }

        /// <summary>
        /// Get the file filter than can be used to filter the extension choices in a file selection box.
        /// </summary>
        public string FileFilter
        {
            get { return fileFilter; }
        }

        /// <summary>
        /// The object originally passed to the SaveAsync method.
        /// </summary>
        public object UserState
        {
            get { return userState; }
        }

        /// <summary>
        /// The user's selection should be stored here, otherwise Canceled should be set to true.
        /// </summary>
        public string ResultName
        {
            get { return resultName; }
            set { resultName = value; }
        }
    }
}

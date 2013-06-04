using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Delete for the file browse event
    /// </summary>
    public delegate void GenericFileSourceFolderBrowseEventHandler(object sender, GenericFileSourceFolderBrowseEventArgs e);

    /// <summary>
    /// EventArgs class for browsing for a folder
    /// </summary>
    public class GenericFileSourceFolderBrowseEventArgs : EventArgs
    {
        string folder;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceFolderBrowseEventArgs(string folder)
        {
            this.folder = folder;
        }

        /// <summary>
        /// The folder
        /// </summary>
        public string Folder
        {
            get { return folder; }
            set { folder = value; }
        }
    }
}

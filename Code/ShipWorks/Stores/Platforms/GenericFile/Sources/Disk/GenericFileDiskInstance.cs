using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Disk
{
    /// <summary>
    /// Generic file instance representing a file on disk
    /// </summary>
    public class GenericFileDiskInstance : GenericFileInstance
    {
        FileInfo fileInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileDiskInstance(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("fileInfo");
            }

            this.fileInfo = fileInfo;
        }

        /// <summary>
        /// The underlying FileInfo represented by this object
        /// </summary>
        public FileInfo FileInfo
        {
            get { return fileInfo; }
        }

        /// <summary>
        /// Name of the file
        /// </summary>
        public override string Name
        {
            get { return fileInfo.Name; }
        }

        /// <summary>
        ///  Contents of the file
        /// </summary>
        public override Stream OpenStream()
        {
            try
            {
                return fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch (IOException ex)
            {
                throw new GenericFileLoadException("Could not read from the import folder: " + ex.Message, ex);
            }
        }
    }
}

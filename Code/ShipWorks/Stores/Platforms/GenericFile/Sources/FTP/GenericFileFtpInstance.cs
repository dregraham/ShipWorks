extern alias rebex2015;

using System.IO;
using rebex2015::Rebex.Net;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources.FTP
{
    /// <summary>
    /// Generic file instance representing a file on an FTP site
    /// </summary>
    public class GenericFileFtpInstance : GenericFileInstance
    {
        IFtp ftp;
        string ftpPath;
        string ftpFile;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileFtpInstance(IFtp ftp, string ftpPath, string ftpFile)
        {
            this.ftp = ftp;
            this.ftpPath = ftpPath;
            this.ftpFile = ftpFile;
        }

        /// <summary>
        /// The FTP folder/path the file is located in
        /// </summary>
        public string Path
        {
            get { return ftpPath; }
        }

        /// <summary>
        /// Name of the file
        /// </summary>
        public override string Name
        {
            get { return ftpFile; }
        }

        /// <summary>
        ///  Contents of the file
        /// </summary>
        public override Stream OpenStream()
        {
            try
            {
                Stream stream = new MemoryStream();

                // Get the file off the server, and return the local stream - don't stream directly from the FTP server
                ftp.GetFile(ftpPath + "/" + ftpFile, stream);
                stream.Position = 0;

                return stream;
            }
            catch (NetworkSessionException ex)
            {
                throw new GenericFileLoadException("Could not read from the import folder: " + ex.Message, ex);
            }
        }
    }
}

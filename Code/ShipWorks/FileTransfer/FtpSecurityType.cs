namespace ShipWorks.FileTransfer
{
    /// <summary>
    /// Security types for FTP connection
    /// </summary>
    public enum FtpSecurityType
    {
        /// <summary>
        /// Unsecure connection, typically port 21
        /// </summary>
        Unsecure = 0,

        /// <summary>
        /// Implicity secure connection, typically port 990
        /// </summary>
        Implicit = 1,

        /// <summary>
        /// Explicitly secure connection, typically port 21
        /// </summary>
        Explicit = 2,

        /// <summary>
        /// SFTP Security, typically port 22
        /// </summary>
        Sftp = 3
    }
}

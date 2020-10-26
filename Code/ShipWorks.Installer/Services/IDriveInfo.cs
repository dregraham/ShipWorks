namespace ShipWorks.Installer.Services
{
    public interface IDriveInfo
    {
        /// <summary>
        /// The available free space on the drive
        /// </summary>
        long AvailableFreeSpace { get; }

        /// <summary>
        /// The name of the drive
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Converts an Array of System.IO.DriveInfo to the wrapped version
        /// </summary>
        /// <param name="drives"></param>
        IDriveInfo[] GetDrives();
    }
}
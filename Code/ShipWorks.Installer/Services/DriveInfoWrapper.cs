using System.IO;
using System.Linq;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Wrapper class for System.IO.DriveInfo
    /// </summary>
    public class DriveInfoWrapper : IDriveInfo
    {
        /// <summary>
        /// Wrapper class for System.IO.DriveInfo
        /// </summary>
        private DriveInfo driveInfo;

        /// <summary>
        /// Empty constructor for IOC
        /// </summary>
        public DriveInfoWrapper()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="driveInfo"></param>
        public DriveInfoWrapper(DriveInfo driveInfo)
        {
            this.driveInfo = driveInfo;
        }

        /// <summary>
        /// The available free space on the drive
        /// </summary>
        public long AvailableFreeSpace => driveInfo.AvailableFreeSpace;

        /// <summary>
        /// The name of the drive
        /// </summary>
        public string Name => driveInfo.Name;

        /// <summary>
        /// Converts an Array of System.IO.DriveInfo to the wrapped version
        /// </summary>
        /// <param name="drives"></param>
        public IDriveInfo[] GetDrives() => DriveInfo.GetDrives().Select(d => new DriveInfoWrapper(d)).ToArray();
    }
}

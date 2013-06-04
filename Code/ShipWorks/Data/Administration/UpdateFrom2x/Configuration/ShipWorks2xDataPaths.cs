using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Utility class for determining common paths to files and data for ShipWorks 2
    /// </summary>
    public class ShipWorks2xDataPaths
    {
        string installPath;

        string oldConfigPath;
        string oldAppData;
        string oldLocalDataPath;
        string oldCommonDataPath;

        string pre24SqlSessionFile;
        string post24SqlSessionFile;

        /// <summary>
        /// Creates an instance based on the given ShipWorks 2x installation path
        /// </summary>
        public ShipWorks2xDataPaths(string installPath)
        {
            if (string.IsNullOrEmpty(installPath))
            {
                throw new ArgumentException("installPath cannot be null or empty", installPath);
            }

            this.installPath = installPath;

            oldConfigPath = Path.Combine(installPath, "Configuration");
            oldLocalDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ShipWorks");
            oldCommonDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ShipWorks");

            // This is the default appdata - it could be overridden, in which case a consumer of this class could update it
            oldAppData = Path.Combine(installPath, "Application Data");

            pre24SqlSessionFile = Path.Combine(oldLocalDataPath, "sqlsession.xml");
            post24SqlSessionFile = Path.Combine(oldConfigPath, "sqlsession.xml");
        }

        /// <summary>
        /// The installation path of ShipWorks that the other paths are based off of
        /// </summary>
        public string InstallPath
        {
            get { return installPath; }
        }

        /// <summary>
        /// The path of the folder that contains the ShipWorks configuration data
        /// </summary>
        public string Configuration
        {
            get { return oldConfigPath; }
        }

        /// <summary>
        /// The old Application Data folder. Can be updated if found to be changed by user settings
        /// </summary>
        public string ApplicationData
        {
            get { return oldAppData; }
            set { oldAppData = value; }
        }

        /// <summary>
        /// The path to the folder that contained ShipWorks 2 windows user specific data
        /// </summary>
        public string WindowsUserData
        {
            get { return oldLocalDataPath; }
        }

        /// <summary>
        /// The path to the folder that contained ShipWorks 2 data accross all users
        /// </summary>
        public string CommonData
        {
            get { return oldCommonDataPath; }
        }

        /// <summary>
        /// The full path to the sqlsession.xml file for version of ShipWorks below 2.4
        /// </summary>
        public string Pre24SqlSessionFile
        {
            get { return pre24SqlSessionFile; }
        }

        /// <summary>
        /// The full path to the sqlsession.xml for versions of ShipWorks 2.4 and greater
        /// </summary>
        public string Post24SqlSessionFile
        {
            get { return post24SqlSessionFile; }
        }
    }
}

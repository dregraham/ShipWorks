using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;
using Interapptive.Shared.IO.Zip;

namespace Interapptive.MSBuild.Tasks
{
    /// <summary>
    /// Utility class with helper functions for packaging modules
    /// </summary>
    public static class PackageModuleUtility
    {
        /// <summary>
        /// Package the given module content into a filename genreated from the platform and version information
        /// </summary>
        public static string PackageModule(string moduleFile, string platform, string version, string outputDirectory, TaskLoggingHelper Log)
        {
            // Generate the target file name
            string packagedName = string.Format("ShipWorks3_Module_{0}_v{1}.zip", platform, version.Replace(".", "_"));
            string packagedFile = Path.Combine(outputDirectory, packagedName);

            Log.LogMessage("Packaging {0}.{1} into {2}...", platform, Path.GetFileName(moduleFile), packagedName);

            Directory.CreateDirectory(outputDirectory);

            ZipWriter zipWriter = new ZipWriter();
            zipWriter.Items.Add(new ZipWriterFileItem(moduleFile, Path.GetFileName(moduleFile)));

            // Special additional inclusions for ZenCart
            if (platform == "ZenCart")
            {
                zipWriter.Items.Add(new ZipWriterFileItem(Path.Combine(Path.GetDirectoryName(moduleFile), "init_admin_auth.php"), "init_admin_auth.php"));
            }

            zipWriter.Save(packagedFile);

            return packagedFile;
        }
    }
}

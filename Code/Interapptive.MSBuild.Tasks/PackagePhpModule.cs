using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Text.RegularExpressions;
using Interapptive.Shared.IO.Zip;

namespace Interapptive.MSBuild.Tasks
{
    /// <summary>
    /// Packages an Interapptive PHP based store module for distribution
    /// </summary>
    public class PackagePhpModule : Task
    {
        static Regex regexModuleVersion = new Regex("[$]moduleVersion = \"(?<version>.*?)\"", RegexOptions.Compiled);

        string moduleFile;
        string outputDirectory;

        string packagedFile;

        /// <summary>
        /// The input .php file module
        /// </summary>
        [Required]
        public string ModuleFile
        {
            get { return moduleFile; }
            set { moduleFile = value; }
        }

        /// <summary>
        /// The directory to output the packaged file
        /// </summary>
        [Required]
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; }
        }

        /// <summary>
        /// The full path of the generated file
        /// </summary>
        [Output]
        public string PackagedFile
        {
            get { return packagedFile; }
        }

        /// <summary>
        /// Package the module specified in ModuleFile
        /// </summary>
        public override bool Execute()
        {
            // Read the contents of the module
            string phpText = File.ReadAllText(moduleFile);

            Match match = regexModuleVersion.Match(phpText);
            if (!match.Success)
            {
                throw new InvalidOperationException(string.Format("Could not find '$moduleVersion = \"x.y.z\"' in '{0}'.", moduleFile));
            }

            // The version
            string version = match.Groups["version"].Value;

            // The target platform
            string platform = Path.GetFileName(Path.GetDirectoryName(moduleFile));

            // Package the output
            packagedFile = PackageModuleUtility.PackageModule(moduleFile, platform, version, outputDirectory, Log);

            return true;
        }
    }
}

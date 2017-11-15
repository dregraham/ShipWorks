using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Interapptive.MSBuild.Tasks
{
    /// <summary>
    /// Package the ShipWorks miva modules for distribution
    /// </summary>
    public class PackageMivaModules : Task
    {
        static Regex regexMiva5Version = new Regex("swModuleVersion.*?VALUE[ ]*=[ ]*\"(?<version>.*?)\"", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        static Regex regexMiva4Version = new Regex("Module_Version.*?VALUE[ ]*=[ ]*\"(?<version>.*?)\"", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        string miva4file;
        string miva5file;
        string miva9file;

        string outputDirectory;

        string[] packagedFiles;

        /// <summary>
        /// The full path to the Miva4 module file
        /// </summary>
        [Required]
        public string ModuleFileMiva4
        {
            get { return miva4file; }
            set { miva4file = value; }
        }

        /// <summary>
        /// The full path to the Miva5 module file
        /// </summary>
        [Required]
        public string ModuleFileMiva5
        {
            get { return miva5file; }
            set { miva5file = value; }
        }

        /// <summary>
        /// The full path to the Miva5 module file
        /// </summary>
        [Required]
        public string ModuleFileMiva9
        {
            get { return miva9file; }
            set { miva9file = value; }
        }

        /// <summary>
        /// The output directory to put the packaged files
        /// </summary>
        [Required]
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; }
        }

        /// <summary>
        /// The packages files that were created
        /// </summary>
        [Output]
        public string[] PackagedFiles
        {
            get { return packagedFiles; }
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        public override bool Execute()
        {
            List<string> files = new List<string>();

            files.Add(PackageModule(miva4file, "MivaUncompiled", false));
            files.Add(PackageModule(miva4file, "Miva4", true));
            files.Add(PackageModule(miva5file, "Miva5", true));
            files.Add(PackageModule(miva9file, "Miva9", true));

            packagedFiles = files.ToArray();

            return true;
        }

        /// <summary>
        /// Compile the given miva5 modules file
        /// </summary>
        private string PackageModule(string mivaFile, string platform, bool compile)
        {
            string mivaContents = File.ReadAllText(mivaFile);

            // Make the file a little hard to read
            mivaContents = StripMivaComments(mivaContents);

            // Read the version
            Match match = regexMiva5Version.Match(mivaContents);
            if (!match.Success)
            {
                match = regexMiva4Version.Match(mivaContents);

                if (!match.Success)
                {
                    throw new InvalidOperationException(string.Format("Could not find version information in '{0}'.", mivaFile));
                }
            }

            // The version
            string version = match.Groups["version"].Value;

            // Write out the module temporarily to a new spot to be compiled
            string tempFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(mivaFile));

            // Strip off the "miva5" part of the file for miva
            tempFile = tempFile.Replace($"{platform}.mv", ".mv");

            // Write the temp contents
            using (StreamWriter writer = File.CreateText(tempFile))
            {
                writer.Write(mivaContents);
            }

            string finalFile;

            if (compile)
            {
                // Perform the miva compilation
                ProcessStartInfo start = new ProcessStartInfo(@"C:\MSC\bin\mvc.exe", "\"" + tempFile + "\"");
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;

                Log.LogMessage(string.Format("Invoking MivaScript Compiler: {0} {1}", start.FileName, start.Arguments));

                Process process = new Process();
                process.StartInfo = start;
                process.Start();

                Log.LogMessage(process.StandardOutput.ReadToEnd());
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException("The Miva Compiler failed. See log for details.");
                }

                finalFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(tempFile) + ".mvc");
            }
            else
            {
                finalFile = tempFile;
            }

            // Create the complied zip
            string packagedFile = PackageModuleUtility.PackageModule(finalFile, platform, version, outputDirectory, Log);

            File.Delete(tempFile);

            if (finalFile != tempFile)
            {
                File.Delete(finalFile);
            }

            return packagedFile;
        }

        /// <summary>
        /// Strip all comments and whitespace from the given miva module contents.
        /// </summary>
        private static string StripMivaComments(string mivaContents)
        {
            mivaContents = Regex.Replace(mivaContents, "<MvCOMMENT>[^-].*?</MvCOMMENT>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            mivaContents = Regex.Replace(mivaContents, "(?<!\\|)\r\n", " ", RegexOptions.Singleline);

            return mivaContents;
        }
    }
}

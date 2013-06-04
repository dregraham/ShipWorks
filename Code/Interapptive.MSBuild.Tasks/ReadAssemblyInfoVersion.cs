using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Text.RegularExpressions;

namespace Interapptive.MSBuild.Tasks
{
    /// <summary>
    /// Read the version data out of an AssemblyInfo file
    /// </summary>
    public class ReadAssemblyInfoVersion : Task
    {
        string assemblyInfoFile = "";

        int major;
        int minor;
        int build;
        int revision;

        /// <summary>
        /// The AssemblyInfo.cs file to read
        /// </summary>
        [Required]
        public string AssemblyInfoFile
        {
            get { return assemblyInfoFile; }
            set { assemblyInfoFile = value; }
        }

        [Output]
        public int Major
        {
            get { return major; }
        }

        [Output]
        public int Minor
        {
            get { return minor; }
        }

        [Output]
        public int Build
        {
            get { return build; }
        }

        [Output]
        public int Revision
        {
            get { return revision; }
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        public override bool Execute()
        {
            Log.LogMessage("ReadAssemblyInfoVersion splitting: [{0}]", AssemblyInfoFile);

            try
            {
                string contents = File.ReadAllText(AssemblyInfoFile);

                Match match = Regex.Match(contents, @"AssemblyVersion\(""(?<Major>\d+).(?<Minor>\d+).(?<Build>\d+).(?<Revision>\d+)");
                if (!match.Success)
                {
                    Log.LogError("AssemblyVersion attribute not found in correct format.");
                }
                else
                {
                    major = Int32.Parse(match.Groups["Major"].Value);
                    minor = Int32.Parse(match.Groups["Minor"].Value);
                    build = Int32.Parse(match.Groups["Build"].Value);
                    revision = Int32.Parse(match.Groups["Revision"].Value);
                }
            }
            catch (IOException ex)
            {
                Log.LogErrorFromException(ex);
            }
            catch (ArgumentException ex)
            {
                Log.LogErrorFromException(ex);
            }

            return true;
        }
    }
}

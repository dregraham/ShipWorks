using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Interapptive.MSBuild.Tasks
{
    /// <summary>
    /// Task for splitting a version number into its parts.
    /// </summary>
    public class SplitVersion : Task
    {
        string version = "";

        int major;
        int minor;
        int build;
        int revision;

        /// <summary>
        /// The version number to be split
        /// </summary>
        [Required]
        public string Version
        {
            get { return version; }
            set { version = value; }
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
            Log.LogMessage("SplitVersion splitting: [{0}]", Version);

            try
            {
                System.Version splitme = new Version(Version);

                major = splitme.Major;
                minor = splitme.Minor;
                build = splitme.Build;
                revision = splitme.Revision;
            }
            catch (ArgumentException ex)
            {
                Log.LogErrorFromException(ex);
            }


            return true;
        }
    }
}

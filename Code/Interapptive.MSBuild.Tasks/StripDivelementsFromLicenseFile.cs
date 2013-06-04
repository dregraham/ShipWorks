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
    /// The licenses.lcx file cannot have entries for Divelements, or the build won't work.  But anytime you go
    /// into a Form with a Divilement component, it adds it.  So this strips it before the build.
    /// </summary>
    public class StripDivelementsFromLicenseFile : Task
    {
        string licenseFile = "";

        /// <summary>
        /// The Liscenses.lcx file to read
        /// </summary>
        [Required]
        public string LicenseFile
        {
            get { return licenseFile; }
            set { licenseFile = value; }
        }

        /// <summary>
        /// Execute the task
        /// </summary>
        public override bool Execute()
        {
            Log.LogMessage("StripDivelementsFromLicenseFile [{0}]", licenseFile);

            try
            {
                string contents = File.ReadAllText(licenseFile);

                string updated = Regex.Replace(contents, "^(Divelements|TD).*$", "", RegexOptions.Multiline);

                File.WriteAllText(licenseFile, updated);
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

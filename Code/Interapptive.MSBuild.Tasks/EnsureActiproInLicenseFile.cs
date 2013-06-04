using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace Interapptive.MSBuild.Tasks
{
    /// <summary>
    /// The licenses.lcx file must have an entry for Actipro in it, but for some reason the 2010 designer keeps wiping it.  This ensures its there or fails
    /// the build instead of creating a build where you can't use the syntax editor.
    /// </summary>
    public class EnsureActiproInLicenseFile : Task
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
            Log.LogMessage("EnsureActiproInLicenseFile [{0}]", licenseFile);

            try
            {
                string contents = File.ReadAllText(licenseFile);

                if (!contents.Contains("ActiproSoftware"))
                {
                    throw new InvalidOperationException("The license file is missing entries for ActiproSoftware.");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Administration.Versioning;

namespace ShipWorks.Data.Administration
{
    public class SchemaVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaVersion"/> class.
        /// </summary>
        /// <param name="versionName">Name of the version.</param>
        public SchemaVersion(string versionName)
        {
            VersionName = versionName;
        }

        /// <summary>
        /// Gets the name of the version.
        /// </summary>
        public string VersionName { get; private set; }

        /// <summary>
        /// Gets the database status.
        /// </summary>
        /// <value>
        /// The database status.
        /// </value>
        public SqlDatabaseStatus DatabaseStatus
        {
            get
            {
                return IsVersionLessThanThree ?
                           SqlDatabaseStatus.ShipWorks2x :
                           SqlDatabaseStatus.ShipWorks;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [is version less than three].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is version less than three]; otherwise, <c>false</c>.
        /// </value>
        public bool IsVersionLessThanThree
        {
            get
            {
                Version installed;

                if (!Version.TryParse(VersionName, out installed))
                {
                    // Schema is no longer using a standard version format. This happened around v3.8.
                    return false;
                }

                return installed.Major < 3;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [is version less than one two].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is version less than one two]; otherwise, <c>false</c>.
        /// </value>
        public bool IsVersionLessThanOneTwo
        {
            get
            {
                if (!IsVersionLessThanThree)
                {
                    return false;
                }

                return Version.Parse(VersionName) < new Version(1, 2);
            }
        }

        /// <summary>
        /// Gets the System.Version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Version GetVersion()
        {
            Version version;

            if (!Version.TryParse(VersionName, out version))
            {
                throw new FormatException(string.Format("Cannot parse {0} into a Version object", VersionName));
            }

            return version;
        }

        /// <summary>
        /// If VersionName is System.Version (x.x.x.x) return true, else false
        /// </summary>
        public bool IsSystemVersion
        {
            get
            {
                Version version;
                return Version.TryParse(VersionName, out version);
            }
        }

        /// <summary>
        /// Compares the specified schema version to compare.
        /// </summary>
        /// <param name="schemaVersionToCompare">The schema version to compare.</param>
        /// <returns>
        /// If the passed in version is newer, return newer.
        /// If the passed in version is older, return older.
        /// If the passed in version is the same, return equal
        /// Else return unknown.
        /// </returns>
        public SchemaVersionComparisonResult Compare(SchemaVersion schemaVersionToCompare)
        {
            SchemaVersion leftVersion = this;
            SchemaVersion rightVersion = schemaVersionToCompare;

            // The versions are equal
            if (leftVersion.VersionName == rightVersion.VersionName)
            {
                return SchemaVersionComparisonResult.Equal;
            }


            // These are old version numbers, use standard comparison methods
            if (leftVersion.IsSystemVersion && rightVersion.IsSystemVersion)
            {
                if (leftVersion.GetVersion() > rightVersion.GetVersion())
                {
                    return SchemaVersionComparisonResult.Older;
                }
                else
                {
                    return SchemaVersionComparisonResult.Newer;
                }
            }

            // Left version is older because it is still using a traditional version number.
            if (leftVersion.IsSystemVersion)
            {
                return SchemaVersionComparisonResult.Newer;
            }

            
            if (rightVersion.IsSystemVersion)
            {
                // Right version is older because it is still using a traditional version number
                return SchemaVersionComparisonResult.Older;
            }

            SchemaVersionManager schemaVersionManager = new SchemaVersionManager();

            try
            {
                schemaVersionManager.GetUpdateScripts(leftVersion, rightVersion);
                // an upgrade path was found. Right version must be newer.
                return SchemaVersionComparisonResult.Newer;
            }
            catch (FindVersionUpgradePathException)
            {
                // no upgrade path from leftVersion to rightVersion
            }

            try
            {
                schemaVersionManager.GetUpdateScripts(rightVersion, leftVersion);
                // an upgrade path was found. Left version must be newer.
                return SchemaVersionComparisonResult.Older;
            }
            catch (FindVersionUpgradePathException)
            {
                // no upgrade path from rightVersion to leftVersion
            }

            return SchemaVersionComparisonResult.Unknown;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return VersionName;
        }
    }
}

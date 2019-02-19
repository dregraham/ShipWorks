using System;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// ShipWorks Release DTO
    /// </summary>
    public class ShipWorksRelease
    {
        /// <summary>
        /// Download Url
        /// </summary>
        public Uri DownloadUrl { get; set; }

        /// <summary>
        /// SHA hash of release
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// Release notes
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Minimum allowed version
        /// </summary>
        public Version MinAllowedReleaseVersion { get; set; }

        /// <summary>
        /// ReleaseVersion
        /// </summary>
        public Version ReleaseVersion { get; internal set; }
    }
}

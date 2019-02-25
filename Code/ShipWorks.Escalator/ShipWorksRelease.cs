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
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Download Uri
        /// </summary>
        public Uri DownloadUri => new Uri(DownloadUrl);

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
        public string MinAllowedReleaseVersion { get; set; }

        /// <summary>
        /// ReleaseVersion
        /// </summary>
        public string ReleaseVersion { get; set; }
    }
}

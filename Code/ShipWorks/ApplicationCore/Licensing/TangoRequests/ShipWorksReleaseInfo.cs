using System;
using System.Xml.Serialization;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Information about a ShipWorks release
    /// </summary>
    [Serializable]
    [XmlRoot("ShipWorksRelease")]
    public class ShipWorksReleaseInfo
    {
        /// <summary>
        /// Url of the installer
        /// </summary>
        [XmlElement("DownloadUrl")]
        public string DownloadUrl { get; set; }

        /// <summary>
        /// Hash of the installer
        /// </summary>
        [XmlElement("Hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Release version
        /// </summary>
        [XmlElement("ReleaseVersion")]
        public string ReleaseVersionString { get; set; }

        /// <summary>
        /// Release version
        /// </summary>
        public Version ReleaseVersion => new Version(ReleaseVersionString);

        /// <summary>
        /// Release notes
        /// </summary>
        [XmlElement("ReleaseNotes")]
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Minimum allowed version of ShipWorks
        /// </summary>
        [XmlElement("MinAllowedReleaseVersion")]
        public string MinAllowedReleaseVersionString { get; set; }

        /// <summary>
        /// Minimum allowed version of ShipWorks
        /// </summary>
        public Version MinAllowedReleaseVersion => new Version(MinAllowedReleaseVersionString);
    }
}
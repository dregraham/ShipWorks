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
        /// Release notes
        /// </summary>
        [XmlElement("ReleaseNotes")]
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Minimum allowed version of ShipWorks
        /// </summary>
        [XmlElement("MinAllowedReleaseVersion")]
        public Version MinAllowedReleaseVersion { get; set; }

        //        <?xml version = '1.0' ?>
        //< ShipWorksRelease xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
        // <DownloadUrl>https://www.example.com</DownloadUrl>
        // <Hash>asgd</Hash>
        // <ReleaseNotes>https://www.shipworks.com#5.31.1.1</ReleaseNotes>
        // <MinAllowedReleaseVersion>5.32.0.102</MinAllowedReleaseVersion>
        //</ShipWorksRelease>
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// The response from a ShipWorks version check
    /// </summary>
    public class ShipWorksOnlineVersion
    {
        Version version;
        string downloadUrl;
        string whatsNewUrl;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksOnlineVersion(Version version, string downloadUrl, string whatsNewUrl)
        {
            this.version = version;
            this.downloadUrl = downloadUrl;
            this.whatsNewUrl = whatsNewUrl;
        }

        /// <summary>
        /// The most recent available version of ShipWorks
        /// </summary>
        public Version Version
        {
            get { return version; }
        }

        /// <summary>
        /// The URL to download the latest version.
        /// </summary>
        public string DownloadUrl
        {
            get { return string.Format(downloadUrl, version); }
        }

        /// <summary>
        /// The URL to download the latest version.
        /// </summary>
        public string WhatsNewUrl
        {
            get { return string.Format(whatsNewUrl, version); }
        }

    }
}

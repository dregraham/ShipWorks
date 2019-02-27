using System;
using System.Threading.Tasks;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Communicates with tango to download the requested version
    /// </summary>
    public interface IUpdaterWebClient : IDisposable
    {
        /// <summary>
        /// Download the requested version
        /// </summary>
        Task<InstallFile> Download(Uri url, string sha);

        /// <summary>
        /// Get the url and sha of requesting customer id
        /// </summary>
        Task<ShipWorksRelease> GetVersionToDownload(string tangoCustomerId, Version currentVersion);

        /// <summary>
        /// Get the url and sha of requested version 
        /// </summary>
        Task<ShipWorksRelease> GetVersionToDownload(Version version);
    }
}
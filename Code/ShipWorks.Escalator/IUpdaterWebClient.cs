using System;
using System.Threading.Tasks;

namespace ShipWorks.Escalator
{
    public interface IUpdaterWebClient : IDisposable
    {
        Task<InstallFile> Download(Uri url, string sha);
        Task<ShipWorksRelease> GetVersionToDownload(string tangoCustomerId, Version currentVersion);
        Task<ShipWorksRelease> GetVersionToDownload(Version version);
    }
}
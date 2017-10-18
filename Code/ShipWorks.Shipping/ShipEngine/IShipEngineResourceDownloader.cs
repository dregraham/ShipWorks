using ShipWorks.ApplicationCore.Logging;
using System;
using System.IO;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Represents a resource downloader for shipengine
    /// </summary>
    public interface IShipEngineResourceDownloader
    {
        /// <summary>
        /// Download a resource from shipengine and return its contents.
        /// </summary>        
        byte[] Download(Uri uri);
    }
}

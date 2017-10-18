using ShipWorks.ApplicationCore.Logging;
using System;
using System.IO;

namespace ShipWorks.Shipping.ShipEngine
{
    public interface IShipEngineResourceDownloader
    {
        /// <summary>
        /// Download a resource from shipengine and return its contents.
        /// </summary>        
        byte[] Download(Uri uri, ApiLogSource logSource);
    }
}

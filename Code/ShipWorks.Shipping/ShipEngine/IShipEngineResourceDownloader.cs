using ShipWorks.ApplicationCore.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.ShipEngine
{
    public interface IShipEngineResourceDownloader
    {
        /// <summary>
        /// Download a resource from shipengine and return its contents.
        /// </summary>        
        byte[] Download(Uri uri, ApiLogSource logSource, string actionName);
    }
}

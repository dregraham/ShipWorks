using System;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using System.Net;
using System.IO;
using Interapptive.Shared.Extensions;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// Download resources from ShipEngine
    /// </summary>
    [Component]
    public class ShipEngineResourceDownloader : IShipEngineResourceDownloader
    {
        /// <summary>
        /// Download the resourcce at the given uri
        /// </summary>
        public byte[] Download(Uri uri, ApiLogSource logSource)
        {
            try
            {
                return WebRequest.Create(uri).GetResponse().GetResponseStream().ToArray();
            }
            catch (Exception ex)
            {
                throw new ShipEngineException($"An error occured while attempting to download reasource from {logSource}.", ex);
            }
        }
    }
}

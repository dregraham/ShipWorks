using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.ShipEngine;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineResourceDownloaderTest
    {
        [Fact]
        public void Download_ThrowsShipEngineExceptionWithLogSource_WhenWebRequestThrowsException()
        {
            ShipEngineResourceDownloader testObject = new ShipEngineResourceDownloader();

            ShipEngineException ex = Assert.Throws<ShipEngineException>(() => testObject.Download(null, ApiLogSource.DHLExpress));

            Assert.Equal($"An error occured while attempting to download reasource from {ApiLogSource.DHLExpress}.", ex.Message);
        }
       
    }
}

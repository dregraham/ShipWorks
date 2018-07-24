using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Stores.Communication;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Communication
{
    public class StoreDownloaderTest : IDisposable
    {
        readonly AutoMock mock;

        public StoreDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        [InlineData("", "")]
        public void UpdateCustomerAddressIfNecessary_DoesNotUpdateAddress_WhenAddressIsEmpty(string orderCity, string orderZip)
        {
            var orderAddress = new PersonAdapter { City = orderCity, PostalCode = orderZip };
            var customerAddress = new PersonAdapter
            {
                City = "Foo"
            };
            var originalAddress = new AddressAdapter();

            StoreDownloader.UpdateCustomerAddressIfNecessary(true, ModifiedOrderCustomerUpdateBehavior.AlwaysCopy, orderAddress, customerAddress, originalAddress);

            Assert.Equal("Foo", customerAddress.City);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}

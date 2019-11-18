using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Rakuten
{
    public class RakutenStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;

        public RakutenStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void TypeCode_IsRakuten()
        {
            RakutenStoreType testObject = mock.Create<RakutenStoreType>(new TypedParameter(typeof(StoreEntity), 
                new RakutenStoreEntity() { StoreTypeCode = StoreTypeCode.Rakuten}));

            Assert.Equal(StoreTypeCode.Rakuten, testObject.TypeCode);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}

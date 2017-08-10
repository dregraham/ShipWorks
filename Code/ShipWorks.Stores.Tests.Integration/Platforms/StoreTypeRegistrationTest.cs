using System;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    public class StoreTypeRegistrationTest : IDisposable
    {
        readonly IContainer container;

        public StoreTypeRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
            IoC.Initialize(container);
        }

        [Fact]
        public void EnsureAllStoreTypeCodesHaveStoreTypeRegistered()
        {
            var storesToTest = Enum.GetValues(typeof(StoreTypeCode)).OfType<StoreTypeCode>().Except(new[] { StoreTypeCode.Invalid });

            foreach (var storeTypeCode in storesToTest)
            {
                StoreType service = container.ResolveKeyed<StoreType>(storeTypeCode, TypedParameter.From<StoreEntity>(null));

                // A failure here means that the wrong store type code was used to register a store type
                Assert.Equal(storeTypeCode, service.TypeCode);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}

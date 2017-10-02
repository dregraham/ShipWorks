using System;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    public class StoreTypeRegistrationTest : IDisposable
    {
        private readonly IContainer container;
        private readonly ITestOutputHelper testOutputHelper;

        public StoreTypeRegistrationTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
            IoC.Initialize(container);
        }

        [Fact]
        public void EnsureAllStoreTypeCodesHaveStoreTypeRegistered()
        {
            var mismatchedStoreTypes = Enum.GetValues(typeof(StoreTypeCode))
                .OfType<StoreTypeCode>()
                .Except(new[] { StoreTypeCode.Invalid })
                .Select(x => new { TypeCode = x, StoreType = container.ResolveKeyed<StoreType>(x, TypedParameter.From<StoreEntity>(null)) })
                .Where(x => x.TypeCode != x.StoreType.TypeCode)
                .ToList();

            foreach (var mismatch in mismatchedStoreTypes)
            {
                testOutputHelper.WriteLine($"{mismatch.TypeCode} resolved to {mismatch.StoreType.TypeCode}");
            }

            Assert.Empty(mismatchedStoreTypes);
        }

        public void Dispose() => container?.Dispose();
    }
}

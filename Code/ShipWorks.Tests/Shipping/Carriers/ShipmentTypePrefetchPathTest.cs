using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers
{
    public class ShipmentTypePrefetchPathTest : IDisposable
    {
        readonly AutoMock mock;

        public ShipmentTypePrefetchPathTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ApplyTo_DoesNotIncludePostalTwice_WhenPostalIsInMultiplePaths()
        {
            var modifiedFoo = ShipmentTypePrefetchPath.Empty
                .With(new TestUspsProvider())
                .With(new TestEndiciaProvider());

            var result = modifiedFoo.ApplyTo(Enumerable.Empty<IPrefetchPathElement2>(), (l, x) => l.Append(x));

            Assert.Equal(1, result.Count());

            var root = result.First();
            Assert.Equal(ShipmentEntity.PrefetchPathPostal, root);
            Assert.Contains(PostalShipmentEntity.PrefetchPathUsps, root.SubPath.OfType<IPrefetchPathElement2>());
            Assert.Contains(PostalShipmentEntity.PrefetchPathEndicia, root.SubPath.OfType<IPrefetchPathElement2>());
        }

        [Fact]
        public void ApplyTo_IncludesBothRoots_WhenMultipleRootPathsArePresent()
        {
            var modifiedFoo = ShipmentTypePrefetchPath.Empty
                .With(new TestUspsProvider())
                .With(new TestUpsProvider());

            var result = modifiedFoo.ApplyTo(Enumerable.Empty<IPrefetchPathElement2>(), (l, x) => l.Append(x));

            Assert.Equal(2, result.Count());
            Assert.Contains(ShipmentEntity.PrefetchPathPostal, result);
            Assert.Contains(ShipmentEntity.PrefetchPathUps, result);
        }

        [Fact]
        public void ApplyTo_AddsPathsCorrectly_WhenPathContainerHasCircularReference()
        {
            var modifiedFoo = ShipmentTypePrefetchPath.Empty
                .With(new CircularPath());

            var result = modifiedFoo.ApplyTo(Enumerable.Empty<IPrefetchPathElement2>(), (l, x) => l.Append(x));

            Assert.Equal(1, result.Count());
            Assert.Contains(ShipmentEntity.PrefetchPathUps, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        public class TestUspsProvider : IShipmentTypePrefetchProvider
        {
            public PrefetchPathContainer GetPath() =>
                ShipmentEntity.PrefetchPathPostal.WithChild(PostalShipmentEntity.PrefetchPathUsps);
        }

        public class TestEndiciaProvider : IShipmentTypePrefetchProvider
        {
            public PrefetchPathContainer GetPath() =>
                ShipmentEntity.PrefetchPathPostal.WithChild(PostalShipmentEntity.PrefetchPathEndicia);
        }

        public class TestUpsProvider : IShipmentTypePrefetchProvider
        {
            public PrefetchPathContainer GetPath() =>
                ShipmentEntity.PrefetchPathUps.ToContainer();
        }

        public class CircularPath : IShipmentTypePrefetchProvider
        {
            public PrefetchPathContainer GetPath() =>
                ShipmentEntity.PrefetchPathUps.WithChild(UpsShipmentEntity.PrefetchPathShipment.WithChild(ShipmentEntity.PrefetchPathUps));
        }
    }
}

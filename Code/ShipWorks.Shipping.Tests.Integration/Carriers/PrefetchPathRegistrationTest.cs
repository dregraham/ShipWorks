using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Endicia;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.IParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.Usps;
using ShipWorks.Shipping.Carriers.WebTools;
using ShipWorks.Startup;
using Xunit;
using ShipWorks.Shipping.Carriers.Dhl;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    public class PrefetchProviderRegistrationTest : IDisposable
    {
        IContainer container;

        public PrefetchProviderRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon, typeof(AmazonPrefetchProvider))]
        [InlineData(ShipmentTypeCode.BestRate, typeof(BestRatePrefetchProvider))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaPrefetchProvider))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(EndiciaPrefetchProvider))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(UspsPrefetchProvider))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(FedExPrefetchProvider))]
        [InlineData(ShipmentTypeCode.iParcel, typeof(IParcelPrefetchProvider))]
        [InlineData(ShipmentTypeCode.OnTrac, typeof(OnTracPrefetchProvider))]
        [InlineData(ShipmentTypeCode.Other, typeof(OtherPrefetchProvider))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(WebToolsPrefetchProvider))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(UpsPrefetchProvider))]
        [InlineData(ShipmentTypeCode.UpsWorldShip, typeof(UpsPrefetchProvider))]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsPrefetchProvider))]
        [InlineData(ShipmentTypeCode.DhlExpress, typeof(DhlExpressPrefetchProvider))]
        public void EnsurePrefetchProvidersAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            IShipmentTypePrefetchProvider retriever = container.ResolveKeyed<IShipmentTypePrefetchProvider>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Fact]
        public void EnsureAllShipmentTypesHavePrefetchProviderRegistered()
        {
            IEnumerable<ShipmentTypeCode> excludedTypes = new[] { ShipmentTypeCode.None };

            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(excludedTypes))
            {
                IShipmentTypePrefetchProvider service = container.ResolveKeyed<IShipmentTypePrefetchProvider>(value);
                Assert.NotNull(service);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}

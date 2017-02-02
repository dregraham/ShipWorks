using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    public class LabelServiceRegistrationTest : IDisposable
    {
        IContainer container;

        public LabelServiceRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon, typeof(AmazonLabelService))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaLabelService))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(Express1EndiciaLabelService))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(Express1UspsLabelService))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(FedExLabelService))]
        [InlineData(ShipmentTypeCode.iParcel, typeof(iParcelLabelService))]
        [InlineData(ShipmentTypeCode.OnTrac, typeof(OnTracLabelService))]
        [InlineData(ShipmentTypeCode.Other, typeof(OtherLabelService))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(WebToolsLabelService))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(UpsOltLabelService))]
        [InlineData(ShipmentTypeCode.UpsWorldShip, typeof(WorldShipLabelService))]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsLabelService))]
        public void EnsureLabelServicesAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            ILabelService retriever = container.ResolveKeyed<ILabelService>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Fact]
        public void EnsureAllShipmentTypesHaveLabelServiceRegistered()
        {
            IEnumerable<ShipmentTypeCode> excludedTypes = new[] { ShipmentTypeCode.BestRate, ShipmentTypeCode.None };

            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(excludedTypes))
            {
                ILabelService service = container.ResolveKeyed<ILabelService>(value);
                Assert.NotNull(service);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}

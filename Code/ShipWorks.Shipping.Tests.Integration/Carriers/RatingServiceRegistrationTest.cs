using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Startup;
using Xunit;
using ShipWorks.Shipping.Carriers.Dhl;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    public class RatingServiceRegistrationTest : IDisposable
    {
        IContainer container;

        public RatingServiceRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon, typeof(AmazonRatingService))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaRatingService))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(Express1EndiciaRatingService))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(Express1UspsRatingService))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(FedExRatingService))]
        [InlineData(ShipmentTypeCode.iParcel, typeof(iParcelRatingService))]
        [InlineData(ShipmentTypeCode.OnTrac, typeof(OnTracRatingService))]
        [InlineData(ShipmentTypeCode.Other, typeof(EmptyRatingService))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(WebToolsRatingService))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(UpsRatingService))]
        [InlineData(ShipmentTypeCode.UpsWorldShip, typeof(UpsRatingService))]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsRatingService))]
        [InlineData(ShipmentTypeCode.DhlExpress, typeof(DhlExpressRatingService))]
        public void EnsureRatingServicesAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            IRatingService retriever = container.ResolveKeyed<IRatingService>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Fact]
        public void EnsureAllShipmentTypesHaveRatingServiceRegistered()
        {
            IEnumerable<ShipmentTypeCode> excludedTypes = new[] { ShipmentTypeCode.BestRate, ShipmentTypeCode.None };

            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(excludedTypes))
            {
                IRatingService service = container.ResolveKeyed<IRatingService>(value);
                Assert.NotNull(service);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}

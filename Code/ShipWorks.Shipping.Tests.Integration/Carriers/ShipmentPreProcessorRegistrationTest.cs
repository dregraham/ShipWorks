using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class ShipmentPreProcessorRegistrationTest : IDisposable
    {
        IContainer container;

        public ShipmentPreProcessorRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void EnsureCarriersThatDoNotUseDefaultPreProcessorAreRegistered()
        {
            foreach (KeyValuePair<ShipmentTypeCode, Type> carrier in CarriersThatDoNotUseDefaultPreProcessor)
            {
                IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(carrier.Key);
                Assert.Equal(carrier.Value, service.GetType());
            }
        }

        [Fact]
        public void EnsureAllCarriersExceptBestRateUseGenericShipmentPreProcessor()
        {
            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Where(x => !CarriersThatDoNotUseDefaultPreProcessor.ContainsKey(x)))
            {
                IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(value);
                Assert.IsType<DefaultShipmentPreProcessor>(service);
            }
        }

        private static IDictionary<ShipmentTypeCode, Type> CarriersThatDoNotUseDefaultPreProcessor
        {
            get
            {
                var dict = new Dictionary<ShipmentTypeCode, Type>();
                dict.Add(ShipmentTypeCode.Endicia, typeof(PostalShipmentPreProcessor));
                dict.Add(ShipmentTypeCode.Usps, typeof(PostalShipmentPreProcessor));
                dict.Add(ShipmentTypeCode.BestRate, typeof(BestRateShipmentPreProcessor));
                return dict;
            }
        }

        public void Dispose() => container?.Dispose();
    }
}

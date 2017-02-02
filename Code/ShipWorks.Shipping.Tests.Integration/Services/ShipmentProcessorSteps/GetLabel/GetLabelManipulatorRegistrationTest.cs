using System;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Services.ShipmentProcessorSteps.GetLabel
{
    [Trait("Category", "ContinuousIntegration")]
    public class GetLabelManipulatorRegistrationTest : IDisposable
    {
        IContainer container;

        public GetLabelManipulatorRegistrationTest()
        {
            container = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build());
        }

        [Fact]
        public void EnsureRegistrationOrder()
        {
            var result = container.Resolve<IOrderedEnumerable<ILabelRetrievalShipmentManipulator>>();

            Assert.IsType<EnsureLoadedManipulator>(result.ElementAt(0));
        }

        [Fact]
        public void EnsureAllManipulatorsAreRegistered()
        {
            var result = container.Resolve<IOrderedEnumerable<ILabelRetrievalShipmentManipulator>>();
            var types = result.Select(x => x.GetType()).ToList();

            Assert.Contains(typeof(EnsureLoadedManipulator), types);
            Assert.Contains(typeof(SupportsReturnsManipulator), types);
        }

        public void Dispose() => container.Dispose();
    }
}
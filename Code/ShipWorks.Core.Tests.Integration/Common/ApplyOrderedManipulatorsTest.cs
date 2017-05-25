using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.Common;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Common
{
    [SuppressMessage("ShipWorks", "SW0002",
        Justification = "Tests aren't obfuscated, so we don't need to worry about this")]
    [Trait("Category", "ContinuousIntegration")]
    public class ApplyOrderedManipulatorsTest : IDisposable
    {
        private readonly IContainer container;

        public ApplyOrderedManipulatorsTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AddFive>().OrderBy(nameof(IThing), 1).AsImplementedInterfaces();
            builder.RegisterType<MultiplyFive>().OrderBy(nameof(IThing), 2).AsImplementedInterfaces();

            container = ContainerInitializer.BuildRegistrations(builder.Build());
        }

        [Fact]
        public void Registration_ComponentRegistered_ForService()
        {
            var applicator = container.Resolve<IOrderedCompositeManipulator<IThing, int>>();
            Assert.IsType<OrderedCompositeManipulator<IThing, int>>(applicator);
        }

        [Theory]
        [InlineData(0, 25)]
        [InlineData(1, 30)]
        public void Apply_WithManipulators_AppliesManipulatorsInCorrectOrder(int input, int expected)
        {
            var applicator = container.Resolve<IOrderedCompositeManipulator<IThing, int>>();
            var result = applicator.Apply(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 10)]
        public void Apply_WithManipulators_AppliesManipulatorsInCorrectOrderWhenReveresed(int input, int expected)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AddFive>().OrderBy(nameof(IThing), 2).AsImplementedInterfaces();
            builder.RegisterType<MultiplyFive>().OrderBy(nameof(IThing), 1).AsImplementedInterfaces();

            using (var container2 = ContainerInitializer.BuildRegistrations(builder.Build()))
            {
                var applicator = container2.Resolve<IOrderedCompositeManipulator<IThing, int>>();
                var result = applicator.Apply(input);
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void Apply_ThrowsInvalidOperationException_WhenNoManipulatorsAreRegistered()
        {
            using (var container2 = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build()))
            {
                var applicator = container2.Resolve<IOrderedCompositeManipulator<IThing, int>>();
                Assert.Throws<InvalidOperationException>(() => applicator.Apply(0));
            }
        }

        public void Dispose() => container.Dispose();
    }

    public class AddFive : IThing
    {
        public int Manipulate(int input) => input + 5;
    }

    public class MultiplyFive : IThing
    {
        public int Manipulate(int input) => input * 5;
    }

    public interface IThing : IManipulator<int>
    {
    }
}
using System;
using Autofac;
using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Common;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Common
{
    [Trait("Category", "ContinuousIntegration")]
    public class ApplyOrderedManipulatorsTest : IDisposable
    {
        private readonly IContainer container;

        public ApplyOrderedManipulatorsTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AddFive>().OrderBy(1).AsImplementedInterfaces();
            builder.RegisterType<MultiplyFive>().OrderBy(2).AsImplementedInterfaces();

            container = ContainerInitializer.BuildRegistrations(builder.Build());
        }

        [Fact]
        public void Registration_ComponentRegistered_ForService()
        {
            var applicator = container.Resolve<IApplyOrderedManipulators<IThing, int>>();
            Assert.IsType<ApplyOrderedManipulators<IThing, int>>(applicator);
        }

        [Theory]
        [InlineData(0, 25)]
        [InlineData(1, 30)]
        public void Apply_WithManipulators_AppliesManipulatorsInCorrectOrder(int input, int expected)
        {
            var applicator = container.Resolve<IApplyOrderedManipulators<IThing, int>>();
            var result = applicator.Apply(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 5)]
        [InlineData(1, 10)]
        public void Apply_WithManipulators_AppliesManipulatorsInCorrectOrderWhenReveresed(int input, int expected)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AddFive>().OrderBy(2).AsImplementedInterfaces();
            builder.RegisterType<MultiplyFive>().OrderBy(1).AsImplementedInterfaces();

            using (var container2 = ContainerInitializer.BuildRegistrations(builder.Build()))
            {
                var applicator = container2.Resolve<IApplyOrderedManipulators<IThing, int>>();
                var result = applicator.Apply(input);
                Assert.Equal(expected, result);
            }
        }

        [Fact]
        public void Apply_ThrowsInvalidOperationException_WhenNoManipulatorsAreRegistered()
        {
            using (var container2 = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build()))
            {
                var applicator = container2.Resolve<IApplyOrderedManipulators<IThing, int>>();
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
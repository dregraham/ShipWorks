using System;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Common;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Common
{
    [Trait("Category", "ContinuousIntegration")]
    public class ApplyValidatorsTest : IDisposable
    {
        private readonly IContainer container;
        private Mock<ITestValidator> first;
        private Mock<ITestValidator> second;

        public ApplyValidatorsTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            first = mock.CreateMock<ITestValidator>();
            second = mock.CreateMock<ITestValidator>();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(first.Object).AsImplementedInterfaces();
            builder.RegisterInstance(second.Object).AsImplementedInterfaces();

            container = ContainerInitializer.BuildRegistrations(builder.Build());
        }

        [Fact]
        public void Registration_ComponentRegistered_ForService()
        {
            var applicator = container.Resolve<IApplyValidators<ITestValidator, int>>();
            Assert.IsType<ApplyValidators<ITestValidator, int>>(applicator);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void Apply_CallsAllValidators(int input)
        {
            var applicator = container.Resolve<IApplyValidators<ITestValidator, int>>();
            applicator.Apply(input);

            first.Verify(x => x.Validate(input));
            second.Verify(x => x.Validate(input));
        }

        [Theory]
        [InlineData("first")]
        [InlineData("second")]
        public void Apply_ReturnsFailure_WhenOneValidatorFails(string message)
        {
            first.Setup(x => x.Validate(It.IsAny<int>()))
                .Returns(message == "first" ? Result.FromSuccess() : Result.FromError(message));
            second.Setup(x => x.Validate(It.IsAny<int>()))
                .Returns(message == "second" ? Result.FromSuccess() : Result.FromError(message));

            var applicator = container.Resolve<IApplyValidators<ITestValidator, int>>();
            var result = applicator.Apply(0);

            Assert.True(result.Failure);
            Assert.Equal(1, result.Errors.Count());
            Assert.Contains(message, result.Errors);
        }

        [Fact]
        public void Apply_ReturnsFailure_WhenBothValidatorsFail()
        {
            first.Setup(x => x.Validate(It.IsAny<int>())).Returns(Result.FromError("first"));
            second.Setup(x => x.Validate(It.IsAny<int>())).Returns(Result.FromError("second"));

            var applicator = container.Resolve<IApplyValidators<ITestValidator, int>>();
            var result = applicator.Apply(0);

            Assert.True(result.Failure);
            Assert.Equal(2, result.Errors.Count());
            Assert.Contains("first", result.Errors);
            Assert.Contains("second", result.Errors);
        }

        [Fact]
        public void Apply_ReturnsSuccess_WhenBothValidatorsSucceed()
        {
            first.Setup(x => x.Validate(It.IsAny<int>())).Returns(Result.FromSuccess());
            second.Setup(x => x.Validate(It.IsAny<int>())).Returns(Result.FromSuccess());

            var applicator = container.Resolve<IApplyValidators<ITestValidator, int>>();
            var result = applicator.Apply(0);

            Assert.True(result.Success);
            Assert.False(result.Errors.Any());
        }

        [Fact]
        public void Apply_ThrowsInvalidOperationException_WhenNoValidatorsAreRegistered()
        {
            using (var container2 = ContainerInitializer.BuildRegistrations(new ContainerBuilder().Build()))
            {
                var applicator = container2.Resolve<IApplyValidators<ITestValidator, int>>();
                Assert.Throws<InvalidOperationException>(() => applicator.Apply(0));
            }
        }

        public void Dispose() => container.Dispose();
    }

    public interface ITestValidator : IValidator<int>
    {
    }
}